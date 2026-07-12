# Sistema de snapshots

Los snapshots en StockSharp representan un mecanismo para guardar el último estado real de los datos de mercado. En lugar de escanear todo el histórico, los snapshots permiten obtener instantáneamente el valor Level1 actual, el libro de órdenes, la posición o la transacción.

## Propósito de los snapshots

Al trabajar con datos en flujo continuo, a menudo es necesario conocer el último estado de un instrumento: precio actual, libro de órdenes, posición abierta. Sin snapshots, obtener esta información requeriría cargar y procesar todo el histórico. El sistema de snapshots resuelve este problema guardando el último estado de cada objeto y proporcionando acceso a él en tiempo mínimo.

## ISnapshotStorage -- Interfaz de almacenamiento de snapshots

La interfaz [ISnapshotStorage](xref:StockSharp.Algo.Storages.ISnapshotStorage) define el contrato base para trabajar con snapshots. La versión tipada `ISnapshotStorage<TKey, TMessage>` proporciona los siguientes métodos:

- **Update(message)** -- guardar o actualizar un snapshot. Si ya existe un snapshot para la clave dada, se actualizará.
- **Get(key)** -- obtener un snapshot por clave (por ejemplo, por identificador de instrumento).
- **GetAll(from, to)** -- obtener todos los snapshots para el rango de fechas especificado.
- **Clear(key)** -- eliminar el snapshot de una clave específica.
- **ClearAll()** -- eliminar todos los snapshots.

## ISnapshotSerializer -- Serialización de snapshots

La interfaz [ISnapshotSerializer](xref:StockSharp.Algo.Storages.ISnapshotSerializer`2) es responsable de convertir snapshots a representación binaria y viceversa:

- **DataType** -- información del tipo de datos del snapshot.
- **Version** -- versión del formato de serialización.
- **Serialize(version, message)** -- serializar un mensaje a un array de bytes.
- **Deserialize(version, buffer)** -- deserializar un array de bytes de vuelta a un mensaje.
- **GetKey(message)** -- extraer la clave de un mensaje.
- **Update(message, changes)** -- aplicar cambios incrementales a un snapshot existente.

## SnapshotRegistry -- Registro de snapshots

La clase [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) es el componente central para la gestión de snapshots. Implementa la interfaz `ISnapshotRegistry` y coordina el funcionamiento de todos los almacenamientos de snapshots.

### Características clave

- **Gestión de almacenamiento** -- proporciona acceso a almacenamientos de snapshots para varios tipos de datos.
- **Escritura periódica** -- los cambios se vuelcan a disco cada 10 segundos, lo que garantiza un equilibrio entre rendimiento y fiabilidad.
- **Seguridad de hilos** -- todas las operaciones son seguras para su uso desde varios hilos.

### Organización de archivos

Los archivos de snapshots se almacenan en la siguiente ruta:

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
| [TransactionBinarySnapshotSerializer](xref:StockSharp.Algo.Storages.Binary.Snapshot.TransactionBinarySnapshotSerializer) | `ExecutionMessage` | Transacciones (órdenes y trades) |

Cada serializador admite versionado de formato, lo que garantiza compatibilidad hacia atrás al actualizar StockSharp.

## Ejemplo de código

### Creación de un registro de snapshots

```cs
var snapshotRegistry = new SnapshotRegistry(Path.Combine(
    Directory.GetCurrentDirectory(), "Snapshots"));
```

### Trabajo con snapshots Level1

```cs
// Obtener el almacenamiento de snapshots Level1
var level1Snapshots = snapshotRegistry.GetSnapshotStorage(
    DataType.Level1);

// Guardar un snapshot
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

### Recuperación de un snapshot

```cs
// Obtener el último snapshot de un instrumento
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
