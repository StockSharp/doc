# Formatos de almacenamiento

StockSharp admite dos formatos de almacenamiento de datos de mercado definidos por la enumeración [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats): **Binary** y **CSV**. Cada formato tiene sus propias ventajas y es adecuado para distintos casos de uso.

## Formato Binary

El formato binario es el formato principal de alto rendimiento para almacenamiento de datos en StockSharp. La implementación se encuentra en la clase `BinaryMarketDataSerializer` dentro de `Algo/Storages/Binary/`.

### Características

- **Compacidad** -- los datos se serializan en forma binaria con compresión, lo que garantiza tamaños de archivo mínimos.
- **Rendimiento** -- la lectura y escritura son significativamente más rápidas en comparación con formatos de texto.
- **Metadatos** -- la clase `BinaryMetaInfo` almacena información auxiliar: primeros y últimos precios, valores fraccionarios, marcas de tiempo. Esto permite obtener rápidamente información general de datos sin leer el archivo completo.
- **Arquitectura preparada para compresión** -- el formato está diseñado desde cero para una compresión de streaming eficiente.

Los archivos binarios tienen la extensión `.bin`.

### Tipos de datos admitidos

El formato binario admite la serialización de todos los tipos principales de datos de mercado: velas, ticks (trades), libros de órdenes (Level2), datos Level1, órdenes y trades propios. Cada tipo tiene un serializador especializado optimizado para la estructura de los datos concretos.

## Formato CSV

El formato de texto CSV (Comma-Separated Values) se implementa en la clase `CsvMarketDataSerializer` ubicada en `Algo/Storages/Csv/`.

### Características

- **Legibilidad** -- los archivos se pueden abrir en cualquier editor de texto o en Excel para análisis visual de datos.
- **Editabilidad** -- los datos se pueden corregir manualmente cuando sea necesario.
- **Metadatos** -- la clase `CsvMetaInfo` proporciona almacenamiento de metadatos con soporte de codificación. Contiene la propiedad `IncrementalOnly` (soporte de datos incrementales) y `LastId` (identificador del último registro).
- **Mayor tamaño de archivo** -- la representación textual ocupa significativamente más espacio en disco.
- **Procesamiento más lento** -- el parseo de datos de texto requiere recursos computacionales adicionales.

Los archivos CSV tienen la extensión `.csv`.

## Cuándo usar cada formato

| Escenario | Formato recomendado |
|----------|-------------------|
| Uso en producción | Binary |
| Grandes volúmenes de datos | Binary |
| Crítico para rendimiento | Binary |
| Depuración y diagnóstico | CSV |
| Análisis visual de datos | CSV |
| Integración con herramientas externas | CSV |
| Corrección manual de datos | CSV |

## Organización de archivos en disco

Los datos en disco se organizan según la siguiente estructura de ruta:

```
{root_folder}/{first_letter}/{instrument_identifier}/{yyyy_MM_dd}/{file_name}.{extension}
```

Donde la extensión es `.bin` para formato binario o `.csv` para formato de texto. Esta organización jerárquica garantiza una búsqueda rápida de datos por instrumento y fecha.

Por ejemplo, las velas de 5 minutos para el instrumento AAPL@NASDAQ del 1 de abril de 2024 en formato binario se ubicarían en una ruta como:

```
Storage/S/AAPL@NASDAQ/2024_04_01/candles_5m.bin
```

## Conversión entre formatos

StockSharp permite cargar datos en un formato y guardarlos en otro. Esto puede ser útil, por ejemplo, para exportar datos binarios a CSV para análisis en herramientas externas:

```cs
// Cargar desde almacenamiento binario
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Binary);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();

// Guardar en almacenamiento CSV
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Csv);
await csvStorage.SaveAsync(candles);
```

## Ejemplo de código

El formato se selecciona al crear un almacenamiento mediante [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry):

```cs
var storageRegistry = new StorageRegistry();

// Crear almacenamiento de velas en formato binario
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Binary);

// Crear almacenamiento de velas en formato CSV
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Csv);

// Cargar datos
var from = new DateTime(2024, 1, 1);
var to = new DateTime(2024, 1, 31);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();
```

El formato de almacenamiento también se puede especificar al trabajar con datos de ticks y libros de órdenes:

```cs
// Ticks en formato binario
var tickStorage = storageRegistry.GetTickMessageStorage(
    securityId, StorageFormats.Binary);

// Libros de órdenes en formato CSV
var depthStorage = storageRegistry.GetQuoteMessageStorage(
    securityId, StorageFormats.Csv);
```

## Véase también

- [Trabajo con la API](api.md)
- [Unidades de almacenamiento](drives.md)
