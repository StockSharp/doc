# TPO（市场剖面图）

TPO（Time Price Opportunity，时间价格机会）图，也称为市场剖面图（Market Profile），通过为每个时间段堆叠一个字母或方块，展示在一个交易时段内价格在各个水平上停留的时间长短。它揭示了该时段的公允价值区域、其控制点（point of control）以及价格几乎没有停留的位置（单笔成交，single prints）。

## 在线演示

```chart-demo tpo
```

## 设置

添加一个 `TpoSeries`，并向其提供各自携带 `sessionId` 的 OHLC 柱线；该系列会自行按时段构建字母/方块分布：

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // 显示模式：Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` 用于在字母与实心方块之间切换（`Auto` 会根据缩放级别自动选择）。各个叠加层——控制点（point of control）、价值区域、初始平衡（initial balance）和单笔成交（single prints）——都可以独立开关。

## 参见

- [JavaScript 图表](../charts.md)
- [成交量分布图](volume_profile.md)
- [足迹图（Footprint）](footprint.md)
