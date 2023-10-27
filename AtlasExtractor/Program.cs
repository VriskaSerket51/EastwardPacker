using System.Drawing;
using System.Drawing.Drawing2D;
using EastwardLib.Assets;
using SimpleJSON;


foreach (var arg in args)
{
    var dir = Path.GetDirectoryName(arg);
    if (dir == null)
    {
        continue;
    }

    var json = File.ReadAllText(arg);
    var atlasNode = JSONNode.Parse(json);
    var atlasesNode = atlasNode["atlases"].AsArray;
    Dictionary<int, Image> atlases = new Dictionary<int, Image>(atlasesNode.Count);
    for (var i = 0; i < atlasesNode.Count; i++)
    {
        var value = atlasesNode[i];
        string name = value["name"].Value;
        try
        {
            var s = File.OpenRead(Path.Combine(dir, name));
            var asset = Asset.Create<HmgAsset>(s);
            asset.SaveTo(Path.Combine(dir, name));
            atlases.Add(i, Image.FromFile(Path.Combine(dir, name)));
        }
        catch
        {
            atlases.Add(i, Image.FromFile(Path.Combine(dir, name)));
        }
    }

    var itemsNode = atlasNode["items"].AsArray;
    foreach (var (_, value) in itemsNode)
    {
        string name = value["name"].Value;
        string path = Path.Combine(dir, name);
        string? dirPath = Path.GetDirectoryName(path);
        if (dirPath == null)
        {
            continue;
        }

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        int atlasIdx = value["atlas"].AsInt;
        var rectNode = value["rect"];
        Rectangle rect = new Rectangle(rectNode[0].AsInt, rectNode[1].AsInt, rectNode[2].AsInt, rectNode[3].AsInt);
        using var bitmap = new Bitmap(rect.Width, rect.Height);
        using var g = Graphics.FromImage(bitmap);
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.DrawImage(atlases[atlasIdx], rect with { X = 0, Y = 0 }, rect, GraphicsUnit.Pixel);
        bitmap.Save(path);
    }
}