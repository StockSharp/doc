# Ordens ativas

`ActiveOrdersWidget` apresenta a lista completa de ordens com os respetivos estados atuais. As linhas executadas, canceladas e rejeitadas permanecem na tabela, permitindo ao utilizador ver toda a sequência de alterações durante a sessão.

## Criação

```ts
import {
  ActiveOrdersWidget,
  OrderStates,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

let orders!: ActiveOrdersWidget;

orders = ActiveOrdersWidget.create(
  document.querySelector<HTMLElement>('#orders')!,
  {},
  {
    host,
    cancelOrder: id => console.log('cancelar', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('substituir', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('cancelar todas'),
    refreshOrders: () => orders.update([]),
  },
);

orders.update([{
  id: 501,
  localId: 1,
  instrument: 'BTC@IMEX',
  side: 0,
  type: 0,
  quantity: 0.01,
  limitPrice: 68_400,
  status: OrderStates.Active,
}]);
```

`update` substitui todo o conjunto de linhas. Para alterações em tempo real, utilize `applyDelta(order)` e, para eliminar uma linha, `removeOrder(orderId)`.

## Edição e ações

Enquanto a ordem estiver no estado `Sent` ou `Active`, a quantidade pode sempre ser alterada com um duplo clique. O preço limite só pode ser editado quando o valor original de `limitPrice` for superior a zero, e o preço de paragem apenas quando o valor original de `stopPrice` for superior a zero. Após a confirmação, o controlo chama `replaceOrder` e fornece simultaneamente os três valores `quantity`, `limitPrice` e `stopPrice`, não apenas o campo alterado.

Para uma ordem ativa, o botão de ação chama `cancelOrder`. Para uma linha num estado terminal, chama `dismissOrder` e elimina a entrada apenas da apresentação local. O motivo da rejeição em `rejectReason` é apresentado numa descrição emergente.

O controlo também permite cancelar todas as ordens, atualizar, ordenar, selecionar linhas, abrir o menu de contexto e exportar para XLSX.

## Métodos públicos

- `update(orders)` — substituir todas as linhas.
- `applyDelta(order)` — adicionar ou atualizar uma ordem.
- `removeOrder(orderId)` — eliminar uma linha.
- `getOrder(orderId)` — obter a linha atual.
- `startInlineEdit(orderId, field)` — começar a editar `quantity`, `limitPrice` ou `stopPrice`.
- `dispose()` — libertar os recursos do controlo.

O objeto `OrderStates` exporta os estados `PendingRisk`, `Sent`, `Active`, `PartiallyFilled`, `Filled`, `Rejected` e `Cancelled`.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Posições](positions.md)
- [Histórico de negócios](trade_history.md)
