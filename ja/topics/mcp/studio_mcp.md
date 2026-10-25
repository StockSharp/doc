# プログラムのコマンド（Studio.Mcp）

**Studio.Mcp** は、エージェントが実行中の StockSharp のプログラムにコマンドを送るための MCP サーバーです。Terminal での注文の発注と取消、Hydra のデータソースの管理、Designer と Shell でのストラテジーのテストの開始を行います。サーバーはボタンを押したりウィンドウを読み取ったりせず、コマンドはプログラム自身が実行します。

サーバーは何も起動せず、何も保存しません。コマンドがプログラムに届くのは、プログラムが開いているときだけです。プログラムが起動していない場合、サーバーはそのように応答します。

## 応答するプログラム

| プログラム | 受け付けるコマンド |
|---|---|
| [Terminal](../terminal.md) | 注文の一覧、注文の発注と取消 |
| [Hydra](../hydra.md) | ソースの種類、ソースの一覧、ソースの追加、有効化、開始、停止、削除 |
| [Designer](../designer.md) | ストラテジーの一覧、履歴でのテストの開始と停止 |
| [Shell](../shell.md) | [エミュレーション](../shell/user_interface/emulation.md) 領域のストラテジーの一覧、その実行の開始と停止 |

コマンドを受け付けるのは、これらのプログラムの Avalonia 版だけで、しかも通常のビルドです。別のビルドは必要ありません。プログラムはメインウィンドウとともにコマンド用のチャネルを開き、ウィンドウを閉じるとチャネルも閉じます。Terminal、Hydra、Designer の WPF 版は、これらのコマンドを受け付けません。

チャネルは、現在の OS ユーザーのプロセスだけがアクセスできるローカルの名前付きパイプです。コマンドはネットワーク経由では送信されません。

## インストール

サーバーは StockSharp のソースコードからビルドします。

```sh
dotnet build Studio/Studio.Mcp/Studio.Mcp.csproj -c Release
```

完成したファイルは `Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe` です。サーバーには独自の設定がなく、引数も環境変数も必要ありません。プロトコルは標準出力に、ログは標準エラー出力に書き込みます。クライアントなしで手動で起動した場合は、入力を待つだけです。

## エージェントの接続

Claude Code — プロジェクトの `.mcp.json` ファイル:

```json
{
  "mcpServers": {
    "stocksharp": {
      "command": "Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe"
    }
  }
}
```

Claude Desktop を含むほかの MCP クライアントでも、サーバーは同じように接続します。コマンドは `StockSharp.Studio.Mcp.exe` へのパスで、引数も環境変数も不要です。

## エージェントにできること

ソース、ストラテジー、注文の識別子は、一覧を返すツールの応答から取得します。まず一覧を取得し、その後に操作を行います。

### 共通

| ツール | 動作 |
|---|---|
| `list_products` | どのプログラムが起動していて、それぞれがどのコマンドを受け付けるか |
| `describe_product` | 1 つのプログラムのコマンド。`product` は `Terminal`、`Hydra`、`Designer`、または `Shell` |
| `close_product` | プログラムに終了を要求します |

### Hydra

| ツール | 動作 |
|---|---|
| `hydra_list_source_types` | Hydra が作成できるソースの種類とその識別子 |
| `hydra_list_sources` | Hydra のソース: それぞれが有効かどうか、現在ダウンロード中かどうか |
| `hydra_add_source` (`typeId`) | 選択した種類のソースを追加して返します。新しいソースは無効の状態です |
| `hydra_enable_source` (`sourceId`, `isEnabled`) | ソースをダウンロードの対象に含める、または対象から外します |
| `hydra_start_source` (`sourceId`) | ソースによるダウンロードを開始します。無効なソースは開始されません |
| `hydra_stop_source` (`sourceId`) | ダウンロードを停止します |
| `hydra_remove_source` (`sourceId`) | ソースをその設定とともに削除します。動作中のソースは、先に停止する必要があります |

新しいソースでデータをダウンロードするには、`hydra_add_source`、`hydra_enable_source`、`hydra_start_source` の 3 回の呼び出しが必要です。ソースの設定（銘柄、データ型、接続パラメーター）はこれらのコマンドでは変更されません。それらは Hydra のウィンドウで指定します（[データ ソース](../hydra/data_sources.md) を参照）。

### Designer

| ツール | 動作 |
|---|---|
| `designer_list_strategies` | Designer のストラテジー: 種類（`Backtest`、`Optimization`、`Live`、`Remote`、`Other`）と現在の状態 |
| `designer_start_backtest` (`strategyId`, `startTime`, `stopTime`) | 種類が `Backtest` または `Optimization` のストラテジーを履歴で開始します。期間は UTC で ISO 8601 形式で指定し、ストラテジーの設定にある期間を置き換えます。指定しない場合は設定の期間が使われます |
| `designer_stop_backtest` (`strategyId`) | 動作中のストラテジーを停止します |

### Terminal

| ツール | 動作 |
|---|---|
| `terminal_list_orders` | Terminal の注文と、それぞれのトランザクション番号 |
| `terminal_register_order` (`securityId`, `portfolioName`, `side`, `volume`, `price`, `orderType`) | 注文を発注し、そのトランザクション番号を返します |
| `terminal_cancel_order` (`orderTransactionId`) | トランザクション番号で注文を取り消します |

銘柄は Terminal が認識している形式で指定します（たとえば `SBER@TQBR`）。ポートフォリオは Terminal が認識している名前で指定し、`side` は `Buy` または `Sell` です。指値注文には価格が必要です。ほかの種類の注文では、渡した価格は使われません。注文を指定するのは、`terminal_register_order` または `terminal_list_orders` が返したトランザクション番号であり、後で取引所が割り当てる番号ではありません。

### Shell

Shell のツールは、プログラム自身が送ってくるコマンドの説明からサーバーが作成します。そのため、これらのツールが存在するのは、サーバーの起動時、または最後に `list_products` か `describe_product` を呼び出した時点で Shell が起動していた場合だけです。

| ツール | 動作 |
|---|---|
| `shell_list_strategies` | 「エミュレーション」領域のストラテジーとその状態 |
| `shell_start_backtest` (`StrategyId`, `StartTime`, `StopTime`) | ストラテジーの履歴での実行を開始します |
| `shell_stop_backtest` (`StrategyId`) | 実行を停止します |

Shell は常に、ストラテジーの設定にある期間でテストします。`StartTime` または `StopTime` を渡すとコマンドは拒否されるため、これらには `null` を渡してください。

## 制限とセキュリティ

> [!CAUTION]
> `terminal_register_order` は、Terminal が接続している先に実際の注文を発注します。サーバーは何も確認しません。確認は MCP クライアントの役割で、人は呼び出しが実行される前にそこで内容を確認できます。

- **実際に取引するストラテジーは開始されません。** Designer が開始するのは、種類が `Backtest` と `Optimization` のストラテジーだけで、それ以外ではコマンドが拒否されます。Shell がコマンド用に公開するのは、「エミュレーション」領域のストラテジーだけです。
- **削除は元に戻せません。** `hydra_remove_source` は、ソースをその設定とともに削除します。
- **終了。** `close_product` はプログラムを閉じます。
- **プログラムが起動している必要があります。** 起動していない場合、コマンドは `<プログラム> is not running.` というエラーを返します。起動していないプログラムからの応答の待ち時間は約 5 秒のため、プログラムが閉じていると `list_products` の応答には時間がかかります。起動しているプログラムがコマンドを拒否した場合、エージェントには `<プログラム> refused the command: <理由>` というテキストが届きます。
- **パスワードはありません。** チャネルは、同じユーザーアカウントで起動したどのプログラムにも開かれています。誰がコマンドを送ったかを別途確認する仕組みはありません。
- **プログラムのコピーは区別されません。** Terminal や Designer のコピーが複数起動している場合や、通常の Hydra の横でテストプロファイルの Hydra が動作している場合、コマンドはそのいずれかに届き、どれに届くかは事前にはわかりません。
- **テストの結果は返されません。** サーバーが返すのはストラテジーの状態だけで、統計や約定は返しません。それらは [DesktopDriver](desktop_driver.md) を使ってプログラムのパネルから読み取れます。

## セッションの例

ユーザーは「Designer で 1 月分のストラテジーのテストを開始して、実行中かどうか確認して」と依頼します。

1. `list_products` — Designer は起動しています。ほかのプログラムについては `… is not running.` と返されます。
2. `designer_list_strategies` — ストラテジーの中に、種類が `Backtest` で状態が `Stopped` の目的のストラテジーがあります。
3. `designer_start_backtest`、`strategyId` は手順 2 のもの、`startTime` = `2026-01-01T00:00:00Z`、`stopTime` = `2026-02-01T00:00:00Z` — 応答は `Strategy <識別子> was started.` です。
4. `designer_list_strategies` — ストラテジーの状態が変わりました。
5. テストを中断する必要がある場合は、同じ `strategyId` で `designer_stop_backtest` を呼び出します。

## 推奨コンテンツ

[AI エージェントの接続（MCP）](../mcp.md)

[インターフェイスの操作（DesktopDriver）](desktop_driver.md)

[リモート管理](../shell/remote_control.md)
