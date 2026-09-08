# チャートからの取引

`TradingLayer` は、チャートの取引状態を扱うブローカー非依存のレイヤーです。正規化された注文、ポジション、約定、気配値を保持し、ユーザーの操作は意図（`TradingIntent`）として外部へ渡します。レイヤー自身は何も送信しません。トランスポートも口座も再送も持たず、ブローカーとのやり取りはホストに残ります。

![チャート上に重ねた注文ライン、ポジション、保護注文](../../../../images/javascript_charts_trading.png)

## 組み込み

このレイヤーは、[@stocksharp/chart](https://www.npmjs.com/package/@stocksharp/chart) パッケージの独立したエントリーポイントとして提供されます。

```ts
import { TradingLayer, TradingLayerPrimitive } from '@stocksharp/chart/trading';
```

バンドラーを使わない場合、同じクラスはグローバルオブジェクト `SSChart` から利用できます（`new SSChart.TradingLayer({ tickSize: 0.25 })`）。

## 作成と更新

レイヤーは価格グリッドのパラメーターとともに作成します。プリミティブ `TradingLayerPrimitive` がその状態をシリーズの上に描画し、`TradingOrderPlacementAdapter` がチャート上のクリックを発注の意図に変換します。

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import {
  TradingLayer,
  TradingLayerPrimitive,
  TradingOrderPlacementAdapter,
  type TradingIntent,
} from '@stocksharp/chart/trading';

declare const broker: { send(intent: TradingIntent): Promise<void> };

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {
  timeScale: { timeVisible: true },
});
const candles = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const layer = new TradingLayer({ tickSize: 0.25 });

chart.attachPrimitive(new TradingLayerPrimitive(layer, { showInactiveOrders: false }), {
  series: candles,
});

// ブローカー側の正本の状態。チャートはそれを表示するだけで、自分で変更することはありません。
layer.setOrders([{
  id: 'ob1',
  side: 'buy',
  type: 'limit',
  status: 'working',
  timeInForce: 'good-till-cancelled',
  quantity: 2,
  filledQuantity: 0,
  price: 96,
  revision: 1,
  permissions: { canModify: true, canCancel: true },
  label: 'BID',
}]);

layer.setPositions([{
  id: 'pos1',
  side: 'long',
  quantity: 3,
  averagePrice: 100,
  revision: 1,
  pnl: { realized: 0, unrealized: 45, currency: 'USD', markPrice: 101.5 },
  permissions: { canClose: true, canReverse: true, canProtect: true },
}]);

layer.setQuote({ time: 1704326400, bidPrice: 100.75, bidSize: 5, askPrice: 101, askSize: 4 });

// 意図はホストへ渡され、ホストはその結果をレイヤーに返します。
layer.subscribeIntents(intent => {
  broker.send(intent).then(
    () => layer.resolveIntent({ intentId: intent.intentId, status: 'accepted' }),
    (error: Error) => layer.resolveIntent({
      intentId: intent.intentId,
      status: 'rejected',
      reason: error.message,
    }),
  );
});

// Ctrl + 左ボタンで指値買い、Ctrl + 右ボタンで指値売り。
new TradingOrderPlacementAdapter(chart, layer, { quantity: 1, orderType: 'limit', modifier: 'ctrl' });
```

`setOrders`、`setPositions`、`setExecutions`、`setQuote` は、対応するコレクションを丸ごと置き換えます。各呼び出しは正規化され、現在の状態と比較されます。何も変わっていなければ購読者は呼ばれず、変わっていれば `added`、`updated`（`previous` / `current` の組）、`removed`、`orderChanged` のフィールドを持つ変更が公開されます。気配値は例外で、その変更には `previous` と `current` しかありません。現在のスナップショットは `state()` が返します。`{ version, orders, positions, executions, quote }` です。

正規化は厳格です。価格は `tickSize` のグリッド（`priceOrigin` のオフセット付き）に乗っている必要があり、数量は `quantityStep` が指定されていればそれに乗っている必要があります。指値注文には `price`、逆指値注文には `stopPrice`、ストップリミット注文には両方のフィールドが必要です。整合しないデータは、黙って修正されるのではなく例外で拒否されます。

## 意図

レイヤーはユーザーの操作を実行せず、意図として公開します。`request*` メソッドは意図オブジェクトを返し、それを待機中のキューに入れて `subscribeIntents` の購読者へ渡します。ホストはその意図をブローカーで実行し、`resolveIntent({ intentId, status, reason })` を `accepted` または `rejected` のステータスで呼び出して終了させます。結果は元の意図とともに `subscribeIntentOutcomes` に届きます。待機中の意図は `pendingIntents()` が列挙します。

権限は公開の前に検証されます。注文の変更には `permissions.canModify`、取り消しには `canCancel`、ポジションに対する操作には `canClose`、`canReverse`、`canProtect` が必要です。権限のない注文は読み取り専用と見なされ、呼び出しは例外を投げます。意図には正本のエンティティから `expectedRevision` が差し込まれ、古いデータに基づいて組み立てられた要求をブローカーが拒否できるようになっています。

## 描画とドラッグ

`TradingLayerPrimitive` はチャートのプリミティブで、接続時にレイヤーを購読し、ラベル付きの注文ライン、P&L を伴うポジションライン、約定マーカー、bid/ask/last のライン、ブラケットの結線を描画します。何をどの色で表示するかは、コンストラクターのオプションと `applyOptions` で指定します。`showOrders`、`showInactiveOrders`、`showPositions`、`showExecutions`、`showExecutionLabels`、`showQuote`、`showPnl`、`showBrackets`、`autoscale`、色の一式（`orderBuyColor`、`orderSellColor`、`inactiveOrderColor`、`longPositionColor`、`shortPositionColor`、`executionBuyColor`、`executionSellColor`、`bidColor`、`askColor`、`lastColor`、`bracketColor`）、`lineWidth`、`fontSize`、`orderLabelSpacing`、`zOrder`、そしてフォーマッター `priceFormatter`、`quantityFormatter`、`pnlFormatter` です。識別子 `id` は作成時に一度だけ指定し、後から変更されることはありません。

注文ラインは、その注文がアクティブ（`pending`、`working`、`partially-filled`）で、成行ではなく、`canModify` の権限を持つ場合にマウスでドラッグできます。ドラッグ中、プリミティブはグリッドにスナップした仮の価格を表示します。ボタンを離すと `requestModifyOrder` の意図が公開され、プレビューはホストがその意図を終了させるまで保たれます。拒否された場合、ラインは正本の価格に戻ります。

カーソルのヒットは `TradingOrderHitData`、`TradingPositionHitData`、`TradingExecutionHitData`、`TradingQuoteHitData` で表されます。ハンドラー内でこれらを判別するには `isTradingPrimitiveHitData(value)` が役立ちます。

## マウスによる発注

`TradingOrderPlacementAdapter` は、チャート標準の配置シグナルをレイヤーに結び付けます。配置モードを有効にし、クリックを受け取り、`requestPlaceOrder` を呼び出します。このアダプター自身は価格ラインを作成せず、ブローカーとも通信しません。

オプション: `quantity`（必須）、`orderType` は `'limit'` または `'stop'`（既定は `'limit'`）、`timeInForce`（既定は `'good-till-cancelled'`）、`modifier` は `'ctrl'`、`'shift'`、`'alt'`（既定は `'ctrl'`）、`color`、`title`、`enabled`、`sideResolver`。既定のリゾルバーは左ボタンで買い、右ボタンで売りを返し、`null` は配置を取り消します。アダプターは `options()`、`applyOptions(patch)`、`setEnabled(enabled)`、`dispose()` のメソッドで制御します。

## レイヤーの公開メソッド

- `setOrders(orders)`、`setPositions(positions)`、`setExecutions(executions)`、`setQuote(quote)` — 正本の状態を置き換えます。`setQuote(null)` は気配値を取り除きます。
- `state()` — バージョン番号を伴う現在の状態のスナップショット。
- `normalizationOptions()` — レイヤーの作成に使われた価格グリッドのパラメーター。
- `subscribeChanges(handler)`、`subscribeIntents(handler)`、`subscribeIntentOutcomes(handler)` — 購読。いずれも購読解除用の関数を返します。
- `pendingIntents()`、`resolveIntent(resolution)` — 待機中の意図のキューと、その終了処理。
- `requestPlaceOrder(order)`、`requestModifyOrder(orderId, changes)`、`requestCancelOrder(orderId)` — 注文の操作。
- `requestClosePosition(positionId, quantity)`、`requestReversePosition(positionId, quantity)` — ポジションの操作。数量を指定しない場合はポジション全体が対象です。
- `requestCreateStopLoss`、`requestEditStopLoss`、`requestRemoveStopLoss`、`requestCreateTakeProfit`、`requestEditTakeProfit`、`requestRemoveTakeProfit` — ブラケットの保護注文。
- `dispose()` — リソースを解放します。

## その他のエクスポート

- モデルの列挙型: `TradingSide`、`ChartOrderType`、`ChartOrderStatus`、`ChartOrderTimeInForce`、`ChartPositionSide`、`ChartBracketRole`、`ChartExecutionLiquidity`、`TradingIntentKind`。
- レイヤーとプリミティブの列挙型: `TradingLayerChangeKind`、`TradingIntentOutcomeStatus`、`TradingPrimitiveEntityKind`、`TradingQuoteKind`。
- データの検証と正規化: `normalizeChartOrder`、`normalizeChartOrders`、`normalizeChartPosition`、`normalizeChartPositions`、`normalizeChartExecution`、`normalizeChartExecutions`、`normalizeChartQuote`、`normalizeChartOrderRequest`、`normalizeTradingIntent`、`normalizeTradingModelOptions`。
- 補助的な計算: `quantizeTradingPrice` は任意の価格をグリッドにスナップし、`chartOrderRemainingQuantity` は注文の未約定残量を、`chartPnlTotal` は実現 P&L と未実現 P&L の合計を返します。
- 型 `ChartOrder`、`ChartPosition`、`ChartExecution`、`ChartQuote`、`ChartOrderRequest`、`ChartOrderModification`、`TradingIntent` と、そこから派生する意図のインターフェイス。

## 関連項目

- [JavaScript チャート](../charts.md)
- [ローソク足](candlestick.md)
- [履歴のバックフィル](backfill.md)
