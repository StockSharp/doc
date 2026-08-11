# Heikin-Ashi K线图

Heikin-Ashi（“平均K线”）K线由原始 OHLC 计算得出，用于平滑噪声：连续的同色K线让趋势更易读，但代价是隐藏了真实的开盘价和收盘价。它们通过将变换后的数值输入到普通的K线序列中来绘制。

## 在线演示

```chart-demo heikin-ashi
```

## 设置

计算 Heikin-Ashi 数值并将其输入到 `CandlestickSeries`：

```js
function heikinAshi(candles) {
  const out = [];
  let prevOpen = candles[0].open, prevClose = candles[0].close;
  for (const c of candles) {
    const haClose = (c.open + c.high + c.low + c.close) / 4;
    const haOpen = (prevOpen + prevClose) / 2;
    out.push({
      time: c.time,
      open: haOpen,
      high: Math.max(c.high, haOpen, haClose),
      low: Math.min(c.low, haOpen, haClose),
      close: haClose,
    });
    prevOpen = haOpen; prevClose = haClose;
  }
  return out;
}

const series = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350',
});
series.setData(heikinAshi(candles));

chart.timeScale().fitContent();
```

## 另请参阅

- [JavaScript 图表](../charts.md)
- [K线图](candlestick.md)
- [Renko](renko.md)
