using SimpleJSON;

namespace EastwardLib.MetaData;

public class ScriptLibrary : Dictionary<string, string>
{
    private ScriptLibrary(int capacity = 100) : base(capacity)
    {
    }

    public static ScriptLibrary Create(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException();
        }


        string json = File.ReadAllText(path);
        var root = JSONNode.Parse(json);
        ScriptLibrary scriptLibrary = new ScriptLibrary(root.Count);

        var exportNode = root["export"];
        var sourceNode = root["source"];

        foreach (var (key, value) in exportNode)
        {
            string src = sourceNode[key].Value;
            scriptLibrary[src] = value.Value;
        }

        return scriptLibrary;
    }
}