using ZstdSharp;

namespace EastwardLib;

public class GArchive
{
    private Dictionary<string, Asset> _assets;
    
    public GArchive(string path) : this(File.OpenRead(path))
    {
    }

    public GArchive(byte[] data) : this(new MemoryStream(data))
    {
    }

    public GArchive(Stream stream)
    {
        using BinaryReader br = new BinaryReader(stream);
        if (br.ReadInt32() != 27191)
        {
            throw new Exception();
        }

        int length = br.ReadInt32();
        _assets = new Dictionary<string, Asset>(length);
        for (int i = 0; i < length; i++)
        {
            string name = br.ReadNullTerminatedString();
            int offset = br.ReadInt32();
            bool isCompressed = br.ReadInt32() == 2;
            int decompressedSize = br.ReadInt32();
            int compressedSize = br.ReadInt32();

            long position = br.BaseStream.Position;
            br.BaseStream.Position = offset;
            byte[] data = br.ReadBytes(compressedSize).RemovePadding();

            if (isCompressed)
            {
                using var decompressor = new Decompressor();
                data = decompressor.Unwrap(data, decompressedSize).ToArray();
            }

            br.BaseStream.Position = position;
            
            _assets.Add(name, Asset.Create(name, data));
        }
        stream.Dispose();
    }

    public void SaveTo(string path)
    {
        
    }
}