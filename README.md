EastwardPacker
=============

## Download
All releases can be found at [here](https://github.com/VriskaSerket51/EastwardPacker/releases).

## How to use
### Console
You can extract assets in ".g" files.

- Single file extraction
`EastwardPacker.exe --index {index_path} -i {input_path} -o {output_path}`
- Multiple files extraction
`EastwardPacker.exe --r --index {index_path} --input_dir {input_path} -o {output_path}`

### Library
By importing "EastwardLib.dll", you can handle Assets and GArchives.

For example,
<br>
```cs
using EastwardLib;
using EastwardLib.MetaData;

AssetIndex.Create("asset_index");
var g = GArchive.Read("locale.g");
g["foo"] = new TextAsset("bar");
g.Write("locale_new.g");
```

`Warning: You must load AssetIndex at first!`

## Features
- Can extract various assets
  - Hmg Texture
  - Packed Data
  - Or just text files
  - For mSprite animations, please check [here](https://github.com/VriskaSerket51/EastwardMSpriteParser).
  - [Now working] deck files
- Create your custom assets, and add into ".g" archive.