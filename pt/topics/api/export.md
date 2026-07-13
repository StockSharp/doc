# Exportação de dados

[S#](../api.md) implementa um subsistema de exportação de dados de mercado que suporta vários formatos. Todos os exportadores herdam da classe base [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) e suportam uma interface assíncrona unificada.

## BaseExporter

A classe abstrata base [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) define o contrato comum para todos os exportadores:

- **DataType** - o tipo de dados a exportar (ticks, velas, livro de ordens, etc.).
- **Encoding** - codificação (UTF-8 por predefinição).
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** - o método principal de exportação. Devolve `Task<(int count, DateTime? lastTime)>` - o número de registos exportados e a hora do último registo.

O método encaminha automaticamente os dados para manipuladores específicos por tipo para: [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage), [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) (ticks, registo de ordens, transações), [CandleMessage](xref:StockSharp.Messages.CandleMessage), [NewsMessage](xref:StockSharp.Messages.NewsMessage), [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage), [IndicatorValue](xref:StockSharp.Messages.IndicatorValue) e [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage).

## Tipos de exportadores

### 1. TextExporter - Exportação CSV/texto

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) exporta dados para formato de texto utilizando modelos SmartFormat.

- **Construtor**: `(DataType dataType, Stream stream, string template, string header)`
- Os modelos utilizam a sintaxe SmartFormat, por exemplo: `{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "Data;Preço;Volume");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter - Exportação JSON

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) guarda dados em formato JSON.

- **Construtor**: `(DataType dataType, Stream stream)`
- **Indent** - formatação com indentação (predefinição `true`).

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter - Exportação XML

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) guarda dados em formato XML.

- **Construtor**: `(DataType dataType, Stream stream)`
- **Indent** - formatação com indentação (predefinição `true`).

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter - Exportação Excel

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) exporta dados para folhas de cálculo Excel.

- **Construtor**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- Número máximo de linhas: 1.048.576 (limitação do formato Excel).

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* tratar interrupção */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter - Exportação para base de dados

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) guarda dados numa base de dados através de LinqToDB.

- **Construtor**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- **BatchSize** - tamanho do lote para registos (predefinição 50).
- **CheckUnique** - verificar unicidade dos registos (predefinição `false`).
- **DropExisting** - eliminar dados existentes antes da exportação (predefinição `false`).

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter - Formato nativo StockSharp

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) guarda dados no formato interno de armazenamento StockSharp.

- **Construtor**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- **BatchSize** - tamanho do lote para registos (predefinição 50).

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry - Registo de modelos de texto

A classe [TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) contém modelos predefinidos para exportar vários tipos de dados através de [TextExporter](xref:StockSharp.Algo.Export.TextExporter):

- **TemplateTxtTick** - modelo para dados de ticks.
- **TemplateTxtDepth** - modelo para livros de ordens.
- **TemplateTxtCandle** - modelo para velas.
- **TemplateTxtLevel1** - modelo para dados Level1.
- **TemplateTxtOrderLog** - modelo para registo de ordens.
- **TemplateTxtTransaction** - modelo para transações.
- **TemplateTxtSecurity** - modelo para instrumentos.
- **TemplateTxtNews** - modelo para notícias.

Os modelos podem ser personalizados ou substituídos conforme necessário. O registo implementa [IPersistable](xref:Ecng.Serialization.IPersistable) e pode ser guardado/carregado a partir das definições.

## Ver também

[Importação de dados](import.md)

[Armazenamento de dados](market_data_storage.md)
