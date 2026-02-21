# Составные свечные паттерны

## Обзор

Класс `ComplexCandlePattern` позволяет создавать составные свечные паттерны, комбинируя несколько простых паттернов (`ICandlePattern`) в один. При распознавании составного паттерна каждый внутренний паттерн проверяется последовательно на своем участке свечей. Паттерн считается распознанным, только если все внутренние паттерны совпали.

## ICandlePattern

Базовый интерфейс для всех свечных паттернов:

```csharp
public interface ICandlePattern : IPersistable
{
    // Название паттерна
    string Name { get; }

    // Количество свечей, необходимых для распознавания
    int CandlesCount { get; }

    // Проверить, распознается ли паттерн на переданных свечах
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

Реестр `CandlePatternRegistry` содержит набор встроенных паттернов: `Flat`, `White`, `Black`, `Hammer`, `BullishEngulfing`, `MorningStar`, `ThreeWhiteSoldiers` и другие.

## ComplexCandlePattern

Класс `ComplexCandlePattern` реализует `ICandlePattern` и объединяет несколько внутренних паттернов:

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // Создание пустого паттерна
    public ComplexCandlePattern() { }

    // Создание паттерна с именем и набором внутренних паттернов
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // Название составного паттерна
    public string Name { get; }

    // Внутренние паттерны
    public IEnumerable<ICandlePattern> Inner { get; }

    // Общее количество свечей (сумма CandlesCount всех внутренних паттернов)
    public int CandlesCount { get; }
}
```

При вызове `Recognize` массив свечей разбивается на последовательные участки в соответствии с `CandlesCount` каждого внутреннего паттерна. Если хотя бы один внутренний паттерн не совпал, метод возвращает `false`.

## Пример создания составного паттерна

```csharp
using StockSharp.Algo.Candles.Patterns;

// Создать составной паттерн: сначала медвежья свеча, затем бычье поглощение
var complex = new ComplexCandlePattern(
    "Разворот вверх",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 свеча: медвежья
        CandlePatternRegistry.BullishEngulfing,  // 2 свечи: бычье поглощение
    }
);

// Для распознавания нужно 3 свечи (1 + 2)
Console.WriteLine($"Необходимо свечей: {complex.CandlesCount}"); // 3
```

## ICandlePatternProvider

Интерфейс `ICandlePatternProvider` управляет хранением и поиском паттернов:

```csharp
public interface ICandlePatternProvider
{
    // События создания, замены и удаления паттернов
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // Инициализация хранилища
    ValueTask InitAsync(CancellationToken cancellationToken);

    // Все доступные паттерны
    IEnumerable<ICandlePattern> Patterns { get; }

    // Поиск паттерна по имени
    bool TryFind(string name, out ICandlePattern pattern);

    // Удалить паттерн
    bool Remove(ICandlePattern pattern);

    // Сохранить (создать или заменить) паттерн
    void Save(ICandlePattern pattern);
}
```

### Реализации

- `InMemoryCandlePatternProvider` -- хранит паттерны в памяти. При инициализации загружает все встроенные паттерны из `CandlePatternRegistry.All`.
- `CandlePatternFileStorage` -- сохраняет пользовательские паттерны в файл (JSON). Встроенные паттерны из `InMemoryCandlePatternProvider` также доступны через этот провайдер.

## Пример работы с ICandlePatternProvider

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

// Создать файловое хранилище паттернов
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// Инициализация (загрузит встроенные + пользовательские паттерны из файла)
await provider.InitAsync(CancellationToken.None);

// Подписка на событие создания нового паттерна
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"Создан паттерн: {pattern.Name}");
};

// Создать и сохранить составной паттерн
var myPattern = new ComplexCandlePattern(
    "Мой паттерн",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// Найти паттерн по имени
if (provider.TryFind("Мой паттерн", out var found))
{
    Console.WriteLine($"Найден: {found.Name}, свечей: {found.CandlesCount}");
}
```

## ExpressionCandlePattern

Для создания паттернов на основе формул используется `ExpressionCandlePattern`. Каждая свеча в паттерне описывается выражением `CandleExpressionCondition`, в котором доступны переменные:

| Переменная | Описание |
|---|---|
| `O` | Цена открытия |
| `H` | Максимальная цена |
| `L` | Минимальная цена |
| `C` | Цена закрытия |
| `V` | Объем |
| `B` | Тело свечи |
| `LEN` | Длина свечи |
| `BS` | Нижняя тень |
| `TS` | Верхняя тень |

Префикс `p` указывает на предыдущую свечу (`pO`, `pC`), `pp` -- на две свечи назад и т.д.

Все встроенные паттерны в `CandlePatternRegistry` построены именно через `ExpressionCandlePattern`.
