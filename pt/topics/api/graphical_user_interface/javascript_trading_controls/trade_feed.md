# Fluxo de negócios

`TradeFeedWidget` apresenta negócios públicos do mercado e execuções da carteira atual. O fluxo do mercado pode alternar entre uma tabela e um gráfico de bolhas.

## Criação e fluxo de dados

```ts
import {
  TradeFeedWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const feed = TradeFeedWidget.create(
  document.querySelector<HTMLElement>('#trade-feed')!,
  {},
  { host },
);

feed.setActiveSymbol('BTC@IMEX');

feed.setTrades([{
  symbol: 'BTC@IMEX',
  side: 0,
  price: 68_420.5,
  quantity: 0.25,
  time: new Date().toISOString(),
}]);

feed.addTrade({
  symbol: 'BTC@IMEX',
  side: 1,
  price: 68_419.9,
  quantity: 0.4,
  time: new Date().toISOString(),
});
```

`setTrades` substitui o conjunto original e `addTrade` adiciona uma execução em tempo real. O componente conserva no máximo 50 linhas da tabela e os 500 últimos registos para o gráfico.

## Gráfico de bolhas

No gráfico, o eixo horizontal representa o tempo, o vertical representa o preço, o raio da bolha representa o volume e a cor representa a direção do negócio. Ao combinar registos adjacentes, a descrição emergente apresenta o VWAP, o volume total e o número de negócios. Um negócio é considerado grande se o respetivo volume exceder em mais do dobro a média móvel.

As cores do canvas provêm de `host.presentation.canvasPalette()`. O modo selecionado é guardado em `host.preferences` sob uma chave comum à página.

## Instrumentos adicionais

Além do símbolo ativo, o painel pode fixar outros:

```ts
await feed.addExtraSymbol('ETH@IMEX');
console.log(feed.getExtraSymbols());
await feed.removeExtraSymbol('ETH@IMEX');
```

São criadas para estes instrumentos subscrições do nível `MarketDataLevels.Tape` e, no gráfico de bolhas, faixas separadas com as suas próprias escalas de preços. A lista de instrumentos adicionais é guardada no estado da instância específica e pode ser fornecida durante a criação como `{ extras: ['ETH@IMEX'] }`.

## Negócios próprios

O segundo separador apresenta as execuções da carteira. O carregamento pode ser chamado explicitamente:

```ts
await feed.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

O método chama `host.trading.api.getExecutions`. O restante fluxo do mercado é sempre fornecido externamente ao controlo, para que vários painéis possam utilizar uma só ligação.

## Métodos públicos

- `setActiveSymbol(symbol)` — definir o instrumento principal.
- `setTrades(...)`, `addTrade(...)` — substituir ou complementar o fluxo do mercado.
- `loadMyTrades(portfolioId, symbol)` — carregar as execuções próprias.
- `addExtraSymbol`, `removeExtraSymbol`, `getExtraSymbols` — gerir os instrumentos fixos.
- `dispose()` — eliminar as subscrições e libertar os recursos.

## Consulte também

- [Controlos de negociação JavaScript](../javascript_trading_controls.md)
- [Histórico de negócios](trade_history.md)
- [Livro de ofertas](order_book.md)
