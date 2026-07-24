# Área

Uma série de área é uma linha com a região abaixo dela preenchida por um gradiente vertical. Ela se lê como um gráfico de linha, mas enfatiza a magnitude de um único valor ao longo do tempo, o que é adequado para curvas de capital e visões gerais de preços.

## Demonstração ao vivo

```chart-demo area
```

## Configuração

Adicione uma `AreaSeries` e alimente-a com pontos `{ time, value }`; `topColor`/`bottomColor` definem o gradiente:

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

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Linha](line.md)
- [Histograma](histogram.md)
