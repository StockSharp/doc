# Candlestick

Os candlesticks são a série de preços padrão: cada barra é desenhada como um corpo entre a abertura e o fechamento, com pavios até a máxima e a mínima, colorida de alta ou de baixa. Eles são a forma mais densa em informação de ler o preço e o ponto de partida para a maioria dos gráficos.

## Demonstração ao vivo

```chart-demo candlestick
```

## Configuração

Adicione uma `CandlestickSeries` e alimente-a com pontos `{ time, open, high, low, close }` (o tempo é em segundos Unix):

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

Chame `series.update({ time, open, high, low, close })` para enviar uma barra em tempo real: o mesmo timestamp substitui a última vela, um timestamp mais recente adiciona uma nova.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Barras OHLC](bar.md)
- [Velas Heikin-Ashi](heikin_ashi.md)
