using SimpleJSON;

namespace EastwardLib.MetaData;

public class AssetIndex : Dictionary<string, AssetInfo>
{
    private static AssetIndex? _instance;

    private AssetIndex(int capacity = 100) : base(capacity)
    {
    }

    public static AssetIndex Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new Exception("AssetIndex not loaded");
            }

            return _instance;
        }
    }

    public static AssetIndex Create(string path)
    {
        if (_instance != null)
        {
            throw new Exception("AssetIndex already loaded");
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }


        string json = File.ReadAllText(path);
        var root = JSONNode.Parse(json);
        AssetIndex assetIndex = new AssetIndex(root.Count);

        foreach (var (key, value) in root)
        {
            string assetName = key;
            string filePath = value["filePath"].IsString ? value["filePath"].Value : string.Empty;
            string fileType = value["fileType"].Value;
            string type = value["type"].Value;
            var objectFilesNode = value["objectFiles"];
            if (objectFilesNode.Count == 0)
            {
                continue;
            }

            var objectFiles = new Dictionary<string, string>(objectFilesNode.Count);
            foreach (var (t, v) in objectFilesNode)
            {
                objectFiles.Add(t, v.Value);
            }

            var assetInfo = new AssetInfo(Path.GetFileName(assetName), filePath, fileType, objectFiles, type);
            foreach (var v in objectFiles.Values)
            {
                assetIndex.Add(v, assetInfo);
            }
        }

        return _instance = assetIndex;
    }

    public AssetInfo? SearchAssetInfo(string key)
    {
        if (!ContainsKey(key))
        {
            return null;
        }

        return this[key];
    }

    public AssetInfo? GetAssetInfoByName(string name)
    {
        foreach (var (_, value) in this)
        {
            if (value.AssetName == name)
            {
                return value;
            }
        }

        return null;
    }
}