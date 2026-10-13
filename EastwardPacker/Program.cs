using CommandLine;
using EastwardLib.Assets;
using EastwardLib.Compilation;
using EastwardLib.Utils;
using EastwardPacker;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(OnParsed)
    .WithNotParsed(_ => Environment.ExitCode = 1);

void OnParsed(Options options)
{
    try
    {
        if (options.Pack)
        {
            if (options.Compile || options.Assets || options.Project || options.InputDirectoryPath == null || options.OutputFilePath == null)
            {
                throw new ArgumentException("--pack requires --input_dir and --output. Use --compile for edited source assets.");
            }

            var input = Path.GetFullPath(options.InputDirectoryPath);
            var output = Path.GetFullPath(options.OutputFilePath);
            if (output.StartsWith(input + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Place the output archive outside its input directory.");
            }

            var archive = GArchive.Create(Path.GetFileNameWithoutExtension(output));
            foreach (var file in Directory.EnumerateFiles(input, "*", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
            {
                archive.Add(archive.ArchiveName + "/" + Path.GetRelativePath(input, file).Replace('\\', '/'), File.ReadAllBytes(file));
            }

            archive.Write(output);
            Console.WriteLine($"Packed {archive.Count} files: {output}");
            return;
        }
        if (options.OutputDirectoryPath == null)
        {
            throw new ArgumentException("Specify --output_dir.");
        }

        if (options.Compile)
        {
            if (options.SourcePath == null || options.Assets || options.LuaSource || options.DecompilerPath != null || options.AssetType != null && !options.Project)
                throw new ArgumentException("--compile requires --source and --asset, or --project; --type requires --project.");
            if (options.Project == (options.AssetPath != null))
                throw new ArgumentException("Choose --asset for one asset or --project for a restored source tree.");
            var manager = LoadManager(options);
            var compiler = new AssetCompiler(manager);
            var resources = options.Project ? compiler.CompileProject(options.SourcePath, options.AssetType == null ? null : info => info.Type == options.AssetType)
                : compiler.Compile(options.AssetPath!, options.SourcePath);
            compiler.SaveTo(options.OutputDirectoryPath, resources);
            Console.WriteLine($"Compiled {resources.Count} resource files: {options.OutputDirectoryPath}");
            return;
        }
        if (options.SourcePath != null || options.AssetPath != null)
            throw new ArgumentException("--source and --asset require --compile.");

        if (options.Assets || options.Project)
        {
            if (options.Assets && options.Project)
            {
                throw new ArgumentException("Choose --assets or --project.");
            }

            using var decompiler = options.LuaSource || options.Project ? new NativeLuaDecompiler(options.DecompilerPath) : null;
            var manager = LoadManager(options);

            var failures = 0;
            if (decompiler != null)
            {
                manager.LuaDecompiler = decompiler.Decompile;
            }

            if (options.Project)
            {
                var report = new ProjectReconstructor(manager).SaveTo(options.OutputDirectoryPath, options.AssetType == null ? null : info => info.Type == options.AssetType,
                    (completed, total, name) => { if (completed % 1000 == 0 || completed == total) { Console.WriteLine($"Restored {completed}/{total}: {name}"); } });
                Console.WriteLine($"Source restoration: {report.Restored.Count} physical assets, {report.VirtualAssets} virtual assets, {report.Unavailable.Count} unavailable resources.");
                foreach (var (name, reason) in report.Unavailable)
                {
                    Console.Error.WriteLine($"{name}: {reason}");
                }

                return;
            }
            foreach (var info in manager.Assets.Values.Where(info => info.Type != "folder" && (options.AssetType == null || info.Type == options.AssetType)))
            {
                try
                {
                    manager.LoadAsset(info.AssetName);
                }
                catch (Exception e) { failures++; Console.Error.WriteLine($"{info.AssetName}: {e.Message}"); }
            }
            manager.ExtractTo(options.OutputDirectoryPath, options.LuaSource, (name, error) =>
            {
                failures++;
                Console.Error.WriteLine($"{name}: {error.Message}");
            });
            if (failures > 0)
            {
                Environment.ExitCode = 1;
            }

            return;
        }
        if (options.AssetType != null || options.LuaSource || options.DecompilerPath != null || options.OverlayDirectoryPath != null)
        {
            throw new ArgumentException("--type, --lua-source, --decompiler and --overlay require --assets or --project.");
        }

        var files = options.Recursive
            ? options.InputDirectoryPath != null ? Directory.EnumerateFiles(options.InputDirectoryPath, "*.g", SearchOption.AllDirectories) : throw new ArgumentException("--recursive requires --input_dir.")
            : options.InputFilePath != null ? new[] { options.InputFilePath } : throw new ArgumentException("Extraction requires --input.");
        foreach (var file in files)
        {
            var archive = GArchive.Read(file);
            archive.ExtractTo(options.OutputDirectoryPath);
            Console.WriteLine($"Extracted {archive.Count} files: {archive.ArchiveName}");
        }
    }
    catch (Exception e)
    {
        Console.Error.WriteLine(e.Message);
        Environment.ExitCode = 1;
    }
}

AssetManager LoadManager(Options options)
{
    var manager = options.ConfigPath != null ? AssetManager.Create(options.ConfigPath)
        : options.AssetIndexPath != null && options.ScriptLibraryPath != null && options.TextureIndexPath != null
            ? AssetManager.Create(options.AssetIndexPath, options.ScriptLibraryPath, options.TextureIndexPath)
            : throw new ArgumentException("Specify --config or --index, --script_lib and --texture_index.");
    if (options.InputDirectoryPath != null)
    {
        manager.RootDirectory = options.InputDirectoryPath;
        foreach (var path in Directory.EnumerateFiles(options.InputDirectoryPath, "*.g", SearchOption.AllDirectories)) manager.RegisterArchive(path);
    }
    else if (options.InputFilePath != null) manager.RegisterArchive(options.InputFilePath);
    else throw new ArgumentException("Specify --input_dir or --input.");
    if (options.OverlayDirectoryPath != null)
        foreach (var path in Directory.EnumerateFiles(options.OverlayDirectoryPath, "*.g", SearchOption.AllDirectories)) manager.RegisterArchive(path);
    return manager;
}
