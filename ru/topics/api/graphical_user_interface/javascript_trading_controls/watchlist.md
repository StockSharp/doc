# Список инструментов

`WatchlistWidget` показывает инструменты и потоковые котировки. Контрол поддерживает поиск, избранное, категории, выбор активного инструмента и подписки только на действительно видимые символы.

## Создание

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
      console.log('selected', symbol);
    },
  },
);
```

При создании автоматически запускается `init()`, который загружает список через `host.trading.api.searchInstruments('')`. Для каждой записи используются поля `symbol`, `name`, `exchange` и `category`.

Поток цен передаётся снаружи:

```ts
watchlist.onPriceUpdate('BTC@IMEX', 68_420.5);
```

Метод обновляет только нужные ячейки цены и процента, сохраняя анимацию изменения. `setCurrentSymbol(symbol)` подсвечивает текущий инструмент.

## Поиск, категории и подписки

Поиск проверяет символ, название и биржу. Вкладки формируются для всех инструментов, избранного и обнаруженных категорий. Избранные символы сохраняются в `host.preferences`.

Контрол подписывает первые 30 видимых инструментов на уровень `MarketDataLevels.Quotes`. На экране отрисовывается не более 300 строк, но фильтрация и экспорт работают со всем найденным набором. Только экземпляр с `host.isPrimary === true` публикует видимые котировки через `host.ticker`.

Процент изменения считается от первой полученной цены текущего дня по UTC. Эти базовые цены относятся к кэшу, поэтому сохраняются в `host.cache`, а не в пользовательских настройках.

## Публичные методы

- `init()` — загрузить инструменты и подготовить подписки; при `create` вызывается автоматически.
- `setCurrentSymbol(symbol)` — отметить активный инструмент.
- `onPriceUpdate(symbol, price)` — применить новую цену.
- `dispose()` — снять подписки и освободить ресурсы.

## Смотрите также

- [Торговые JavaScript-контролы](../javascript_trading_controls.md)
- [Стакан](order_book.md)
- [Ввод заявки](order_entry.md)
