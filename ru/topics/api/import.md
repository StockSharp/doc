# Импорт данных

В [S\#](../api.md) реализована подсистема импорта маркет-данных из CSV-файлов. Основные классы расположены в пространстве имён `StockSharp.Algo.Import`.

## CsvParser — базовый парсер

Класс [CsvParser](xref:StockSharp.Algo.Import.CsvParser) выполняет разбор CSV-файлов и преобразование строк в сообщения [S\#](../api.md).

- **Конструктор**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **ColumnSeparator** — разделитель колонок (по умолчанию `","`).
- **LineSeparator** — разделитель строк (по умолчанию CRLF).
- **SkipFromHeader** — количество строк, пропускаемых от начала файла (по умолчанию `0`).
- **IgnoreNonIdSecurities** — игнорировать строки с нераспознанными инструментами (по умолчанию `true`).
- **Parse(Stream)** — метод разбора, возвращает `IAsyncEnumerable<Message>`.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);
var parser = new CsvParser(DataType.Ticks, fields)
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");

await foreach (var msg in parser.Parse(stream))
{
    // обработка каждого сообщения
}
```

## CsvImporter — импорт с сохранением в хранилище

Класс [CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) расширяет [CsvParser](xref:StockSharp.Algo.Import.CsvParser), добавляя возможность автоматического сохранения данных в хранилище маркет-данных.

- **Конструктор**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** — выполняет импорт и возвращает `ValueTask<(int count, DateTime? lastTime)>`.
- **UpdateDuplicateSecurities** — обновлять ли дубликаты инструментов (по умолчанию `false`).
- **SecurityUpdated** — событие, вызываемое при обновлении инструмента.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);

var importer = new CsvImporter(
    DataType.Ticks,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, DataType.Ticks))
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");
var (count, lastTime) = await importer.Import(
    stream,
    p => Console.WriteLine($"Прогресс: {p}%"),
    token);

Console.WriteLine($"Импортировано {count} записей, последняя: {lastTime}");
```

## FieldMapping — описание полей

Класс [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) описывает соответствие между колонками CSV-файла и свойствами сообщений.

Основные свойства:

- **Name** — имя поля в сообщении.
- **DisplayName** — отображаемое имя.
- **Type** — тип значения.
- **Order** — порядковый номер колонки в файле (начиная с 0).
- **IsRequired** — обязательное ли поле.
- **Format** — формат разбора (например, формат даты).
- **DefaultValue** — значение по умолчанию.
- **ZeroAsNull** — интерпретировать ли нулевые значения как `null`.

Для пользовательских преобразований значений используется [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue). Например, можно задать маппинг текстовых значений в перечисления:

```cs
var sideField = fields.First(f => f.Name == "Side");
sideField.Order = 3;
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "B",
    ValueTo = Sides.Buy
});
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "S",
    ValueTo = Sides.Sell
});
```

## FieldMappingRegistry — реестр стандартных полей

Статический класс [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) предоставляет метод для создания стандартного набора полей:

- **CreateFields(DataType)** — возвращает список [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) для указанного типа данных.

Поддерживаемые типы данных: тики, свечи, стаканы, Level1, лог заявок, транзакции, инструменты, новости и позиции.

## ImportSettings — настройки импорта

Класс [ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) объединяет все параметры импорта в единый конфигурационный объект:

- **DataType** — тип импортируемых данных.
- **FileName** — путь к файлу.
- **Directory** — директория для поиска файлов.
- **FileMask** — маска для поиска файлов (например, `*.csv`).
- **ColumnSeparator** — разделитель колонок.
- **SkipFromHeader** — количество пропускаемых строк.
- **SelectedFields** — выбранные для импорта поля.
- **UpdateDuplicateSecurities** — обновлять ли дубликаты инструментов.

Вспомогательные методы:

- **GetFiles(IFileSystem)** — получить список файлов по маске.
- **FillParser(CsvParser)** — заполнить настройки парсера.
- **FillImporter(CsvImporter)** — заполнить настройки импортёра.

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// настройка порядка колонок
fields[0].Order = 0; // SecurityId
fields[1].Order = 1; // Date
fields[2].Order = 2; // Price
fields[3].Order = 3; // Volume

var importer = new CsvImporter(
    settings.DataType,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, settings.DataType));

settings.FillImporter(importer);

await using var stream = File.OpenRead(settings.FileName);
var (count, lastTime) = await importer.Import(stream, p => { }, token);
```

## См. также

[Экспорт данных](export.md)

[Хранение данных](market_data_storage.md)
