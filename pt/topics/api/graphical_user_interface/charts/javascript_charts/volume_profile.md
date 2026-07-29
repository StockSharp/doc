# Perfil de volume

Um perfil de volume agrega o volume negociado por preço e representa-o como um histograma horizontal, assinalando o ponto de controlo (*point of control*, o preço mais negociado) e a área de valor (*value area*). Mostra «onde foram efetuados os negócios», independentemente do momento em que ocorreram.

## Demonstração em direto

O perfil é recalculado sobre todas as barras visíveis — desloque e amplie o gráfico para observar a alteração.

```chart-demo volume-profile
```

![Perfil de volume com ponto de controlo e área de valor](../../../../../images/chart_volume_profile.png)

## Configuração

Adicione uma `ExactVolumeProfileSeries` (geralmente sobre uma série de velas para dar contexto) e alimente-a com as mesmas barras de order flow usadas pelo footprint:

```js
const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
});
candles.setData(exactBars);

const profile = chart.addSeries(SSChart.ExactVolumeProfileSeries, {
  tickSize: 0.25,
  rangeMode: SSChart.VolumeProfileRangeMode.Visible,     // modo de intervalo: Visible | Fixed | Session
  displayMode: SSChart.VolumeProfileDisplayMode.BidAsk,  // modo de exibição: Total | BidAsk | Delta
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showLabels: true,
  profileWidth: 0.32,
});
profile.setData(exactBars);

chart.timeScale().fitContent();
```

`rangeMode` define o que o perfil abrange: `Visible` recalcula sobre a área visível, `Fixed` fixa um intervalo, `Session` cria um perfil por sessão.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Footprint](footprint.md)
- [TPO (Perfil de mercado)](tpo.md)
