using CommandLine;
using EastwardLib;
using EastwardLib.MetaData;
using EastwardPacker;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(OnParsed);

void OnParsed(Options o)
{
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
            g.ExtractTo(o.OutputDirectoryPath);
        }
    }
    else
    {
        if (o.InputFilePath == null)
        {
            Console.WriteLine("'input' should not be null on recursive mode!!!");
            return;
        }

        var g = GArchive.Read(o.InputFilePath);
        g.ExtractTo(o.OutputDirectoryPath);
    }
}