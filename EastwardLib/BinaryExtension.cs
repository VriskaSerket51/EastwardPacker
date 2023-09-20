using System.Text;

namespace EastwardLib;

public static class BinaryExtension
{
    public static string ReadNullTerminatedString(this BinaryReader br)
    {
        StringBuilder sb = new StringBuilder(10);
        char c;
        while ((c = br.ReadChar()) != 0)
        {
            sb.Append(c);
        }

        return sb.ToString();
    }
    
    public static void WriteNullTerminatedString(this BinaryWriter bw, string s)
    {
        foreach (var c in s.ToCharArray())
        {
            bw.Write(c);
        }
        bw.Write('\0');
    }

    public static byte[] RemovePadding(this byte[] data)
    {
        int length = data.Length;
        for (int i = data.Length - 1; i >= 0; i--)
        {
            if (data[i] != 0)
            {
                break;
            }

            length--;
        }

        return data.Take(length).ToArray();
    }
}