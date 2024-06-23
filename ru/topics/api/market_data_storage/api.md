# Работа с API

## Подготовка

Для работы с историческими данными в примерах используется NuGet пакет с образцами исторических данных. Его можно установить из [NuGet Gallery](https://www.nuget.org/packages/StockSharp.Samples.HistoryData). Этот пакет предоставляет набор данных, которые можно использовать для демонстрации работы с хранилищем.

Все коды доступны в [репозитории StockSharp](https://github.com/StockSharp/StockSharp/tree/master/Samples/03_Storage).

## Создание реестра хранилища

Для работы с хранилищем маркет-данных в StockSharp используется класс [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). При создании объекта этого класса можно задать путь к хранилищу по умолчанию через свойство [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive) или указать конкретную папку для работы с историческими данными, используя [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive).

```cs
// Создание StorageRegistry с путем по умолчанию
var storageRegistry = new StorageRegistry();
```

```cs
// Создание StorageRegistry с указанием пути к данным из NuGet пакета
var pathHistory = Paths.HistoryDataPath; // путь к данным из NuGet пакета
var localDrive = new LocalMarketDataDrive(pathHistory);
var storageRegistry = new StorageRegistry()
{
    DefaultDrive = localDrive,
};
```

## Получение данных

Через [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) можно получить доступ к различным типам маркет-данных за нужный временной диапазон. Для этого используются методы:

- [StorageRegistry.GetTimeFrameCandleMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTimeFrameCandleMessageStorage(StockSharp.Messages.SecurityId,System.TimeSpan,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) для свечей
- [StorageRegistry.GetTickMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTickMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats)) для тиков
- [StorageRegistry.GetQuoteMessageStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetQuoteMessageStorage(StockSharp.Messages.SecurityId,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,System.Boolean)) для стаканов

Каждый из этих методов возвращает соответствующее хранилище, из которого можно загрузить данные методом `Load`, указав начальную и конечную даты.

```cs
// Получение свечей
var securityId = "SBER@TQBR".ToSecurityId();
var candleStorage = storageRegistry.GetTimeFrameCandleMessageStorage(securityId, TimeSpan.FromMinutes(1), StorageFormats.Binary);
var candles = candleStorage.Load(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

foreach (var candle in candles)
{
    Console.WriteLine(candle);
}
```

```cs
// Получение тиков
var tradeStorage = storageRegistry.GetTickMessageStorage(securityId, StorageFormats.Binary);
var trades = tradeStorage.Load(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

foreach (var trade in trades)
{
    Console.WriteLine(trade);
}
```

```cs
// Получение стаканов
var marketDepthStorage = storageRegistry.GetQuoteMessageStorage(securityId, StorageFormats.Binary);
var marketDepths = marketDepthStorage.Load(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));

foreach (var marketDepth in marketDepths)
{
    Console.WriteLine(marketDepth);
}
```

## Сохранение данных

Для сохранения новых данных в существующее хранилище используется метод `Save` соответствующего хранилища. Это позволяет дополнить исторические данные новыми значениями.

```cs
// Сохранение новых свечей
var newCandles = new List<CandleMessage>
{
    // Здесь создаются новые объекты CandleMessage
};
candleStorage.Save(newCandles);
```

```cs
// Сохранение новых тиков
var newTrades = new List<ExecutionMessage>
{
    // Здесь создаются новые объекты ExecutionMessage для тиков
};
tradeStorage.Save(newTrades);
```

```cs
// Сохранение новых стаканов
var newMarketDepths = new List<QuoteChangeMessage>
{
    // Здесь создаются новые объекты QuoteChangeMessage для стаканов
};
marketDepthStorage.Save(newMarketDepths);
```

## Удаление данных

Для удаления данных за определенный период используется метод `Delete` соответствующего хранилища. Будьте осторожны при удалении данных из пакета с образцами.

```cs
// Удаление свечей за указанный период
candleStorage.Delete(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Удаление тиков за указанный период
tradeStorage.Delete(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

```cs
// Удаление стаканов за указанный период
marketDepthStorage.Delete(new DateTime(2020, 4, 1), new DateTime(2020, 4, 2));
```

Эти операции позволяют эффективно управлять историческими данными, как загруженными через [Hydra](../../hydra.md) или предоставленными в NuGet пакете, так и созданными в процессе работы вашего приложения.