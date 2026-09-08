# ログ

`LogMonitorWidget` は動作ログを表示します。左側にソースのツリー、右側に選択したソースとその配下すべてのメッセージを並べたテーブルがあります。メッセージは 1 度だけ保存され、それを書き込んだソースの識別子を保持します。保持する行数には上限があります。

![ソースツリー、レベルフィルター、メッセージテーブルを備えたログ](../../../../images/javascript_controls_log_monitor.png)

## 作成と更新

```ts
import {
  LogLevels,
  LogMonitorWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const log = LogMonitorWidget.create(
  document.querySelector<HTMLElement>('#log')!,
  {},
  {
    host,
    maxMessages: 20_000,
  },
);

log.setSources([
  { id: 'connector', name: 'Connector' },
  { id: 'strategy-1', name: 'SMA', parentId: 'connector' },
]);

log.append([{
  id: 1,
  time: Date.now(),
  level: LogLevels.Warning,
  sourceId: 'strategy-1',
  message: '注文が拒否されました: 資金が不足しています',
}]);

log.select('connector');
```

依存関係のうち必須なのは `host` だけです。ログは書き込む対象であって、そこから操作を起こすものではないため、このコントロールにハンドラーは必要ありません。残りは任意です。

| 依存関係 | 既定値 | 用途 |
|---|---|---|
| `maxMessages` | `5000` | 保持するメッセージ数。超過分はリストの先頭から破棄されます。 |
| `chrome` | `true` | 閉じるボタン付きの独自ヘッダーを描画するかどうか。パネルの見出しを自分で付け、（ドッキングのタブなどで）自分で閉じるホストは `false` を渡します。 |
| `sources` | `true` | 作成時にソースツリーを表示するかどうか。これは初期状態にすぎず、ツリーはテーブルのコンテキストメニューまたは `showSources` の呼び出しで戻せます。 |

`setSources` はソースのリストを丸ごと渡します。消えたソースはツリーから取り除かれ、そのとき選択は「すべてのソース」に戻ります。`append` は記録されたばかりのものを追加し、`clear` はソースツリーをそのままにメッセージだけをすべて忘れます。

静的プロパティ `LogMonitorWidget.TYPE` には、コントロールの識別子 `logMonitor` が入っています。

## ソースとフィルター

ソースは子ではなく親（`parentId`）を宣言し、親より先に現れることもあります。ツリーはすでに届いたものから組み立てられます。親が不明なソースはルートになり、循環は最初のノードで切断されます。ツリーの行はフラットで、階層はインデントで示されます。最上段の行はすべてのソースをまとめて選択します。

フィルターは 3 つが同時に働きます。有効なレベルの集合、メッセージ本文に対する検索文字列（大文字小文字を区別しません）、そして選択されたソースのサブツリーです。レベルはツールバーのボタンで切り替えます。`LogLevels` オブジェクトの `error`、`warning`、`info`、`debug`、`verbose` で、幅の狭い列ではレベルが `E`、`W`、`I`、`D`、`V` の 1 文字で表示されます。`visible` メソッドは、すべてのフィルターを通過して残ったものを返します。

テーブルはソース、時刻、レベル、メッセージの列で構成され、時刻の昇順で並び、複数選択、並べ替え、列の非表示、フィルター、コンテキストメニューに対応します。メニューにはソースツリーを表示する項目が追加されています。ツールバーのボタンは、表示されている行を XLSX に出力します。出力には `host.presentation.timeText` で整形された時刻と、1 文字ではなくレベルの完全な名前が含まれます。

## ホストの役割

コントロールはホストから翻訳（`host.t`）、時刻の書式（`host.presentation.timeText`）、パネルを閉じる処理（`host.close`）を受け取り、さらに `host.register` で登録され、`dispose` で登録解除されます。`host.preferences` に独自の設定は保存せず、インスタンスの状態も保存しません。`create` の第 2 引数は統一のために受け取るだけで、読み取られることはありません。メッセージの収集、その配信、パネル位置の復元はホストが担います。

## 公開メソッド

- `setSources(sources)` — ソースのリストを置き換えます。
- `append(messages)` — `maxMessages` の上限を考慮してメッセージを追加します。
- `clear()` — メッセージを消去します。
- `select(sourceId)` — ソースのサブツリーを表示します。`null` はすべてを表示します。
- `visible()` — すべてのフィルターを通過して残ったメッセージ。
- `sourcesShown()` — ソースツリーが表示されているかどうか。
- `showSources(on)` — ツリーを表示または非表示にします。選択されているソースフィルターはそのまま保たれます。
- `dispose()` — リソースを解放します。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [ストラテジー](strategies.md)
- [統計](statistics.md)
