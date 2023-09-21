using System.Text;

namespace EastwardLib;

public class TextAsset : Asset
{
    private StreamReader sr;
    private string text;

    public TextAsset(byte[] data) : base(data)
    {
        sr = new StreamReader(ms);
    }

    public override byte[] Encode()
    {
        return Encoding.UTF8.GetBytes(text);
    }

    protected override void Decode()
    {
        text = sr.ReadToEnd();
    }

    public override void SaveTo(string path, bool revealExtension = false)
    {
        base.SaveTo(path, revealExtension);
        if (revealExtension)
        {
            path += ".txt";
        }

        File.WriteAllText(path, text);
    }

    public override void Dispose()
    {
        base.Dispose();
        sr.Dispose();
    }
}