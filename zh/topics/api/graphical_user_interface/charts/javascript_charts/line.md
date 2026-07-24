# 折线图

折线序列将每根K线的单一数值（通常是收盘价）连成一条连续的折线。它是展示趋势或绘制派生序列（例如移动平均线）最简洁的方式。

## 在线演示

```chart-demo line
```

## 配置

添加一个 `LineSeries`，并向其提供 `{ time, value }` 数据点：

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

任何单值数据集都适用于此——把 `c.close` 换成指标值，即可将其叠加到价格图表上。

## 参见

- [JavaScript 图表](../javascript_charts.md)
- [面积图](area.md)
- [带状图](band.md)
