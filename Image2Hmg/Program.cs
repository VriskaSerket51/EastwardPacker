using System.Drawing;
using EastwardLib.Assets;

foreach (var arg in args)
{
    try
    {
        if (!File.Exists(arg))
        {
            continue;
        }

        var asset = new HmgAsset(Image.FromFile(arg));
        var data = asset.Encode();
        File.WriteAllBytes(arg + ".hmg", data);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Exception from {arg}, message: {e.Message}");
    }
}