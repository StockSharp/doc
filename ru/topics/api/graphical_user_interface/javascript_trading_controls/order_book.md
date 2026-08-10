# Стакан

`OrderBookWidget` отображает уровни bid и ask, среднюю цену, спред, накопленный объём и настроение рынка. Доступны диагональное и составное представления, инверсия сторон, глубина 5 или 10 уровней и canvas-график глубины.

## Создание

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
      console.log('prefill', price, side),
    onPriceExecuted: (price, side) =>
      console.log('execute', price, side),
    maxDepth: () => 10,
    pixelRatio: () => window.devicePixelRatio || 1,
  },
);

book.setSymbol('BTC@IMEX');
```

Обычный щелчок по уровню вызывает `onPriceSelected`, а щелчок с Ctrl или Cmd — `onPriceExecuted`. Числовая сторона `0` означает покупку, `1` — продажу: щелчок по ask выбирает покупку, по bid — продажу. Контрол передаёт намерение хосту, но сам заявку не отправляет.

## Снимки и изменения стакана

Первый кадр должен быть полным снимком:

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

Последующие кадры с `isSnapshot: false` применяются как изменения. Количество `0` удаляет уровень. `sequence` должен возрастать без разрывов; при пропуске контрол вызывает `host.trading.marketData.resubscribe(symbol, MarketDataLevels.Full)`, чтобы получить новый снимок. Некорректные уровни и пересечённый стакан передаются в `host.log`.

## Представление и состояние

```ts
book.setDepth(10);
book.setView('stacked');
book.setInvertSides(false);
book.setShowDepthChart(true);
```

`maxDepth()` позволяет хосту уменьшить число уровней для небольшого экрана, а `pixelRatio()` задаёт плотность backing store canvas. Цвета графика возвращает `host.presentation.canvasPalette()`.

Для панели, следующей за активным инструментом, настройки представления сохраняются в `host.preferences`. Закреплённый экземпляр хранит их в состоянии панели. При создании можно передать `symbol`, `depth`, `view`, `invertSides`, `showDepthChart` и `followsActive`.

Активные заявки из `host.trading.marketData.getOrders()` отмечаются рядом с соответствующими уровнями.

## Публичные методы

- `setSymbol`, `getSymbol` — управляют инструментом.
- `setDepth`, `getDepth` — задают и возвращают глубину.
- `setView`, `setInvertSides`, `setShowDepthChart` — меняют представление.
- `getBids`, `getAsks` — возвращают текущие уровни.
- `isFollowsActive` — сообщает, следует ли панель за активным инструментом.
- `applyFrame` — применяет снимок или изменение.
- `dispose` — освобождает ресурсы.

## Смотрите также

- [Торговые JavaScript-контролы](../javascript_trading_controls.md)
- [Список инструментов](watchlist.md)
- [Ввод заявки](order_entry.md)
