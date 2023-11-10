EastwardPacker
=============

## Download
All releases can be found at [here](https://github.com/VriskaSerket51/EastwardPacker/releases).

## How to use
### EastwardPacker
You can extract assets in ".g" files.

- Single file extraction
`EastwardPacker.exe --index {index_path} -i {input_path} -o {output_path}`
- Multiple files extraction
`EastwardPacker.exe --r --index {index_path} --input_dir {input_path} -o {output_path}`

### EastwardLib
By importing "EastwardLib.dll", you can handle Assets and GArchives.

Example for inserting new asset into archive.
```cs
using EastwardLib.Assets;
using EastwardLib.MetaData;

var g = GArchive.Read("locale.g");
g["foo"] = new TextAsset("bar").Encode();
g.Write("locale_new.g");
```

Example for extracting from archive.
```cs
using EastwardLib.Assets;
using EastwardLib.MetaData;

var g = GArchive.Read("locale.g");
g.ExtractTo("./locale_extracted");
```

Example for extracting assets with restoring filename.
```cs
using EastwardLib.Assets;
using EastwardLib.MetaData;

var assetManager = AssetManager.Create("./asset_index", "./script_library", "./texture_index");
foreach (var file in Directory.GetFiles("./archives"))
{
    try
    {
        assetManager.LoadArchive(GArchive.Read(file));
    }
    catch (Exception)
    {
    }
}
assetManager.RootDirectory = "./";
assetManager.LoadAssets();
assetManager.ExtractTo("./exported");
```
> **Warning**
> AssetManager.LoadAssets() load all assets, it might consumes a bunch of memories.

Currently, you cannot export audio assets.
Some lua assets are precompiled with LuaJIT.

## Tools
### AtlasExtractor
Pass multiple files through args, and extract images from each atlas file.

### Hmg2Image
Pass multiple files through args, and decode hmg files to images.

### Image2Hmg
Pass multiple files through args, and encode images to hmg files.

## Features
- Can extract various assets
  - Hmg Texture
  - Packed Data
  - Deck files
  - Scenes
  - ETC
  - For mSprite animations, please check [here](https://github.com/VriskaSerket51/EastwardMSpriteParser).
- Create your custom assets, and add into ".g" archive.