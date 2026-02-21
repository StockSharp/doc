# Измерение задержки

В [S\#](../api.md) задержка (latency) регистрации и отмены заявок измеряется через менеджер [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager). Менеджер определяет, сколько времени проходит между отправкой заявки и получением подтверждения от биржи.

## Интерфейс ILatencyManager

Интерфейс [ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) определяет базовый контракт:

- **LatencyRegistration** — суммарная задержка регистрации по всем заявкам (TimeSpan).
- **LatencyCancellation** — суммарная задержка отмены по всем заявкам (TimeSpan).
- **Reset()** — сброс состояния менеджера.
- **ProcessMessage(Message)** — обработка сообщения; возвращает задержку для данной операции или `null`.

## Принцип работы

Менеджер задержки работает по схеме "запрос — ответ":

### 1. Регистрация заявки

При получении [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) менеджер сохраняет пару (`TransactionId`, `LocalTime`) — момент отправки заявки.

### 2. Отмена заявки

При получении [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) менеджер сохраняет пару (`TransactionId`, `LocalTime`) — момент отправки отмены.

### 3. Замена заявки

При получении [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) менеджер регистрирует и отмену (старой заявки), и регистрацию (новой заявки) одновременно.

### 4. Подтверждение

При получении [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) с информацией о заявке (не в состоянии `Pending` и не `Failed`) менеджер вычисляет задержку:

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

Результат добавляется к `LatencyRegistration` или `LatencyCancellation` в зависимости от типа операции.

## Состояние: ILatencyManagerState

Интерфейс [ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) хранит внутреннее состояние менеджера:

- Ожидающие регистрации: `AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- Ожидающие отмены: `AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- Накопленные задержки: `LatencyRegistration`, `LatencyCancellation`
- Методы добавления: `AddLatencyRegistration(TimeSpan)`, `AddLatencyCancellation(TimeSpan)`

Реализация по умолчанию — [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState).

## Обработка ошибок

Если заявка завершается с ошибкой (`OrderState == Failed`), задержка не учитывается — запись просто удаляется из хранилища состояния.

## Интеграция через адаптер

Класс [LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) оборачивает внутренний адаптер и автоматически измеряет задержку для всех операций с заявками.

## Интеграция со стратегией

Стратегия ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) предоставляет свойство `Latency` для отслеживания задержки.

## Пример использования

```cs
// Создание менеджера с хранилищем состояния
var manager = new LatencyManager(new LatencyManagerState());

// Обработка регистрации заявки (запоминание времени отправки)
manager.ProcessMessage(orderRegisterMsg);

// Обработка подтверждения (расчёт задержки)
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Задержка: {latency.Value.TotalMilliseconds} мс");
}

// Суммарные задержки
Console.WriteLine($"Задержка регистрации: {manager.LatencyRegistration.TotalMilliseconds} мс");
Console.WriteLine($"Задержка отмены: {manager.LatencyCancellation.TotalMilliseconds} мс");
```

## Сброс состояния

Метод `Reset()` очищает все ожидающие записи и обнуляет накопленные задержки:

```cs
manager.Reset();
```
