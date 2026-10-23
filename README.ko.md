EastwardPacker
=============

[English](README.md) | 한국어

Eastward 아카이브와 에셋을 추출하고 다시 묶는 비공식 CLI 도구입니다.

## eastward.js

[JavaScript 버전](https://github.com/VriskaSerket51/eastward.js)도 있습니다!

## 빌드 방법

.NET 10이 필요합니다. 서브모듈을 함께 클론하세요.

```sh
git clone --recurse-submodules https://github.com/VriskaSerket51/EastwardPacker.git
cd EastwardPacker
dotnet build EastwardPacker.sln -c Release
```

## 사용 방법

### EastwardPacker

`.g` 파일을 추출하고 수정한 파일을 다시 묶을 수 있습니다.

- 단일 파일 추출

```sh
dotnet run --project EastwardPacker -- -i /path/to/locale.g -o ./raw
```

- 여러 파일 추출

```sh
dotnet run --project EastwardPacker -- --recursive --input_dir /path/to/content/game -o ./raw
```

- 아카이브 생성

```sh
dotnet run --project EastwardPacker -- --pack --input_dir ./raw/locale --output ./modified/locale.g
```

추출 결과는 `raw/<아카이브 이름>/...`에 저장됩니다. 다시 묶을 때는 그 안의 디렉터리를 입력으로 사용합니다.

### 에셋 추출

인덱스에 기록된 이름과 경로로 에셋을 추출할 수 있습니다.

```sh
dotnet run --project EastwardPacker -- --assets --config /path/to/content/game/config.g --input_dir /path/to/content/game --overlay /path/to/content_dlc -o ./assets
```

`--type texture`처럼 종류를 지정할 수 있습니다. LuaJIT 바이트코드를 Lua 코드로 변환하려면 `--lua-source --decompiler /path/to/native-library`를 추가합니다. 결과는 원본 소스와 다를 수 있습니다.

### 편집용 파일 내보내기

```sh
dotnet run --project EastwardPacker -- --project --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./restored --decompiler /path/to/native-library
```

내보낸 파일은 `game/asset`에 저장됩니다. 원본 프로젝트 설정과 배포본에서 사라진 편집 정보는 복원되지 않습니다.

### 소스 컴파일

```sh
dotnet run --project EastwardPacker -- --compile --asset script/example.sq --source ./edited/example.sq --config /path/to/content/game/config.g --input_dir /path/to/content/game -o ./compiled
```

내보낸 파일 트리 전체는 `--asset` 대신 `--project`를 사용합니다. 프로젝트 루트의 `.eastward-project.json`도 함께 보관하세요. 설치된 DLC 아카이브를 함께 읽으려면 `--overlay`로 해당 디렉터리를 지정합니다. 출력은 게임 밖의 새 디렉터리에 만듭니다.

## 도구

### AtlasExtractor

아틀라스의 이미지를 추출합니다.

```sh
dotnet run --project AtlasExtractor -- /path/to/atlas.json
```

### Hmg2Image / Image2Hmg

HMG를 PNG로, PNG/JPEG를 HMG로 변환합니다.

```sh
dotnet run --project Hmg2Image -- texture.hmg
dotnet run --project Image2Hmg -- texture.png
```

라이브러리에서 사용하려면 [EastwardLib](https://github.com/VriskaSerket51/EastwardLib/blob/main/README.ko.md), `.ewmod`를 만들고 설치하려면 [EastwardModTool](https://github.com/VriskaSerket51/EastwardModTool)을 참고하세요.
