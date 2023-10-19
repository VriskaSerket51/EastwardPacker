using CommandLine;

namespace EastwardPacker;

public class Options
{
    [Option("index", Required = true, HelpText = "Set AssetIndex path.")]
    public required string AssetIndexPath { get; set; }
    
    [Option("script_lib", Required = true, HelpText = "Set ScriptLibrary path.")]
    public required string ScriptLibraryPath { get; set; }

    [Option("r", Required = false, HelpText = "Whether use recursive mode or not.", Default = false)]
    public bool Recursive { get; set; }

    [Option('i', "input", Required = false, HelpText = "Set Input file path.")]
    public string? InputFilePath { get; set; }

    [Option("input_dir", Required = false, HelpText = "Set Input directory path (recursive mode only).")]
    public string? InputDirectoryPath { get; set; }

    [Option('o', "output_dir", Required = true, HelpText = "Set Output directory path.")]
    public required string OutputDirectoryPath { get; set; }

    [Option("fallback_dir", Required = false, HelpText = "Set Fallback directory path.")]
    public string? FallbackDirectoryPath { get; set; }
}