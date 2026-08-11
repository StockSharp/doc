# Renko

Renko 图表由固定大小的价格*砖块*构成，并且忽略时间：只有当价格移动达到一个箱体大小时才会添加新的砖块，因此横盘噪声被压缩，而趋势则更加突出。每一块砖沿着价格移动的方向跨越一个箱体。

## 在线演示

```chart-demo renko
```

## 设置

添加一个带有 `boxSize` 的 `RenkoSeries`，并向其输入原始的 `{ time, open, high, low, close }` K线数据——该系列会自行构建砖块：

```js
const series = chart.addSeries(SSChart.RenkoSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

根据品种的价格区间选择 `boxSize`：过小会产生噪声，过大则会掩盖价格波动。一个常用的经验法则是取平均K线波幅的一个比例。

## 参见

- [JavaScript 图表](../charts.md)
- [点数图 (Point and Figure)](point_figure.md)
- [Heikin-Ashi K线图](heikin_ashi.md)
