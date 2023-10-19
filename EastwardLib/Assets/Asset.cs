using EastwardLib.MetaData;

namespace EastwardLib.Assets;

public class Asset
{
    public static Asset Create(string name, byte[] data, bool isPackage = false)
    {
        string originName = name;
        if (isPackage)
        {
            var split = name.Split('/');
            name = $"{split[0]}/{split[1]}";
        }

        var assetInfo = AssetIndex.Instance.SearchAssetInfo(name);
        if (!assetInfo.HasValue)
        {
            return Create<FallbackAsset>(data);
        }

        if (isPackage)
        {
            var extension = Path.GetExtension(originName);
            if (extension == ".hmg")
            {
                return Create<HmgAsset>(data);
            }

            if (extension == ".json")
            {
                return Create<TextAsset>(data);
            }

            // if (!string.IsNullOrEmpty(extension))
            // {
            //     Console.WriteLine($"Warning: unhandled extension: {name}");
            // }

            return Create<BinaryAsset>(data);
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