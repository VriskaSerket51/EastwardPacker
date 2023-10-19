namespace EastwardLib.MetaData;

public struct AssetInfo
{
    public string AssetName;
    public string FilePath;
    public string FileType;
    public readonly Dictionary<string, string> ObjectFiles;
    public string Type;

    public AssetInfo(string assetName, string filePath, string fileType, Dictionary<string, string> objectFiles,
        string type)
    {
        AssetName = assetName;
        FilePath = filePath;
        FileType = fileType;
        ObjectFiles = objectFiles;
        Type = type;
    }
}