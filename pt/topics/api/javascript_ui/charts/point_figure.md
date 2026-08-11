# Ponto e Figura

Os gráficos Point & Figure ignoram totalmente o tempo e representam colunas de X (subida) e O (descida). Uma coluna continua a crescer enquanto o preço se move na respetiva direção por caixas inteiras; um número `reversal` de caixas em sentido contrário inicia uma nova coluna. O resultado realça o suporte, a resistência e os rompimentos.

## Demonstração em direto

```chart-demo point-figure
```

## Configuração

Adicione uma `PointFigureSeries` com um `boxSize` e um `reversal` e, em seguida, forneça-lhe velas brutas — a série constrói as colunas de forma autónoma:

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

- [Gráficos em JavaScript](../charts.md)
- [Renko](renko.md)
- [Candlestick](candlestick.md)
