using System.Drawing;
using EastwardLib.Assets;

foreach (var arg in args)
{
    if (!File.Exists(arg))
    {
        continue;
    }

    var asset = new HmgAsset(Image.FromFile(arg));
    var data = asset.Encode();
    File.WriteAllBytes(arg + ".encoded", data);
}