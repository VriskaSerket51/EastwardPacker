namespace EastwardLib;

public class BinaryAsset : Asset
{
    protected BinaryReader br;
    
    public BinaryAsset(byte[] data) : base(data)
    {
        br = new BinaryReader(ms);
    }

    public override void Dispose()
    {
        base.Dispose();
        br.Dispose();
    }
}