# Sistema de instantáneas

Las instantáneas en StockSharp representan un mecanismo para guardar el último estado real de los datos de mercado. En lugar de escanear todo el histórico, las instantáneas permiten obtener instantáneamente el valor Level1 actual, el libro de órdenes, la posición o la transacción.

## Propósito de las instantáneas

Al trabajar con datos en flujo continuo, a menudo es necesario conocer el último estado de un instrumento: precio actual, libro de órdenes, posición abierta. Sin instantáneas, obtener esta información requeriría cargar y procesar todo el histórico. El sistema de instantáneas resuelve este problema guardando el último estado de cada objeto y proporcionando acceso a él en tiempo mínimo.

## ISnapshotStorage -- Interfaz de almacenamiento de instantáneas

La interfaz [ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) define el contrato base para trabajar con instantáneas. La versión tipada `ISnapshotStorage<TKey, TMessage>` proporciona los siguientes métodos:

- **Update(message)** -- guardar o actualizar una instantánea. Si ya existe una instantánea para la clave dada, se actualizará.
- **Get(key)** -- obtener una instantánea por clave (por ejemplo, por identificador de instrumento).
- **GetAll(from, to)** -- obtener todas las instantáneas para el rango de fechas especificado.
- **Clear(key)** -- eliminar la instantánea de una clave específica.
- **ClearAll()** -- eliminar todas las instantáneas.

## ISnapshotSerializer -- Serialización de instantáneas

La interfaz [ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) es responsable de convertir instantáneas a una representación binaria y viceversa:

- `DataType` -- información del tipo de datos de la instantánea.
- **Version** -- versión del formato de serialización.
- **Serialize(version, message)** -- serializar un mensaje a un array de bytes.
- **Deserialize(version, buffer)** -- deserializar un array de bytes de vuelta a un mensaje.
- **GetKey(message)** -- extraer la clave de un mensaje.
- **Update(message, changes)** -- aplicar cambios incrementales a una instantánea existente.

## SnapshotRegistry -- Registro de instantáneas

La clase [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) es el componente central para la gestión de instantáneas. Implementa la interfaz `ISnapshotRegistry` y coordina el funcionamiento de todos los almacenamientos de instantáneas.

### Características clave

- **Gestión de almacenamiento** -- proporciona acceso a almacenamientos de instantáneas para varios tipos de datos.
- **Escritura periódica** -- los cambios se vuelcan a disco cada 10 segundos, lo que garantiza un equilibrio entre rendimiento y fiabilidad.
- **Seguridad de hilos** -- todas las operaciones son seguras para su uso desde varios hilos.

### Organización de archivos

Los archivos de instantáneas se almacenan en la siguiente ruta:

```
{path}/{yyyy_MM_dd}/{serializer_name}.bin
```

## Serializadores integrados

StockSharp incluye cuatro serializadores para los principales tipos de datos de mercado:

| Serializador | Tipo de mensaje | Propósito |
|------------|-------------|---------|
| [Level1BinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.Level1BinarySnapshotSerializer) | `Level1ChangeMessage` | Datos Level1 (precios, volúmenes, spreads) |
| [QuotesBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.QuotesBinarySnapshotSerializer) | `QuoteChangeMessage` | Libro de órdenes |
| [PositionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.PositionBinarySnapshotSerializer) | `PositionChangeMessage` | Posiciones |
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | Transacciones (órdenes y operaciones) |

Cada serializador admite versionado de formato, lo que garantiza compatibilidad hacia atrás al actualizar StockSharp.

## Ejemplo de código

### Creación de un registro de instantáneas

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Trabajo con instantáneas Level1

```cs
// Obtener el almacenamiento de instantáneas Level1
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Guardar una instantánea
var level1Msg = new Level1ChangeMessage
{
    SecurityId = "AAPL@NASDAQ".ToSecurityId(),
    ServerTime = DateTimeOffset.Now,
};
level1Msg.TryAdd(Level1Fields.LastTradePrice, 260.5m);
level1Msg.TryAdd(Level1Fields.BestBidPrice, 260.4m);
level1Msg.TryAdd(Level1Fields.BestAskPrice, 260.6m);

level1Snapshots.Update(level1Msg);
```

### Recuperación de una instantánea

```cs
// Obtener la última instantánea de un instrumento
var secId = "AAPL@NASDAQ".ToSecurityId();
var snapshot = level1Snapshots.Get(secId);

if (snapshot != null)
{
    Console.WriteLine($"Último precio: {snapshot.Changes[Level1Fields.LastTradePrice]}");
}
```

## Véase también

- [Trabajo con la API](api.md)
- [Formatos de almacenamiento](formats.md)
- [Unidades de almacenamiento](drives.md)
