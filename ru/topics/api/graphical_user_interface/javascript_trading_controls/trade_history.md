# История сделок

`TradeHistoryWidget` — таблица исполнений текущего портфеля. Последние сделки располагаются сверху; для каждой строки показываются время, инструмент, сторона, количество, цена, идентификатор сделки и идентификатор заявки.

## Создание и загрузка

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

Создание контрола не запускает загрузку автоматически. Метод `refresh()` получает актуальный портфель через `host.trading.portfolioId()`, проверяет разрешение на действие и вызывает:

```ts
const portfolioId = host.trading.portfolioId();
if (portfolioId && host.allow('load trade history'))
  host.trading.api.getExecutions(portfolioId, null, 200);
```

Такой подход важен при переключении портфелей: идентификатор читается непосредственно перед каждым обновлением, а не сохраняется при создании панели.

## Поведение

Панель предназначена только для чтения. Пользователь может сортировать и выделять строки, открыть контекстное меню, обновить данные и экспортировать видимые столбцы в XLSX. Ошибки загрузки передаются в `host.log`.

Публичный API контрола состоит из `refresh(): Promise<void>` и `dispose(): void`.

## Смотрите также

- [Торговые JavaScript-контролы](../javascript_trading_controls.md)
- [Активные заявки](active_orders.md)
- [Лента сделок](trade_feed.md)
