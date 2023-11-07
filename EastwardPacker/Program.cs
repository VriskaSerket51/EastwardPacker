using CommandLine;
using EastwardLib;
using EastwardLib.MetaData;
using EastwardPacker;

#if DEBUG
Test.DoTest();
#else
Parser.Default.ParseArguments<Options>(args)
    .WithParsed(OnParsed);
#endif

void OnParsed(Options o)
{
    ScriptLibrary.Create(o.ScriptLibraryPath);
    AssetIndex.Create(o.AssetIndexPath);

    if (o.Recursive)
    {
        if (o.InputDirectoryPath == null)
        {
            Console.WriteLine("'input_dir' should not be null on recursive mode!!!");
            return;
        }

        foreach (var file in Directory.GetFiles(o.InputDirectoryPath, "*.*", SearchOption.AllDirectories))
        {
            if (!file.EndsWith(".g"))
            {
                continue;
            }

            var g = GArchive.Read(file);
            g.ExtractTo(Path.Combine(o.OutputDirectoryPath, Path.GetFileName(file)));
        }
    }
    else
    {
        if (o.InputFilePath == null)
        {
            Console.WriteLine("'input' should not be null!!!");
            return;
        }

        var g = GArchive.Read(o.InputFilePath);
        g.ExtractTo(Path.Combine(o.OutputDirectoryPath, Path.GetFileName(o.InputFilePath)));
    }
}