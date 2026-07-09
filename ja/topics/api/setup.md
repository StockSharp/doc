# 環境のセットアップ

## 要件

StockSharp を使用するには、次が必要です。

- **.NET 10**（SDK および Runtime）— [ダウンロード](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** — Visual Studio 2022+、JetBrains Rider、または VS Code
- **NuGet** — パッケージマネージャー（Visual Studio と Rider に組み込み）

SDK がインストールされていることを確認します。

```bash
dotnet --version
```

## プロジェクトの作成

### Visual Studio 2022+

1. **ファイル → 新規作成 → プロジェクト**
2. **コンソール アプリ** または **WPF アプリケーション** テンプレートを選択します
3. ターゲットフレームワークを **.NET 10** に設定します

### JetBrains Rider

1. **ファイル → 新しいソリューション**
2. **.NET / .NET Core → コンソール アプリケーション** を選択します
3. **ターゲット フレームワーク: net10.0** を設定します

### コマンドライン（CLI）

```bash
# コンソールアプリケーション
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# WPF アプリケーション（Windows のみ）
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## NuGet パッケージカタログ

StockSharp は NuGet 経由で配布されます。以下はカテゴリ別に整理された完全なパッケージカタログです。

### コア

| パッケージ | 説明 |
|---------|-------------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | 基本メッセージと契約。フレームワーク全体の基盤 |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | 取引エンティティ: Security、Order、Trade、Portfolio など |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | コアのアルゴリズム取引、Connector、サブスクリプション、ローソク足 |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | 構成管理、接続設定 |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | ローカリゼーションシステム（デフォルトは英語） |

### 戦略と指標

| パッケージ | 説明 |
|---------|-------------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | 戦略フレームワーク — 基本 Strategy クラス、ポジション、PnL |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | 100 種類以上のテクニカル指標（SMA、EMA、RSI、MACD、Bollinger など） |

### テスト

| パッケージ | 説明 |
|---------|-------------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | 履歴データでのバックテスト、取引エミュレーション |

### ストレージとデータ

| パッケージ | 説明 |
|---------|-------------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | 市場データのエクスポート（CSV、Excel、JSON など） |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | 外部形式からのデータインポート |

### 分析と計算

| パッケージ | 説明 |
|---------|-------------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | 分析スクリプトのインターフェイス |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | ランタイムコードコンパイル |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | GPU アクセラレーションによる指標計算（ILGPU 経由の CUDA） |

### GUI コンポーネント（Windows のみ）

| パッケージ | 説明 |
|---------|-------------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | WPF コントロール: 銘柄、ポートフォリオ、注文テーブル |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | ローソク足チャート、指標、エクイティカーブ |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | チャートコンポーネントのインターフェイス |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | アラートシステムのインターフェイス |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | ビジュアル戦略デザイナーのコア |

### コネクター（取引所とブローカー）

各コネクターは個別の NuGet パッケージです。主なコネクター:

| パッケージ | 取引所/ブローカー |
|---------|----------------|
| `StockSharp.Binance` | Binance |
| `StockSharp.InteractiveBrokers` | Interactive Brokers |
| `StockSharp.Fix` | FIX プロトコル（汎用） |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | Bitstamp |
| `StockSharp.Connectors.Bittrex` | Bittrex |

> [!NOTE]
> コネクターの完全な一覧については、[コネクター](connectors.md) セクションを参照してください。一部のコネクターは [プライベート NuGet サーバー](#private-nuget-server) 経由でのみ利用できます。

### ローカリゼーション

基本の `StockSharp.Localization` パッケージには英語が含まれます。追加言語は別個のパッケージとしてインストールされます。

| パッケージ | 言語 |
|---------|----------|
| `StockSharp.Localization.ru` | ロシア語 |
| `StockSharp.Localization.zh` | 中国語 |
| `StockSharp.Localization.de` | ドイツ語 |
| `StockSharp.Localization.es` | スペイン語 |
| `StockSharp.Localization.ja` | 日本語 |
| `StockSharp.Localization.ko` | 韓国語 |
| `StockSharp.Localization.All` | すべての言語（メタパッケージ） |

次も利用可能です: `ar`、`bn`、`ca`、`cs`、`da`、`el`、`fa`、`fi`、`fr`、`he`、`hi`、`hu`、`it`、`jv`、`ms`、`my`、`nl`、`no`、`pa`、`pl`、`pt`、`ro`、`sk`、`sr`、`sv`、`ta`、`th`、`tl`、`tr`、`uk`、`uz`、`vi`。

## パッケージのインストール

### CLI 経由（推奨）

```bash
# コアパッケージ
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies

# コネクター（例 — Binance）
dotnet add package StockSharp.Binance

# 指標
dotnet add package StockSharp.Algo.Indicators

# バックテスト
dotnet add package StockSharp.Algo.Testing

# ローカリゼーション（ロシア語）
dotnet add package StockSharp.Localization.ru
```

### Visual Studio 経由

1. プロジェクトを右クリック → **NuGet パッケージの管理...**
2. `StockSharp` を検索します
3. 目的のパッケージを選択 → **インストール**

すべての依存関係は自動的にインストールされます。

### JetBrains Rider 経由

1. プロジェクトを右クリック → **NuGet パッケージの管理**
2. `StockSharp` を検索します
3. パッケージを選択 → **インストール**

### パッケージ マネージャー コンソール経由（Visual Studio）

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## プライベート NuGet サーバー {#private-nuget-server}

一部のコンポーネント（暗号資産コネクターなど）は、登録ユーザー向けのプライベート NuGet サーバー経由でのみ利用できます。

### 方法 1: URL 内のトークンによる認証

1. StockSharp Web サイトで登録します。
2. [個人アカウント](https://stocksharp.ru/profile/) からトークンをコピーします。
3. パッケージソースを追加します。

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** **ツール → オプション → NuGet パッケージ マネージャー → パッケージ ソース** を開き、URL `https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json` で新しいソースを追加します。

**Rider:** **設定 → ビルド、実行、デプロイ → NuGet → ソース** を開き、ソースを追加します。

### 方法 2: ユーザー名とパスワードによる認証

1. URL `https://nuget.stocksharp.com/x/v3/index.json` でパッケージソースを追加します
2. このソースを使用しようとすると、ログインプロンプトが表示されます
3. StockSharp アカウントの認証情報（またはユーザー名として `x`、パスワードとしてトークン）を入力します

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username YOUR_LOGIN --password YOUR_PASSWORD --store-password-in-clear-text
```

> [!TIP]
> Windows で保存済み認証情報をリセットするには、**コントロール パネル → ユーザー アカウント → 資格情報マネージャー** を開き、`nuget.stocksharp.com` に関連するエントリを削除します。

## パッケージの更新

### CLI

```bash
# 利用可能な更新を確認
dotnet list package --outdated

# 特定のパッケージを更新
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **NuGet パッケージの管理...** → **更新** タブ
2. パッケージを選択 → **更新**

### Rider

1. **NuGet パッケージの管理** → **アップグレード** タブ
2. パッケージを選択 → **アップグレード**

## トラブルシューティング

### バージョン不一致

すべての StockSharp パッケージは同じバージョンである必要があります。型またはメソッドのエラーが発生した場合は、すべての `StockSharp.*` パッケージが同じバージョンに更新されていることを確認してください。

### Public NuGet でパッケージが見つからない

一部のコネクターは [プライベートサーバー](#private-nuget-server) 経由でのみ利用できます。正しいソースが追加されていることを確認してください。

### 非 Windows での GUI の問題

GUI コンポーネント（`StockSharp.Xaml`、`StockSharp.Xaml.Charting`）は Windows でのみ動作します。Linux/macOS では、GUI パッケージなしのコンソールアプリケーションを使用してください。

### パッケージ復元エラー

```bash
# NuGet キャッシュをクリア
dotnet nuget locals all --clear

# 復元を再試行
dotnet restore
```

## プロジェクトファイル構造（.csproj）

### 最小構成（コンソール取引ボット）

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

### 拡張構成（指標とテストを含む戦略）

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- コア -->
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Configuration" Version="*" />

    <!-- 戦略と指標 -->
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Algo.Indicators" Version="*" />

    <!-- コネクター -->
    <PackageReference Include="StockSharp.Binance" Version="*" />

    <!-- バックテスト -->
    <PackageReference Include="StockSharp.Algo.Testing" Version="*" />

    <!-- ローカリゼーション -->
    <PackageReference Include="StockSharp.Localization.ru" Version="*" />
  </ItemGroup>
</Project>
```

### チャート付き WPF アプリケーション

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Xaml.Charting" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

## インストールの確認

最小アプリケーションを作成します。

```csharp
using StockSharp.Algo;
using StockSharp.BusinessEntities;

Console.WriteLine("StockSharp successfully configured!");

var connector = new Connector();
Console.WriteLine($"Connector created: {connector}");
```

```bash
dotnet run
```

## 例

StockSharp の使用例は、リポジトリの [Samples/](https://github.com/stocksharp/stocksharp/tree/master/Samples) ディレクトリに用意されています。これらは、取引所への接続、データ購読、ローソク足、指標、戦略、テストの構築を網羅しています。
