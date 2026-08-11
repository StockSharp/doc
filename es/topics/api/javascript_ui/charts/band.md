# Banda

Una serie de banda dibuja una línea de límite superior y otra inferior con el canal que las separa relleno. Es la opción natural para envolventes y bandas de Bollinger, o para cualquier estudio que produzca un corredor de precios.

## Demostración en vivo

```chart-demo band
```

## Configuración

Añada una `BandSeries` y aliméntela con puntos `{ time, upper, lower }`:

```js
const band = chart.addSeries(SSChart.BandSeries, {
  upperColor: '#26a69a',
  lowerColor: '#ef5350',
  fillColor: 'rgba(74,158,255,0.10)',
});

band.setData(data.map(d => ({ time: d.time, upper: d.upper, lower: d.lower })));

chart.timeScale().fitContent();
```

Calcule `upper`/`lower` según lo requiera el estudio: para las bandas de Bollinger, tome una media móvil del cierre y sume/reste un múltiplo de su desviación estándar. Superponga la banda sobre una serie de líneas o de velas que contenga el precio en sí.

## Véase también

- [Gráficos en JavaScript](../charts.md)
- [Línea](line.md)
- [Perfil de volumen](volume_profile.md)
