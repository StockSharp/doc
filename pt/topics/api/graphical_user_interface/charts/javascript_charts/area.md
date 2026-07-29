# Área

Uma série de área é uma linha cuja região inferior é preenchida por um gradiente vertical. Lê-se como um gráfico de linha, mas realça a magnitude de um único valor ao longo do tempo, sendo adequada para curvas de capital e panorâmicas de preços.

## Demonstração em direto

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
