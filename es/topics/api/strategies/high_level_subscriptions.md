# Suscripciones de alto nivel

## Descripción general

La clase `Strategy` proporciona un conjunto de métodos de suscripción a datos de mercado de alto nivel: `SubscribeCandles`, `SubscribeTicks`, `SubscribeLevel1` y `SubscribeOrderBook`. Estos métodos devuelven un objeto `ISubscriptionHandler<T>` que permite vincular controladores de datos e indicadores con un estilo fluido y cómodo.

A diferencia de crear manualmente un objeto `Subscription` y llamar a `Subscribe()`, los métodos de alto nivel:

- Crean automáticamente una suscripción con los parámetros correctos
- Proporcionan un `ISubscriptionHandler<T>` tipado para vincular controladores
- Se integran con el sistema de indicadores mediante métodos `Bind`
- Admiten renderizado automático en gráficos
- Gestionan correctamente el ciclo de vida de la suscripción

## Métodos de suscripción

### SubscribeCandles

Se suscribe a velas. Acepta un timeframe o `DataType`:

```csharp
// Suscribirse por timeframe
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// Suscribirse por DataType (admite todos los tipos de velas)
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// Suscribirse con un objeto Subscription listo
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

El parámetro `isFinishedOnly` es `true` de forma predeterminada -- el controlador recibe solo velas completadas.

### SubscribeTicks

Se suscribe a operaciones tick:

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### SubscribeLevel1

Se suscribe a datos Level1 (mejores precios bid/ask, última operación y otros campos):

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### SubscribeOrderBook

Se suscribe al libro de órdenes:

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

Si no se especifica el parámetro `security`, se usa el `Security` de la estrategia.

## Interfaz ISubscriptionHandler

El objeto `ISubscriptionHandler<T>` proporciona los siguientes métodos:

### Start / Stop

Inicio y parada de la suscripción:

```csharp
handler.Start();   // llama a Subscribe
handler.Stop();    // llama a UnSubscribe
```

### Bind (sin indicadores)

Vincular un controlador de datos simple:

```csharp
handler.Bind(Action<T> callback);
```

### Bind (con indicadores)

Vincular un controlador con uno o más indicadores. El indicador procesa automáticamente los datos entrantes y el controlador recibe el valor ya calculado:

```csharp
// Un indicador -- valor decimal
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// Dos indicadores
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// Hasta ocho indicadores
handler.Bind(ind1, ind2, ind3, ..., callback);

// Array de indicadores
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

El controlador con `Bind` se llama solo cuando todos los indicadores han devuelto un valor no vacío.

### BindWithEmpty

Similar a `Bind`, pero el controlador se llama incluso si el indicador devolvió un valor vacío. Los valores se representan como `decimal?`:

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### BindEx

Proporciona acceso al objeto `IIndicatorValue` completo en lugar del `decimal` extraído:

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## Ejemplo: estrategia con indicadores

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // Vincular dos indicadores -- se llama al controlador
        // cuando ambos indicadores están formados
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // Configuración del gráfico
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## Ejemplo: suscripción a ticks

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("Tick: price={0}, volume={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## Ejemplo: suscripción al libro de órdenes

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Spread: {0}", spread);
            }
        })
        .Start();
}
```

## Diferencias frente a la creación manual de suscripciones

| Aspecto | Suscripción manual | Método de alto nivel |
|--------|-------------------|-------------------|
| Creación | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| Manejo de datos | Suscripción a eventos del conector | `Bind(callback)` |
| Indicadores | Llamada manual a `indicator.Process()` | Automáticamente mediante `Bind(indicator, callback)` |
| Registro de indicadores | Adición manual a `Indicators` | Automáticamente al usar `Bind` |
| Renderizado en gráfico | Integración manual con `IChart` | `DrawCandles`, `DrawIndicator` |

Los métodos de alto nivel se recomiendan para la mayoría de las estrategias, ya que reducen significativamente la cantidad de código y disminuyen la probabilidad de errores.
