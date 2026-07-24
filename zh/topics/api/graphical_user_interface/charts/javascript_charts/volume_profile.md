# 成交量分布图

成交量分布图按价格聚合成交量，并将其绘制为水平直方图，标注出控制点（成交最活跃的价格）和价值区。它回答的是"交易在哪里发生"，与何时发生无关。

## 在线演示

分布图会基于视图中当前显示的柱线重新计算——滚动和缩放即可观察它的变化。

```chart-demo volume-profile
```

![带有控制点和价值区的成交量分布图](../../../../../images/chart_volume_profile.png)

## 设置

添加一个 `ExactVolumeProfileSeries`（通常叠加在K线图序列之上以提供上下文），并向它输入与足迹图相同的精确订单流柱线：

```js
const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
});
candles.setData(exactBars);

const profile = chart.addSeries(SSChart.ExactVolumeProfileSeries, {
  tickSize: 0.25,
  rangeMode: SSChart.VolumeProfileRangeMode.Visible,     // 范围模式：Visible | Fixed | Session
  displayMode: SSChart.VolumeProfileDisplayMode.BidAsk,  // 显示模式：Total | BidAsk | Delta
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showLabels: true,
  profileWidth: 0.32,
});
profile.setData(exactBars);

chart.timeScale().fitContent();
```

`rangeMode` 决定分布图的覆盖范围：`Visible` 在视口范围内重新计算，`Fixed` 固定一个范围，`Session` 为每个交易时段构建一个分布图。

## 参见

- [JavaScript 图表](../javascript_charts.md)
- [足迹图（Footprint）](footprint.md)
- [TPO（市场剖面图）](tpo.md)
