# Renko

Os gráficos Renko são construídos a partir de *tijolos* de preço de tamanho fixo e ignoram o tempo: um novo tijolo é adicionado apenas quando o preço se move pelo tamanho da caixa, de modo que o ruído lateral desaparece e as tendências se destacam. Cada tijolo abrange uma caixa na direção do movimento.

## Demonstração em direto

```chart-demo renko
```

## Configuração

Adicione uma `RenkoSeries` com um `boxSize` e forneça-lhe velas brutas `{ time, open, high, low, close }` — a série constrói os tijolos de forma autónoma:

```js
const series = chart.addSeries(SSChart.RenkoSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

Escolha o `boxSize` a partir do intervalo de preços do instrumento: valores demasiado pequenos produzem ruído e valores demasiado grandes ocultam os movimentos. Uma regra comum consiste em utilizar uma fração do intervalo médio das barras.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Ponto e Figura](point_figure.md)
- [Velas Heikin-Ashi](heikin_ashi.md)
