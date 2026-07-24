# バンド

バンドシリーズは上限と下限の2本の境界線を描き、その間のチャネルを塗りつぶします。エンベロープやボリンジャーバンド、あるいは価格コリドーを生成するあらゆるスタディに自然に適合します。

## ライブデモ

```chart-demo band
```

## セットアップ

`BandSeries` を追加し、`{ time, upper, lower }` のポイントを与えます。

```js
const band = chart.addSeries(SSChart.BandSeries, {
  upperColor: '#26a69a',
  lowerColor: '#ef5350',
  fillColor: 'rgba(74,158,255,0.10)',
});

band.setData(data.map(d => ({ time: d.time, upper: d.upper, lower: d.lower })));

chart.timeScale().fitContent();
```

`upper` / `lower` は、スタディが要求する方法で計算します。ボリンジャーバンドの場合は、終値の移動平均を取り、その標準偏差の倍数を加算・減算します。価格そのものを保持するラインシリーズまたはローソク足シリーズにバンドを重ねて表示します。

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [ライン](line.md)
- [ボリュームプロファイル](volume_profile.md)
