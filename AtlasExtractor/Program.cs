using System.Text.Json.Nodes;
using EastwardLib.Assets;
using EastwardLib.Utils;

foreach (var arg in args)
{
    try
    {
        var path = Path.GetFullPath(arg);
        var directory = Path.GetDirectoryName(path)!;
        var json = File.ReadAllText(path);
        var definition = JsonNode.Parse(json) ?? throw new InvalidDataException("Missing atlas index");
        var files = new Dictionary<string, Asset>(StringComparer.Ordinal) { ["atlas.json"] = new TextAsset(json) };
        foreach (var atlas in definition["atlases"]!.AsArray())
        {
            var name = atlas!["name"]!.GetValue<string>();
            var bytes = File.ReadAllBytes(SafePath.ResolveWithin(directory, name));
            files[name] = bytes.AsSpan().StartsWith("PGF"u8) ? Asset.Create<HmgAsset>(bytes) : new BinaryAsset(bytes);
        }
        new AtlasAsset(files).SaveTo(Path.Combine(directory, Path.GetFileNameWithoutExtension(path) + "-extracted"));
    }
    catch (Exception e)
    {
        Console.Error.WriteLine($"{arg}: {e.Message}");
        Environment.ExitCode = 1;
    }
}
