EastwardPacker
=============

English | [한국어](README.ko.md)

Unofficial command-line tools for extracting and repacking Eastward archives and assets.

## eastward.js

A [JavaScript version](https://github.com/VriskaSerket51/eastward.js) is also available!

## How to build

Requires .NET 10. Clone with submodules:

```sh
git clone --recurse-submodules https://github.com/VriskaSerket51/EastwardPacker.git
cd EastwardPacker
dotnet build EastwardPacker.sln -c Release
```

## How to use

### EastwardPacker

Extract `.g` archives and repack edited files.

- Extract a single archive

```sh
dotnet run --project EastwardPacker -- -i /path/to/locale.g -o ./raw
```

- Extract multiple archives

```sh
dotnet run --project EastwardPacker -- --recursive --input_dir /path/to/content/game -o ./raw
```

- Create an archive

```sh
dotnet run --project EastwardPacker -- --pack --input_dir ./raw/locale --output ./modified/locale.g
```

Files are extracted to `raw/<archive name>/...`. Use the directory inside it as the input when repacking.

### Asset extraction

Extract assets using the names and paths recorded in the index:

```sh
dotnet run --project EastwardPacker -- --assets --config /path/to/content/game/config.g --input_dir /path/to/content/game --overlay /path/to/content_dlc -o ./assets
```

Select a type with options such as `--type texture`. To convert LuaJIT bytecode to Lua code, add `--lua-source --decompiler /path/to/native-library`. The result may differ from the original source.

### Exporting editable files

```sh
dotnet run --project EastwardPacker -- --project --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./restored --decompiler /path/to/native-library
```

Exported files are saved in `game/asset`. Original project settings and editing information absent from the shipped game cannot be restored.

### Source compilation

```sh
dotnet run --project EastwardPacker -- --compile --asset script/example.sq --source ./edited/example.sq --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./compiled
```

Use `--project` instead of `--asset` for an entire exported file tree. Keep `.eastward-project.json` in the project root. Use `--overlay` to select a directory containing installed DLC archives. Write the output to a new directory outside the game.

## Tools

### AtlasExtractor

Extract images from an atlas:

```sh
dotnet run --project AtlasExtractor -- /path/to/atlas.json
```

### Hmg2Image / Image2Hmg

Convert HMG to PNG, or PNG/JPEG to HMG:

```sh
dotnet run --project Hmg2Image -- texture.hmg
dotnet run --project Image2Hmg -- texture.png
```

See [EastwardLib](https://github.com/VriskaSerket51/EastwardLib/blob/main/README.md) for library usage, or [EastwardModTool](https://github.com/VriskaSerket51/EastwardModTool) to create and install `.ewmod` packages.
