# ストラテジー

`StrategiesWidget` は実行中のストラテジーの一覧を表示します。ストラテジー 1 つにつき 1 行で、状態、取引モード、ポジション、注文と約定のカウンター、損益、操作ボタンを並べます。コントロールの識別子は `strategies`（`ControlTypes.Strategies`）で、静的プロパティ `StrategiesWidget.TYPE` からも取得できます。

![状態、ポジション、PnL、損益カーブを表示したストラテジー一覧](../../../../images/javascript_controls_strategies.png)

## 作成と更新

```ts
import {
  StrategiesWidget,
  StrategyStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let strategies!: StrategiesWidget;

strategies = StrategiesWidget.create(
  document.querySelector<HTMLElement>('#strategies')!,
  {},
  {
    host,
    tradingModes: ['Disabled', 'CancelOrders', 'ReducePosition', 'Full'],
    start: id => console.log('start', id),
    stop: id => console.log('stop', id),
    closePosition: id => console.log('flatten', id),
    openStrategy: id => console.log('open', id),
    riskRules: id => console.log('risk', id),
    setTradingMode: (id, mode) => console.log('mode', id, mode),
  },
);

strategies.update([{
  id: 'sma-1',
  name: 'SMA crossover',
  state: StrategyStates.Started,
  online: true,
  tradingMode: 'Full',
  portfolio: 'Demo',
  security: 'BTC@IMEX',
  position: 0.25,
  ordersCount: 12,
  tradesCount: 8,
  pnlChange: 105,
  realized: 20,
  unrealized: 85,
  pnl: [
    { time: 1, value: 0 },
    { time: 2, value: 60 },
    { time: 3, value: 105 },
  ],
}]);
```

`update` は行の完全なセットを受け取り、テーブルをそれで置き換えます。渡されたリストに含まれないストラテジーは削除されたものと見なされ、その行は消えます。1 行だけをストリーミングで更新する仕組みはなく、新しい状態はリスト全体として届きます。

`create` の第 2 引数はインスタンスの保存状態です。コントロールはこれを読み取らず、自分でも何も保存しません。`host.preferences` にキーを置くことも、`host.persistState` を呼ぶこともありません。

`StrategyStates` オブジェクトは、状態 `Stopped`、`Starting`、`Started`、`Stopping` をエクスポートします。

## 依存関係

必須なのはホストだけです。ホストが完全かどうかは作成時に `assertHost` が検証するため、不完全な `TradingHost` はボタンが動かないという結果ではなく、欠落しているメンバー名を示す例外になります。

| 依存関係 | 必須かどうか | 既定の動作 |
|---|---|---|
| `host` | 必須 | — |
| `start(id)` | 任意 | 開始ボタンが作成されません。 |
| `stop(id)` | 任意 | 停止ボタンが作成されません。 |
| `closePosition(id)` | 任意 | ポジション列には数値だけが残ります。 |
| `openStrategy(id)` | 任意 | ストラテジーへ移動するボタンが作成されません。 |
| `riskRules(id)` | 任意 | リスクルールのボタンが作成されません。 |
| `setTradingMode(id, mode)` | 任意 | 取引モードはテキストで表示されます。 |
| `tradingModes` | 任意 | 空のリストとなり、モードのドロップダウンが作成されません。 |

この構成により、読み取り専用のパネルを組むこともできます。操作用の関数を 1 つも渡さなければ、テーブルはデータを表示するだけで、ボタンは 1 つも現れません。

`tradingModes` の文字列は `host.t` を通るため、翻訳キーとして機能します。これらはパッケージではなくホストに属するもので、`translation-keys.json` に含まれていないのが正常であり、翻訳はホストが行います。

## 状態と操作

状態セルはドットと単語で構成されます。ドットは一覧をざっと見るときに読み取れ、単語は `Starting` と `Started` を区別します。行の `error` フィールドが埋まっている場合、エラーテキストはドットと単語の両方のツールチップに入り、障害で停止したストラテジーは「停止」ではなく「エラー」という語で示されます。

行のボタンは、ホストから渡された関数についてのみ作成され、状態が許す場合にのみ有効になります。

- 開始 — `Stopped` 状態のストラテジーのみ。
- 停止 — `Started` 状態のストラテジーのみ。
- ポジションの決済 — 動作中でポジションがゼロでないストラテジーのみ。
- リスクルールとストラテジーへの移動 — 常に有効。

取引モードのドロップダウンが有効になるのは、停止中のストラテジーだけです。モードはストラテジーをどの条件で起動するかを決めるものであり、取引中に操作するレバーではありません。モードの変更は `setTradingMode` を呼び出します。コントロールが行の値を自分で書き換えることはなく、次の `update` を待ちます。

## 列と表示

テーブルには、状態、操作、オンライン表示、取引モード、名前、ポートフォリオ、銘柄、ポジション、注文数と約定数、損益の変化、損益グラフ、実現損益と未実現損益、エラーが表示されます。オンライン表示は総合的なもので、ストラテジーが構築済みかつ接続済みである場合にのみオンラインと見なされ、それを判断するのはデータプロバイダーです。

ポジション、損益の変化、および 2 種類の損益の色クラスは `host.presentation.pnlClass` が返します。損益の変化には方向を示す矢印も付きます。変化がゼロの場合、矢印は付きません。

グラフ列は、`pnl` のポイントから累積損益のカーブを 140 × 26 CSS ピクセルの領域に描画します。キャンバスは `devicePixelRatio` を考慮して作成されるため、高精細なディスプレイでもラインは鮮明なままです。色は `host.presentation.canvasPalette()` から取得され、カーブは実行の最終結果に応じて着色されます。ピークまで伸びてすべてを戻したストラテジーは、損失として表示されます。`pnl` のポイントがない場合、セルは空のままです。

既定の並べ替えは名前の昇順です。一覧は特定のストラテジーを探して上から下へ読むものであり、損益に合わせて行が入れ替わるとその読み方の妨げになります。パネルは複数行の選択、コンテキストメニュー、フィルター、XLSX へのエクスポートにも対応しています。

## ホストに委ねられること

コントロールはストラテジーを起動も停止もせず、注文も送らず、ポジションも決済しません。渡された関数を呼び出し、新しい行のリストを待つだけです。

`pnlChange` の値はそのまま受け取ります。基準となる時点はデータプロバイダーが選びます。そのため、再起動されたストラテジーが再起動前の時点からの変化を計算し続けることはありません。

パネルの閉じるボタンは `host.close()` を呼び出し、エクスポートは `strategies` というファイルを出力します。インスタンスは作成時にホストへ登録され、`dispose` で登録解除されます。

## 公開メソッド

- `StrategiesWidget.create(hostEl, state, deps)` — パネルを構築してコンテナーに追加します。
- `update(rows)` — ストラテジーの一覧全体を置き換えます。
- `dispose()` — 登録を解除し、テーブルを削除してリソースを解放します。

## 関連項目

- [JavaScript トレーディングコントロール](../trading_controls.md)
- [ポジション](positions.md)
- [有効注文](active_orders.md)
- [約定履歴](trade_history.md)
