EastwardPacker
=============

[English](README.md) | 한국어

Eastward 에셋을 추출하고 다시 묶는 비공식 도구입니다.

[JavaScript 버전](https://github.com/VriskaSerket51/eastward.js)도 있습니다.

## 빌드 방법

.NET 10이 필요합니다. 클론할 때 서브모듈도 함께 받습니다.

```sh
git clone --recurse-submodules https://github.com/VriskaSerket51/EastwardPacker.git
cd EastwardPacker
dotnet build EastwardPacker.sln -c Release
```

## 사용 방법

### 아카이브 추출

```sh
dotnet run --project EastwardPacker -- -i /path/to/locale.g -o ./raw
dotnet run --project EastwardPacker -- --recursive --input_dir /path/to/content/game -o ./raw
```

결과는 `raw/<아카이브 이름>/`에 저장됩니다.

### 아카이브 생성

```sh
dotnet run --project EastwardPacker -- --pack --input_dir ./raw/locale --output ./modified/locale.g
```

### 에셋 추출

```sh
dotnet run --project EastwardPacker -- --assets --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./assets
```

`--type texture`로 종류를 지정할 수 있습니다. 설치된 DLC도 읽으려면 `--overlay /path/to/content_dlc`를 추가합니다.

### 편집용 파일 추출

```sh
dotnet run --project EastwardPacker -- --project --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./restored --decompiler /path/to/native-library
```

> 파일은 `game/asset`에 저장됩니다. `.eastward-project.json`도 함께 보관해야 합니다.
> 게임에 없는 프로젝트 설정과 편집 정보는 복구할 수 없습니다.

### 소스 컴파일

```sh
dotnet run --project EastwardPacker -- --compile --asset script/example.sq --source ./edited/example.sq --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./compiled
```

전체 프로젝트를 컴파일할 때는 `--asset` 대신 `--project`를 사용합니다. 출력 경로는 게임 밖의 새 폴더로 지정해야 합니다.

Lua 소스 추출에는 `--lua-source --decompiler /path/to/native-library`가 필요합니다. 결과는 원본 소스와 다를 수 있습니다. 네이티브 빌드 방법은 [EastwardLib](https://github.com/VriskaSerket51/EastwardLib/blob/main/README.ko.md#lua-소스-추출)에 있습니다.

## 기타 도구

```sh
dotnet run --project AtlasExtractor -- /path/to/atlas.json
dotnet run --project Hmg2Image -- texture.hmg
dotnet run --project Image2Hmg -- texture.png
```

- AtlasExtractor: 아틀라스 이미지 추출
- Hmg2Image: HMG를 PNG로 변환
- Image2Hmg: PNG/JPEG를 HMG로 변환

GUI 도구로는 [EastwardModTool](https://github.com/VriskaSerket51/EastwardModTool)이 있습니다.
