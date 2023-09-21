using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using K4os.Compression.LZ4;

namespace EastwardLib;

public class PgfAsset : BinaryAsset
{
    public const string MAGIC_HEADER = "PGF";

    private byte[] data;
    private int width;
    private int height;
    private byte bpp;

    public PgfAsset(byte[] data) : base(data)
    {
    }

    public override byte[] Encode()
    {
        using MemoryStream ms = new MemoryStream(data);
        using BinaryWriter bw = new BinaryWriter(ms);
        byte[] compressedData = new byte[LZ4Codec.MaximumOutputSize(data.Length)];
        int length = LZ4Codec.Encode(data, compressedData);
        bw.Write(Encoding.UTF8.GetBytes(MAGIC_HEADER));
        bw.Write((byte)2);
        bw.Write(GetFileSize(length));
        bw.Write((ushort)24984);
        bw.Write(data.Length);
        bw.Write(width);
        bw.Write(height);
        bw.Write(bpp);
        bw.Write((byte)1);
        bw.Write((byte)2);
        bw.Write((byte)0);
        bw.Write((ushort)48297);
        bw.Write(compressedData.Take(length).ToArray());
        return ms.ToArray();
    }

    protected override void Decode()
    {
        if (new string(br.ReadChars(3)) != MAGIC_HEADER)
        {
            throw new Exception();
        }

        int unkLen1 = br.ReadByte();
        br.ReadInt32(); // File Size
        br.ReadBytes(unkLen1); // Unknown Chunk 1

        int compressedSize = br.ReadInt32();
        width = br.ReadInt32();
        height = br.ReadInt32();
        bpp = br.ReadByte();
        if (bpp != 32)
        {
            throw new Exception();
        }

        br.ReadByte(); // Unknown Bit 1
        byte unkLen2 = br.ReadByte();
        br.ReadByte(); // Unknown Bit 1
        br.ReadBytes(unkLen2); // Unknown Chunk 2

        byte[] compressedData = br.ReadBytes(compressedSize);
        data = new byte[width * height * bpp / 8];
        LZ4Codec.Decode(compressedData, data);
    }

    private int GetFileSize(int compressedSize)
    {
        int result = 0;
        result += 3; // Magic Header
        result += 1;
        result += 4; // File Size
        result += 2;
        result += 4; // Compressed Size
        result += 4; // Width
        result += 4; // Height
        result += 1;
        result += 1;
        result += 1;
        result += 1;
        result += 2;
        result += compressedSize;
        return result;
    }

    public override void SaveTo(string path, bool revealExtension = false)
    {
        base.SaveTo(path, revealExtension);
        if (revealExtension)
        {
            path += ".png";
        }

        unsafe
        {
            fixed (byte* ptr = data)
            {
                using Bitmap image = new Bitmap(width, height, width * bpp / 8, PixelFormat.Format32bppRgb,
                    new IntPtr(ptr));
                image.Save(path, ImageFormat.Png);
            }
        }
    }
}