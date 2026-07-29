# Velas Heikin-Ashi

As velas Heikin-Ashi («barra média») são calculadas a partir do OHLC bruto para suavizar o ruído: velas consecutivas da mesma cor tornam as tendências mais fáceis de interpretar, à custa de ocultar a abertura e o fecho reais. São desenhadas com a série de candlestick comum, alimentada com valores transformados.

## Demonstração em direto

```chart-demo heikin-ashi
```

## Configuração

Calcule os valores Heikin-Ashi e forneça-os a uma `CandlestickSeries`:

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

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Renko](renko.md)
