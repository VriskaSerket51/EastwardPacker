using EastwardLib.Assets;

foreach (var arg in args)
{
    try
    {
        var s = File.OpenRead(arg);
        HmgAsset asset = new HmgAsset();
        asset.Decode(s);
        asset.SaveTo(arg);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Exception from {arg}, message: {e.Message}");
    }
}