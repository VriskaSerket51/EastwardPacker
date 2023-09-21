using MessagePack;

namespace EastwardLib;

public class PackedAsset : Asset
{
    private object obj;

    public PackedAsset(byte[] data) : base(data)
    {
    }

    public override byte[] Encode()
    {
        return MessagePackSerializer.Serialize(obj);
    }

    protected override void Decode()
    {
        obj = MessagePackSerializer.Deserialize<object>(ms);
    }

    public override void SaveTo(string path, bool revealExtension = false)
    {
        base.SaveTo(path, revealExtension);
        if (revealExtension)
        {
            path += ".json";
        }

        var json = MessagePackSerializer.SerializeToJson(obj);
        File.WriteAllText(path, json);
    }
}