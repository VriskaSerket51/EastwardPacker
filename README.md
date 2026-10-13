# EastwardPacker

Portable .NET 10 tools for Eastward resources. Also see [eastward.js](https://github.com/VriskaSerket51/eastward.js).

Clone with submodules and build:

```sh
git clone --recurse-submodules https://github.com/VriskaSerket51/EastwardPacker.git
dotnet build EastwardPacker.sln -c Release
dotnet run --project EastwardLib/tests/EastwardLib.Tests -c Release
```

## Extract and repack runtime files

```sh
dotnet run --project EastwardPacker -- -i /path/to/locale.g -o ./raw
dotnet run --project EastwardPacker -- --recursive --input_dir /path/to/content/game -o ./raw
dotnet run --project EastwardPacker -- --pack --input_dir ./raw/locale --output ./modified/locale.g
```

Raw extraction creates `raw/<archive name>/...`. Repacking takes the contents of that archive directory and retains relative names. Put the output archive outside the input directory. Raw operations do not require asset or script indices. Extraction failures return a nonzero exit code.

## Decode indexed assets

```sh
dotnet run --project EastwardPacker -- --assets --config /path/to/content/game/config.g --input_dir /path/to/content/game --overlay /path/to/content_dlc -o ./decoded
```

Use `--type texture` (or another index type) to select a category. As an alternative to `--config`, supply `--index`, `--script_lib` and `--texture_index` JSON files. Later overlay entries override base entries without discarding unrelated files.

The output follows logical asset paths. HMG images become PNG, workbooks become `.xlsx`, sprites become editable `.ase`, and available texture-pack layers become PSD. Scene source groups, prototypes, SQ source, localization PO and mesh OBJ/MTL are supported. FMOD banks are copied without invoking FMOD. Missing deployed resources are reported per asset and produce a nonzero exit code; available assets are still exported.

Lua bytecode is preserved by default. Use `--lua-source --decompiler /path/to/eastward-luajit.dll` to request source; the optional native bridge loads on the first decompilation. The CLI does not bundle a binary. See [EastwardLib](EastwardLib/README.md) for its portable CMake build instructions and API.

## Restore Gii source assets

```sh
dotnet run --project EastwardPacker -- --project --config /path/to/content/game/config.g --input_dir /path/to/content/game --overlay /path/to/content_dlc -o ./restored --decompiler /path/to/native-library
```

Use a fresh output directory. `--project` writes `game/asset` with original logical paths, source scene groups, decompiled Lua, SQ, XLS, PO, banks and meshes. Sprites become folders of source ASE animations; textures and tilesets use available PSD layers. GraphML preserves available graph topology. Deck PSD packages become Gii raw `.deckpack` folders; source document and embedded prototype references are remapped together.

The adjacent `restored.recovery.json` records restored files, missing deployed resources, JPEG pixel alternatives and path changes. Missing original sources are expected in packaged builds and are reported without suppressing unexpected conversion errors. `--type` can select one category. `--project` and `--assets` are separate output modes.

This reconstructs the source asset tree for an existing matching Gii project. Original project settings, host/AKU binaries and discarded editing history are unavailable. Full editor loading and engine builds need that environment. Source-to-runtime compilation is not implemented here. Public Gii reader/compiler checks and their compatibility requirements are documented in [EastwardLib](EastwardLib/README.md).

## Image tools

```sh
dotnet run --project Hmg2Image -- texture.hmg
dotnet run --project Image2Hmg -- texture.png
dotnet run --project AtlasExtractor -- /path/to/atlas.json
```

Each tool accepts multiple input paths. Hmg2Image writes `<input>.png`; Image2Hmg writes `<input>.hmg`. AtlasExtractor writes an `atlas-extracted` sibling directory. Inputs are never overwritten. Image processing uses managed RGBA code and StbImageSharp; System.Drawing and libgdiplus are not required.

## Other operating systems

All four tools target plain `net10.0`. Publish with the appropriate runtime identifier, for example:

```sh
dotnet publish EastwardPacker -c Release -r linux-x64 --self-contained false -o ./publish/linux-x64
dotnet publish EastwardPacker -c Release -r osx-arm64 --self-contained false -o ./publish/osx-arm64
```

Framework-dependent outputs require the .NET 10 runtime. Use `--self-contained true` to include it. Windows is verified locally; cross-publishing does not establish Linux/macOS execution or editor-import compatibility.
