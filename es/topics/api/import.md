# Importación de datos

[S#](../api.md) implementa un subsistema de importación de datos de mercado desde archivos CSV. Las clases principales se encuentran en el namespace `StockSharp.Algo.Import`.

## CsvParser — parser base

La clase [CsvParser](xref:StockSharp.Algo.Import.CsvParser) analiza archivos CSV y convierte filas en mensajes [S#](../api.md).

- **Constructor**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **ColumnSeparator** — separador de columnas (por defecto `","`).
- **LineSeparator** — separador de líneas (por defecto CRLF).
- **SkipFromHeader** — número de filas que se omiten desde el inicio del archivo (por defecto `0`).
- **IgnoreNonIdSecurities** — ignorar filas con instrumentos no reconocidos (por defecto `true`).
- **Parse(Stream)** — método de parsing, devuelve `IAsyncEnumerable<Message>`.

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
    // procesar cada mensaje
}
```

## CsvImporter — importación con guardado en almacenamiento

La clase [CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) extiende [CsvParser](xref:StockSharp.Algo.Import.CsvParser), agregando la capacidad de guardar datos automáticamente en un almacenamiento de datos de mercado.

- **Constructor**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** — realiza la importación y devuelve `ValueTask<(int count, DateTime? lastTime)>`.
- **UpdateDuplicateSecurities** — si se deben actualizar instrumentos duplicados (por defecto `false`).
- **SecurityUpdated** — evento generado cuando se actualiza un instrumento.

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
    p => Console.WriteLine($"Progress: {p}%"),
    token);

Console.WriteLine($"Imported {count} records, last: {lastTime}");
```

## FieldMapping — descripciones de campos

La clase [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) describe el mapeo entre columnas del archivo CSV y propiedades del mensaje.

Propiedades principales:

- **Name** — nombre del campo en el mensaje.
- **DisplayName** — nombre mostrado.
- **Type** — tipo del valor.
- **Order** — índice de columna en el archivo (empezando desde 0).
- **IsRequired** — si el campo es obligatorio.
- **Format** — formato de parsing (por ejemplo, formato de fecha).
- **DefaultValue** — valor predeterminado.
- **ZeroAsNull** — si se deben interpretar valores cero como `null`.

Para transformaciones de valor personalizadas, use [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue). Por ejemplo, puede definir un mapeo de valores de texto a enumeraciones:

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

## FieldMappingRegistry — registro de campos estándar

La clase estática [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) proporciona un método para crear un conjunto estándar de campos:

- **CreateFields(DataType)** — devuelve una lista de [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) para el tipo de datos especificado.

Tipos de datos admitidos: ticks, velas, libros de órdenes, Level1, order log, transacciones, instrumentos, noticias y posiciones.

## ImportSettings — configuración de importación

La clase [ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) combina todos los parámetros de importación en un único objeto de configuración:

- **DataType** — tipo de datos que se importan.
- **FileName** — ruta del archivo.
- **Directory** — directorio para buscar archivos.
- **FileMask** — máscara de búsqueda de archivos (por ejemplo, `*.csv`).
- **ColumnSeparator** — separador de columnas.
- **SkipFromHeader** — número de filas que se omiten.
- **SelectedFields** — campos seleccionados para importar.
- **UpdateDuplicateSecurities** — si se deben actualizar instrumentos duplicados.

Métodos auxiliares:

- **GetFiles(IFileSystem)** — obtener una lista de archivos que coinciden con la máscara.
- **FillParser(CsvParser)** — rellenar la configuración del parser.
- **FillImporter(CsvImporter)** — rellenar la configuración del importer.

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// configurar orden de columnas
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

## Véase también

[Exportación de datos](export.md)

[Almacenamiento de datos](market_data_storage.md)
