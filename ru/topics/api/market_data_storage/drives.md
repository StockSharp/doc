# Драйверы хранения

Драйверы хранения в StockSharp отвечают за физическое размещение маркет-данных -- на локальном диске или на удаленном сервере. Базовый интерфейс [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) определяет общий контракт для всех реализаций.

## IMarketDataDrive -- базовый интерфейс

Интерфейс [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) предоставляет следующие ключевые возможности:

- **Path** -- путь к хранилищу данных.
- **GetAvailableSecuritiesAsync()** -- получение списка всех доступных инструментов в хранилище.
- **GetAvailableDataTypesAsync()** -- получение списка типов данных, доступных для конкретного инструмента.
- **GetStorageDrive()** -- получение драйвера хранения для конкретного инструмента и типа данных.
- **VerifyAsync()** -- проверка целостности хранилища.
- **LookupSecuritiesAsync()** -- поиск инструментов по заданным критериям.

## LocalMarketDataDrive -- локальное файловое хранилище

Класс [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) является основной реализацией драйвера, хранящей данные на локальном диске в файловой системе.

### Основные возможности

- **Файловая система** -- данные организованы в иерархической структуре каталогов по инструментам и датам.
- **Система индексов** -- внутренний класс `Index` обеспечивает быстрый доступ к данным без сканирования файловой системы. Файлы индексов хранятся в формате `{путь_к_инструменту}{имя_файла}Dates2.bin`.
- **Потокобезопасность** -- доступ к данным защищен механизмами блокировки для корректной работы в многопоточных приложениях.
- **Построение индексов** -- метод `BuildIndexAsync()` позволяет перестроить индексы для повышения производительности после массовых операций с данными.

### Пример использования

```cs
// Создание локального драйвера с указанием пути
var localDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));

// Использование с реестром хранилищ
var storageRegistry = new StorageRegistry
{
    DefaultDrive = localDrive,
};

// Получение списка доступных инструментов
await foreach (var secId in localDrive.GetAvailableSecuritiesAsync())
{
    Console.WriteLine(secId);
}
```

## RemoteMarketDataDrive -- удаленное хранилище

Класс [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) позволяет подключаться к удаленному серверу Hydra для доступа к маркет-данным по сети.

### Настройки подключения

- **Address** -- адрес удаленного сервера. По умолчанию `127.0.0.1:5002`.
- **Credentials** -- учетные данные для аутентификации (Email и Password).
- **TargetCompId** -- идентификатор целевого компонента, по умолчанию `"StockSharpHydraMD"`.
- **SecurityBatchSize** -- размер пакета при загрузке инструментов, по умолчанию 1000.
- **Timeout** -- таймаут подключения, по умолчанию 2 минуты.

### Пример использования

```cs
// Создание удаленного драйвера
var remoteDrive = new RemoteMarketDataDrive
{
    Address = "192.168.1.100:5002".To<EndPoint>(),
    Credentials = { Email = "user", Password = "pass".Secure() }
};

// Получение доступных типов данных для инструмента
var secId = "SBER@TQBR".ToSecurityId();
await foreach (var dataType in remoteDrive.GetAvailableDataTypesAsync(secId, StorageFormats.Binary))
{
    Console.WriteLine(dataType);
}
```

Подробнее о работе с удаленным хранилищем см. раздел [Работа с удаленным хранилищем](remote.md).

## DriveCache -- управление драйверами

Класс [DriveCache](xref:StockSharp.Algo.Storages.DriveCache) управляет коллекцией драйверов хранения и обеспечивает кэширование для повторного использования.

### Ключевые методы и свойства

- **GetDrive(path)** -- получить существующий драйвер по пути или создать новый.
- **DeleteDrive(drive)** -- удалить драйвер из кэша.
- **TryDefaultDrive** -- первый доступный [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).
- **NewDriveCreated** -- событие создания нового драйвера.
- **DriveDeleted** -- событие удаления драйвера.
- **Changed** -- событие изменения коллекции драйверов.

Класс реализует интерфейс `IPersistable`, что позволяет сохранять и загружать конфигурацию драйверов.

### Пример использования

```cs
// Создание кэша с локальным драйвером по умолчанию
var defaultDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));
var driveCache = new DriveCache(defaultDrive);

// Получение или создание драйвера по пути
var anotherDrive = driveCache.GetDrive(@"D:\MarketData");

// Подписка на события
driveCache.NewDriveCreated += drive =>
    Console.WriteLine($"Создан драйвер: {drive.Path}");
```

## См. также

- [Работа с API](api.md)
- [Работа с удаленным хранилищем](remote.md)
- [Форматы хранения](formats.md)
