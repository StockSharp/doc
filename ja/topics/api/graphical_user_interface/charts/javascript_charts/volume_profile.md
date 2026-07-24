# ボリュームプロファイル

ボリュームプロファイルは、約定した出来高を価格ごとに集計し、水平ヒストグラムとして描画します。ポイント・オブ・コントロール（最も多く約定した価格）とバリューエリアを示します。「いつ」約定したかとは無関係に、「どこで取引が成立したか」を明らかにします。

## ライブデモ

プロファイルは表示中のバー全体を対象に再計算されます。スクロールやズームをして変化を確認してください。

```chart-demo volume-profile
```

![ポイント・オブ・コントロールとバリューエリアを備えたボリュームプロファイル](../../../../../images/chart_volume_profile.png)

## セットアップ

（コンテキストのために通常はローソク足シリーズの上に）`ExactVolumeProfileSeries` を追加し、フットプリントで使用したものとまったく同じオーダーフローのバーを与えます。

```js
const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
});
candles.setData(exactBars);

const profile = chart.addSeries(SSChart.ExactVolumeProfileSeries, {
  tickSize: 0.25,
  rangeMode: SSChart.VolumeProfileRangeMode.Visible,     // 範囲モード: Visible | Fixed | Session
  displayMode: SSChart.VolumeProfileDisplayMode.BidAsk,  // 表示モード: Total | BidAsk | Delta
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showLabels: true,
  profileWidth: 0.32,
});
profile.setData(exactBars);

chart.timeScale().fitContent();
```

`rangeMode` はプロファイルが対象とする範囲を設定します。`Visible` はビューポート全体で再計算し、`Fixed` は 1 つの範囲に固定し、`Session` はセッションごとに 1 つのプロファイルを構築します。

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [フットプリント](footprint.md)
- [TPO（マーケットプロファイル）](tpo.md)
