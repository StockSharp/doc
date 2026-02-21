# Продвинутая оптимизация

## Обзор

StockSharp предоставляет расширенные инструменты для тонкой настройки процесса оптимизации стратегий. В этом разделе описаны продвинутые компоненты: конфигурация генетического алгоритма через [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings), компиляция пользовательских фитнес-формул через [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider), а также мониторинг прогресса через [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker).

## GeneticSettings

Класс [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) управляет всеми аспектами генетического алгоритма оптимизации. Расположен в пространстве имен `StockSharp.Algo.Strategies.Optimization`.

### Параметры популяции и поколений

| Свойство | Тип | По умолчанию | Описание |
|----------|-----|-------------|----------|
| `Population` | int | 8 | Начальный размер популяции |
| `PopulationMax` | int | 16 | Максимальный размер популяции |
| `GenerationsMax` | int | 20 | Максимальное количество поколений |
| `GenerationsStagnation` | int | 5 | Остановка при стагнации (поколений без улучшения) |

### Параметры вероятностей

| Свойство | Тип | По умолчанию | Описание |
|----------|-----|-------------|----------|
| `MutationProbability` | decimal | 0.1 | Вероятность мутации (0-1) |
| `CrossoverProbability` | decimal | 0.8 | Вероятность скрещивания (0-1) |

### Генетические операторы

| Свойство | По умолчанию | Описание |
|----------|-------------|----------|
| `Reinsertion` | ElitistReinsertion | Стратегия замены поколений |
| `Mutation` | UniformMutation | Оператор мутации |
| `Crossover` | OnePointCrossover | Оператор скрещивания |
| `Selection` | TournamentSelection | Оператор отбора |

## FitnessFormulaProvider

Класс [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) компилирует строковые выражения C# в функции `Func<Strategy, decimal>`, используемые для оценки стратегий во время оптимизации.

### Метод Compile

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### Доступные переменные

В формулах фитнес-функции доступны следующие переменные, соответствующие статистическим показателям стратегии:

| Переменная | Описание |
|-----------|----------|
| PnL | Чистая прибыль |
| WinTrades | Прибыльные сделки |
| LosTrades | Убыточные сделки |
| TCount | Всего сделок |
| RTrip | Раундтрипы |
| AvgTPnL | Средняя прибыль на сделку |
| AvgWTrades | Средняя прибыльная сделка |
| AvgLTrades | Средняя убыточная сделка |
| MaxLong | Макс. длинная позиция |
| MaxShort | Макс. короткая позиция |
| MaxPnL | Макс. прибыль |
| MaxDD | Макс. просадка |
| MaxRelDD | Макс. относительная просадка |
| Ret | Доходность |
| Recovery | Фактор восстановления |
| MaxLatReg | Макс. задержка регистрации |
| MaxLatCan | Макс. задержка отмены |
| MinLatReg | Мин. задержка регистрации |
| MinLatCan | Мин. задержка отмены |
| OrdCount | Количество заявок |
| OrdRegErrCount | Ошибки регистрации |
| OrdCancelErrCount | Ошибки отмены |
| OrdFundErrCount | Ошибки "недостаточно средств" |

Переменные можно комбинировать в произвольных математических выражениях: `"PnL - MaxDD * 2"`, `"Recovery * WinTrades"`, `"PnL / (MaxDD + 1)"`.

## OptimizationProgressTracker

Класс [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) предоставляет удобный мониторинг процесса оптимизации.

### Свойства

| Свойство | Тип | Описание |
|----------|-----|----------|
| `TotalIterations` | int | Общее количество итераций |
| `CompletedIterations` | int | Количество завершенных итераций |
| `TotalProgress` | double | Общий прогресс (0-100) |
| `StartedAt` | DateTimeOffset | Время начала оптимизации |
| `Elapsed` | TimeSpan | Затраченное время |
| `Remaining` | TimeSpan | Оценка оставшегося времени |

### Методы

| Метод | Описание |
|-------|----------|
| `IterationCompleted()` | Отмечает завершение одной итерации |
| `Reset(totalIterations)` | Сбрасывает трекер для нового запуска |

## Классы оптимизаторов

StockSharp предоставляет два оптимизатора, наследующих от `BaseOptimizer`:

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** -- полный перебор всех комбинаций параметров. Подходит для небольшого пространства параметров.
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- генетический алгоритм. Эффективен при большом количестве параметров, автоматически сходится к оптимальному решению.

## Пример использования

```cs
var geneticSettings = new GeneticSettings
{
	Population = 16,
	PopulationMax = 32,
	GenerationsMax = 50,
	GenerationsStagnation = 10,
	MutationProbability = 0.15m,
	CrossoverProbability = 0.75m,
};

// Пользовательская формула фитнес-функции
var fitnessProvider = new FitnessFormulaProvider();
var fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");

// Создание генетического оптимизатора
var optimizer = new GeneticOptimizer(
	new CollectionSecurityProvider(new[] { security }),
	new CollectionPortfolioProvider(new[] { portfolio }),
	storageRegistry,
	Paths.FileSystem);

optimizer.Settings.Population = geneticSettings.Population;
optimizer.Settings.PopulationMax = geneticSettings.PopulationMax;
optimizer.Settings.GenerationsMax = geneticSettings.GenerationsMax;
optimizer.Settings.GenerationsStagnation = geneticSettings.GenerationsStagnation;
optimizer.Settings.MutationProbability = geneticSettings.MutationProbability;
optimizer.Settings.CrossoverProbability = geneticSettings.CrossoverProbability;

// Мониторинг прогресса
var tracker = new OptimizationProgressTracker();
tracker.Reset(totalIterations: 100);

optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
	if (progress == 100)
	{
		tracker.IterationCompleted();
		Console.WriteLine($"Прогресс: {tracker.TotalProgress:F1}%, " +
			$"Осталось: {tracker.Remaining:hh\\:mm\\:ss}");
	}
};
```

## См. также

- [Оптимизация стратегий](../testing/optimization.md)
- [Статистика стратегий](statistics.md)
- [Генетическая оптимизация в Дизайнере](../../designer/optimization/genetic.md)
