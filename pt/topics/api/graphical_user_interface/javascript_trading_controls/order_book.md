# Livro de ofertas

`OrderBookWidget` apresenta os níveis de compra e venda, o preço médio, o diferencial, o volume acumulado e o sentimento do mercado. Estão disponíveis as vistas diagonal e empilhada, a inversão dos lados, a profundidade de 5 ou 10 níveis e um gráfico de profundidade em canvas.

## Criação

```ts
import {
  OrderBookWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const book = OrderBookWidget.create(
  document.querySelector<HTMLElement>('#order-book')!,
  { followsActive: true },
  {
    host,
    onPriceSelected: (price, side) =>
      console.log('preencher', price, side),
    onPriceExecuted: (price, side) =>
      console.log('executar', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

Um clique normal num nível chama `onPriceSelected`, enquanto um clique com Ctrl ou Cmd chama `onPriceExecuted`. O lado numérico `0` significa compra e `1` significa venda: clicar num preço de venda seleciona a compra e clicar num preço de compra seleciona a venda. O controlo fornece a intenção ao anfitrião, mas não envia a ordem.

## Imagens e alterações do livro de ofertas

O primeiro quadro deve ser uma imagem completa:

```ts
book.applyFrame({
  symbol: 'BTC@IMEX',
  sequence: 1,
  isSnapshot: true,
  bids: [
    { price: 68_420.4, quantity: 2.5 },
    { price: 68_420.3, quantity: 4.1 },
  ],
  asks: [
    { price: 68_420.5, quantity: 1.8 },
    { price: 68_420.6, quantity: 3.2 },
  ],
});
```

Os quadros seguintes com `isSnapshot: false` são aplicados como alterações. Uma quantidade `0` elimina o nível. `sequence` deve aumentar sem interrupções; em caso de lacuna, o controlo chama `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)` para obter uma nova imagem. Os níveis inválidos e um livro de ofertas cruzado são enviados para `host.log`.

## Vista e estado

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

`maxDepth()` permite ao anfitrião reduzir o número de níveis num ecrã pequeno, enquanto `pixelRatio()` define a densidade da memória de suporte do canvas. As cores do gráfico são devolvidas por `host.presentation.canvasPalette()`.

Para um painel que segue o instrumento ativo, as definições da vista são guardadas em `host.preferences`. Uma instância fixa guarda-as no estado do painel. Durante a criação, podem ser fornecidos `symbol`, `depth`, `view`, `invertSides`, `showDepthChart` e `followsActive`.

As ordens ativas de `host.trading.marketData.getOrders()` são assinaladas junto dos níveis correspondentes.

## Métodos públicos

- `setSymbol`, `getSymbol` — gerem o instrumento.
- `setDepth`, `getDepth` — definem e devolvem a profundidade.
- `setView`, `setInvertSides`, `setShowDepthChart` — alteram a vista.
- `getBids`, `getAsks` — devolvem os níveis atuais.
- `isFollowsActive` — indica se o painel segue o instrumento ativo.
- `applyFrame` — aplica uma imagem ou alteração.
- `dispose` — liberta os recursos.

## Consulte também

- [Controlos de negociação JavaScript](../javascript_trading_controls.md)
- [Lista de instrumentos](watchlist.md)
- [Introdução de ordens](order_entry.md)
