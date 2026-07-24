# 点数图 (Point and Figure)

点数图 (Point & Figure) 完全舍弃时间维度，只用一列列的 X（上涨）和 O（下跌）来绘制。当价格沿着某列方向按整格 (box) 移动时，该列会持续延伸；一旦出现 `reversal` 个格子的反向移动，就会开启新的一列。这样的结果能够突出显示支撑、阻力和突破。

## 在线演示

```chart-demo point-figure
```

## 设置

添加一个带有 `boxSize` 和 `reversal` 的 `PointFigureSeries`，然后向它输入原始蜡烛图数据——该系列会自行构建这些列：

```js
const series = chart.addSeries(SSChart.PointFigureSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
  reversal: 2,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

`boxSize` 设定每个 X/O 所代表的价格；`reversal` 则是相对于当前列需要多少个反向格子才会开启新的一列（3 是经典取值）。

## 另请参阅

- [JavaScript 图表](../javascript_charts.md)
- [Renko](renko.md)
- [蜡烛图](candlestick.md)
