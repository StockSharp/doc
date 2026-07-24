# Point and Figure

Os gráficos Point & Figure descartam totalmente o tempo e plotam colunas de X (alta) e O (baixa). Uma coluna continua crescendo enquanto o preço se move na sua direção por caixas inteiras; um número `reversal` de caixas contra ela inicia uma nova coluna. O resultado destaca suporte, resistência e rompimentos.

## Demonstração ao vivo

```chart-demo point-figure
```

## Configuração

Adicione uma `PointFigureSeries` com um `boxSize` e um `reversal`, depois alimente-a com candles brutos — a série constrói as colunas por conta própria:

```js
const series = chart.addSeries(SSChart.PointFigureSeries, {
  upColor: '#26a69a',
  downColor: '#ef5350',
  boxSize: 0.5,
  reversal: 2,
});

series.setData(candles.map(c => ({
  time: c.time, open: c.open, high: c.high, low: c.low, close: c.close,
})));

chart.timeScale().fitContent();
```

`boxSize` define o preço por X/O; `reversal` é quantas caixas contra a coluna atual são necessárias para iniciar uma nova (3 é o valor clássico).

## Veja também

- [Gráficos JavaScript](../javascript_charts.md)
- [Renko](renko.md)
- [Candlestick](candlestick.md)
