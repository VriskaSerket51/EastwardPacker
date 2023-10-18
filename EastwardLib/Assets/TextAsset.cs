using System.Text;

namespace EastwardLib.Assets;

public class TextAsset : Asset
{
    private string? _text;

    public TextAsset()
    {
    }

    public TextAsset(string text)
    {
        _text = text;
    }

    public override byte[] Encode()
    {
        if (_text == null)
        {
            throw new Exception("Text Data is NULL!!!");
        }

        return Encoding.UTF8.GetBytes(_text);
    }

    public override void Decode(MemoryStream ms)
    {
        using StreamReader sr = new StreamReader(ms);
        _text = sr.ReadToEnd();
    }

    public override void SaveTo(string path)
    {
        PrepareDirectory(path);

        File.WriteAllText(path, _text);
    }
}