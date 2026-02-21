# Управление позициями

В StockSharp предусмотрена гибкая система управления позициями, позволяющая отслеживать текущее состояние позиций, рассчитывать их на основе заявок или сделок, а также вести историю жизненного цикла (открытие, закрытие, развороты).

## PositionManager

Класс [PositionManager](xref:StockSharp.Algo.Positions.PositionManager) реализует интерфейс [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) и является основным компонентом для расчёта текущих позиций на основе входящих сообщений.

### Создание менеджера

Конструктор принимает два параметра:

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` -- позиция рассчитывается на основе изменений баланса заявок. Подходит, когда торговая система получает обновления состояния заявок, но не отдельные сделки.
- `byOrders = false` -- позиция рассчитывается на основе объёмов сделок (рекомендуемый режим). Обеспечивает более точный учёт исполненных операций.

### Обработка сообщений

Метод `ProcessMessage` принимает входящее сообщение ([Message](xref:StockSharp.Messages.Message)) и возвращает [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) при изменении позиции, или `null`, если позиция не изменилась:

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"Позиция: {posChange.CurrentValue}");
}
```

## IPositionManagerState

Интерфейс [IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) описывает внутреннее состояние менеджера позиций. Реализация [PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) хранит информацию о текущих заявках и позициях.

### Основные методы

| Метод | Описание |
|-------|----------|
| `AddOrGetOrder` | Регистрирует новую заявку или возвращает существующую по `transactionId` |
| `TryGetOrder` | Получает параметры заявки (инструмент, портфель, направление, баланс) |
| `UpdateOrderBalance` | Обновляет текущий баланс заявки после частичного исполнения |
| `RemoveOrder` | Удаляет завершённую заявку из отслеживания |
| `UpdatePosition` | Обновляет позицию по инструменту и портфелю, возвращает новое значение |
| `Clear` | Сбрасывает всё состояние менеджера |

### Пример работы с состоянием

```cs
var state = new PositionManagerState();

// Регистрация заявки
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// Обновление после частичного исполнения
state.UpdateOrderBalance(12345, newBalance: 60);

// Обновление позиции напрямую
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"Текущая позиция: {newPosition}");

// Очистка
state.Clear();
```

## PositionLifecycleTracker

Класс [PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) отслеживает полный жизненный цикл позиций -- от открытия до закрытия (round-trip). Это полезно для анализа отдельных сделок, расчёта прибыли по каждой позиции и построения отчётов.

### Основные возможности

- **История**: свойство `History` (`IReadOnlyList<ReportPosition>`) содержит все завершённые round-trip позиции.
- **Событие `RoundTripClosed`**: срабатывает при закрытии позиции (значение достигло нуля) или при развороте (смена знака позиции).
- **Метод `ProcessPosition`**: принимает объект [Position](xref:StockSharp.BusinessEntities.Position) и обновляет внутреннее состояние.

### Детектируемые состояния

| Состояние | Описание |
|-----------|----------|
| Открытие | Позиция из нуля переходит в ненулевое значение |
| Закрытие | Значение позиции достигает нуля |
| Разворот | Знак позиции меняется (например, с длинной на короткую) |

### Пример использования

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"Round-trip завершён:");
    Console.WriteLine($"  Открытие: {report.OpenTime}");
    Console.WriteLine($"  Закрытие: {report.CloseTime}");
};

// Обработка обновлений позиций
tracker.ProcessPosition(position);

// Просмотр истории
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## PositionMessageAdapter

Класс [PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) представляет собой обёртку над адаптером сообщений, которая автоматически рассчитывает позиции из потока сообщений. Используется во внутренней инфраструктуре коннекторов.

### Принцип работы

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

Адаптер перехватывает сообщения об исполнении заявок и сделок, вызывает `PositionManager.ProcessMessage` и генерирует соответствующие `PositionChangeMessage` для вышестоящих обработчиков.

## Позиции в стратегиях

В классе [Strategy](xref:StockSharp.Algo.Strategies.Strategy) доступ к текущей позиции осуществляется через свойство `Position`:

```cs
// Текущая позиция по основному инструменту
decimal currentPosition = Position;

// Закрытие позиции
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// Или через встроенный метод
ClosePosition();
```

Подробнее о торговых операциях в стратегиях -- в разделе [Торговые операции](strategies/trading_operations.md).

## См. также

- [Торговые операции](strategies/trading_operations.md)
- [Защита позиций](strategies/take_profit_and_stop_loss.md)
- [Управление целевой позицией](strategies/target_position_management.md)
- [Отчётность](strategies/reporting.md)
