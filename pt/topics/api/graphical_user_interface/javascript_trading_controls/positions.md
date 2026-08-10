# Posições

`PositionsWidget` apresenta as posições abertas e uma linha de saldo monetário fixa no topo. Por predefinição, as posições estão dispostas por ordem alfabética e o saldo não participa na ordenação, na seleção nem na exportação.

## Criação e atualização

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let positions!: PositionsWidget;

positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('fechar', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('inverter', portfolioId, instrumentId, symbol),
    refreshPositions: () => positions.update([]),
  },
);

positions.update([{
  portfolioId: 10,
  instrumentId: 42,
  instrument: 'BTC@IMEX',
  quantity: 0.25,
  avgPrice: 68_000,
  currentPrice: 68_420,
  unrealizedPnl: 105,
  realizedPnl: 20,
}]);

positions.updateBalance({
  available: 48_251,
  locked: 1_749,
  total: 50_000,
});
```

`update` substitui a lista de posições. `applyDelta` atualiza uma posição através da combinação de carteira e instrumento; uma linha com quantidade zero é eliminada. `updateBalance(null)` remove o saldo fixo.

## Dados e ações

A linha apresenta a quantidade, os preços médio e atual e um PnL total calculado como a soma de `realizedPnl` e `unrealizedPnl`. A classe da respetiva cor é devolvida por `host.presentation.pnlClass`.

Os botões da linha chamam `closePosition` e `reversePosition`, fornecidos pelo anfitrião. O controlo não cria nem envia ordens de negociação.

## Métodos públicos

- `update(positions)` — substituir todas as posições.
- `updateBalance(balance)` — definir ou remover o saldo monetário.
- `applyDelta(position)` — aplicar uma alteração em tempo real a uma posição.
- `dispose()` — libertar os recursos.

O painel também permite atualizar, ordenar, abrir o menu de contexto e exportar as posições para XLSX.

## Consulte também

- [Controlos de negociação JavaScript](../javascript_trading_controls.md)
- [Ordens ativas](active_orders.md)
- [Introdução de ordens](order_entry.md)
