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
<br/>
For more description, please check [here](https://github.com/VriskaSerket51/EastwardLib).

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