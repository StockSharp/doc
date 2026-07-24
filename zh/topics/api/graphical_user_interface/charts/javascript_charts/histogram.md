# 直方图

直方图为每个数据点从一个基准值开始绘制一根垂直柱。它最常见的用途是成交量，其中每根柱的颜色取决于该K线是收涨还是收跌，但任何按柱统计的数值都适用。

## 在线演示

```chart-demo histogram
```

## 设置

添加一个 `HistogramSeries` 并向其提供 `{ time, value, color? }` 数据点；单个数据点的 `color` 会覆盖整个序列的颜色：

```js
const series = chart.addSeries(SSChart.HistogramSeries, {
  priceFormat: { type: 'volume' },
});

series.setData(candles.map(c => ({
  time: c.time,
  value: c.volume,
  color: c.close >= c.open ? 'rgba(38,166,154,0.7)' : 'rgba(239,83,80,0.7)',
})));

chart.timeScale().fitContent();
```

若想让成交量显示在价格图表下方而非独立显示，可将直方图放在叠加的价格刻度上，并将其固定到底部：

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## 另请参阅

- [JavaScript 图表](../javascript_charts.md)
- [面积图](area.md)
- [蜡烛图](candlestick.md)
