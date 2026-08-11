# Активные заявки

`ActiveOrdersWidget` показывает полный список заявок с их текущими состояниями. Исполненные, отменённые и отклонённые строки остаются в таблице, поэтому пользователь видит всю последовательность изменений в рамках сессии.

## Создание

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
    cancelOrder: id => console.log('cancel', id),
    dismissOrder: id => orders.removeOrder(id),
    editOrderField: (id, field) => {
      if (field === 'quantity' || field === 'limitPrice' || field === 'stopPrice')
        orders.startInlineEdit(id, field);
    },
    replaceOrder: (id, quantity, limitPrice, stopPrice) =>
      console.log('replace', id, quantity, limitPrice, stopPrice),
    cancelAllOrders: () => console.log('cancel all'),
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

`update` заменяет весь набор строк. Для потоковых изменений используйте `applyDelta(order)`, а для удаления одной строки — `removeOrder(orderId)`.

## Редактирование и действия

Пока заявка находится в состоянии `Sent` или `Active`, двойным щелчком всегда можно изменить количество. Лимитная цена редактируется только у заявки, где она уже больше нуля, а стоп-цена — только там, где больше нуля её исходное значение. После подтверждения контрол вызывает `replaceOrder` и передаёт сразу всю тройку `quantity`, `limitPrice`, `stopPrice`, а не только изменённое поле.

Для активной заявки кнопка действия вызывает `cancelOrder`. Для терминальной строки она вызывает `dismissOrder` и удаляет запись только из локального представления. Причина отказа `rejectReason` показывается во всплывающей подсказке.

Контрол также предоставляет отмену всех заявок, обновление, сортировку, выделение строк, контекстное меню и экспорт в XLSX.

## Публичные методы

- `update(orders)` — заменить все строки.
- `applyDelta(order)` — добавить или обновить одну заявку.
- `removeOrder(orderId)` — удалить строку.
- `getOrder(orderId)` — получить текущую строку.
- `startInlineEdit(orderId, field)` — начать редактирование `quantity`, `limitPrice` или `stopPrice`.
- `dispose()` — освободить ресурсы контрола.

Объект `OrderStates` экспортирует состояния `PendingRisk`, `Sent`, `Active`, `PartiallyFilled`, `Filled`, `Rejected` и `Cancelled`.

## Смотрите также

- [Торговые JavaScript-контролы](../trading_controls.md)
- [Позиции](positions.md)
- [История сделок](trade_history.md)
