# IFileSystem y Paths.FileSystem

## Resumen

`IFileSystem` es una abstracción del sistema de archivos usada en todo StockSharp. En lugar de acceder directamente a `System.IO.File` y `System.IO.Directory`, los componentes de la plataforma trabajan mediante esta interfaz. Esto proporciona:

- **Testabilidad** -- la posibilidad de sustituir el sistema de archivos en pruebas unitarias
- **Portabilidad** -- una interfaz unificada para distintos entornos (disco local, almacenamiento en la nube, memoria)
- **Consistencia** -- todos los componentes usan el mismo enfoque para trabajar con archivos

La interfaz `IFileSystem` se define en el paquete `Ecng.Common` y proporciona métodos para trabajar con archivos y directorios: creación, lectura, escritura, eliminación, comprobación de existencia y enumeración de archivos.

## Paths.FileSystem

La clase `Paths` (namespace `StockSharp.Configuration`) proporciona la propiedad estática `FileSystem`: la implementación predeterminada de `IFileSystem`:

```csharp
public static class Paths
{
    // Standard file system (LocalFileSystem)
    public static readonly IFileSystem FileSystem = Messages.Extensions.DefaultFileSystem;
}
```

`Paths.FileSystem` referencia `LocalFileSystem.Instance`, un singleton que trabaja con el disco local mediante las capacidades estándar de `System.IO`.

## Métodos principales de IFileSystem

La interfaz `IFileSystem` incluye métodos para operaciones típicas:

| Método | Descripción |
|--------|-------------|
| `FileExists(path)` | Comprobar si existe un archivo |
| `DirectoryExists(path)` | Comprobar si existe un directorio |
| `CreateDirectory(path)` | Crear un directorio |
| `OpenRead(path)` | Abrir un archivo para lectura (`Stream`) |
| `OpenWrite(path)` | Abrir un archivo para escritura (`Stream`) |
| `MoveFile(src, dst)` | Mover un archivo |
| `DeleteFile(path)` | Eliminar un archivo |
| `EnumerateFiles(path, mask)` | Enumerar archivos en un directorio |
| `WriteAllTextAsync(path, text)` | Escribir texto en un archivo de forma asíncrona |

## Dónde se usa

Prácticamente todos los componentes de StockSharp que trabajan con el sistema de archivos aceptan `IFileSystem` en su constructor. A continuación se muestran los casos más comunes.

### LocalMarketDataDrive

Almacenamiento local de datos de mercado en disco:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Forma recomendada -- pasar IFileSystem explícitamente
var drive = new LocalMarketDataDrive(Paths.FileSystem, @"C:\MarketData");

// Deprecated way (uses Paths.FileSystem internally)
// var drive = new LocalMarketDataDrive(@"C:\MarketData"); // [Obsolete]
```

### CsvEntityRegistry

Registro de entidades (exchanges, instrumentos, portfolios) en formato CSV:

```csharp
using StockSharp.Algo.Storages.Csv;
using StockSharp.Configuration;

var executor = new ChannelExecutor();
var registry = new CsvEntityRegistry(Paths.FileSystem, @"C:\Data", executor);
```

### SnapshotRegistry

Registro de snapshots de datos de mercado:

```csharp
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

var snapshots = new SnapshotRegistry(Paths.FileSystem, Paths.SnapshotsDir);
```

### Serialización y deserialización

Métodos de extensión en `Paths` para trabajar con configuraciones JSON:

```csharp
using StockSharp.Configuration;

var fs = Paths.FileSystem;

// Serializar objeto a un archivo
settings.Serialize(fs, @"C:\config.json");

// Deserializar objeto desde un archivo
var loaded = @"C:\config.json".Deserialize<SettingsStorage>(fs);

// Deserialización asíncrona
var data = await @"C:\data.json".DeserializeAsync<MyData>(fs, cancellationToken);

// Comprobar si existe un archivo de configuración
if (@"C:\config.json".IsConfigExists(fs))
{
    // ...
}
```

### CandlePatternFileStorage

Almacenamiento de patrones de velas:

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

var patternStorage = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);
```

### NativeIdStorage, SecurityMappingStorage y otros

La mayoría de los almacenamientos de mapeos e identificadores también aceptan `IFileSystem`:

```csharp
// NativeIdStorage
var nativeIdStorage = new NativeIdStorage(Paths.FileSystem, Paths.SecurityNativeIdDir, executor);

// SecurityMappingStorage
var mappingStorage = new SecurityMappingStorage(Paths.FileSystem, Paths.SecurityMappingDir, executor);

// ExtendedInfoStorage
var extInfoStorage = new ExtendedInfoStorage(Paths.FileSystem, Paths.SecurityExtendedInfo, executor);
```

## Patrón de uso típico

En aplicaciones StockSharp, se recomienda conservar una referencia a `IFileSystem` y pasarla a todos los componentes:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Configuration;

// Obtener sistema de archivos
var fs = Paths.FileSystem;

// Crear almacenamiento de datos
var drive = new LocalMarketDataDrive(fs, Paths.StorageDir);

// Crear conector y configurar almacenamiento
var connector = new Connector();
connector.Adapter.StorageSettings.Drive = drive;

// Cargar ajustes desde archivo
var configFile = Path.Combine(Paths.AppDataPath, "connector_config.json");

if (configFile.IsConfigExists(fs))
{
    var config = configFile.Deserialize<SettingsStorage>(fs);
    connector.Load(config);
}
```

## Sobrecargas obsoletas

Muchas clases conservan constructores sin `IFileSystem` por compatibilidad hacia atrás, pero están marcados con el atributo `[Obsolete]`. Estos constructores usan `Paths.FileSystem` internamente:

```csharp
// Forma obsoleta
[Obsolete("Use IFileSystem overload.")]
public LocalMarketDataDrive(string path)
    : this(Paths.FileSystem, path) { }

// Forma recomendada
public LocalMarketDataDrive(IFileSystem fileSystem, string path) { }
```

Se recomienda usar siempre las sobrecargas con paso explícito de `IFileSystem`, ya que los constructores obsoletos se eliminarán en versiones futuras.
