# Лента сделок

`TradeFeedWidget` показывает публичные рыночные сделки и исполнения текущего портфеля. Рыночную ленту можно переключать между таблицей и пузырьковым графиком.

## Создание и поток данных

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

`setTrades` заменяет исходный набор, а `addTrade` добавляет одно потоковое исполнение. Виджет хранит не более 50 строк таблицы и 500 последних отпечатков для графика.

## Пузырьковый график

На графике горизонтальная ось представляет время, вертикальная — цену, радиус пузыря — объём, а цвет — сторону сделки. При объединении соседних отпечатков подсказка показывает VWAP, суммарный объём и количество сделок. Сделка считается крупной, если её объём более чем вдвое превышает скользящее среднее.

Цвета canvas приходят из `host.presentation.canvasPalette()`. Выбранный режим хранится в `host.preferences` под общим для страницы ключом.

## Дополнительные инструменты

Кроме активного символа панель может закреплять дополнительные:

```ts
await feed.addExtraSymbol('ETH@IMEX');
console.log(feed.getExtraSymbols());
await feed.removeExtraSymbol('ETH@IMEX');
```

Для них создаются подписки уровня `MarketDataLevels.Tape`, а на пузырьковом графике — отдельные полосы со своими ценовыми масштабами. Список дополнительных инструментов сохраняется в состоянии конкретного экземпляра и может быть передан при создании как `{ extras: ['ETH@IMEX'] }`.

## Собственные сделки

Вторая вкладка показывает исполнения портфеля. Загрузку можно вызвать явно:

```ts
await feed.loadMyTrades(host.trading.portfolioId(), 'BTC@IMEX');
```

Метод обращается к `host.trading.api.getExecutions`. Остальная рыночная лента всегда передаётся в контрол снаружи, чтобы несколько панелей могли использовать одно соединение.

## Публичные методы

- `setActiveSymbol(symbol)` — задать основной инструмент.
- `setTrades(...)`, `addTrade(...)` — заменить или дополнить рыночную ленту.
- `loadMyTrades(portfolioId, symbol)` — загрузить собственные исполнения.
- `addExtraSymbol`, `removeExtraSymbol`, `getExtraSymbols` — управлять закреплёнными инструментами.
- `dispose()` — удалить подписки и освободить ресурсы.

## Смотрите также

- [Торговые JavaScript-контролы](../javascript_trading_controls.md)
- [История сделок](trade_history.md)
- [Стакан](order_book.md)
