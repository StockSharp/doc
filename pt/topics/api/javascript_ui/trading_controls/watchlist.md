# Lista de instrumentos

`WatchlistWidget` apresenta instrumentos e cotações em tempo real. O controlo suporta pesquisa, favoritos, categorias, seleção do instrumento ativo e subscrições apenas para os símbolos realmente visíveis.

## Criação

```ts
import {
  WatchlistWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let watchlist!: WatchlistWidget;

watchlist = WatchlistWidget.create(
  document.querySelector<HTMLElement>('#watchlist')!,
  {},
  {
    host,
    onSelect: symbol => {
      watchlist.setCurrentSymbol(symbol);
      console.log('selecionado', symbol);
    },
  },
);
```

Durante a criação, é iniciado automaticamente `init()`, que carrega a lista através de `host.trading.api.searchInstruments('')`. Para cada entrada são utilizados o símbolo, o nome, a bolsa e a categoria.

O fluxo de preços é fornecido externamente:

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

O método atualiza apenas as células necessárias do preço e da percentagem, preservando a animação da alteração. `setCurrentSymbol(symbol)` realça o instrumento atual.

## Pesquisa, categorias e subscrições

A pesquisa verifica o símbolo, o nome e a bolsa. São criados separadores para todos os instrumentos, os favoritos e as categorias detetadas. Os símbolos favoritos são guardados em `host.preferences`.

O controlo subscreve os primeiros 30 instrumentos visíveis no nível `MarketDataLevels.Quotes`. Não são apresentadas no ecrã mais de 300 linhas, mas a filtragem e a exportação abrangem todo o conjunto encontrado. Apenas a instância com `host.isPrimary === true` publica as cotações visíveis através de `host.ticker`.

A variação percentual é calculada a partir do primeiro preço recebido no dia UTC atual. Estes preços de referência pertencem à cache, pelo que são guardados em `host.cache` e não nas definições do utilizador.

## Métodos públicos

- `init()` — carregar os instrumentos e preparar as subscrições; é chamado automaticamente por `create`.
- `setCurrentSymbol(symbol)` — assinalar o instrumento ativo.
- `onPriceUpdate(symbol, price)` — aplicar um novo preço.
- `dispose()` — remover as subscrições e libertar os recursos.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Livro de ofertas](order_book.md)
- [Introdução de ordens](order_entry.md)
