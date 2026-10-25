# インターフェイスの操作（DesktopDriver）

**DesktopDriver** は、エージェントが実行中のプログラムのウィンドウを人と同じように操作するための MCP サーバーです。パネルやボタンを見つけ、テーブル、チャート、板情報、プロパティエディターを読み取り、クリックし、テキストを入力し、スクロールし、スクリーンショットを撮ります。

サーバーが読み取るのは画面の画像ではなく、インターフェイスそのものです。テーブルは列と行で、チャートは描画した点で、板情報は価格レベルで応答します。応答はプログラムのテーマ、フォント、言語に左右されません。スクリーンショットは人が見るためのもので、内容の確認は読み取りで行います。

サーバーは NuGet パッケージ `StockSharp.DesktopDriver.Mcp` として配布され、`StockSharp.DesktopDriver` と名乗ります。MCP Registry には `io.github.StockSharp/desktop-driver` として登録されています。ソースコードは [DesktopDriver.Mcp](https://github.com/StockSharp/DesktopDriver.Mcp) リポジトリにあります。

## 仕組み

1. ドライバーは、プログラムのビルド時にプログラムに組み込まれます。通常のビルドには含まれません。
2. ドライバー付きのプログラムを `--ui-automation` キーで起動すると、ローカルチャネル（現在の OS ユーザーだけがアクセスできる名前付きパイプ）が開き、そのアドレスが `--ui-automation-endpoint=<パス>` キーで指定したファイルに書き込まれます。このファイルは、ウィンドウが構築されて描画された後に作成されます。`--ui-automation` キーがない場合、プログラムは何も開きません。
3. MCP サーバーは、自分のカタログにあるプログラムをこれらのキーで起動するか、ほかの誰かが起動したプログラムに接続します。接続時には、目的のプログラム、目的のコピー、互換性のあるプロトコルのバージョンが応答したことを確認し、その後で初めてエージェントの呼び出しを実行します。

同じ操作は、`StockSharp.DesktopDriver.Cli` パッケージの `desktop-driver` プログラムを使ってコマンドラインからも実行できます。

## 接続できるビルド

接続できるのは、自動化を有効にしてビルドしたプログラム（`EnableUiAutomation=true` プロパティ付き）だけです。このプロパティは既定でオフになっており、通常のビルドにはドライバーもチャネルもありません。通常のビルドを `--ui-automation` 付きで起動すると、標準エラー出力に `<識別子> was built without automation, so --ui-automation does nothing. Build it with EnableUiAutomation=true.` というメッセージが書き込まれ、プログラムは通常どおり動作します。MCP サーバーはこのようなプログラムのチャネルを 90 秒待ち、その後プログラムが起動しなかったことを報告して、プログラムを閉じます。

自動化付きのビルドは、プログラムのソースコードから作成します。その横には DesktopDriver.Mcp リポジトリが必要です。ビルドは、通常のビルドと混ざらないように別の `uiauto\bin` フォルダーに出力されます。

```sh
dotnet build Terminal/Avalonia/Terminal.Avalonia/Terminal.Avalonia.csproj -c Release -p:EnableUiAutomation=true
```

このビルドの実行ファイルは `Terminal/Avalonia/Terminal.Avalonia/uiauto/bin/Release/net10.0/StockSharp.Terminal.Avalonia.exe` です。

| プログラム | Avalonia 版の識別子 | WPF 版の識別子 | テストプロファイルなしでのクリックと入力 |
|---|---|---|---|
| [Terminal](../terminal.md) | `stocksharp.terminal` | `stocksharp.terminal.wpf` | 拒否されます |
| [Hydra](../hydra.md) | `stocksharp.hydra` | `stocksharp.hydra.wpf` | 拒否されます |
| [Designer](../designer.md) | `stocksharp.designer` | `stocksharp.designer.wpf` | 拒否されます |
| [インストーラー](../installer.md) | `stocksharp.installer` | `stocksharp.installer.wpf` | 拒否されます |
| [Shell](../shell.md) | `stocksharp.shell` | — | 拒否されます |
| [UDP Dumper](../udp_dumper.md) | `tool.udpdumper` | — | 受け付けられます |

識別子はプログラム自身が通知し、同じ識別子をサーバーのカタログに指定します。

Avalonia 版では、ドライバーはテーブル、パネル付きのワークスペース、チャート、板情報、プロパティエディター、ストラテジーのスキーム、コードエディター、Markdown ビューアーを読み取ります。WPF 版では、テーブル、ドックパネル、チャート、板情報、プロパティエディター、ストラテジーのスキーム、入力フィールド、リボン、メニュー、ツールバーを読み取ります。Terminal、Hydra、Designer の Avalonia 版では、パネル上にあるものはすべて、そのパネルを基準にアドレス指定されます: `panel:<パネルの種類>/<識別子>`。

## サーバーのインストール

### .NET ツール

[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) が必要です。

```sh
dotnet tool install StockSharp.DesktopDriver.Mcp --tool-path tools
```

サーバーのコマンドは `tools/desktop-driver-mcp` です。サーバーはプロトコルを標準出力に、ログを標準エラー出力に書き込みます。クライアントなしで手動で起動した場合は、入力を待つだけです。

### ソースコードから

```sh
dotnet build DesktopDriver.Mcp/DesktopDriver.Mcp.csproj -c Release
```

### Claude Desktop 用の拡張機能

Claude Desktop 向けには、サーバーは拡張機能、つまり `stocksharp-desktop-driver-<バージョン>.mcpb` ファイルにパッケージ化されます。このファイルは、DesktopDriver.Mcp リポジトリの `scripts/Pack-McpBundle.ps1` スクリプトで作成します。拡張機能はコンピューターにインストールされた .NET でサーバーを起動するため、`PATH` から利用できる .NET 10 SDK が必要です。

1. Claude Desktop で **Settings → Extensions → Advanced settings → Install Extension** を開き、拡張機能のファイルを選択します。
2. プログラムカタログ（JSON ファイル、下記参照）と、スクリーンショットの保存先フォルダーを指定します。

## プログラムカタログ

エージェントは任意のファイルを起動できません。サーバーが起動するのは、カタログ（サーバーを設定する人が作成する JSON ファイル）にあるプログラムだけです。エージェントはプログラムをパスではなく識別子で指定します。

サーバーは設定を環境変数からのみ読み取ります。

| 変数 | 指定する内容 | 指定しない場合 |
|---|---|---|
| `STOCKSHARP_UI_CATALOGUE` | プログラムカタログへのパス | サーバーの隣にある `applications.json` ファイル |
| `STOCKSHARP_UI_ARTIFACTS` | スクリーンショットの保存先フォルダー | サーバーの隣にある `artifacts` フォルダー |

.NET ツールとしてインストールしたサーバーの場合、「サーバーの隣」とはツールのストアフォルダーを指すため、両方の変数を明示的に指定する必要があります。カタログがない場合、サーバーはほかの誰かが起動したプログラムに接続することしかできません。

プログラムが 1 つだけのカタログの例:

```json
{
  "applications": [
    {
      "appId": "stocksharp.terminal",
      "executable": "..\\Terminal\\Avalonia\\Terminal.Avalonia\\uiauto\\bin\\Release\\net10.0\\StockSharp.Terminal.Avalonia.exe",
      "arguments": "--ui-test",
      "description": "取引ターミナル。"
    }
  ]
}
```

- `appId` — 上の表にあるプログラムの識別子。起動したプログラムが別の識別子を通知した場合、接続は拒否されます。
- `executable` — 自動化付きビルドへのパス。相対パスはカタログファイルを基準にします。
- `arguments` — 追加の起動キー。サーバーはこれに `--ui-automation` と `--ui-automation-endpoint` を追加します。文字列は空白でキーに分割されるため、空白を含むパスはここでは渡せません。
- `description` — エージェントに表示される説明。

カタログファイルには `//` コメントを書けます。

## テストプロファイル

StockSharp のプログラムはブローカー、取引所、データソースに接続するため、エージェントからのクリックと入力を受け付けるのは、テストプロファイルで起動した場合だけです。テストプロファイルがない場合、プログラムは読み取りとスクリーンショットには応答しますが、クリック、テキスト入力、キー、スクロール、ドラッグは `inputUnavailable` エラー（`Input is delivered only to a run started on a test profile.`）で拒否します。例外は UDP Dumper です。起動時にどこにも接続しないため、入力は常に許可されます。

テストプロファイルは起動キーで有効にします。カタログでは `arguments` に記述します。

| キー | 動作 |
|---|---|
| `--ui-test` | 独立した設定フォルダーでプログラムを起動します |
| `--ui-test-dir=<パス>` | そのフォルダーを指定します。このキーがない場合、実行ファイルの隣に新しいフォルダー `ui-test\<起動の識別子>` が作成されます |
| `--ui-test-lang=<言語コード>` | インターフェイスの言語。たとえば `ru` |
| `--ui-test-view=<識別子>` | プログラムを開いたときに表示されるセクション。識別子の意味はプログラム自身が決めます |

テストプロファイルでは、プログラムはすべてをプロファイルのフォルダーに保存し、コンピューターのユーザーの設定を読み取ったり変更したりせず、既定のレイアウトで開始します。Terminal、Hydra、Designer、Installer は、アカウントへのサインイン情報が保存されている StockSharp の共通フォルダーもプロファイルに移すため、テストプロファイルではアカウントにサインインしません。Avalonia 版のプログラムでは、ファイルダイアログはプロファイルの `Files` サブフォルダーを使います。プログラムに開かせたいファイルはそこに置き、プログラムが保存したものもそこで探してください。

> [!WARNING]
> テストプロファイルは独立した設定フォルダーであって、サンドボックスではありません。プロファイルに接続設定を置いた場合や、エージェント自身が接続設定を入力した場合、プログラムは実際に接続し、エージェントからのクリックも引き続き受け付けられます。そのようなプロファイルで Terminal の買いボタンを押すと、注文が発注されます。

## エージェントの接続

Claude Code — プロジェクトの `.mcp.json` ファイル。ソースコードからビルドした場合の、サーバーの説明にある例:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "DesktopDriver.Mcp/applications.json"
      }
    }
  }
}
```

.NET ツールとしてインストールしたサーバーの場合は、`command` に `desktop-driver-mcp` へのパスを指定し、両方の変数を設定します。

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "tools/desktop-driver-mcp",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "<プログラムカタログへのパス>",
        "STOCKSHARP_UI_ARTIFACTS": "<スクリーンショットの保存先フォルダー>"
      }
    }
  }
}
```

Codex — `~/.codex/config.toml` ファイル:

```toml
[mcp_servers.desktop-driver]
command = "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe"
env = { STOCKSHARP_UI_CATALOGUE = "DesktopDriver.Mcp/applications.json" }
```

Claude Desktop — 上で説明したとおりに拡張機能をインストールします。カタログとスクリーンショットの保存先フォルダーは、その設定で指定します。

## エージェントにできること

`ui_list_applications`、`ui_start_application`、`ui_attach_application` 以外のすべてのツールは、`instance` パラメーターでプログラムを指定します。これは `ui_start_application` または `ui_attach_application` が返した名前です。同じプログラムの 2 つ目のコピーには、`#2` という接尾辞の付いた名前が付きます。

インターフェイスの要素は、`領域/識別子` という形式のアドレスで指定します。たとえば `panel:OrdersPanel/OrdersGrid` は Terminal の注文パネルにあるテーブルです。アドレスは `ui_windows`、`ui_find`、`ui_tree` が返します。パネルを別のグループへ移動したり、別のウィンドウに切り離したり、再構築したりしても、アドレスは変わりません。

### プログラム

| ツール | 動作 |
|---|---|
| `ui_list_applications` | カタログのプログラム: それぞれがビルド済みかどうか、すでに起動しているコピーはどれか |
| `ui_start_application` | カタログのプログラムを起動し、そのウィンドウが描画されるまで待ちます（最大 90 秒） |
| `ui_attach_application` | 誰かが `--ui-automation` 付きで起動したプログラムに接続します。プログラムが書き込んだアドレスファイルが必要です |
| `ui_release_application` | プログラムを解放します。このサーバーが起動したプログラムは閉じ、ほかの誰かが起動したプログラムは動作させたままにします |
| `ui_session` | どのプログラムが応答し、何ができるか: バージョン、テストプロファイル、入力方式、応答サイズの上限 |

### 画面にあるもの

| ツール | 動作 |
|---|---|
| `ui_windows` | 開いているウィンドウとポップアップウィンドウ、およびそれらのルート要素のアドレス |
| `ui_find` | 名前、種類（`grid`、`chart`、`orderBook`、`propertyEditor`、`workspace`、`button`、`textBox` など）、オートメーション識別子、またはテキストによる要素の検索 |
| `ui_tree` | 深さと件数で制限した要素のツリー |
| `ui_snapshot` | 1 つの要素の状態のすべて: 存在するか、使用可能か、表示されているか、フォーカスがあるか、さらにその種類に応じた状態（テーブルの行数と並べ替え、チャートの範囲、板情報の最良価格） |

### 要素の読み取り

| ツール | 動作 |
|---|---|
| `ui_grid_columns`、`ui_grid_rows`、`ui_grid_groups` | テーブルの列、行、グループ。並べ替え、フィルター、グループ化の後にテーブル自身が表示している順序で返します |
| `ui_chart_series`、`ui_chart_points` | チャートの系列と、実際に描画された点。時刻は UTC です |
| `ui_order_book_levels` | サイドと価格ごとの板情報のレベル |
| `ui_property_items` | プロパティエディターに表示されているプロパティ: 値、画面上のテキスト、エディターの種類、エラー |
| `ui_tree_items` | パスで指定したツリーの要素 |
| `ui_dock_layout` | ワークスペースのレイアウト: パネル、タブグループ、フローティングウィンドウ |
| `ui_document_content` | コードエディターまたは Markdown ビューアーの行。番号は 1 から始まります |
| `ui_diagram_nodes`、`ui_diagram_connections` | ストラテジーのスキームのキューブと、その間の接続（描画されているとおり） |

### 操作

| ツール | 動作 |
|---|---|
| `ui_click` | 要素またはその一部をクリックします: 列見出し、セル、パネルのタブとその閉じるボタン、リストやツリーの要素、プロパティ、チャートでは価格または注文。`Shift`、`Control`、`Alt`、`Meta` を押したままにすることもできます |
| `ui_drag` | 1 つの要素内でのドラッグ。たとえば、チャート上の注文ラインを別の価格へ移動します |
| `ui_type_text` | 1 文字ずつのテキスト入力: 既存のテキストに追加する（`Append`）か、置き換える（`Replace`） |
| `ui_press_key` | 1 つのキーを押します。たとえば `Enter`、`Escape`、`Tab`、`Delete`。`Shift`、`Control`、`Alt`、`Meta` と組み合わせることもできます |
| `ui_scroll` | 指定した目盛り数だけホイールでスクロールします |
| `ui_hover` | 要素またはその一部にポインターを合わせるだけで、何も押しません。ポインターの下でしか見えないもの（行のボタン、ツールチップ、チャートの十字カーソル）が表示されます |
| `ui_leave` | 要素があるウィンドウからポインターを外します。ウィンドウ内のどの部分もホバー状態で強調表示されなくなります。「静止状態」のウィンドウのスクリーンショットを撮る前に必要です |
| `ui_bring_into_view` | 入力を送らずに、要素の一部（たとえばテーブルのずっと下にある行）を画面に表示します |

### 結果の確認

| ツール | 動作 |
|---|---|
| `ui_wait` | 要素が表示または非表示になるまで、あるいは `ui_snapshot` の応答のフィールドが目的の値になるまで待ちます |
| `ui_action_status` | 送信済みの入力がどうなったかを、受領票の識別子で確認します |

### スクリーンショットと診断

| ツール | 動作 |
|---|---|
| `ui_screenshot` | 要素のスクリーンショットをファイルに保存します。応答はファイルへのパスとスクリーンショットのパラメーターです |
| `ui_screenshot_image` | 要素のスクリーンショットを撮り、画像としてエージェントに返します |
| `ui_diagnostics` | 自動化モジュール自体の記録: 拒否、読み取れなかったもの、配信できなかった入力 |

> [!NOTE]
> `ui_drag`、`ui_hover`、`ui_leave`、キーを押したままのクリック、チャート上の価格・注文・パネルのバツ印のクリック、およびスクリーンショットの `highlight` 枠は、サーバーのソースコードにはありますが、公開済みのバージョン 1.0.1 のパッケージにはありません。

読み取りは何も変更しません。サーバーは一度も表示されていないパネルを開いたり、プロパティを展開したり、テーブルをスクロールしたりしません。そのために応答を得られない場合は、そのことが応答自体に示されます。1 回の応答のサイズには上限（既定では 256 KB）があるため、大きなテーブルは `limit`、`startIndex`、`columns`、`rowKeys` パラメーターを使って分割して読み取ります。

## 制限とセキュリティ

- **カタログのプログラムのみ。** エージェントはカタログからプログラムを選択し、独自のファイルパスをサーバーに渡すことはできません。
- **このコンピューターとこのユーザーのみ。** チャネルにアクセスできるのは、現在の OS ユーザーのプロセスだけです。接続時、サーバーはプログラム、そのコピー、プロトコルのバージョンを確認します。
- **クリックはテストプロファイルでのみ。** テストプロファイルがない場合、プログラムは読み取りとスクリーンショットには応答しますが、入力は拒否します（「テストプロファイル」を参照）。入力が許可されているかどうかは `ui_session` で確認できます。`safeTestProfile` が `true` で、`inputBackend` が `none` 以外であれば許可されています。
- **入力はコンピューターのマウスとキーボードを使いません。** イベントはプログラム内部の入力パイプラインに渡されるため、フォーカスやハンドラーは人が押した場合と同じように動作します。要素全体に対する通常の左クリックは、その要素のオートメーション操作として実行されます。送信の前に、ドライバーは要素が表示され、使用可能で、何にも覆われていないことを確認します。
- **「配信済み」は「実行済み」を意味しません。** `dispatched` という受領票は、入力がプログラムに届いたことを意味するだけです。プログラムが期待どおりに動作したかどうかは、`ui_snapshot` と `ui_wait` で確認します。
- **ポインターは人と同じように動きます。** ポインターによる操作のたびにポインターは新しい位置に移動し、ポインターが離れた要素と移動先の要素は、本物のマウスの場合と同じようにそれを認識します。強調表示されたボタンは元に戻り、ポインターがチャートから離れるとチャートの十字カーソルは消えます。
- **WPF 版** は、キーを押したままのクリック、ドラッグ、`ui_hover`、`ui_leave` を単純なクリックで代用せず、拒否します。
- **チャートからの注文。** チャート上の価格を `Control` を押しながらクリックすると、注文が発注されます。接続が設定されたテストプロファイルでは、これは実際の注文です。
- **保存せずに閉じます。** `ui_release_application` は、このサーバーが起動したプログラムを、何も保存を提案せずに閉じます。サーバーが接続しただけのプログラムは閉じません。
- **スクリーンショット** は `STOCKSHARP_UI_ARTIFACTS` フォルダーに、1 枚につき `<スクリーンショットの識別子>.png` ファイル 1 つとして書き込まれます。`maxPixelWidth` または `maxPixelHeight` の上限が指定されている場合、大きすぎるスクリーンショットは縮小されずに拒否されます。

## セッションの例

Terminal は自動化付きでビルドされ、カタログは上の例のとおりです。ユーザーは「Terminal で注文パネルの設定を開いて、どんなフィールドがあるか見せて」と依頼します。

1. `ui_list_applications` — カタログに `stocksharp.terminal` があり、ビルド済みで、起動していません。
2. `ui_start_application`、`appId` = `stocksharp.terminal` — サーバーは Terminal をテストプロファイルで起動し、`instance` = `stocksharp.terminal` を返します。
3. `ui_session` — `safeTestProfile` = `true`、入力は許可されています。
4. `ui_wait`、`node` = `panel:BuySellPanel/SettingsButton`、`timeoutSeconds` = `180` — Terminal がワークスペースを構築するまで待ちます。ウィンドウはパネルより先に表示されます。
5. `ui_click`、`node` = `panel:BuySellPanel/SettingsButton` — 注文パネルの設定を開きます。受領票は `dispatched` です。
6. `ui_property_items`、`node` = `panel:BuySellPanel/SettingsPropertyGrid` — 現在の値付きのプロパティ。その中に `Security`（銘柄）と `Depth` があります。
7. 同じ `node` で `ui_screenshot` — エディターのスクリーンショットのファイルへのパス。ユーザーに見せるためのものです。
8. `ui_release_application` — Terminal が閉じます。

## 推奨コンテンツ

[AI エージェントの接続（MCP）](../mcp.md)

[プログラムのコマンド（Studio.Mcp）](studio_mcp.md)
