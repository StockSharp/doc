# Histórico de negócios

`TradeHistoryWidget` é uma tabela de execuções da carteira atual. Os negócios mais recentes aparecem no topo; cada linha apresenta a hora, o instrumento, a direção, a quantidade, o preço, o identificador do negócio e o identificador da ordem.

## Criação e carregamento

```ts
import {
  TradeHistoryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const history = TradeHistoryWidget.create(
  document.querySelector<HTMLElement>('#history')!,
  {},
  { host },
);

await history.refresh();
```

A criação do controlo não inicia automaticamente o carregamento. O método `refresh()` obtém a carteira atual através de `host.trading.portfolioId()`, verifica a permissão para carregar o histórico e chama:

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

Esta abordagem é importante ao alternar entre carteiras: o identificador é lido imediatamente antes de cada atualização e não é guardado durante a criação do painel.

## Comportamento

O painel destina-se apenas à leitura. O utilizador pode ordenar e selecionar linhas, abrir o menu de contexto, atualizar os dados e exportar as colunas visíveis para XLSX. Os erros de carregamento são enviados para `host.log`.

A API pública do controlo é composta por `refresh(): Promise<void>` e `dispose(): void`.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Ordens ativas](active_orders.md)
- [Fluxo de negócios](trade_feed.md)
