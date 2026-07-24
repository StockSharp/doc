# 面积图

面积序列是一条折线，其下方区域用垂直渐变填充。它的观感与折线图相同，但更强调单个数值随时间变化的量级，非常适合权益曲线和价格概览。

## 在线演示

```chart-demo area
```

## 设置

添加一个 `AreaSeries` 并向其提供 `{ time, value }` 数据点；`topColor`/`bottomColor` 定义渐变：

```js
const series = chart.addSeries(SSChart.AreaSeries, {
  topColor: 'rgba(74,158,255,0.3)',
  bottomColor: 'rgba(74,158,255,0.02)',
  lineColor: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

## 另请参阅

- [JavaScript 图表](../javascript_charts.md)
- [折线图](line.md)
- [直方图](histogram.md)
