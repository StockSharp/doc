# Preenchimento de histórico

Um gráfico não consegue manter todo o histórico de um instrumento de uma só vez, então ele carrega uma janela de barras recentes e busca as mais antigas sob demanda. O `ChartDataController` monitora o intervalo visível e, à medida que você rola ou dá zoom em direção à borda esquerda, obtém a próxima página de histórico da sua fonte de dados e a adiciona ao início.

## Demonstração ao vivo

Role ou dê zoom em direção à borda esquerda — as barras mais antigas são carregadas em páginas (observe a legenda de status). O feed aqui possui um atraso artificial para que o estado de carregamento fique visível.

```chart-demo backfill
```

## Configuração

Forneça ao controlador o gráfico, a série que ele controla e uma fonte de dados. A fonte de dados resolve um símbolo e retorna páginas de barras; o controlador solicita páginas mais antigas automaticamente:

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

`historyPrefetchThreshold` define quão perto da barra mais antiga carregada a viewport deve chegar antes que a próxima página seja buscada; `autoPrefetch` ativa esse carregamento automático. Enquanto uma página está em trânsito, `snapshot.loadingHistory` é `true`, então você pode exibir um indicador de progresso. Chame `controller.loadMoreBefore()` para acionar uma página manualmente e `controller.dispose()` para liberar os recursos.

## Veja também

- [Gráficos JavaScript](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Indicadores](indicators.md)
