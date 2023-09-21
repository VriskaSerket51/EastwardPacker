using Newtonsoft.Json;

namespace EastwardLib;

public class AssetIndex
{
    private Dictionary<string, AssetInfo> assets;

    public AssetIndex(string path)
    {
        string json = File.ReadAllText(path);
        assets = JsonConvert.DeserializeObject<Dictionary<string, AssetInfo>>(json) ?? throw new Exception();
    }

    public void Print()
    {
        foreach (var (name, asset) in assets)
        {
            // Console.WriteLine($"{name}: {asset.filePath}");
        }
    }
}