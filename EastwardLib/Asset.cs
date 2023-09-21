namespace EastwardLib;

public class Asset : IDisposable
{
    private byte[] raw;
    protected MemoryStream ms;

    public Asset(byte[] data)
    {
        raw = data;
        ms = new MemoryStream(raw);
    }

    public static Asset Create(string name, byte[] data)
    {
        Asset asset;
        using MemoryStream ms = new MemoryStream(data);
        using BinaryReader br = new BinaryReader(ms);

        if (new string(br.ReadChars(3)) == PgfAsset.MAGIC_HEADER)
        {
            asset = new PgfAsset(data);
        }
        else if (name.EndsWith(".packed"))
        {
            asset = new PackedAsset(data);
        }
        else
        {
            asset = new TextAsset(data);
        }

        asset.Decode();

        return asset;
    }

    public virtual byte[] Encode()
    {
        throw new NotImplementedException();
    }

    protected virtual void Decode()
    {
        throw new NotImplementedException();
    }

    public virtual void SaveTo(string path, bool revealExtension = false)
    {
        string? directory = Path.GetDirectoryName(path);
        if (directory == null)
        {
            throw new Exception();
        }

        Directory.CreateDirectory(directory);
    }

    public virtual void Dispose()
    {
        ms.Dispose();
    }
}