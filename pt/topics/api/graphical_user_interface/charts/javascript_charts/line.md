# Linha

Uma série de linha une um único valor por barra — normalmente o fecho — numa única polilinha contínua. É a forma mais simples de apresentar uma tendência ou representar uma série derivada, como uma média móvel.

## Demonstração em direto

```chart-demo line
```

## Configuração

Adicione uma `LineSeries` e alimente-a com pontos `{ time, value }`:

```js
const series = chart.addSeries(SSChart.LineSeries, {
  color: '#4a9eff',
  lineWidth: 2,
});

series.setData(candles.map(c => ({ time: c.time, value: c.close })));

chart.timeScale().fitContent();
```

Qualquer conjunto de dados de valor único funciona aqui — troque `c.close` por um valor de indicador para sobrepô-lo ao gráfico de preços.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Área](area.md)
- [Banda](band.md)
