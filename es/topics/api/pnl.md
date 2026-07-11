# Gestión de pérdidas y ganancias

[S#](../api.md) implementa el cálculo de pérdidas y ganancias (PnL) mediante [PnLManager](xref:StockSharp.Algo.PnL.PnLManager). El administrador procesa un flujo de mensajes (trades, datos de mercado) y calcula el beneficio realizado y no realizado.

## Interfaz IPnLManager

La interfaz [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) define el contrato base:

- **RealizedPnL** — beneficio/pérdida realizado (decimal). Se acumula cuando se cierran posiciones.
- **UnrealizedPnL** — beneficio/pérdida no realizado (decimal). Se recalcula basándose en precios de mercado actuales.
- **Reset()** — restablece el estado del administrador.
- **UpdateSecurity(Level1ChangeMessage)** — actualiza parámetros del instrumento (paso de precio, precio del paso, multiplicador de lote).
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** — procesa un mensaje; devuelve [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) cuando se cierra una posición, de lo contrario `null`.

## Arquitectura

El sistema PnL tiene una jerarquía de tres niveles:

```
PnLManager
  └── PortfolioPnLManager (por nombre de cartera)
        └── PnLQueue (por SecurityId)
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) — nivel superior, gestiona un diccionario de administradores de carteras.
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) — administrador PnL para una cartera específica, gestiona colas por instrumento.
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) — cola FIFO para emparejar trades en un único instrumento.

### PnLQueue — Cola de cálculo

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) es responsable de emparejar trades de apertura y cierre:

- **PriceStep** — paso de precio del instrumento.
- **StepPrice** — coste del paso de precio (para futuros).
- **Leverage** — apalancamiento.
- **LotMultiplier** — multiplicador de lote.

El multiplicador de beneficio se calcula usando la fórmula:

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

Para acciones normales (donde `StepPrice` no está establecido), el multiplicador es igual a `1 * Leverage * LotMultiplier`.

## PnLInfo — Resultado del procesamiento de trades

La clase [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) contiene el resultado de cerrar una posición:

- **ServerTime** — hora del trade.
- **ClosedVolume** — volumen de la posición cerrada.
- **PnL** — beneficio realizado de este trade.

Por ejemplo, si la posición era +2 y llegó un trade de -5 contratos, entonces `ClosedVolume = 2` (se cerraron 2 contratos de la posición).

## Configuración de fuentes de datos

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) permite seleccionar fuentes de datos de mercado para el cálculo de beneficio no realizado:

| Propiedad | Predeterminado | Descripción |
|----------|:-------:|-------------|
| `UseTick` | `true` | Usar tick trades. |
| `UseOrderBook` | `false` | Usar libro de órdenes (mejor bid/ask). |
| `UseLevel1` | `false` | Usar datos Level1. |
| `UseOrderLog` | `false` | Usar order log. |
| `UseCandles` | `true` | Usar velas (precio de cierre). |

## Integración mediante adaptador

La clase [PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) envuelve un adaptador interno y procesa automáticamente todos los mensajes para el cálculo de PnL.

## Integración con Strategy

La estrategia ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) proporciona:

- Propiedad `PnLManager` — la instancia del administrador.
- Propiedad `PnL` — beneficio total (`RealizedPnL + UnrealizedPnL`).
- Evento `PnLChanged` — notificación de cambios de beneficio.
- Evento `PnLReceived2` — notificación cuando se reciben nuevos datos PnL.

## Ejemplo de uso

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// Procesamiento de mensajes
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"Cerrado: {info.ClosedVolume}, PnL: {info.PnL}");
}

// Beneficio/pérdida total
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Realized PnL: {realizedPnL}");
Console.WriteLine($"Unrealized PnL: {unrealizedPnL}");
Console.WriteLine($"Total PnL: {totalPnL}");
```

## Restablecimiento del estado

El método `Reset()` borra todos los administradores de carteras, colas de cálculo y restablece el PnL realizado a cero:

```cs
pnlManager.Reset();
```
