# Gestión de posiciones

StockSharp proporciona un sistema flexible de gestión de posiciones que permite realizar seguimiento del estado actual de las posiciones, calcularlas basándose en órdenes u operaciones, y mantener un historial de ciclo de vida (apertura, cierre, reversiones).

## PositionManager

La clase [PositionManager](xref:StockSharp.Algo.Positions.PositionManager) implementa la interfaz [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) y sirve como el componente principal para calcular posiciones actuales basándose en mensajes entrantes.

### Creación del administrador

El constructor acepta dos parámetros:

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` -- la posición se calcula basándose en cambios del saldo de órdenes. Es adecuado cuando el sistema de negociación recibe actualizaciones de estado de órdenes pero no operaciones individuales.
- `byOrders = false` -- la posición se calcula basándose en volúmenes de operaciones (modo recomendado). Proporciona una contabilidad más precisa de las operaciones ejecutadas.

### Procesamiento de mensajes

El método `ProcessMessage` acepta un mensaje entrante ([Message](xref:StockSharp.Messages.Message)) y devuelve un [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) cuando la posición cambia, o `null` si la posición no ha cambiado:

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"Posición: {posChange.CurrentValue}");
}
```

## IPositionManagerState

La interfaz [IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) describe el estado interno del administrador de posiciones. La implementación [PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) almacena información sobre órdenes y posiciones actuales.

### Métodos principales

| Método | Descripción |
|--------|-------------|
| `AddOrGetOrder` | Registra una nueva orden o devuelve una existente por `transactionId` |
| `TryGetOrder` | Recupera parámetros de orden (instrumento, cartera, dirección, saldo) |
| `UpdateOrderBalance` | Actualiza el saldo actual de la orden después de una ejecución parcial |
| `RemoveOrder` | Elimina una orden completada del seguimiento |
| `UpdatePosition` | Actualiza la posición por instrumento y cartera, devuelve el nuevo valor |
| `Clear` | Restablece todo el estado del administrador |

### Ejemplo de trabajo con estado

```cs
var state = new PositionManagerState();

// Registrar una orden
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// Actualizar después de ejecución parcial
state.UpdateOrderBalance(12345, newBalance: 60);

// Actualizar posición directamente
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"Posición actual: {newPosition}");

// Limpiar
state.Clear();
```

## PositionLifecycleTracker

La clase [PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) realiza seguimiento del ciclo de vida completo de las posiciones -- desde la apertura hasta el cierre (ciclo completo). Esto es útil para analizar operaciones individuales, calcular beneficio por cada posición y generar informes.

### Características clave

- **Historial**: la propiedad `History` (`IReadOnlyList<ReportPosition>`) contiene todos los ciclos completos de posición finalizados.
- **Evento `RoundTripClosed`**: se dispara cuando una posición se cierra (el valor alcanza cero) o se revierte (el signo de la posición cambia).
- **Método `ProcessPosition`**: acepta un objeto [Position](xref:StockSharp.BusinessEntities.Position) y actualiza el estado interno.

### Estados detectados

| Estado | Descripción |
|-------|-------------|
| Opening | La posición pasa de cero a un valor distinto de cero |
| Closing | El valor de la posición alcanza cero |
| Reversal | El signo de la posición cambia (por ejemplo, de largo a corto) |

### Ejemplo de uso

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"Operación de ida y vuelta completada:");
    Console.WriteLine($"  Abierta: {report.OpenTime}");
    Console.WriteLine($"  Cerrada: {report.CloseTime}");
};

// Procesar actualizaciones de posición
tracker.ProcessPosition(position);

// Ver historial
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## PositionMessageAdapter

La clase [PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) encapsula un adaptador de mensajes que calcula automáticamente posiciones a partir del flujo de mensajes. Se usa dentro de la infraestructura interna del conector.

### Cómo funciona

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

El adaptador intercepta mensajes de ejecución de órdenes y operaciones, llama a `PositionManager.ProcessMessage` y genera las instancias `PositionChangeMessage` correspondientes para los manejadores superiores.

## Posiciones en estrategias

En la clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy), se accede a la posición actual mediante la propiedad `Position`:

```cs
// Posición actual del instrumento principal
decimal currentPosition = Position;

// Cerrar posición
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// O mediante un método integrado
ClosePosition();
```

Para más detalles sobre operaciones de negociación en estrategias, consulte la sección [Operaciones de negociación](strategies/trading_operations.md).

## Véase también

- [Operaciones de negociación](strategies/trading_operations.md)
- [Protección de posiciones](strategies/take_profit_and_stop_loss.md)
- [Gestión de posición objetivo](strategies/target_position_management.md)
- [Informes](strategies/reporting.md)
