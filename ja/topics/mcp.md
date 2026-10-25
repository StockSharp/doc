# AI エージェントの接続（MCP）

**MCP**（Model Context Protocol）は、AI エージェント（Claude Code、Claude Desktop、またはその他の MCP クライアント）が外部プログラム、つまり MCP サーバーのツールを呼び出すためのオープンなプロトコルです。クライアントはサーバーを子プロセスとして自ら起動し、標準入力と標準出力を通じてメッセージをやり取りします。サーバーは、どのようなツールがあり、それぞれが何をし、どのパラメーターを受け取るかを通知し、エージェントはどのツールを呼び出すかを判断して応答を受け取ります。

StockSharp のプログラム向けには、2 つの MCP サーバーがあります。どちらもプログラムと同じコンピューター上でのみ動作し、同じユーザーアカウントのプログラムとのみ通信します。

| | [DesktopDriver](mcp/desktop_driver.md) | [Studio.Mcp](mcp/studio_mcp.md) |
|---|---|---|
| 機能 | プログラムのインターフェイスを読み取り、ボタンを押し、テキストを入力し、スクロールし、画像を取得する | プログラムにコマンドを送る: 注文の発注や取消、Hydra のソースの起動、ストラテジーのテストの開始 |
| 対象プログラム | Terminal、Hydra、Designer、Installer（Avalonia 版と WPF 版）、Shell、UDP Dumper — 自動化を有効にしたビルドのみ | Terminal、Hydra、Designer、Shell の Avalonia 版 — 通常のビルド |
| エージェントに見えるもの | ウィンドウ、パネル、テーブル、チャート、板情報、プロパティエディター、ストラテジーのスキーム | コマンドが返すものだけ: ソース、ストラテジー、注文の一覧 |
| 操作の制限 | クリックと入力を受け付けるのは、テストプロファイルで起動したプログラムだけです（UDP Dumper は常に受け付けます） | プロファイルによる制限はありません: 注文は Terminal が接続している先へ送られます |
| サーバーの入手先 | NuGet パッケージ `StockSharp.DesktopDriver.Mcp`、Claude Desktop 用の拡張機能 | ソースコードからビルドします |

## どちらのサーバーを選ぶか

- ウィンドウに何が表示されているかを確認したい、人が操作するようにボタンでシナリオを進めたい、パネルの画像を取得したい — [DesktopDriver](mcp/desktop_driver.md)。
- クリックせずに、コマンドでプログラムに操作を実行させたい（注文の発注、Hydra のソースによるダウンロードの開始、ストラテジーのテストの開始） — [Studio.Mcp](mcp/studio_mcp.md)。
- プログラムが自動化なしの通常の方法でビルドされている — 使えるのは Studio.Mcp だけで、対象は Avalonia 版のプログラムのみです。
- WPF 版のプログラム — 使えるのは DesktopDriver だけで、自動化を有効にしたビルドに限ります。

両方のサーバーを同時にエージェントに接続することもできます。たとえば、Studio.Mcp は Designer でストラテジーのテストを開始しますが、結果は返しません。Designer が自動化を有効にしてビルドされていれば、エージェントは DesktopDriver を使って Designer のパネルから結果を読み取れます。

MCP は、サイトや [Telegram ボット](telegram_services/control_panel.md) からの [リモート管理](shell/remote_control.md) とは別のものです。リモート管理は StockSharp アカウントを通じて動作し、エージェントも MCP サーバーも必要ありません。

## 推奨コンテンツ

[インターフェイスの操作（DesktopDriver）](mcp/desktop_driver.md)

[プログラムのコマンド（Studio.Mcp）](mcp/studio_mcp.md)

[AI 支援開発](api/ai_development.md)
