using EastwardLib.Assets;

foreach (var arg in args)
{
    try
    {
        using var stream = File.OpenRead(arg);
        Asset.Create<HmgAsset>(stream).SaveTo(arg + ".png");
    }
    catch (Exception e)
    {
        Console.Error.WriteLine($"{arg}: {e.Message}");
        Environment.ExitCode = 1;
    }
}
