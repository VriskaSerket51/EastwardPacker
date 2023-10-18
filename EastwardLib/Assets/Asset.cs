using EastwardLib.MetaData;

namespace EastwardLib.Assets;

public class Asset
{
    public static Asset Create(string name, byte[] data)
    {
        var assetInfo = AssetIndex.Instance.SearchAssetInfo(name);
        if (!assetInfo.HasValue)
        {
            throw new Exception($"AssetIndex doesn't contain the asset info: {name}");
        }

        switch (assetInfo.Value.ObjectFiles.First(o => o.Value == name).Key)
        {
            case "pixmap":
                return Create<HmgAsset>(data);
            case "packed_def":
            case "compiled":
                return Create<PackedAsset>(data);
            default:
                return Create<TextAsset>(data);
        }
    }

    public static T Create<T>(byte[] data) where T : Asset, new()
    {
        using MemoryStream ms = new MemoryStream(data);

        T asset = new T();
        asset.Decode(ms);

        return asset;
    }

    public virtual byte[] Encode()
    {
        throw new Exception();
    }

    public virtual void Decode(MemoryStream ms)
    {
        throw new Exception();
    }

    protected void PrepareDirectory(string path)
    {
        string? directory = Path.GetDirectoryName(path);
        if (directory == null)
        {
            throw new Exception();
        }

        Directory.CreateDirectory(directory);
    }

    public virtual void SaveTo(string path)
    {
    }
}