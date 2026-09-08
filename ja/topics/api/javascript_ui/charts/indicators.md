# インジケーター

約160種類のテクニカルインジケーターのカタログは、独立したパッケージ [@stocksharp/indicators](https://www.npmjs.com/package/@stocksharp/indicators) にあります。チャートの依存関係として一緒にインストールされますが、定義のインポート元はこのパッケージです。どの定義も公開クラス `IndicatorRuntime` がローソク足に対して計算し、その結果は通常のシリーズとして自分で描画します。価格ペイン上のオーバーレイとして、あるいは独自のサブペイン内のオシレーターとして表示できます。

## ライブデモ

ローソク足に重ねたボリンジャーバンドと、その下の別ペインに表示した RSI と MACD。

```chart-demo indicators
```

## セットアップ

ランタイムと必要な定義をインポートし、それぞれをローソク足に対して実行して出力をプロットします。ランタイムには `{ time, value }` 形式の入力を渡します。ここで `value` はローソク足です。

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import {
  IndicatorRuntime,
  BollingerBandsIndicator,
  RelativeStrengthIndexIndicator,
  MacdHistogramIndicator,
} from '@stocksharp/indicators';

// ローソク足に対してインジケーターを1つ計算し、出力 id ごとにグループ化したポイントを返します。
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of runtime.points()) {
    if (p.time == null || p.value == null) continue;   // ウォームアップ中のバーは何も出力しません
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// ボリンジャーバンド (20, 2) を価格ペイン上のオーバーレイのエンベロープとして表示します。
const bb = compute(BollingerBandsIndicator, { length: 20, width: 2 }, candles);   // 出力: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) を独自のサブペインに表示します。
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);     // 出力: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) を2つ目のサブペインに表示します。
const macdPane = chart.addPane();
const macd = compute(
  MacdHistogramIndicator,
  { shortMaLength: 12, longMaLength: 26, signalMaLength: 9 },                    // 出力: macd, signal, histogram
  candles);
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

## パラメーターと出力

各定義は独自のパラメーターと出力 id を宣言しており、説明的な名前ではなく、これらの id で参照する必要があります。

| 定義 | パラメーター | 出力 | ペイン |
|---|---|---|---|
| `BollingerBandsIndicator` | `length` (20)、`width` (2)、`upBandWidth`、`lowBandWidth` | `upper`、`middle`、`lower` | オーバーレイ |
| `RelativeStrengthIndexIndicator` | `length` (15) | `oscillator` | 独立 |
| `MacdHistogramIndicator` | `shortMaLength` (12)、`longMaLength` (26)、`signalMaLength` (9) | `macd`、`signal`、`histogram` | 独立 |

> [!CAUTION]
> 未知のパラメーター id は**警告なく破棄され**、インジケーターはデフォルト値で計算されます。エラーは出ず、間違ったラインが描かれるだけです。同じ理由で、ボリンジャーバンドの偏差は `stdDev` ではなく `width` キーで指定し、RSI のデフォルト長は 14 ではなく 15 です。よく使われる期間が必要なら、上の例のように明示的に渡してください。

定義とそのパラメーターの完全な一覧はカタログから取得できます。

```js
import { getClientCatalog } from '@stocksharp/indicators';

for (const item of getClientCatalog()) {
  console.log(item.id, item.name, item.parameters.map(p => p.id));
}
```

## リアルタイムデータ

バーごとに全体を再計算する必要はありません。`IndicatorRuntime` を保持したまま `update` を呼び出すと、変化した内容だけを含むパッチが返ります。

```js
const runtime = new IndicatorRuntime({ definition: RelativeStrengthIndexIndicator, parameters: { length: 14 } });
runtime.resetStreaming(history.map(c => ({ time: c.time, value: c })));

// 未確定のバー: false は、次の呼び出しで置き換えられる暫定値であることを意味します。
runtime.update({ time: bar.time, value: bar }, false);

// バーが確定し、値が最終的なものになります。
const patch = runtime.update({ time: bar.time, value: bar }, true);
```

バーが確定するまで、ランタイムは暫定ポイントを保持し、呼び出しのたびにそれを置き換えます。そのため、同じバー内の更新で履歴が増えることはありません。`discardPreview()` は暫定ポイントを取り除き、`correct(index, input)` は修正されて届いた過去のバーを再計算します。

## 関連情報

- [JavaScript チャート](../charts.md)
- [バンド](band.md)
- [履歴のバックフィル](backfill.md)
