# OHLC 柱状图

OHLC 柱状图展示与K线图相同的四个价格，但没有填充的实体：一条竖线表示最高价到最低价的区间，左侧的小横线标记开盘价，右侧的小横线标记收盘价。它们让图表保持轻盈，同时仍能显示每根柱子的开盘价和收盘价。

## 在线演示

```chart-demo bar
```

## 设置

添加一个 `BarSeries` 并向其提供 `{ time, open, high, low, close }` 数据点：

```js
const series = chart.addSeries(SSChart.BarSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

当收盘价等于或高于开盘价时，柱子显示为上涨颜色，否则显示为下跌颜色。

## 参见

- [JavaScript 图表](../charts.md)
- [K线图](candlestick.md)
- [折线图](line.md)
