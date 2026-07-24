# 足迹图（Footprint）

足迹图会展开每一根柱子，显示柱内每个价位上的成交量，并按买价（bid）和卖价（ask）拆分。失衡着色突出显示激进买方或卖方占据主导的位置，这正是订单流（order-flow）解读的核心。

## 在线演示

放大以读取单个单元格——足迹图一次只有几根柱子时才清晰可读。

```chart-demo footprint
```

![足迹图](../../../../../images/chart_footprint.png)

## 设置

添加一个 `FootprintSeries` 并向其提供精确的订单流柱子。每根柱子都带有 `dataMode: 'exact'`、其 OHLC，以及一个由 `{ price, bidVolume, askVolume, tradeCount }` 组成的 `levels` 数组：

```js
const series = chart.addSeries(SSChart.FootprintSeries, {
  tickSize: 0.25,
  mode: SSChart.FootprintDisplayMode.BidAsk,       // 显示模式：BidAsk | Delta | Total | Ladder
  detailLevel: SSChart.FootprintDetailLevel.Auto,  // 细节级别：Auto | Numbers | Heatmap | Summary
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showUnfinishedAuctions: true,
});

series.setData(exactBars);
```

`mode` 决定每个单元格显示的内容（买价 × 卖价、delta、总量或阶梯 ladder）；`detailLevel` 在缩放时用数字细节换取密度——`Auto` 会自动切换。

## 另请参阅

- [JavaScript 图表](../javascript_charts.md)
- [成交量分布图](volume_profile.md)
- [TPO（市场剖面图）](tpo.md)
