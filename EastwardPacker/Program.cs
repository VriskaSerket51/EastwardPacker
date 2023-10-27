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
    var scriptLibrary = ScriptLibrary.Create(o.ScriptLibraryPath);
    AssetIndex.Create(o.AssetIndexPath, scriptLibrary);

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
            g.ExtractTo(o.OutputDirectoryPath, o.FallbackDirectoryPath);
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
        g.ExtractTo(o.OutputDirectoryPath, o.FallbackDirectoryPath);
    }
}