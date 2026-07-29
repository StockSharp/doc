# Barras OHLC

As barras OHLC mostram os mesmos quatro preços que os candlesticks, mas sem o corpo preenchido: uma linha vertical abrange o intervalo entre o máximo e o mínimo, um traço à esquerda marca a abertura e um traço à direita marca o fecho. Mantêm o gráfico leve e, ainda assim, apresentam a abertura e o fecho de cada barra.

## Demonstração em direto

```chart-demo bar
```

## Configuração

Adicione uma `BarSeries` e alimente-a com pontos `{ time, open, high, low, close }`:

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

A barra é colorida como ascendente quando o fecho é igual ou superior à abertura e como descendente no caso contrário.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Linha](line.md)
