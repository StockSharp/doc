# Управление прибылью и убытками

В [S\#](../api.md) расчёт прибылей и убытков (PnL) реализован через менеджер [PnLManager](xref:StockSharp.Algo.PnL.PnLManager). Менеджер обрабатывает поток сообщений (сделки, рыночные данные) и вычисляет реализованную и нереализованную прибыль.

## Интерфейс IPnLManager

Интерфейс [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) определяет базовый контракт:

- **RealizedPnL** — реализованная прибыль/убыток (decimal). Формируется при закрытии позиций.
- **UnrealizedPnL** — нереализованная прибыль/убыток (decimal). Пересчитывается по текущим рыночным ценам.
- **Reset()** — сброс состояния менеджера.
- **UpdateSecurity(Level1ChangeMessage)** — обновление параметров инструмента (шаг цены, стоимость шага, мультипликатор лота).
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** — обработка сообщения; возвращает [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) при закрытии позиции, иначе `null`.

## Архитектура

Система PnL имеет трёхуровневую иерархию:

```
PnLManager
  └── PortfolioPnLManager (по имени портфеля)
        └── PnLQueue (по SecurityId)
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) — верхний уровень, управляет словарём портфельных менеджеров.
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) — менеджер PnL для конкретного портфеля, управляет очередями по инструментам.
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) — FIFO-очередь для сопоставления сделок по одному инструменту.

### PnLQueue — очередь расчёта

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) отвечает за сопоставление открывающих и закрывающих сделок:

- **PriceStep** — шаг цены инструмента.
- **StepPrice** — стоимость шага цены (для фьючерсов).
- **Leverage** — кредитное плечо.
- **LotMultiplier** — множитель лота.

Мультипликатор прибыли вычисляется по формуле:

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

Для обычных акций (где `StepPrice` не задан) мультипликатор равен `1 * Leverage * LotMultiplier`.

## PnLInfo — результат обработки сделки

Класс [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) содержит результат закрытия позиции:

- **ServerTime** — время сделки.
- **ClosedVolume** — объём закрытой позиции.
- **PnL** — реализованная прибыль от данной сделки.

Например, если позиция была +2, а пришла сделка на -5 контрактов, то `ClosedVolume = 2` (закрыты 2 контракта из позиции).

## Настройка источников данных

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) позволяет выбирать источники рыночных данных для расчёта нереализованной прибыли:

| Свойство | По умолчанию | Описание |
|----------|:------------:|----------|
| `UseTick` | `true` | Использовать тиковые сделки. |
| `UseOrderBook` | `false` | Использовать стакан (лучший bid/ask). |
| `UseLevel1` | `false` | Использовать данные Level1. |
| `UseOrderLog` | `false` | Использовать лог заявок. |
| `UseCandles` | `true` | Использовать свечи (цена закрытия). |

## Интеграция через адаптер

Класс [PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) оборачивает внутренний адаптер и автоматически обрабатывает все сообщения для расчёта PnL.

## Интеграция со стратегией

Стратегия ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) предоставляет:

- Свойство `PnLManager` — экземпляр менеджера.
- Свойство `PnL` — суммарная прибыль (`RealizedPnL + UnrealizedPnL`).
- Событие `PnLChanged` — уведомление об изменении прибыли.
- Событие `PnLReceived2` — уведомление при получении новых данных о PnL.

## Пример использования

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// Обработка сообщений
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"Закрыто: {info.ClosedVolume}, PnL: {info.PnL}");
}

// Общая прибыль/убыток
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Реализованный PnL: {realizedPnL}");
Console.WriteLine($"Нереализованный PnL: {unrealizedPnL}");
Console.WriteLine($"Итого PnL: {totalPnL}");
```

## Сброс состояния

Метод `Reset()` очищает все портфельные менеджеры, очереди расчёта и обнуляет реализованный PnL:

```cs
pnlManager.Reset();
```
