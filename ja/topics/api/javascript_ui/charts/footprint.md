# フットプリント

フットプリントチャートは各バーを展開し、その内部の各価格で成立した出来高をビッドとアスクに分けて表示します。インバランスの色分けにより、攻撃的な買い手または売り手が優勢だった箇所が強調されます。これがオーダーフロー分析の核心です。

## ライブデモ

個々のセルを読み取るにはズームインしてください。フットプリントは一度に数本のバーだけを表示したときにのみ判読可能です。

```chart-demo footprint
```

![フットプリントチャート](../../../../images/chart_footprint.png)

## セットアップ

`FootprintSeries` を追加し、正確なオーダーフローバーを供給します。各バーは `dataMode: 'exact'`、そのOHLC、および `{ price, bidVolume, askVolume, tradeCount }` からなる `levels` 配列を持ちます。

```js
const series = chart.addSeries(SSChart.FootprintSeries, {
  tickSize: 0.25,
  mode: SSChart.FootprintDisplayMode.BidAsk,       // 表示モード: BidAsk | Delta | Total | Ladder
  detailLevel: SSChart.FootprintDetailLevel.Auto,  // 詳細度: Auto | Numbers | Heatmap | Summary
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showUnfinishedAuctions: true,
});

series.setData(exactBars);
```

`mode` は各セルが何を表示するか（ビッド×アスク、デルタ、合計、またはラダー）を選択します。`detailLevel` はズームに応じて数値的な詳細度と密度のバランスを取り、`Auto` は自動的に切り替わります。

## 関連項目

- [JavaScript チャート](../charts.md)
- [ボリュームプロファイル](volume_profile.md)
- [TPO（マーケットプロファイル）](tpo.md)
