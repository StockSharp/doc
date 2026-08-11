# 带状图（Band）

带状图序列会绘制上边界线和下边界线，并填充两者之间的通道。它天然适合展示包络线（Envelope）和布林带，或任何生成价格通道的指标。

## 在线演示

```chart-demo band
```

## 设置

添加一个 `BandSeries` 并向其传入 `{ time, upper, lower }` 数据点：

```js
const band = chart.addSeries(SSChart.BandSeries, {
  upperColor: '#26a69a',
  lowerColor: '#ef5350',
  fillColor: 'rgba(74,158,255,0.10)',
});

band.setData(data.map(d => ({ time: d.time, upper: d.upper, lower: d.lower })));

chart.timeScale().fitContent();
```

按照指标的要求计算 `upper`/`lower`——对于布林带，取收盘价的移动平均，再加上/减去其标准差的若干倍数。将带状图叠加在承载价格本身的折线或K线图序列之上。

## 另请参阅

- [JavaScript 图表](../charts.md)
- [折线图](line.md)
- [成交量分布图](volume_profile.md)
