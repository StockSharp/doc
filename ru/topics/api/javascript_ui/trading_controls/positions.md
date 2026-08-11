# Позиции

`PositionsWidget` отображает открытые позиции и закреплённую сверху строку денежного баланса. Позиции по умолчанию расположены по алфавиту, а баланс не участвует в сортировке, выделении и экспорте.

## Создание и обновление

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
      console.log('close', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('reverse', portfolioId, instrumentId, symbol),
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

`update` заменяет список позиций. `applyDelta` обновляет одну позицию по сочетанию портфеля и инструмента; строка с нулевым количеством удаляется. `updateBalance(null)` убирает закреплённый баланс.

## Данные и действия

В строке показываются количество, средняя и текущая цены, а также один итоговый PnL, рассчитанный как сумма `realizedPnl` и `unrealizedPnl`. Класс его цвета возвращает `host.presentation.pnlClass`.

Кнопки строки вызывают переданные хостом `closePosition` и `reversePosition`. Контрол сам не формирует и не отправляет торговые заявки.

## Публичные методы

- `update(positions)` — заменить все позиции.
- `updateBalance(balance)` — установить или убрать денежный баланс.
- `applyDelta(position)` — применить потоковое изменение одной позиции.
- `dispose()` — освободить ресурсы.

Панель также поддерживает обновление, сортировку, контекстное меню и экспорт позиций в XLSX.

## Смотрите также

- [Торговые JavaScript-контролы](../trading_controls.md)
- [Активные заявки](active_orders.md)
- [Ввод заявки](order_entry.md)
