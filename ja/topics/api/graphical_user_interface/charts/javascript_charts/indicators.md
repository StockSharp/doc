# インジケーター

チャートには約160種類のテクニカルインジケーターのカタログが付属しています。公開クラス `IndicatorRuntime` を使ってローソク足に対して任意のインジケーターを計算し、その結果を通常のシリーズとして自分で描画します。価格ペイン上のオーバーレイとして、あるいは独自のサブペイン内のオシレーターとして表示できます。

## ライブデモ

ローソク足に重ねたボリンジャーバンドと、その下の別ペインに表示した RSI と MACD。

```chart-demo indicators
```

## セットアップ

ランタイムと必要なインジケーター定義をインポートし、それぞれをローソク足に対して実行して出力をプロットします。ランタイムには `{ time, value }` 形式の入力を渡します。ここで `value` はローソク足です。

```js
import { createChart, CandlestickSeries, LineSeries, HistogramSeries, BandSeries } from '@stocksharp/chart';
import { IndicatorRuntime, BollingerBandsIndicator, RelativeStrengthIndexIndicator, MacdIndicator } from '@stocksharp/chart/indicators';

// Compute one indicator over the candles; returns its points grouped by output id.
function compute(definition, parameters, candles) {
  const runtime = new IndicatorRuntime({ definition, parameters });
  const points = runtime.resetStreaming(candles.map(c => ({ time: c.time, value: c })));
  const out = {};
  for (const p of points) {
    if (p.time == null || p.value == null) continue;   // warm-up bars emit nothing
    (out[p.outputId] ||= []).push({ time: p.time, value: p.value });
  }
  return out;
}

const chart = createChart(document.getElementById('chart'), {});
const price = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });
price.setData(candles);

// Bollinger Bands (20, 2) as an overlay envelope on the price pane.
const bb = compute(BollingerBandsIndicator, { length: 20, stdDev: 2 }, candles);   // outputs: upper, middle, lower
chart.addSeries(BandSeries, { upperColor: '#42a5f5', lowerColor: '#42a5f5' })
  .setData(bb.upper.map((u, i) => ({ time: u.time, value: bb.middle[i].value, upper: u.value, lower: bb.lower[i].value })));

// RSI (14) in its own sub-pane.
const rsiPane = chart.addPane();
const rsi = compute(RelativeStrengthIndexIndicator, { length: 14 }, candles);       // output: oscillator
chart.addSeries(LineSeries, { color: '#4a9eff', lineWidth: 2 }, rsiPane).setData(rsi.oscillator);

// MACD (12, 26, 9) in a second sub-pane.
const macdPane = chart.addPane();
const macd = compute(MacdIndicator, { fastLength: 12, slowLength: 26, signalLength: 9 }, candles);   // outputs: macd, signal, histogram
chart.addSeries(HistogramSeries, {}, macdPane).setData(macd.histogram);
chart.addSeries(LineSeries, { color: '#4a9eff' }, macdPane).setData(macd.macd);
chart.addSeries(LineSeries, { color: '#f5c542' }, macdPane).setData(macd.signal);

chart.timeScale().fitContent();
```

各定義は独自のパラメーターと出力 id を宣言します（ボリンジャーは `upper`/`middle`/`lower` を出力し、RSI は単一の `oscillator`、MACD は `macd`/`signal`/`histogram` を出力します）。リアルタイムデータの場合は `IndicatorRuntime` を保持し、再計算する代わりにバーごとに `runtime.update({ time, value }, isFinal)` を呼び出します。

## 関連情報

- [JavaScript チャート](../javascript_charts.md)
- [バンド](band.md)
- [ヒストリーのバックフィル](backfill.md)
