# Gestión de riesgos

## Descripción general

Cada estrategia en StockSharp tiene un `RiskManager` integrado que permite el control automático de riesgos de trading. Cuando se activa una regla, el gestor puede cerrar posiciones, cancelar órdenes activas o detener por completo el trading de la estrategia.

El gestor de riesgos procesa todos los mensajes de trading que pasan por la estrategia y, cuando se cumplen las condiciones de una regla, realiza automáticamente la acción especificada sin código adicional en la lógica de la estrategia.

## Propiedades de estrategia

### RiskManager

El objeto `IRiskManager` que gestiona la lista de reglas. Se crea automáticamente cuando se inicializa la estrategia:

```csharp
// Acceder al gestor de riesgos
IRiskManager manager = strategy.RiskManager;
```

### RiskRules

Una propiedad cómoda para leer y establecer la lista de reglas:

```csharp
// Leer reglas actuales
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// Establecer nuevas reglas
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## Acciones de activación

La enumeración `RiskActions` define las acciones posibles:

| Acción | Descripción |
|--------|-------------|
| `ClosePositions` | Cerrar todas las posiciones abiertas con una orden de mercado |
| `StopTrading` | Bloquear el trading de la estrategia |
| `CancelOrders` | Cancelar todas las órdenes activas |

Cuando se activa una regla, la estrategia registra una advertencia con el nombre de la regla, sus parámetros y la acción realizada.

## Reglas disponibles

### RiskPnLRule -- control de beneficios/pérdidas

Se activa cuando se alcanza el nivel de P&L especificado. Un valor positivo controla beneficios y uno negativo controla pérdidas:

```csharp
// Detener el trading ante una pérdida superior a 5000
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### RiskPositionSizeRule -- control del tamaño de posición

Se activa cuando se alcanza el tamaño de posición especificado:

```csharp
// Cancelar órdenes cuando position >= 100
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- control de frecuencia de órdenes

Se activa cuando el número de órdenes dentro del intervalo especificado supera el límite:

```csharp
// Detener con más de 50 órdenes por minuto
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### RiskOrderVolumeRule -- control de volumen de órdenes

Se activa cuando se supera el volumen de una sola orden.

### RiskOrderPriceRule -- control de precio de órdenes

Se activa cuando el precio de la orden va más allá de los límites establecidos.

### RiskTradeVolumeRule -- control de volumen de operaciones

Se activa cuando se supera el volumen de una sola operación.

### RiskTradePriceRule -- control de precio de operaciones

Se activa cuando el precio de la operación va más allá de los límites establecidos.

### RiskTradeFreqRule -- control de frecuencia de operaciones

Se activa cuando se supera el número de operaciones dentro del intervalo.

### RiskPositionTimeRule -- control del tiempo de mantenimiento de posición

Se activa cuando una posición se ha mantenido durante más tiempo que el establecido.

### RiskCommissionRule -- control de comisión

Se activa cuando se supera la comisión total.

### RiskSlippageRule -- control de slippage

Se activa cuando se supera el nivel de slippage.

### RiskErrorRule -- control de errores

Se activa cuando se ha acumulado una determinada cantidad de errores.

### RiskOrderErrorRule -- control de errores de registro de órdenes

Se activa cuando se han acumulado errores de registro de órdenes.

## Ejemplo de uso

```csharp
public class RiskAwareStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public RiskAwareStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configurar reglas de gestión de riesgos
        RiskRules = new IRiskRule[]
        {
            // Cerrar posiciones ante una pérdida superior a 10000
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // Detener el trading cuando la posición supera 500 contratos
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // Cancelar órdenes cuando la frecuencia supera 100 por minuto
            new RiskOrderFreqRule
            {
                Count = 100,
                Interval = TimeSpan.FromMinutes(1),
                Action = RiskActions.CancelOrders
            },
        };

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Lógica de trading
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

En este ejemplo, las reglas de gestión de riesgos se establecen cuando se inicia la estrategia. Cuando se alcanza una pérdida de 10000, las posiciones se cerrarán automáticamente. Cuando el tamaño de posición supera 500 contratos, el trading se bloqueará. Cuando las órdenes se colocan con demasiada frecuencia, las órdenes activas se cancelarán. Todas estas comprobaciones se realizan automáticamente sin código adicional en el método `ProcessCandle`.
