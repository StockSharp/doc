# Unidades de almacenamiento

Las unidades de almacenamiento en StockSharp son responsables de la ubicación física de los datos de mercado: en un disco local o en un servidor remoto. La interfaz base [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) define el contrato común para todas las implementaciones.

## IMarketDataDrive -- Interfaz base

La interfaz [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) proporciona las siguientes capacidades clave:

- **Path** -- ruta al almacenamiento de datos.
- **GetAvailableSecuritiesAsync()** -- obtener una lista de todos los instrumentos disponibles en el almacenamiento.
- **GetAvailableDataTypesAsync()** -- obtener una lista de tipos de datos disponibles para un instrumento específico.
- **GetStorageDrive()** -- obtener una unidad de almacenamiento para un instrumento y tipo de datos específicos.
- **VerifyAsync()** -- verificar la integridad del almacenamiento.
- **LookupSecuritiesAsync()** -- buscar instrumentos por criterios especificados.

## LocalMarketDataDrive -- Almacenamiento local de archivos

La clase [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) es la implementación principal de unidad que almacena datos en el disco local en el sistema de archivos.

### Características clave

- **Sistema de archivos** -- los datos se organizan en una estructura jerárquica de directorios por instrumentos y fechas.
- **Sistema de índices** -- la clase interna `Index` proporciona acceso rápido a datos sin escanear el sistema de archivos. Los archivos de índice se almacenan en el formato `{instrument_path}{file_name}Dates2.bin`.
- **Seguridad de hilos** -- el acceso a datos está protegido por mecanismos de bloqueo para un funcionamiento correcto en aplicaciones multihilo.
- **Construcción de índices** -- el método `BuildIndexAsync()` permite reconstruir índices para mejorar el rendimiento después de operaciones masivas con datos.

### Ejemplo de uso

```cs
// Crear una unidad local con una ruta especificada
var localDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));

// Usar con el registro de almacenamiento
var storageRegistry = new StorageRegistry
{
    DefaultDrive = localDrive,
};

// Obtener la lista de instrumentos disponibles
await foreach (var secId in localDrive.GetAvailableSecuritiesAsync())
{
    Console.WriteLine(secId);
}
```

## RemoteMarketDataDrive -- Almacenamiento remoto

La clase [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) permite conectarse a un servidor Hydra remoto para acceder a datos de mercado por red.

### Ajustes de conexión

- **Address** -- dirección del servidor remoto. El valor predeterminado es `127.0.0.1:5002`.
- **Credenciales** -- credenciales de autenticación (correo electrónico y contraseña).
- **TargetCompId** -- identificador del componente de destino, por defecto `"StockSharpHydraMD"`.
- **SecurityBatchSize** -- tamaño de lote al cargar instrumentos, por defecto 1000.
- **Timeout** -- tiempo de espera de conexión, por defecto 2 minutos.

### Ejemplo de uso

```cs
// Crear una unidad remota
var remoteDrive = new RemoteMarketDataDrive
{
    Address = "192.168.1.100:5002".To<EndPoint>(),
    Credentials = { Email = "user", Password = "pass".Secure() }
};

// Obtener tipos de datos disponibles para un instrumento
var secId = "AAPL@NASDAQ".ToSecurityId();
await foreach (var dataType in remoteDrive.GetAvailableDataTypesAsync(secId, StorageFormats.Binary))
{
    Console.WriteLine(dataType);
}
```

Para más detalles sobre el trabajo con almacenamiento remoto, consulte la sección [Trabajo con almacenamiento remoto](remote.md).

## DriveCache -- Gestión de unidades

La clase [DriveCache](xref:StockSharp.Algo.Storages.DriveCache) gestiona una colección de unidades de almacenamiento y proporciona caché para reutilización.

### Métodos y propiedades clave

- **GetDrive(path)** -- obtener una unidad existente por ruta o crear una nueva.
- **DeleteDrive(drive)** -- eliminar una unidad de la caché.
- **TryDefaultDrive** -- el primer [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) disponible.
- **NewDriveCreated** -- evento de creación de nueva unidad.
- **DriveDeleted** -- evento de eliminación de unidad.
- **Changed** -- evento de cambios en la colección de unidades.

La clase implementa la interfaz `IPersistable`, lo que permite guardar y cargar configuraciones de unidades.

### Ejemplo de uso

```cs
// Crear una caché con una unidad local predeterminada
var defaultDrive = new LocalMarketDataDrive(Path.Combine(
    Directory.GetCurrentDirectory(), "Storage"));
var driveCache = new DriveCache(defaultDrive);

// Obtener o crear una unidad por ruta
var anotherDrive = driveCache.GetDrive(@"D:\MarketData");

// Suscribirse a eventos
driveCache.NewDriveCreated += drive =>
    Console.WriteLine($"Unidad creada: {drive.Path}");
```

## Véase también

- [Trabajo con la API](api.md)
- [Trabajo con almacenamiento remoto](remote.md)
- [Formatos de almacenamiento](formats.md)
