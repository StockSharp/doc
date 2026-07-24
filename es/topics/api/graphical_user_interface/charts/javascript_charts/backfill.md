# Relleno de historial

Un gráfico no puede contener todo el historial de un instrumento a la vez, por lo que carga una ventana de barras recientes y obtiene las más antiguas bajo demanda. El `ChartDataController` vigila el rango visible y, a medida que te desplazas o haces zoom hacia el borde izquierdo, extrae la siguiente página de historial de tu fuente de datos y la antepone.

## Demostración en vivo

Desplázate o haz zoom hacia el borde izquierdo — las barras más antiguas se cargan por páginas (observa el texto de estado). El feed aquí tiene un retardo artificial para que el estado de carga sea visible.

```chart-demo backfill
```

## Configuración

Proporciona al controlador el gráfico, la serie que gestiona y una fuente de datos. La fuente de datos resuelve un símbolo y devuelve páginas de barras; el controlador solicita las páginas más antiguas automáticamente:

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { ChartDataController } from '@stocksharp/chart/data';

// A data source: resolve the symbol, then serve pages of bars ending before `to`.
const dataSource = {
  resolveSymbol(request) {
    return Promise.resolve({ id: request.symbol, priceFormat: { type: 'price', precision: 2, minMove: 0.01 } });
  },
  getBars(request) {
    // request: { symbol, resolution, to?, countBack }. Return { bars, hasMoreBefore, hasMoreAfter }.
    return fetchBars(request).then(bars => ({ bars, hasMoreBefore: bars.length > 0, hasMoreAfter: false }));
  },
  subscribeBars(request, listener) {
    // Push realtime bars via listener({ bar, isFinal }); return an unsubscribe function.
    return () => {};
  },
};

const chart = createChart(document.getElementById('chart'), { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const controller = new ChartDataController({
  chart,
  series,
  dataSource,
  initialCount: 300,               // bars loaded first
  historyCount: 250,               // bars per older page
  historyPrefetchThreshold: 40,    // prefetch when within 40 bars of the left edge
  autoPrefetch: true,              // load older bars automatically on scroll / zoom
});

// Optional: observe loading state and progress.
controller.subscribe(snap => {
  console.log(snap.loadedBars, snap.hasMoreBefore, snap.loadingHistory);
});

// Load the first page, then park the viewport near the right edge so there is room to scroll left.
controller.setSelection({ symbol: 'DEMO', resolution: '1h' }).then(() => {
  const loaded = controller.rawData().length;
  chart.timeScale().setVisibleLogicalRange({ from: Math.max(0, loaded - 90), to: loaded + 3 });
});
```

`historyPrefetchThreshold` es lo cerca que la ventana visible debe llegar a la barra cargada más antigua antes de que se obtenga la siguiente página; `autoPrefetch` activa esa carga automática. Mientras una página está en tránsito, `snapshot.loadingHistory` es `true`, de modo que puedes mostrar un indicador de carga. Llama a `controller.loadMoreBefore()` para desencadenar una página manualmente, y a `controller.dispose()` para limpiar.

## Véase también

- [Gráficos JavaScript](../javascript_charts.md)
- [Velas](candlestick.md)
- [Indicadores](indicators.md)
