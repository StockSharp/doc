# Экспорт данных

В [S\#](../api.md) реализована подсистема экспорта маркет-данных в различные форматы. Все экспортёры наследуются от базового класса [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) и поддерживают единый асинхронный интерфейс.

## BaseExporter

Базовый абстрактный класс [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) определяет общий контракт для всех экспортёров:

- **DataType** — тип экспортируемых данных (тики, свечи, стакан и т.д.).
- **Encoding** — кодировка (по умолчанию UTF-8).
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** — основной метод экспорта. Возвращает `Task<(int count, DateTime? lastTime)>` — количество экспортированных записей и время последней записи.

Метод автоматически маршрутизирует данные к типоспецифичным обработчикам для: [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage), [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) (тики, лог заявок, транзакции), [CandleMessage](xref:StockSharp.Messages.CandleMessage), [NewsMessage](xref:StockSharp.Messages.NewsMessage), [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage), [IndicatorValue](xref:StockSharp.Messages.IndicatorValue) и [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage).

## Типы экспортёров

### 1. TextExporter — CSV/текстовый экспорт

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) выполняет экспорт данных в текстовый формат с использованием шаблонов SmartFormat.

- **Конструктор**: `(DataType dataType, Stream stream, string template, string header)`
- Шаблоны используют синтаксис SmartFormat, например: `{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "Date;Price;Volume");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter — экспорт в JSON

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) сохраняет данные в формате JSON.

- **Конструктор**: `(DataType dataType, Stream stream)`
- **Indent** — форматирование с отступами (по умолчанию `true`).

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter — экспорт в XML

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) сохраняет данные в формате XML.

- **Конструктор**: `(DataType dataType, Stream stream)`
- **Indent** — форматирование с отступами (по умолчанию `true`).

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter — экспорт в Excel

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) выполняет экспорт в электронные таблицы Excel.

- **Конструктор**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- Максимальное количество строк: 1 048 576 (ограничение формата Excel).

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* обработка прерывания */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter — экспорт в базу данных

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) сохраняет данные в базу данных через LinqToDB.

- **Конструктор**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- **BatchSize** — размер пакета записей (по умолчанию 50).
- **CheckUnique** — проверка уникальности записей (по умолчанию `false`).
- **DropExisting** — удалить существующие данные перед экспортом (по умолчанию `false`).

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter — нативный формат StockSharp

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) сохраняет данные во внутренний формат хранилища StockSharp.

- **Конструктор**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- **BatchSize** — размер пакета записей (по умолчанию 50).

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry — реестр текстовых шаблонов

Класс [TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) содержит предопределённые шаблоны для экспорта различных типов данных через [TextExporter](xref:StockSharp.Algo.Export.TextExporter):

- **TemplateTxtTick** — шаблон для тиковых данных.
- **TemplateTxtDepth** — шаблон для стаканов.
- **TemplateTxtCandle** — шаблон для свечей.
- **TemplateTxtLevel1** — шаблон для данных Level1.
- **TemplateTxtOrderLog** — шаблон для лога заявок.
- **TemplateTxtTransaction** — шаблон для транзакций.
- **TemplateTxtSecurity** — шаблон для инструментов.
- **TemplateTxtNews** — шаблон для новостей.

Шаблоны можно настроить или заменить при необходимости. Реестр реализует [IPersistable](xref:Ecng.Serialization.IPersistable) и может сохраняться/загружаться из настроек.

## См. также

[Импорт данных](import.md)

[Хранение данных](market_data_storage.md)
