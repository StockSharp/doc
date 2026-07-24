# Banda

Uma série de banda desenha uma linha de fronteira superior e uma inferior, com o canal entre elas preenchido. É o ajuste natural para envelopes e Bandas de Bollinger, ou qualquer estudo que produza um corredor de preços.

## Demonstração ao vivo

```chart-demo band
```

## Configuração

Adicione uma `BandSeries` e alimente-a com pontos `{ time, upper, lower }`:

```js
const band = chart.addSeries(SSChart.BandSeries, {
  upperColor: '#26a69a',
  lowerColor: '#ef5350',
  fillColor: 'rgba(74,158,255,0.10)',
});

band.setData(data.map(d => ({ time: d.time, upper: d.upper, lower: d.lower })));

chart.timeScale().fitContent();
```

Calcule `upper`/`lower` conforme o estudo exigir — para as Bandas de Bollinger, tome uma média móvel do fechamento e some/subtraia um múltiplo do seu desvio padrão. Sobreponha a banda a uma série de linha ou de velas que carregue o próprio preço.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Linha](line.md)
- [Perfil de volume](volume_profile.md)
