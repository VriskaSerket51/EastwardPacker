﻿using ZstdSharp;

namespace EastwardLib;

public class GArchive : Dictionary<string, Asset>, IDisposable
{
    private const int MAGIC_HEADER = 27191;

    public GArchive(int capacity) : base(capacity)
    {
    }

    public static GArchive Read(string path)
    {
        return Read(File.OpenRead(path));
    }

    public static GArchive Read(byte[] data)
    {
        return Read(new MemoryStream(data));
    }

    public static GArchive Read(Stream s)
    {
        using BinaryReader br = new BinaryReader(s);
        if (br.ReadInt32() != MAGIC_HEADER) // Yeah Magic Number
        {
            throw new Exception();
        }

        int length = br.ReadInt32();
        GArchive g = new GArchive(length);
        for (int i = 0; i < length; i++)
        {
            string name = br.ReadNullTerminatedString();
            int offset = br.ReadInt32();
            bool isCompressed = br.ReadInt32() == 2;
            int decompressedSize = br.ReadInt32();
            int compressedSize = br.ReadInt32();

            long position = br.BaseStream.Position;
            br.BaseStream.Position = offset;
            byte[] data = br.ReadBytes(compressedSize).Take(Compressor.GetCompressBound(decompressedSize)).ToArray();

            if (isCompressed)
            {
                using var decompressor = new Decompressor();
                File.WriteAllBytes(@"C:\Users\i_am_\Desktop\fnt\test\tt", data);
                data = decompressor.Unwrap(data).ToArray();
            }

            br.BaseStream.Position = position;

            g.Add(name, Asset.Create(name, data));
        }

        s.Dispose();

        return g;
    }

    private int CalculateOffset()
    {
        int result = 0;
        result += 4; // Magic Header
        result += 4; // Length
        foreach (var name in Keys)
        {
            result += name.Length + 1; // Including NULL Byte
            result += 4; // Offset
            result += 4; // Is Compressed?
            result += 4; // Decompressed Size
            result += 4; // Compressed Size
        }

        return result;
    }

    public void Write(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (directory == null)
        {
            throw new Exception();
        }

        Directory.CreateDirectory(directory);

        using var fs = File.OpenWrite(path);
        Write(fs);
    }

    public void Write(Stream s)
    {
        using BinaryWriter bw = new BinaryWriter(s);
        bw.Write(MAGIC_HEADER);
        bw.Write(Count);
        int baseOffset = CalculateOffset();
        foreach (var (name, asset) in this)
        {
            bw.WriteNullTerminatedString(name);
            bw.Write(baseOffset);
            bw.Write(2);
            Compressor compressor = new Compressor();
            var data = asset.Encode();
            var compressed = compressor.Wrap(data);
            bw.Write(data.Length);
            bw.Write(compressed.Length);
            long offset = bw.BaseStream.Position;
            bw.BaseStream.Position = baseOffset;
            bw.Write(compressed);
            baseOffset += compressed.Length;
            bw.BaseStream.Position = offset;
        }
    }

    public void ExtractTo(string path, bool revealExtension = false)
    {
        if (!Directory.Exists(path))
        {
            throw new Exception();
        }

        foreach (var (name, asset) in this)
        {
            asset.SaveTo(Path.Combine(path, name), revealExtension);
        }
    }

    public void Dispose()
    {
        foreach (var asset in Values)
        {
            asset.Dispose();
        }

        Clear();
    }
}