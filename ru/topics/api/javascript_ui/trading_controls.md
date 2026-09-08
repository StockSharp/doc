# Торговые JavaScript-контролы

[JavaScript-контролы StockSharp для торговли](https://github.com/StockSharp/JS-TradingControls) — набор браузерных панелей для торгового терминала. Пакет [опубликован в npm](https://www.npmjs.com/package/@stocksharp/trading-controls) под именем `@stocksharp/trading-controls`, а все контролы можно увидеть в [онлайн-демо](https://stocksharp.github.io/JS-TradingControls/demo/).

![Торговый экран с лентой сделок, стаканом, списком инструментов, вводом заявки и таблицами](../../../images/javascript_trading_controls.jpg)

На снимке также показан свечной график из отдельного пакета `@stocksharp/chart`. В `@stocksharp/trading-controls` входят пятнадцать самостоятельных контролов:

| Контрол | Класс | Идентификатор |
|---|---|---|
| [Активные заявки](trading_controls/active_orders.md) | `ActiveOrdersWidget` | `activeOrders` |
| [Позиции](trading_controls/positions.md) | `PositionsWidget` | `positions` |
| [История сделок](trading_controls/trade_history.md) | `TradeHistoryWidget` | `tradeHistory` |
| [Список инструментов](trading_controls/watchlist.md) | `WatchlistWidget` | `watchlist` |
| [Ввод заявки](trading_controls/order_entry.md) | `OrderEntryWidget` | `orderEntry` |
| [Стакан](trading_controls/order_book.md) | `OrderBookWidget` | `orderbook` |
| [Лента сделок](trading_controls/trade_feed.md) | `TradeFeedWidget` | `tradefeed` |
| [Статистика](trading_controls/statistics.md) | `StatisticsWidget` | `statistics` |
| [Журнал](trading_controls/log_monitor.md) | `LogMonitorWidget` | `logMonitor` |
| [Стратегии](trading_controls/strategies.md) | `StrategiesWidget` | `strategies` |
| [Доска опционов](trading_controls/option_desk.md) | `OptionDeskWidget` | `optionDesk` |
| [Улыбка волатильности](trading_controls/option_smile.md) | `OptionSmileWidget` | `optionSmile` |
| [Кривая эквити](trading_controls/equity.md) | `EquityWidget` | `equity` |
| [Тепловая карта оптимизации](trading_controls/optimization_heatmap.md) | `OptimizationHeatmapWidget` | `optimizationHeatmap` |
| [Поверхность оптимизации](trading_controls/optimization_surface.md) | `SurfaceWidget` | `optimizationSurface` |

Значения идентификаторов доступны через экспортируемый объект `ControlTypes`. Обратите внимание: у поверхности оптимизации имя класса не совпадает с идентификатором — класс называется `SurfaceWidget`, а идентификатор `optimizationSurface`.

## Установка

```bash
npm install @stocksharp/trading-controls
```

Таблицы контролы рисуют через [@stocksharp/grids](grids.md) — он приезжает автоматически как обычная зависимость. А вот [@stocksharp/chart](charts.md) объявлен **peer-зависимостью**: npm его не поставит, и установить его нужно самому, если вы используете кривую эквити или улыбку волатильности — они построены на движке графиков.

```bash
npm install @stocksharp/chart
```

Помимо корневого импорта пакет объявляет подпути: по одному на каждый контрол (`@stocksharp/trading-controls/watchlist` и т. д.), вспомогательные модули (`/trading-host`, `/control-types`, `/formatters`, `/dom`, `/trading-data`) и параллельное семейство `/source/*` с исходниками на TypeScript — для тех, кто собирает контролы своим сборщиком вместе с остальным кодом.

Основные стили обязательны. Готовую светлую и тёмную палитру можно подключить дополнительно либо заменить собственными CSS-переменными `--t-*`:

```ts
import '@stocksharp/trading-controls/styles.css';
import '@stocksharp/trading-controls/theme.css'; // Необязательно: готовая тема.
```

Контролы используют классы [Bootstrap Icons](https://icons.getbootstrap.com/), но не поставляют сами шрифты и SVG. Хост-страница должна подключить иконки отдельно.

Для страницы без сборщика предназначен файл `dist/sstradingcontrols.js`, который создаёт глобальный объект `window.SSTradingControls`.

## Общая схема создания

Каждый контрол создаётся статическим методом `create`. Метод проверяет хост, строит собственный DOM и добавляет корневой элемент в переданный контейнер:

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('close', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('reverse', portfolioId, instrumentId, symbol),
    refreshPositions: () => console.log('refresh'),
  },
);

positions.update([]);
```

Второй аргумент — сохранённое состояние экземпляра. Набор зависимостей в третьем аргументе различается у контролов: например, панель позиций получает обработчики закрытия и переворота, а стакан — обработчики выбора и исполнения цены.

## Контракт TradingHost

Контролы не обращаются напрямую к глобальному переводчику, хранилищу настроек, торговому соединению или менеджеру окон. Всё внешнее взаимодействие проходит через один объект `TradingHost`.

| Член хоста | Назначение |
|---|---|
| `isPrimary` | Указывает основной экземпляр контрола на странице. |
| `t(key, ...args)` | Переводит видимый текст и подставляет аргументы. |
| `presentation` | Форматирует сторону, тип и состояние заявки, классы прибыли и палитру canvas. |
| `preferences`, `cache` | Хранят долговременные настройки и временные данные. |
| `trading.api` | Ищет инструменты и загружает исполнения. |
| `trading.marketData` | Управляет подписками и предоставляет активные заявки. |
| `trading.portfolioId()` | Возвращает текущий портфель. |
| `trading.pickInstrument(...)` | Открывает выбор инструмента. |
| `ticker` | Получает видимые инструменты и их котировки. |
| `allow(action)` | Проверяет разрешение на действие. |
| `close`, `spawn`, `persistState`, `saveLayout` | Управляют жизненным циклом и состоянием панели. |
| `register`, `unregister`, `broadcast` | Регистрируют экземпляры и рассылают изменения между ними. |
| `log(message)` | Принимает диагностические сообщения. |

Все члены обязательны. `assertHost` проверяет вложенные функции до отрисовки контрола и сообщает точный отсутствующий путь. Если приложению не нужна часть возможностей, для обязательных команд можно передать осмысленные заглушки, например `log: console.warn` или пустой `saveLayout`.

## Локализация и оформление

Видимый текст контролы получают только через `host.t`. Полный актуальный список из 235 ключей поставляется в `@stocksharp/trading-controls/translation-keys.json`. Это не массив, а объект `{ $comment, count, keys }` — сами ключи лежат в поле `keys`. Неизвестный ключ будет показан пользователю как есть, поэтому хост должен определить переводы для всего списка.

Файл `styles.css` содержит правила, но берёт цвета, шрифты и размеры из CSS-переменных `--t-*`. Если готовый `theme.css` не используется, эти переменные определяет приложение. Цвета canvas для стакана и пузырьковой ленты возвращает `host.presentation.canvasPalette()`.

## Освобождение ресурсов

При удалении панели вызовите `dispose()`. Метод снимает обработчики, при наличии отключает наблюдатели и подписки конкретного контрола, а затем вызывает `host.unregister`.

```ts
positions.dispose();
```

## Сборка из исходного кода

```bash
git clone https://github.com/StockSharp/JS-TradingControls.git
cd JS-TradingControls
npm install
npm test
npm run build
```

## Смотрите также

- [JavaScript-таблицы](grids.md)
- [JavaScript-графики](charts.md)
- [Репозиторий JS-TradingControls](https://github.com/StockSharp/JS-TradingControls)
- [Онлайн-демо](https://stocksharp.github.io/JS-TradingControls/demo/)
