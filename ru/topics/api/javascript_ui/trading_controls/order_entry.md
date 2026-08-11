# Ввод заявки

`OrderEntryWidget` — двухсторонняя панель покупки и продажи. Она поддерживает рыночную, лимитную, стоп- и стоп-лимитную заявку, показывает только относящиеся к выбранному типу поля и проверяет значения до передачи хосту.

## Создание и настройка инструмента

```ts
import {
  OrderEntrySides,
  OrderEntryTypes,
  OrderEntryWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const pad = OrderEntryWidget.create(
  document.querySelector<HTMLElement>('#order-entry')!,
  {},
  {
    host,
    submitOrder: (side, values) =>
      console.log('submit', side, values),
  },
);

pad.setInstrument({
  symbol: 'BTC@IMEX',
  lotSize: 0.001,
  tickSize: 0.1,
  minVolume: 0.001,
  maxVolume: 5,
});

pad.setOrderType(OrderEntryTypes.Limit);
pad.setBbo(68_420.4, 68_420.5);
pad.setAvailable(OrderEntrySides.Buy, 48_251);
pad.setMaxQuantity(OrderEntrySides.Buy, 0.7);
pad.setQuantity(0.01);
```

`OrderEntryTypes` содержит `Market`, `Limit`, `Stop` и `StopLimit`, а `OrderEntrySides` — `Buy` и `Sell`.

## Проверка и отправка

Контрол проверяет положительные значения, размер лота, шаг цены, минимальный и максимальный объём. Необязательные Take Profit и Stop Loss включаются методом `toggleTpSl`.

`submit(side)` сначала вызывает `validate(side)`. При корректных данных обработчик `submitOrder` получает сторону и объект:

```ts
interface OrderEntryValues {
  type: 'market' | 'limit' | 'stop' | 'stoplimit';
  quantity: number;
  limitPrice: number | null;
  stopPrice: number | null;
  takeProfit: number | null;
  stopLoss: number | null;
}
```

Сам контрол не выбирает портфель, не проверяет соединение и не отправляет заявку на сервер. Эти действия остаются в обработчике хоста.

## Цены, объём и состояние

- `setBbo(bid, ask)` обновляет лучшую покупку и продажу.
- `applyBbo(side)` подставляет в выбранную колонку противоположную сторону BBO для немедленного исполнения.
- `setAvailable(side, value)` задаёт отображаемый доступный баланс.
- `setMaxQuantity(side, value)` задаёт максимум, от которого процентные кнопки рассчитывают количество.
- `applyPercent(side, pct)` применяет 25, 50, 75 или 100 процентов от максимума, заданного через `setMaxQuantity`.
- `preselect(side)` выделяет сторону после жеста «торговать по клику».
- `setEnabled(false)` запрещает отправку, не уничтожая введённые значения.
- `getValues(side)` и `validate(side)` позволяют проверить форму снаружи.

Статический метод `toApiType` преобразует `Limit` в `0`, `Market` в `1`, а `Stop` и `StopLimit` в `2`.

## Смотрите также

- [Торговые JavaScript-контролы](../trading_controls.md)
- [Стакан](order_book.md)
- [Позиции](positions.md)
