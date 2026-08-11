# Histograma

Um histograma desenha uma barra vertical por ponto a partir de um valor base. O seu uso mais comum é o volume, onde cada barra é colorida conforme a vela fechou em alta ou em baixa, mas qualquer quantidade por barra funciona.

## Demonstração em direto

```chart-demo histogram
```

## Configuração

Adicione uma `HistogramSeries` e alimente-a com pontos `{ time, value, color? }`; uma `color` por ponto sobrescreve a cor da série:

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

Para apresentar o volume por baixo de um gráfico de preços, em vez de o colocar no respetivo espaço, adicione o histograma a uma escala de preços sobreposta (*overlay*) e fixe-o na parte inferior:

```js
const volume = chart.addSeries(SSChart.HistogramSeries, { priceScaleId: '', priceFormat: { type: 'volume' } });
volume.priceScale().applyOptions({ scaleMargins: { top: 0.82, bottom: 0 } });
```

## Veja também

- [Gráficos em JavaScript](../charts.md)
- [Área](area.md)
- [Candlestick](candlestick.md)
