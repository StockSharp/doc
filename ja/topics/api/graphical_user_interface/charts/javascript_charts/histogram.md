# ヒストグラム

ヒストグラムは、基準値から各ポイントごとに垂直のバーを描画します。最も一般的な用途は出来高で、各バーはローソク足が上昇して引けたか下降して引けたかによって色付けされますが、バーごとの任意の数量にも利用できます。

## ライブデモ

```chart-demo histogram
```

## セットアップ

`HistogramSeries` を追加し、`{ time, value, color? }` のポイントを渡します。ポイントごとの `color` はシリーズの色を上書きします。

```js
const series = chart.addSeries(SSChart.HistogramSeries, {
  priceFormat: { type: 'volume' },
});

series.setData(candles.map(c => ({
  time: c.time,
  value: c.volume,
  color: c.close >= c.open ? 'rgba(38,166,154,0.7)' : 'rgba(239,83,80,0.7)',
})));

chart.timeScale().fitContent();
```

出来高を単独で表示するのではなく価格チャートの下に表示するには、ヒストグラムをオーバーレイの価格スケールに配置し、下部に固定します。

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [エリア](area.md)
- [ローソク足](candlestick.md)
