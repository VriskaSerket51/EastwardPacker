using EastwardLib.Assets;

foreach (var arg in args)
{
    try
    {
        File.WriteAllBytes(arg + ".hmg", HmgAsset.FromImage(arg).Encode());
    }
    catch (Exception e)
    {
        Console.Error.WriteLine($"{arg}: {e.Message}");
        Environment.ExitCode = 1;
    }
}
