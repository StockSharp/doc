# Relleno de historial

Un gráfico no puede contener todo el historial de un instrumento a la vez, por lo que carga una ventana de barras recientes y obtiene las más antiguas bajo demanda. El `ChartDataController` vigila el rango visible y, a medida que te desplazas o haces zoom hacia el borde izquierdo, extrae la siguiente página de historial de tu fuente de datos y la antepone.

## Demostración en vivo

Desplázate o haz zoom hacia el borde izquierdo — las barras más antiguas se cargan por páginas (observa el texto de estado). El flujo de datos aquí tiene un retardo artificial para que el estado de carga sea visible.

```chart-demo backfill
```

## Configuración

Proporciona al controlador el gráfico, la serie que gestiona y una fuente de datos. La fuente de datos resuelve un símbolo y devuelve páginas de barras; el controlador solicita las páginas más antiguas automáticamente:

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { ChartDataController } from '@stocksharp/chart/data';

// Una fuente de datos: resuelve el símbolo y luego entrega páginas de barras que terminan antes de `to`.
const dataSource = {
  resolveSymbol(request) {
    return Promise.resolve({ id: request.symbol, priceFormat: { type: 'price', precision: 2, minMove: 0.01 } });
  },
  getBars(request) {
    // request: { symbol, resolution, to?, countBack }. Devuelve { bars, hasMoreBefore, hasMoreAfter }.
    return fetchBars(request).then(bars => ({ bars, hasMoreBefore: bars.length > 0, hasMoreAfter: false }));
  },
  subscribeBars(request, listener) {
    // Envía barras en tiempo real mediante el callback de suscripción ({ bar, isFinal }); devuelve una función para cancelar la suscripción.
    return () => {};
  },
};

const chart = createChart(document.getElementById('chart'), { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const controller = new ChartDataController({
  chart,
  series,
  dataSource,
  initialCount: 300,               // barras cargadas primero
  historyCount: 250,               // barras por página más antigua
  historyPrefetchThreshold: 40,    // precarga cuando la vista queda a 40 barras del borde izquierdo
  autoPrefetch: true,              // carga barras más antiguas automáticamente al desplazar / hacer zoom
});

// Opcional: observa el estado de carga y el progreso.
controller.subscribe(snap => {
  console.log(snap.loadedBars, snap.hasMoreBefore, snap.loadingHistory);
});

// Carga la primera página y luego coloca la vista cerca del borde derecho para dejar espacio para desplazarse hacia la izquierda.
controller.setSelection({ symbol: 'DEMO', resolution: '1h' }).then(() => {
  const loaded = controller.rawData().length;
  chart.timeScale().setVisibleLogicalRange({ from: Math.max(0, loaded - 90), to: loaded + 3 });
});
```

`historyPrefetchThreshold` es lo cerca que la ventana visible debe llegar a la barra cargada más antigua antes de que se obtenga la siguiente página; `autoPrefetch` activa esa carga automática. Mientras una página está en tránsito, `snapshot.loadingHistory` es `true`, de modo que puedes mostrar un indicador de carga. Llama a `controller.loadMoreBefore()` para desencadenar una página manualmente, y a `controller.dispose()` para limpiar.

## Véase también

- [Gráficos en JavaScript](../javascript_charts.md)
- [Velas](candlestick.md)
- [Indicadores](indicators.md)
