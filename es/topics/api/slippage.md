# Medición de deslizamiento

[S#](../api.md) calcula el deslizamiento mediante [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). El deslizamiento es la diferencia entre el precio esperado de ejecución de una orden y el precio real de la operación.

## Interfaz ISlippageManager

La interfaz [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) define el contrato base:

- **Deslizamiento** — deslizamiento acumulado total (decimal).
- **Reset()** — restablece el estado del administrador.
- **ProcessMessage(Message)** — procesa un mensaje; devuelve el deslizamiento de la ejecución dada o `null`.

## Cómo funciona

El administrador de deslizamiento funciona en tres etapas:

### 1. Actualización de precios de mercado

Cuando se recibe un [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) o [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), el administrador guarda los mejores precios bid y ask para cada instrumento.

### 2. Guardado del precio planificado

Cuando se recibe un [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage), el administrador guarda el "precio planificado": el mejor precio de mercado en el momento de registrar la orden:

- Para una compra (`Buy`), se usa el mejor ask.
- Para una venta (`Sell`), se usa el mejor bid.

### 3. Cálculo de deslizamiento

Cuando se recibe un [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con una operación, el administrador calcula el deslizamiento:

- Para una compra: `(TradePrice - PlannedPrice) * TradeVolume`
- Para una venta: `(PlannedPrice - TradePrice) * TradeVolume`

Un valor positivo significa deslizamiento desfavorable (precio peor de lo esperado), mientras que un valor negativo significa deslizamiento favorable (precio mejor de lo esperado).

## Estado: ISlippageManagerState

La interfaz [ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) almacena el estado interno del administrador:

- Mejores precios bid/ask para cada instrumento ([SecurityId](xref:StockSharp.Messages.SecurityId)).
- Precios planificados y direcciones para cada transacción (`TransactionId`).
- Deslizamiento acumulado total.

La implementación predeterminada es [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState).

## Ajustes

| Propiedad | Predeterminado | Descripción |
|----------|:-------:|-------------|
| `CalculateNegative` | `true` | Tener en cuenta el deslizamiento favorable. Si es `false`, los valores negativos se sustituyen por cero. |

## Integración mediante adaptador

La clase [SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) envuelve un adaptador interno y calcula automáticamente el deslizamiento de todas las operaciones.

## Integración con Strategy

La estrategia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expone la propiedad `Slippage` para realizar seguimiento del deslizamiento general.

## Ejemplo de uso

```cs
// Creación de un administrador con un almacén de estado
var manager = new SlippageManager(new SlippageManagerState());

// Tener en cuenta solo deslizamiento desfavorable
manager.CalculateNegative = false;

// Procesamiento de datos de mercado (actualizar mejores precios)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Procesamiento de registro de orden (guardar el precio planificado)
manager.ProcessMessage(orderRegisterMsg);

// Procesamiento de una operación (cálculo de deslizamiento)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Deslizamiento calculado: {slippage.Value}");
}

// Deslizamiento acumulado total
Console.WriteLine($"Deslizamiento total: {manager.Slippage}");
```

## Restablecimiento del estado

El método `Reset()` borra por completo el estado interno: mejores precios, precios planificados y deslizamiento acumulado:

```cs
manager.Reset();
```
