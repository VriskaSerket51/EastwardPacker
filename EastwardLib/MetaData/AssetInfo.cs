namespace EastwardLib.MetaData;

public struct AssetInfo
{
    public string AssetName;
    public string FilePath;
    public readonly Dictionary<string, string> ObjectFiles;
    public string Type;

    public AssetInfo(string assetName, string filePath, Dictionary<string, string> objectFiles, string type)
    {
        AssetName = assetName;
        FilePath = filePath;
        ObjectFiles = objectFiles;
        Type = type;
    }
}