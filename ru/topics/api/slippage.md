# Измерение проскальзывания

В [S\#](../api.md) проскальзывание (slippage) рассчитывается через менеджер [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). Проскальзывание — это разница между ожидаемой ценой исполнения заявки и фактической ценой сделки.

## Интерфейс ISlippageManager

Интерфейс [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) определяет базовый контракт:

- **Slippage** — общее накопленное проскальзывание (decimal).
- **Reset()** — сброс состояния менеджера.
- **ProcessMessage(Message)** — обработка сообщения; возвращает проскальзывание для данного исполнения или `null`.

## Принцип работы

Менеджер проскальзывания работает в три этапа:

### 1. Обновление рыночных цен

При получении [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) или [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) менеджер сохраняет лучшие цены bid и ask для каждого инструмента.

### 2. Запоминание планируемой цены

При получении [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) менеджер сохраняет "планируемую цену" — лучшую рыночную цену на момент регистрации заявки:

- Для покупки (`Buy`) берётся лучший ask.
- Для продажи (`Sell`) берётся лучший bid.

### 3. Расчёт проскальзывания

При получении [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) со сделкой менеджер вычисляет проскальзывание:

- Для покупки: `(TradePrice - PlannedPrice) * TradeVolume`
- Для продажи: `(PlannedPrice - TradePrice) * TradeVolume`

Положительное значение означает неблагоприятное проскальзывание (цена хуже ожидаемой), отрицательное — благоприятное (цена лучше ожидаемой).

## Состояние: ISlippageManagerState

Интерфейс [ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) хранит внутреннее состояние менеджера:

- Лучшие цены bid/ask по каждому инструменту ([SecurityId](xref:StockSharp.Messages.SecurityId)).
- Планируемые цены и направления по каждой транзакции (`TransactionId`).
- Общее накопленное проскальзывание.

Реализация по умолчанию — [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState).

## Настройки

| Свойство | По умолчанию | Описание |
|----------|:------------:|----------|
| `CalculateNegative` | `true` | Учитывать благоприятное проскальзывание. Если `false`, отрицательные значения заменяются нулём. |

## Интеграция через адаптер

Класс [SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) оборачивает внутренний адаптер и автоматически рассчитывает проскальзывание для всех сделок.

## Интеграция со стратегией

Стратегия ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) предоставляет свойство `Slippage` для отслеживания общего проскальзывания.

## Пример использования

```cs
// Создание менеджера с хранилищем состояния
var manager = new SlippageManager(new SlippageManagerState());

// Учитывать только неблагоприятное проскальзывание
manager.CalculateNegative = false;

// Обработка рыночных данных (обновление лучших цен)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Обработка регистрации заявки (запоминание планируемой цены)
manager.ProcessMessage(orderRegisterMsg);

// Обработка сделки (расчёт проскальзывания)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Проскальзывание: {slippage.Value}");
}

// Общее накопленное проскальзывание
Console.WriteLine($"Итого проскальзывание: {manager.Slippage}");
```

## Сброс состояния

Метод `Reset()` полностью очищает внутреннее состояние: лучшие цены, планируемые цены и накопленное проскальзывание:

```cs
manager.Reset();
```
