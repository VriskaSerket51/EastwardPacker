EastwardPacker
=============

## eastward.js

You can also get [Javascript version of Eastward Packer](https://github.com/VriskaSerket51/eastward.js)!

## Download

All releases can be found at [here](https://github.com/VriskaSerket51/EastwardPacker/releases).

## How to build

You need .NET 10 to build these tools. Clone with submodules.

```sh
git clone --recurse-submodules https://github.com/VriskaSerket51/EastwardPacker.git
dotnet build EastwardPacker.sln -c Release
```

## How to use

### EastwardPacker

You can extract files in ".g" archives, and pack modified files back into an archive.

- Single file extraction

```sh
dotnet run --project EastwardPacker -- -i /path/to/locale.g -o ./raw
```

- Multiple files extraction

```sh
dotnet run --project EastwardPacker -- --recursive --input_dir /path/to/content/game -o ./raw
```

- Create an archive

```sh
dotnet run --project EastwardPacker -- --pack --input_dir ./raw/locale --output ./modified/locale.g
```

Extraction creates `raw/<archive name>/...`. Repacking uses paths relative to the input directory. Keep the output archive outside that directory. You do not need asset indices for these operations.

### Asset extraction

You can decode indexed assets with their original filenames.

```sh
dotnet run --project EastwardPacker -- --assets --config /path/to/content/game/config.g --input_dir /path/to/content/game --overlay /path/to/content_dlc -o ./decoded
```

Use `--type texture` to extract only one asset type. You can also pass `--index`, `--script_lib` and `--texture_index` JSON files instead of `--config`. Later overlay entries take priority.

HMG images become PNG, workbooks become XLSX, sprites become ASE, and texture packs become PSD. Scenes, prototypes, SQ, PO and OBJ/MTL are also supported. FMOD banks are copied without loading FMOD.

Some Lua assets are precompiled with LuaJIT. Bytecode is kept by default. Use `--lua-source --decompiler /path/to/eastward-luajit.dll` to extract Lua source. The native library loads only when needed and is not included in the CLI. Please check [EastwardLib](EastwardLib/README.md) for build instructions.

Missing resources are reported separately, and available assets are still extracted. Extraction failures return a nonzero exit code.

### Source restoration

You can restore source assets for a matching Gii project.

```sh
dotnet run --project EastwardPacker -- --project --config /path/to/content/game/config.g --input_dir /path/to/content/game --overlay /path/to/content_dlc -o ./restored --decompiler /path/to/native-library
```

Use a new output directory. The output contains `game/asset` with scenes, Lua, SQ, XLS, PO, banks and meshes. Sprites use separate ASE animation files. Textures and tilesets use available PSD layers, decks use raw `.deckpack` folders, and graphs use GraphML. References are updated for the restored paths.

The separate `restored.recovery.json` report lists missing resources, JPEG pixel alternatives and path changes. Use `--type` to select one asset type. `--project` and `--assets` are separate modes.

Original project settings, Gii/AKU binaries and discarded editing history are not included. You need the matching editor environment to open and build the full project. Please check [EastwardLib](EastwardLib/README.md) for compatibility details.

### Source compilation

You can compile edited sources into copies of the original archives.

```sh
dotnet run --project EastwardPacker -- --compile --asset script/example.sq --source ./edited/example.sq --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./compiled
```

Use `--project --source ./restored` instead of `--asset` to compile a restored tree with its recovery report. You can add `--type` to select one asset type and `--overlay` for DLC resources. Use a new output directory outside the game resources.

Other archive entries are kept. Keep existing workbook sheet names and indexed atlas texture dimensions. Mesh materials are retained, and Lua source is saved without compiling LuaJIT bytecode. New assets need separate index registration.

### EastwardLib

By importing "EastwardLib.dll", you can handle Assets and GArchives.

Example for inserting a new asset into an archive.

```cs
using EastwardLib.Assets;

var g = GArchive.Read("locale.g");
g["locale/foo"] = new TextAsset("bar").Encode();
g.Write("locale_new.g");
```

Example for extracting from an archive.

```cs
using EastwardLib.Assets;

var g = GArchive.Read("locale.g");
g.ExtractTo("./locale_extracted");
```

You can also load indexed assets, edit metadata and restore source assets. Please check [EastwardLib](EastwardLib/README.md) for more examples.

## Tools

### AtlasExtractor

Pass multiple files through args, and extract images from each atlas file. Output is saved in an `atlas-extracted` directory beside the input file.

### Hmg2Image

Pass multiple files through args, and decode HMG files to PNG images.

### Image2Hmg

Pass multiple files through args, and encode PNG or JPEG images to HMG files.

```sh
dotnet run --project AtlasExtractor -- /path/to/atlas.json
dotnet run --project Hmg2Image -- texture.hmg
dotnet run --project Image2Hmg -- texture.png
```

Image tools do not overwrite input files. They do not need System.Drawing or libgdiplus.

## Features

- Extract and repack ".g" archives.
- Decode textures, packed data, scenes and other indexed assets.
- Export ASE animations, PSD textures, workbooks and mesh files.
- Restore Gii source assets from available game data.
- Edit and save original metadata with EastwardLib.
- Use an optional native LuaJIT decompiler.
- Build for Windows, Linux and macOS.

## Other operating systems

You can publish all four tools for other operating systems.

```sh
dotnet publish EastwardPacker -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64
dotnet publish EastwardPacker -c Release -r osx-arm64 --self-contained false -o ./publish/osx-arm64
```

These outputs need the .NET 10 runtime. Use `--self-contained true` to include it.
