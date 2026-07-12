# Exportación de datos

[S#](../api.md) implementa un subsistema de exportación de datos de mercado con soporte para varios formatos. Todos los exportadores heredan de la clase base [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) y admiten una interfaz asíncrona unificada.

## BaseExporter

La clase abstracta base [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) define el contrato común para todos los exportadores:

- **DataType** — tipo de datos que se exportan (ticks, velas, libro de órdenes, etc.).
- **Encoding** — codificación (UTF-8 por defecto).
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** — método principal de exportación. Devuelve `Task<(int count, DateTime? lastTime)>` — el número de registros exportados y la hora del último registro.

El método enruta automáticamente los datos a manejadores específicos por tipo para: [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage), [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) (ticks, registro de órdenes, transacciones), [CandleMessage](xref:StockSharp.Messages.CandleMessage), [NewsMessage](xref:StockSharp.Messages.NewsMessage), [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage), [IndicatorValue](xref:StockSharp.Messages.IndicatorValue) y [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage).

## Tipos de exportadores

### 1. TextExporter — exportación CSV/texto

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) exporta datos a formato de texto usando plantillas SmartFormat.

- **Constructor**: `(DataType dataType, Stream stream, string template, string header)`
- Las plantillas usan sintaxis SmartFormat, por ejemplo: `{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "Fecha;Precio;Volumen");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter — exportación JSON

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) guarda datos en formato JSON.

- **Constructor**: `(DataType dataType, Stream stream)`
- **Indent** — formato con sangría (por defecto `true`).

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter — exportación XML

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) guarda datos en formato XML.

- **Constructor**: `(DataType dataType, Stream stream)`
- **Indent** — formato con sangría (por defecto `true`).

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter — exportación Excel

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) exporta datos a hojas de cálculo Excel.

- **Constructor**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- Número máximo de filas: 1.048.576 (limitación del formato Excel).

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* gestionar la interrupción */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter — exportación a base de datos

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) guarda datos en una base de datos mediante LinqToDB.

- **Constructor**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- **BatchSize** — tamaño del lote para registros (por defecto 50).
- **CheckUnique** — comprobar unicidad de registros (por defecto `false`).
- **DropExisting** — eliminar datos existentes antes de exportar (por defecto `false`).

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter — formato nativo StockSharp

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) guarda datos en el formato interno de almacenamiento StockSharp.

- **Constructor**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- **BatchSize** — tamaño del lote para registros (por defecto 50).

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry — registro de plantillas de texto

La clase [TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) contiene plantillas predefinidas para exportar varios tipos de datos mediante [TextExporter](xref:StockSharp.Algo.Export.TextExporter):

- **TemplateTxtTick** — plantilla para datos de ticks.
- **TemplateTxtDepth** — plantilla para libros de órdenes.
- **TemplateTxtCandle** — plantilla para velas.
- **TemplateTxtLevel1** — plantilla para datos Level1.
- **TemplateTxtOrderLog** — plantilla para registro de órdenes.
- **TemplateTxtTransaction** — plantilla para transacciones.
- **TemplateTxtSecurity** — plantilla para instrumentos.
- **TemplateTxtNews** — plantilla para noticias.

Las plantillas pueden personalizarse o reemplazarse según sea necesario. El registro implementa [IPersistable](xref:Ecng.Serialization.IPersistable) y puede guardarse/cargarse desde configuraciones.

## Véase también

[Importación de datos](import.md)

[Almacenamiento de datos](market_data_storage.md)
