using CommandLine;

namespace EastwardPacker;

public class Options
{
    [Option("compile", HelpText = "Compile edited source assets into copies of their original archives.")]
    public bool Compile
    {
        get; set;
    }

    [Option("source", HelpText = "Edited source file, package directory or restored project directory.")]
    public string? SourcePath
    {
        get; set;
    }

    [Option("asset", HelpText = "Indexed asset path to compile; omit with --project.")]
    public string? AssetPath
    {
        get; set;
    }

    [Option("index", HelpText = "Asset index JSON for decoded asset extraction.")]
    public string? AssetIndexPath
    {
        get; set;
    }

    [Option("script_lib", HelpText = "Script library JSON for decoded asset extraction.")]
    public string? ScriptLibraryPath
    {
        get; set;
    }

    [Option("texture_index", HelpText = "Texture index JSON for decoded asset extraction.")]
    public string? TextureIndexPath
    {
        get; set;
    }

    [Option("config", HelpText = "config.g for decoded asset extraction.")]
    public string? ConfigPath
    {
        get; set;
    }

    [Option("assets", HelpText = "Decode indexed assets instead of extracting archive bytes.")]
    public bool Assets
    {
        get; set;
    }

    [Option("project", HelpText = "Restore a Gii source tree, or compile it with --compile --source.")]
    public bool Project
    {
        get; set;
    }

    [Option("type", HelpText = "Select one indexed asset type for extraction or project compilation.")]
    public string? AssetType
    {
        get; set;
    }

    [Option("pack", HelpText = "Pack the files below --input_dir into --output.")]
    public bool Pack
    {
        get; set;
    }

    [Option("output", HelpText = "Output .g file for --pack.")]
    public string? OutputFilePath
    {
        get; set;
    }

    [Option('r', "recursive", HelpText = "Read all .g archives below --input_dir.")]
    public bool Recursive
    {
        get; set;
    }

    [Option('i', "input", HelpText = "Input .g archive.")]
    public string? InputFilePath
    {
        get; set;
    }

    [Option("input_dir", HelpText = "Archive directory for extraction, compilation or packing.")]
    public string? InputDirectoryPath
    {
        get; set;
    }

    [Option("overlay", HelpText = "Overlay archive directory for indexed assets, e.g. content_dlc.")]
    public string? OverlayDirectoryPath
    {
        get; set;
    }

    [Option("lua-source", HelpText = "Decompile LuaJIT only when exporting Lua source (requires --assets).")]
    public bool LuaSource
    {
        get; set;
    }

    [Option("decompiler", HelpText = "Optional path to eastward-luajit.dll, libeastward-luajit.so or .dylib.")]
    public string? DecompilerPath
    {
        get; set;
    }

    [Option('o', "output_dir", HelpText = "Extraction or compilation output directory.")]
    public string? OutputDirectoryPath
    {
        get; set;
    }
}
