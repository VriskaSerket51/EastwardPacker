EastwardPacker
=============

English | [한국어](README.ko.md)

Unofficial tools for extracting and repacking Eastward assets.

A [JavaScript version](https://github.com/VriskaSerket51/eastward.js) is also available.

## How to build

.NET 10 required. Clone with submodules:

```sh
git clone --recurse-submodules https://github.com/VriskaSerket51/EastwardPacker.git
cd EastwardPacker
dotnet build EastwardPacker.sln -c Release
```

## How to use

### Extract archives

```sh
dotnet run --project EastwardPacker -- -i /path/to/locale.g -o ./raw
dotnet run --project EastwardPacker -- --recursive --input_dir /path/to/content/game -o ./raw
```

Files go to `raw/<archive name>/`.

### Pack an archive

```sh
dotnet run --project EastwardPacker -- --pack --input_dir ./raw/locale --output ./modified/locale.g
```

### Extract assets

```sh
dotnet run --project EastwardPacker -- --assets --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./assets
```

Use `--type texture` to select a type. Add `--overlay /path/to/content_dlc` to read installed DLC archives.

### Export editable files

```sh
dotnet run --project EastwardPacker -- --project --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./restored --decompiler /path/to/native-library
```

Files go to `game/asset`. Keep `.eastward-project.json`. Project settings and editing data missing from the game cannot be recovered.

### Compile sources

```sh
dotnet run --project EastwardPacker -- --compile --asset script/example.sq --source ./edited/example.sq --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./compiled
```

Use `--project` instead of `--asset` for a whole exported project. Write output outside the game directory.

Lua source export needs `--lua-source --decompiler /path/to/native-library`. Decompiled code may differ from the original source. See [EastwardLib](https://github.com/VriskaSerket51/EastwardLib#lua-source-export) for the native build.

## Other tools

```sh
dotnet run --project AtlasExtractor -- /path/to/atlas.json
dotnet run --project Hmg2Image -- texture.hmg
dotnet run --project Image2Hmg -- texture.png
```

- AtlasExtractor: extract atlas images
- Hmg2Image: convert HMG to PNG
- Image2Hmg: convert PNG/JPEG to HMG

For a GUI, see [EastwardModTool](https://github.com/VriskaSerket51/EastwardModTool).
