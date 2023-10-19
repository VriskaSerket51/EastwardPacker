using MessagePack;

namespace EastwardLib.Assets;

public class PackedAsset : Asset
{
    private object? _obj;

    public PackedAsset()
    {
    }

    public PackedAsset(object obj)
    {
        _obj = obj;
    }

    public override byte[] Encode()
    {
        return MessagePackSerializer.Serialize(_obj);
    }

    public override void Decode(Stream s)
    {
        _obj = MessagePackSerializer.Deserialize<object>(s);
    }

    public override void SaveTo(string path)
    {
        PrepareDirectory(path);

        var json = MessagePackSerializer.SerializeToJson(_obj);
        File.WriteAllText(path, json);
    }
}