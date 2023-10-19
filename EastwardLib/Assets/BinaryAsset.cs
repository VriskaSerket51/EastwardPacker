namespace EastwardLib.Assets;

public class BinaryAsset : Asset
{
    private byte[]? _data;

    public BinaryAsset()
    {
    }

    public BinaryAsset(byte[] data)
    {
        _data = data;
    }

    public override byte[] Encode()
    {
        if (_data == null)
        {
            throw new InvalidOperationException("You should Encode() after Decode()");
        }

        return _data;
    }

    public override void Decode(Stream s)
    {
        using BinaryReader br = new BinaryReader(s);
        _data = br.ReadBytes((int)s.Length);
    }

    public override void SaveTo(string path)
    {
        if (_data == null)
        {
            return;
        }

        PrepareDirectory(path);

        File.WriteAllBytes(path, _data);
    }
}