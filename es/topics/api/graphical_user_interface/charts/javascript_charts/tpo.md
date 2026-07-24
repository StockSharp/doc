# TPO (Perfil de mercado)

Un gráfico TPO (Time Price Opportunity), también llamado perfil de mercado (Market Profile), muestra cuánto tiempo se negoció el precio en cada nivel durante una sesión, apilando una letra o un bloque por cada intervalo de tiempo. Revela el área de valor justo de la sesión, su punto de control (point of control) y los niveles donde el precio permaneció poco tiempo (single prints).

## Demostración en vivo

```chart-demo tpo
```

## Configuración

Añade una `TpoSeries` y aliméntala con barras OHLC que lleven cada una un `sessionId`; la serie construye por sí misma la distribución de letras/bloques por sesión:

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // modo de visualización: Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` alterna entre letras y bloques sólidos (`Auto` elige según el zoom). Las superposiciones —punto de control (point of control), área de valor, balance inicial y single prints— se pueden activar o desactivar de forma independiente.

## Véase también

- [Gráficos en JavaScript](../javascript_charts.md)
- [Perfil de volumen](volume_profile.md)
- [Footprint](footprint.md)
