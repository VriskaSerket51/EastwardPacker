using Newtonsoft.Json;

namespace EastwardLib;

public class AssetIndex
{
    public Dictionary<string, Asset> assets;

    public static AssetIndex Create(string path)
    {
        string json = File.ReadAllText(path);
        AssetIndex assetIndex = new()
        {
            assets = JsonConvert.DeserializeObject<Dictionary<string, Asset>>(json) ?? throw new Exception()
        };
        return assetIndex;
    }

    public void Print()
    {
        foreach (var (name, asset) in assets)
        {
            Console.WriteLine($"{name}: {asset.filePath}");
        }
    }
}