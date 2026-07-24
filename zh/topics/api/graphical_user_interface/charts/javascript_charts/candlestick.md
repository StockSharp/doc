# 蜡烛图

蜡烛图是默认的价格序列：每根 K 线在开盘价与收盘价之间绘制实体，并向最高价和最低价延伸出上下影线，按涨跌着色。它是解读价格时信息密度最高的方式，也是大多数图表的起点。

## 在线演示

```chart-demo candlestick
```

## 设置

添加一个 `CandlestickSeries`，并向其提供 `{ time, open, high, low, close }` 数据点（time 为 Unix 秒）：

```js
const chart = SSChart.createChart(document.getElementById('chart'), {
  timeScale: { timeVisible: true },
});

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  borderUpColor: '#26a69a',
  borderDownColor: '#ef5350',
  wickUpColor: '#26a69a',
  wickDownColor: '#ef5350',
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

调用 `series.update({ time, open, high, low, close })` 可推送实时 K 线：使用相同的时间戳会替换最后一根蜡烛，使用更新的时间戳则会追加一根新蜡烛。

## 另请参阅

- [JavaScript 图表](../javascript_charts.md)
- [OHLC 柱状图](bar.md)
- [Heikin-Ashi](heikin_ashi.md)
