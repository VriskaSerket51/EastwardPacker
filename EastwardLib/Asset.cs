namespace EastwardLib;

public class Asset
{
    public Dictionary<string, string> dependency;
    // public Dictionary<string, string> deployMeta;
    public string filePath;
    // public double fileTime;
    public string fileType;
    public string groupType;
    // public object managerVersion;
    public Dictionary<string, string> objectFiles;
    // public Dictionary<string, object> properties;
    // public string[] tags;
    public string type;

    public Asset(byte[] data)
    {
    }

    public static Asset Create(string name, byte[] data)
    {
        Asset asset = null;
        using MemoryStream ms = new MemoryStream(data);
        using BinaryReader br = new BinaryReader(ms);

        if (new string(br.ReadChars(3)) == "PGF")
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

        return asset;
    }

    protected virtual void Encode()
    {
        
    }

    protected virtual void Decode()
    {
        
    }

    protected virtual void SaveTo(string path)
    {
        
    }
}