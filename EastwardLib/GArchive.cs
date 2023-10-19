using EastwardLib.Assets;
using EastwardLib.MetaData;
using ZstdSharp;

namespace EastwardLib;

public class GArchive : Dictionary<string, Asset>, IDisposable
{
    private const int MagicHeader = 27191;

    private readonly string _archiveName;

    private GArchive(string name, int capacity = 100) : base(capacity)
    {
        _archiveName = name;
    }

    public static GArchive Read(string path)
    {
        string archiveName = Path.GetFileNameWithoutExtension(path);
        return Read(archiveName, File.OpenRead(path));
    }

    public static GArchive Read(string archiveName, byte[] data)
    {
        return Read(archiveName, new MemoryStream(data));
    }

    public static GArchive Read(string archiveName, Stream s)
    {
        using BinaryReader br = new BinaryReader(s);
        if (br.ReadInt32() != MagicHeader) // Yeah Magic Number
        {
            throw new Exception();
        }

        int length = br.ReadInt32();
        GArchive g = new GArchive(archiveName, length);
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
                data = decompressor.Unwrap(data).ToArray();
            }

            br.BaseStream.Position = position;

            bool isPackage = name.Contains('/');
            string fullName = $"{archiveName}/{name}";
            g.Add(fullName, Asset.Create(fullName, data, isPackage));
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
        bw.Write(MagicHeader);
        bw.Write(Count);
        int baseOffset = CalculateOffset();
        foreach (var (name, asset) in this)
        {
            bw.WriteNullTerminatedString(name.Substring(_archiveName.Length + 1));
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

    public void ExtractTo(string path, string fallbackPath)
    {
        if (!Directory.Exists(path))
        {
            throw new Exception();
        }

        AssetIndex assetIndex = AssetIndex.Instance;

        foreach (var (name, asset) in this)
        {
            if (asset is FallbackAsset)
            {
                if (!string.IsNullOrEmpty(fallbackPath))
                {
                    asset.SaveTo(Path.Combine(fallbackPath, name));
                }

                continue;
            }

            string originName = name.Substring(_archiveName.Length + 1);
            bool isPackage = originName.Contains('/');

            if (isPackage)
            {
                var split = originName.Split('/');
                string containerName = $"{_archiveName}/{split[0]}";
                AssetInfo? assetInfo = assetIndex.SearchAssetInfo(containerName);
                if (assetInfo == null)
                {
                    Console.WriteLine($"{containerName} has null asset info");
                    continue;
                }

                if (assetInfo.Value.FileType == "f")
                {
                    // WTF?!
                    continue;
                }

                asset.SaveTo(Path.Combine(path, assetInfo.Value.FilePath,
                    originName.Substring(split[0].Length + 1)));
            }
            else
            {
                AssetInfo? assetInfo = assetIndex.SearchAssetInfo(name);
                if (assetInfo == null)
                {
                    Console.WriteLine($"{name} has null asset info");
                    continue;
                }

                string assetName = assetInfo.Value.AssetName;
                string filePath = assetInfo.Value.FilePath;
                string fileType = assetInfo.Value.FileType;
                var objectFiles = assetInfo.Value.ObjectFiles;
                string type = assetInfo.Value.Type;

                if (fileType == "d")
                {
                    string key = objectFiles.First(o => o.Value == name).Key;
                    asset.SaveTo(Path.Combine(path, filePath, key));
                }
                else if (fileType == "v")
                {
                    if (type == "texture")
                    {
                        string mSpriteName = assetName.Substring(0, assetName.Length - 9);
                        var mSpriteInfo = assetIndex.GetAssetInfoByName(mSpriteName);
                        if (mSpriteInfo == null)
                        {
                            continue;
                        }

                        asset.SaveTo(Path.Combine(path, Path.GetDirectoryName(mSpriteInfo.Value.FilePath)!,
                            assetName + ".png"));
                    }
                }
                else if (fileType == "f")
                {
                    asset.SaveTo(Path.Combine(path, filePath));
                }
            }
        }
    }

    public void Dispose()
    {
        Clear();
    }
}