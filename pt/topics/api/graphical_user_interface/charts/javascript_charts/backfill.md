# Preenchimento de histórico

Um gráfico não consegue manter todo o histórico de um instrumento de uma só vez, pelo que carrega uma janela de barras recentes e obtém as mais antigas a pedido. O `ChartDataController` monitoriza o intervalo visível e, à medida que o gráfico é deslocado ou ampliado em direção à margem esquerda, obtém a página de histórico seguinte da fonte de dados e adiciona-a ao início.

## Demonstração em direto

Desloque ou amplie o gráfico em direção à margem esquerda — as barras mais antigas são carregadas por páginas (consulte a legenda de estado). Este fluxo de dados tem um atraso artificial para tornar visível o estado de carregamento.

```chart-demo backfill
```

## Configuração

Forneça ao controlador o gráfico, a série que ele controla e uma fonte de dados. A fonte de dados resolve um símbolo e devolve páginas de barras; o controlador solicita páginas mais antigas automaticamente:

```js
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { ChartDataController } from '@stocksharp/chart/data';

// Uma fonte de dados: resolve o símbolo e então serve páginas de barras terminando antes de `to`.
const dataSource = {
  resolveSymbol(request) {
    return Promise.resolve({ id: request.symbol, priceFormat: { type: 'price', precision: 2, minMove: 0.01 } });
  },
  getBars(request) {
    // request: { symbol, resolution, to?, countBack }. Devolve { bars, hasMoreBefore, hasMoreAfter }.
    return fetchBars(request).then(bars => ({ bars, hasMoreBefore: bars.length > 0, hasMoreAfter: false }));
  },
  subscribeBars(request, listener) {
    // Envie barras em tempo real pelo callback ({ bar, isFinal }); devolva uma função de cancelamento da subscrição.
    return () => {};
  },
};

const chart = createChart(document.getElementById('chart'), { timeScale: { timeVisible: true } });
const series = chart.addSeries(CandlestickSeries, { upColor: '#26a69a', downColor: '#ef5350' });

const controller = new ChartDataController({
  chart,
  series,
  dataSource,
  initialCount: 300,               // barras carregadas primeiro
  historyCount: 250,               // barras por página mais antiga
  historyPrefetchThreshold: 40,    // pré-carrega quando estiver a 40 barras da margem esquerda
  autoPrefetch: true,              // carrega barras mais antigas ao deslocar / ampliar
});

// Opcional: observe o estado e o progresso do carregamento.
controller.subscribe(snap => {
  console.log(snap.loadedBars, snap.hasMoreBefore, snap.loadingHistory);
});

// Carrega a primeira página e posiciona a viewport perto da margem direita para permitir o deslocamento para a esquerda.
controller.setSelection({ symbol: 'DEMO', resolution: '1h' }).then(() => {
  const loaded = controller.rawData().length;
  chart.timeScale().setVisibleLogicalRange({ from: Math.max(0, loaded - 90), to: loaded + 3 });
});
```

`historyPrefetchThreshold` define a proximidade da barra mais antiga carregada que a viewport deve atingir antes de ser obtida a página seguinte; `autoPrefetch` ativa este carregamento automático. Enquanto uma página está em trânsito, `snapshot.loadingHistory` é `true`, pelo que é possível apresentar um indicador de progresso. Invoque `controller.loadMoreBefore()` para obter manualmente uma página e `controller.dispose()` para libertar os recursos.

## Veja também

- [Gráficos em JavaScript](../javascript_charts.md)
- [Candlestick](candlestick.md)
- [Indicadores](indicators.md)
