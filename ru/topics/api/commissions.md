# Система комиссий

В [S\#](../api.md) реализована гибкая система расчёта комиссий через менеджер [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager). Менеджер принимает сообщения о заявках и сделках и вычисляет комиссию на основе настроенных правил.

## Интерфейс ICommissionManager

Интерфейс [ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) определяет базовый контракт:

- **Rules** — коллекция правил [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) для расчёта комиссии.
- **Commission** — общая накопленная сумма комиссии (decimal).
- **Reset()** — сброс состояния менеджера и всех правил.
- **Process(Message)** — обработка сообщения; возвращает комиссию за данное сообщение или `null`.

## Интерфейс ICommissionRule

Каждое правило реализует [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule):

- **Title** — заголовок правила.
- **Value** — значение комиссии ([Unit](xref:Ecng.ComponentModel.Unit)), может быть абсолютным или процентным.
- **Process(ExecutionMessage)** — рассчитать комиссию для конкретного сообщения.

Базовый класс [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) содержит вспомогательный метод `GetValue(price, volume)`:

- Для **абсолютных** значений возвращает `Value` как есть.
- Для **процентных** значений вычисляет `(price * volume * Value) / 100`.

## Типы правил

### Правила для заявок

| Класс | Описание |
|-------|----------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | Комиссия за каждую заявку (по цене и объёму заявки). |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | Комиссия за объём заявки. Для абсолютных значений: `Value * volume`. |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | Комиссия за каждые N заявок. Свойство `Count` задаёт порог. |

### Правила для сделок

| Класс | Описание |
|-------|----------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | Комиссия за каждую сделку (по цене и объёму сделки). |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | Комиссия за объём сделки. |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | Комиссия: `price * volume * Value`. |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | Комиссия за каждые N сделок. Свойство `Count` задаёт порог. |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | Комиссия за каждый порог оборота. Свойство `TurnOver` задаёт порог. |

### Правила-фильтры

| Класс | Описание |
|-------|----------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | Комиссия только для конкретного инструмента. Свойство `Security`. |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | Комиссия только для конкретной площадки. Свойство `Board`. |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | Комиссия только для определённого типа инструмента. Свойство `SecurityType`. |

## Интеграция через адаптер

Класс [CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) оборачивает внутренний адаптер и автоматически рассчитывает комиссию на входящих и исходящих [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage). Если у сообщения не установлено поле `Commission`, адаптер заполняет его из менеджера.

## Интеграция со стратегией

Стратегия ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) предоставляет свойство `Commission`, через которое можно отслеживать накопленную комиссию.

## Пример использования

```cs
var manager = new CommissionManager();

// Фиксированная комиссия 1.5 за каждую сделку
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 0.1% от оборота для фьючерсов
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// Комиссия 50 за каждые 100 заявок
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// Комиссия 10 за каждые 1 000 000 оборота
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// Обработка сообщения
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Комиссия за сообщение: {commission.Value}");
}

// Общая накопленная комиссия
Console.WriteLine($"Итого комиссия: {manager.Commission}");
```

## Сброс состояния

Метод `Reset()` обнуляет общую комиссию и вызывает `Reset()` у каждого правила, что сбрасывает внутренние счётчики (количество заявок, текущий оборот и т.д.):

```cs
manager.Reset();
```
