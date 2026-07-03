# Medición de slippage

[S#](../api.md) calcula el slippage mediante [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). El slippage es la diferencia entre el precio esperado de ejecución de una orden y el precio real del trade.

## Interfaz ISlippageManager

La interfaz [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) define el contrato base:

- **Slippage** — slippage acumulado total (decimal).
- **Reset()** — restablece el estado del administrador.
- **ProcessMessage(Message)** — procesa un mensaje; devuelve el slippage de la ejecución dada o `null`.

## Cómo funciona

El administrador de slippage funciona en tres etapas:

### 1. Actualización de precios de mercado

Cuando se recibe un [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) o [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), el administrador guarda los mejores precios bid y ask para cada instrumento.

### 2. Guardado del precio planificado

Cuando se recibe un [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage), el administrador guarda el "precio planificado": el mejor precio de mercado en el momento de registrar la orden:

- Para una compra (`Buy`), se usa el mejor ask.
- Para una venta (`Sell`), se usa el mejor bid.

### 3. Cálculo de slippage

Cuando se recibe un [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con un trade, el administrador calcula el slippage:

- Para una compra: `(TradePrice - PlannedPrice) * TradeVolume`
- Para una venta: `(PlannedPrice - TradePrice) * TradeVolume`

Un valor positivo significa slippage desfavorable (precio peor de lo esperado), mientras que un valor negativo significa slippage favorable (precio mejor de lo esperado).

## Estado: ISlippageManagerState

La interfaz [ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) almacena el estado interno del administrador:

- Mejores precios bid/ask para cada instrumento ([SecurityId](xref:StockSharp.Messages.SecurityId)).
- Precios planificados y direcciones para cada transacción (`TransactionId`).
- Slippage acumulado total.

La implementación predeterminada es [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState).

## Ajustes

| Propiedad | Predeterminado | Descripción |
|----------|:-------:|-------------|
| `CalculateNegative` | `true` | Tener en cuenta el slippage favorable. Si es `false`, los valores negativos se sustituyen por cero. |

## Integración mediante adaptador

La clase [SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) envuelve un adaptador interno y calcula automáticamente el slippage de todos los trades.

## Integración con Strategy

La estrategia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) expone la propiedad `Slippage` para realizar seguimiento del slippage general.

## Ejemplo de uso

```cs
// Creación de un administrador con un almacén de estado
var manager = new SlippageManager(new SlippageManagerState());

// Tener en cuenta solo slippage desfavorable
manager.CalculateNegative = false;

// Procesamiento de datos de mercado (actualizar mejores precios)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Procesamiento de registro de orden (guardar el precio planificado)
manager.ProcessMessage(orderRegisterMsg);

// Procesamiento de un trade (cálculo de slippage)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Slippage: {slippage.Value}");
}

// Slippage acumulado total
Console.WriteLine($"Total slippage: {manager.Slippage}");
```

## Restablecimiento del estado

El método `Reset()` borra por completo el estado interno: mejores precios, precios planificados y slippage acumulado:

```cs
manager.Reset();
```
