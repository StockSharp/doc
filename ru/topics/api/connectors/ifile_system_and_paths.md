# IFileSystem и Paths.FileSystem

## Обзор

`IFileSystem` -- абстракция файловой системы, используемая повсеместно в StockSharp. Вместо прямого обращения к `System.IO.File` и `System.IO.Directory`, компоненты платформы работают через этот интерфейс. Это обеспечивает:

- **Тестируемость** -- возможность подменить файловую систему в модульных тестах
- **Переносимость** -- единый интерфейс для разных сред (локальный диск, облачное хранилище, память)
- **Единообразие** -- все компоненты используют одинаковый подход к работе с файлами

Интерфейс `IFileSystem` определен в пакете `Ecng.Common` и предоставляет методы для работы с файлами и директориями: создание, чтение, запись, удаление, проверка существования, перечисление файлов.

## Paths.FileSystem

Класс `Paths` (пространство имен `StockSharp.Configuration`) предоставляет статическое свойство `FileSystem` -- реализацию `IFileSystem` по умолчанию:

```csharp
public static class Paths
{
    // Стандартная файловая система (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` ссылается на `LocalFileSystem.Instance` -- синглтон, работающий с локальным диском через стандартные средства `System.IO`.

## Основные методы IFileSystem

Интерфейс `IFileSystem` включает методы для типичных операций:

| Метод | Описание |
|---|---|
| `FileExists(path)` | Проверить существование файла |
| `DirectoryExists(path)` | Проверить существование директории |
| `CreateDirectory(path)` | Создать директорию |
| `OpenRead(path)` | Открыть файл для чтения (`Stream`) |
| `OpenWrite(path)` | Открыть файл для записи (`Stream`) |
| `MoveFile(src, dst)` | Переместить файл |
| `DeleteFile(path)` | Удалить файл |
| `EnumerateFiles(path, mask)` | Перечислить файлы в директории |
| `WriteAllTextAsync(path, text)` | Записать текст в файл асинхронно |

## Где используется

Практически все компоненты StockSharp, работающие с файловой системой, принимают `IFileSystem` в конструкторе. Ниже приведены наиболее частые случаи.

### LocalMarketDataDrive

Хранилище рыночных данных на локальном диске:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Рекомендуемый способ -- явная передача IFileSystem
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// Устаревший способ (использует Paths.FileSystem внутри)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### CsvEntityRegistry

Реестр сущностей (биржи, инструменты, портфели) в CSV-формате:

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### SnapshotRegistry

Реестр снимков рыночных данных:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### Сериализация и десериализация

Методы расширения в `Paths` для работы с JSON-конфигурациями:

```csharp
using StockSharp.Configuration;

var fs = Paths.FileSystem;

// Сериализовать объект в файл
settings.Serialize(fs, @"C:\config.json");

// Десериализовать объект из файла
var loaded = @"C:\config.json".Deserialize<SettingsStorage>(fs);

// Асинхронная десериализация
var data = await @"C:\data.json".DeserializeAsync<MyData>(fs, cancellationToken);

// Проверить существование конфигурационного файла
if (@"C:\config.json".IsConfigExists(fs))
{
    // ...
}
```

### CandlePatternFileStorage

Хранилище свечных паттернов:

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage, SecurityMappingStorage и другие

Большинство хранилищ маппингов и идентификаторов также принимают `IFileSystem`:

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## Типичный паттерн использования

В приложениях StockSharp рекомендуется сохранять ссылку на `IFileSystem` и передавать ее во все компоненты:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Получить файловую систему
var fs = Paths.FileSystem;

// Создать хранилище данных
var drive = new LocalMarketDataDrive(fs, Paths.StorageDir);

// Создать коннектор и настроить хранилище
var connector = new Connector();
connector.Adapter.StorageSettings.Drive = drive;

// Загрузить настройки из файла
var configFile = Path.Combine(Paths.AppDataPath, "connector_config.json");

if (configFile.IsConfigExists(fs))
{
    var config = configFile.Deserialize<SettingsStorage>(fs);
    connector.Load(config);
}
```

## Устаревшие перегрузки

Многие классы сохраняют конструкторы без `IFileSystem` для обратной совместимости, но они помечены атрибутом `[Obsolete]`. Эти конструкторы используют `Paths.FileSystem` внутри:

```csharp
// Устаревший способ
[Obsolete("Use IFileSystem overload.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// Рекомендуемый способ
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

Рекомендуется всегда использовать перегрузки с явной передачей `IFileSystem`, так как устаревшие конструкторы будут удалены в будущих версиях.
