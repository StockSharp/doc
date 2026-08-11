# Perfil de volumen

Un perfil de volumen agrega el volumen negociado por precio y lo dibuja como un histograma horizontal, marcando el point of control (el precio más negociado) y el área de valor. Responde a la pregunta «dónde se negoció», con independencia de cuándo.

## Demostración en vivo

El perfil se recalcula sobre las barras que estén a la vista: desplácese y haga zoom para verlo cambiar.

```chart-demo volume-profile
```

![Perfil de volumen con point of control y área de valor](../../../../images/chart_volume_profile.png)

## Configuración

Añada una `ExactVolumeProfileSeries` (normalmente sobre una serie de velas para dar contexto) y aliméntela con las mismas barras exactas de order flow que usa el footprint:

```js
const candles = chart.addSeries(SSChart.CandlestickSeries, {
  upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
});
candles.setData(exactBars);

const profile = chart.addSeries(SSChart.ExactVolumeProfileSeries, {
  tickSize: 0.25,
  rangeMode: SSChart.VolumeProfileRangeMode.Visible,     // modo de rango: Visible | Fixed | Session
  displayMode: SSChart.VolumeProfileDisplayMode.BidAsk,  // modo de visualización: Total | BidAsk | Delta
  bidColor: '#26a69a',
  askColor: '#ef5350',
  showLabels: true,
  profileWidth: 0.32,
});
profile.setData(exactBars);

chart.timeScale().fitContent();
```

`rangeMode` define lo que abarca el perfil: `Visible` lo recalcula sobre la ventana visible, `Fixed` fija un rango, `Session` construye un perfil por cada sesión.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Footprint](footprint.md)
- [TPO (Perfil de mercado)](tpo.md)
