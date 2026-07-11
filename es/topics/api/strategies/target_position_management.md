# Gestión de posición objetivo

## Descripción general

El sistema de gestión de posición objetivo permite que una estrategia especifique declarativamente el tamaño de posición deseado, mientras la plataforma coloca automáticamente las órdenes necesarias para alcanzar ese nivel. En lugar de calcular manualmente el volumen y la dirección de una operación, simplemente llama a `SetTargetPosition(10)` -- y el gestor determinará si debe comprar o vender, y en qué volumen.

El componente clave es la clase `PositionTargetManager`, que automáticamente:

- Calcula la diferencia entre la posición actual y la posición objetivo
- Determina la dirección y el volumen de la orden
- Maneja la ejecución, cancelación y errores de órdenes
- Admite reintentos ante fallos

## Métodos de estrategia

### SetTargetPosition

Establece la posición objetivo. Hay dos variantes de llamada disponibles:

```csharp
// Para el instrumento y la cartera principales de la estrategia
SetTargetPosition(decimal target);

// Para un instrumento y una cartera arbitrarios
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

Cuando `target` es mayor que la posición actual, el gestor colocará una orden de compra. Cuando es menor, una orden de venta. Si la posición ya es igual al objetivo (teniendo en cuenta `PositionTolerance`), no se realiza ninguna acción.

### CancelTargetPosition

Cancela una posición objetivo establecida previamente y detiene todas las órdenes activas relacionadas:

```csharp
// Para el instrumento y la cartera principales de la estrategia
CancelTargetPosition();

// Para un instrumento y una cartera arbitrarios
CancelTargetPosition(Security security, Portfolio portfolio);
```

### GetTargetPosition

Devuelve el valor actual de la posición objetivo, o `null` si no hay ningún objetivo establecido:

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## Propiedad TargetPositionManager

La propiedad `TargetPositionManager` proporciona acceso directo al objeto `PositionTargetManager` para ajustes finos:

```csharp
// Número máximo de reintentos ante error de orden (el valor predeterminado es 3)
TargetPositionManager.MaxRetries = 5;

// Tolerancia para determinar si se alcanzó la posición objetivo
TargetPositionManager.PositionTolerance = 0.01m;

// Tipo de orden (el valor predeterminado es Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

El gestor genera los siguientes eventos:

- `TargetReached` -- se alcanzó la posición objetivo
- `Error` -- ocurrió un error durante la ejecución de la orden
- `OrderRegistered` -- el gestor ha registrado una orden

## Propiedad TargetAlgoFactory

La propiedad `TargetAlgoFactory` permite establecer una fábrica para algoritmos de cambio de posición. De forma predeterminada se usa `MarketOrderAlgo`, que crea órdenes de mercado:

```csharp
// Usar un algoritmo personalizado en lugar de órdenes de mercado
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## Ejemplo de uso

```csharp
public class TargetPositionStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TargetPositionStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configurar el gestor de posición objetivo
        TargetPositionManager.MaxRetries = 5;
        TargetPositionManager.TargetReached += (sec, pf) =>
        {
            this.AddInfoLog("Posición objetivo alcanzada: {0}, {1}", sec, pf);
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

        if (candle.OpenPrice < candle.ClosePrice)
        {
            // Vela alcista -- establecer posición objetivo para comprar
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // Vela bajista -- establecer posición objetivo para vender
            SetTargetPosition(-Volume);
        }
    }
}
```

En este ejemplo, la estrategia no se ocupa de cálculos manuales de volumen y dirección. Simplemente declara el tamaño de posición deseado, y `PositionTargetManager` se encarga de todo el trabajo de colocación de órdenes.
