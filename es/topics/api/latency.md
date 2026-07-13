# Medición de latencia

[S#](../api.md) mide la latencia de registro y cancelación de órdenes mediante [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager). El administrador determina cuánto tiempo pasa entre el envío de una orden y la recepción de la confirmación de la bolsa.

## Interfaz ILatencyManager

La interfaz [ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) define el contrato base:

- **LatencyRegistration** — latencia total de registro en todas las órdenes (TimeSpan).
- **LatencyCancellation** — latencia total de cancelación en todas las órdenes (TimeSpan).
- **Reset()** — restablece el estado del administrador.
- **ProcessMessage(Message)** — procesa un mensaje; devuelve la latencia de la operación dada o `null`.

## Cómo funciona

El administrador de latencia funciona según el principio "solicitud-respuesta":

### 1. Registro de orden

Cuando se recibe un [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage), el administrador guarda el par (`TransactionId`, `LocalTime`): el momento en que se envió la orden.

### 2. Cancelación de orden

Cuando se recibe un [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage), el administrador guarda el par (`TransactionId`, `LocalTime`): el momento en que se envió la cancelación.

### 3. Reemplazo de orden

Cuando se recibe un [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage), el administrador registra simultáneamente una cancelación (de la orden antigua) y un registro (de la nueva orden).

### 4. Confirmación

Cuando se recibe un [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con información de orden (no en estado `Pending` y no `Failed`), el administrador calcula la latencia:

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

El resultado se agrega a `LatencyRegistration` o `LatencyCancellation` según el tipo de operación.

## Estado: ILatencyManagerState

La interfaz [ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) almacena el estado interno del administrador:

- Registros pendientes: `AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- Cancelaciones pendientes: `AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- Latencias acumuladas: `LatencyRegistration`, `LatencyCancellation`
- Métodos de adición: `AddLatencyRegistration(TimeSpan)`, `AddLatencyCancellation(TimeSpan)`

La implementación predeterminada es [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState).

## Manejo de errores

Si una orden falla (`OrderState == Failed`), la latencia no se contabiliza: el registro simplemente se elimina del almacén de estado.

## Integración mediante adaptador

La clase [LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) envuelve un adaptador interno y mide automáticamente la latencia de todas las operaciones con órdenes.

## Integración con Strategy

La estrategia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expone la propiedad `Latency` para realizar seguimiento de la latencia.

## Ejemplo de uso

```cs
// Creación de un administrador con un almacén de estado
var manager = new LatencyManager(new LatencyManagerState());

// Procesamiento del registro de orden (guardar la hora de envío)
manager.ProcessMessage(orderRegisterMsg);

// Procesamiento de la confirmación (cálculo de latencia)
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Latencia: {latency.Value.TotalMilliseconds} ms");
}

// Latencias totales
Console.WriteLine($"Latencia de registro: {manager.LatencyRegistration.TotalMilliseconds} ms");
Console.WriteLine($"Latencia de cancelación: {manager.LatencyCancellation.TotalMilliseconds} ms");
```

## Restablecimiento del estado

El método `Reset()` borra todos los registros pendientes y restablece las latencias acumuladas a cero:

```cs
manager.Reset();
```
