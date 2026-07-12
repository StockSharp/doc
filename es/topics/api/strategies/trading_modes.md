# Modos de trading de estrategias

## Descripción general

La propiedad `TradingMode` permite restringir la actividad de trading de una estrategia sin detenerla por completo. Esto es útil para la gestión de riesgos: por ejemplo, prohibir la apertura de nuevas posiciones permitiendo solo cerrar las existentes, o bloquear por completo el envío de órdenes.

El modo se establece mediante la enumeración `StrategyTradingModes` y se puede cambiar mientras la estrategia se ejecuta.

## Enumeración StrategyTradingModes

| Valor | Descripción |
|-------|-------------|
| `Full` | Acceso completo a trading. Sin restricciones sobre órdenes. Valor predeterminado. |
| `Disabled` | El trading está completamente prohibido. Todos los intentos de colocación de órdenes serán rechazados. |
| `CancelOrdersOnly` | Solo se permite cancelar órdenes. Se prohíben nuevas órdenes y modificaciones de órdenes existentes. |
| `ReducePositionOnly` | Solo se permiten órdenes que reducen la posición actual. Se prohíbe abrir nuevas posiciones y aumentar las existentes. |
| `LongOnly` | Solo se permiten posiciones largas. La venta solo se permite para cerrar una posición larga existente (el volumen de venta no puede superar la posición actual). Se prohíbe abrir posiciones cortas. |

## Establecer el modo

```csharp
// Al crear la estrategia
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Cambio dinámico durante la operación
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## Lógica de comprobación del modo

Al intentar registrar una orden, la estrategia comprueba el modo actual:

- **`Disabled`** -- la orden se rechaza con el motivo "negociación prohibida".
- **`ReducePositionOnly`** -- la orden se rechaza si la posición actual es cero, si la dirección de la orden coincide con la dirección de la posición o si el volumen de la orden supera el valor absoluto de la posición.
- **`LongOnly`** -- una orden de venta se rechaza si la posición actual no es positiva o si el volumen de venta supera la posición actual.
- **`Full`** -- sin restricciones.
- **`CancelOrdersOnly`** -- solo se permite cancelar órdenes.

## Método IsFormedAndOnlineAndAllowTrading

El método de extensión `IsFormedAndOnlineAndAllowTrading` comprueba que la estrategia esté formada (`IsFormed`), esté en estado online (`IsOnline`) y que el modo de trading permita la acción requerida:

```csharp
// Comprobar permiso para trading completo (predeterminado)
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// Comprobar permiso solo para cancelación de órdenes
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// Comprobar permiso para reducción de posición
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

Lógica de permisos al llamar con un parámetro `required`:

| TradingMode actual \ required | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|-------------------------------|--------|---------------------|---------------------|
| `Full` | sí | sí | sí |
| `Disabled` | no | no | no |
| `CancelOrdersOnly` | no | sí | no |
| `ReducePositionOnly` | no | sí | sí |
| `LongOnly` | no | sí | sí |

## Ejemplo de uso

```csharp
public class TradingModeStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TradingModeStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        // Comprobar que la estrategia esté lista para trading completo
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.ClosePrice > candle.OpenPrice)
        {
            BuyMarket(Volume);
        }
        else if (candle.ClosePrice < candle.OpenPrice)
        {
            SellMarket(Volume);
        }
    }
}

// Iniciar la estrategia con una restricción -- solo posiciones largas
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// Más tarde -- cambiar al modo de cierre de posiciones
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Bloqueo completo del trading
strategy.TradingMode = StrategyTradingModes.Disabled;
```

En este ejemplo, la estrategia opera inicialmente en modo `LongOnly`, que permite solo compras y cierre de posiciones largas. Cuando cambian las condiciones de mercado, el modo puede cambiarse a `ReducePositionOnly` para un cierre gradual de posiciones, y luego a `Disabled` para detener por completo la actividad de trading.
