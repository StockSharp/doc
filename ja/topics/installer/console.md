# コンソールインストーラー

`Installer.Console` アプリケーションは、StockSharp Installer のクロスプラットフォーム版です。グラフィカルインターフェイスを使用せずに、製品のダウンロード、更新、削除を行えます。このツールは、[.NET 6](https://dotnet.microsoft.com/) ランタイムが利用可能な任意の OS で動作します。

## 実行

1. 使用するプラットフォーム向けの .NET 6 SDK またはランタイムをインストールします。
2. [ダウンロードページ](https://stocksharp.com/ja/products/download/) から `StockSharp.Installer.Console.zip` をダウンロードします。
3. アーカイブを展開し、コマンドラインからユーティリティを実行します。

   ```bash
   dotnet StockSharp.Installer.Console.dll <Command> [product] [dir] [options]
   ```

`<Command>` は次のいずれかです。

- `Install` - 製品をインストールします。
- `Update` - インストール済みの製品を更新します。
- `Repair` - 既存のインストールを修復します。
- `Remove` - 製品をアンインストールします。
- `License` - 製品のライセンスを表示します。
- `Licenses` - 利用可能なライセンスを一覧表示します。
- `HddId` - ハードドライブ識別子を出力します。
- `Products` - 利用可能な製品を一覧表示します。
- `Updates` - 利用可能な更新を表示します。
- `Installed` - インストール済みプログラムを一覧表示します。
- `Sign` - DLL ファイルに署名します。

省略可能な `[product]` パラメーターは、[ストア](https://stocksharp.com/ja/store/) の製品 ID です。この ID は、製品ページ、たとえば [Hydra サーバーページ](https://stocksharp.com/ja/store/hydra-server/) で確認できます。または、`StockSharp.Installer.Console.exe Products -s hydra` を実行して確認できます。`[dir]` はインストールディレクトリを指定します。

## オプション

このユーティリティは次のオプションを受け付けます。

- `-s`, `--search` - 名前で製品をフィルターします。
- `-r`, `--run` - インストール後にアプリケーション（例: `StockSharp.Hydra.Server.exe`）を自動的に実行します。
- `-c`, `--cache` - NuGet キャッシュを使用します。
- `-f`, `--force` - 設定済みの間隔を無視して更新チェックを強制します。
- `-p`, `--pre` - プレリリースバージョンのインストールを許可します。
- `-e`, `--noerror` - すべてのエラーを抑制します。
- `-b`, `--backup` - 修復または更新の前に以前の設定をバックアップします。
- `-l`, `--clear` - インストール前に対象ディレクトリをクリアします。
- `-t`, `--fw` - 対象の .NET フレームワークを指定します。
- `-d`, `--data` - アプリケーションデータフォルダーを削除します。
- `-i`, `--in` - 署名する DLL。
- `-o`, `--out` - 署名後の結果 DLL。

コマンド例:

```bash
dotnet StockSharp.Installer.Console.dll Install 1269 /home/user/stocksharp -p -r StockSharp.Hydra.Server.exe
```

これは製品 **1269**（ここでは単なる例として使用）を指定したディレクトリにインストールし、プレリリースビルドを許可し、完了後に `StockSharp.Hydra.Server.exe` を起動します。

## 署名

`Sign` コマンドは、ロボット DLL にデジタル署名します。独自の API ベースのロボットを配布する際、[無料プラン](https://stocksharp.com/ja/pricing/) のユーザーを含むすべての受信者が実行できるようにするために使用します。

事前にロボットをコンパイルし、`StockSharp.Configuration` 名前空間の `[assembly: ProductId(9)]` 属性を追加します。`.exe` ファイルではなく、DLL（例: `MyRobot.dll`）に署名してください。

例:

```bash
StockSharp.Installer.Console.exe Sign -i "MyRobot.dll"
```

プロジェクトを再ビルドすると署名は削除されるため、それ以上のコンパイルを予定していない開発の最終段階でアセンブリに署名してください。

