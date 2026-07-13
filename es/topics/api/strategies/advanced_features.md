# Funciones avanzadas de estrategia

## Descripción general

La clase `Strategy` proporciona varias propiedades adicionales para ajustar el comportamiento: comentario automático de órdenes, horario de negociación, tasa libre de riesgo para estadísticas, fuente de datos para indicadores y gestión del periodo histórico.

## CommentMode -- comentarios de órdenes

La propiedad `CommentMode` controla el rellenado automático del campo `Order.Comment` para todas las órdenes enviadas por la estrategia. Esto permite identificar qué estrategia creó una orden, lo que resulta especialmente útil al ejecutar varias estrategias simultáneamente en la misma cuenta.

### Enumeración StrategyCommentModes

| Valor | Descripción |
|-------|-------------|
| `Disabled` | El comentario no se rellena automáticamente. Valor predeterminado. |
| `Id` | El comentario se establece en `Strategy.Id` (identificador GUID único). |
| `Name` | El comentario se establece en `Strategy.Name` (nombre de la estrategia). |

### Ejemplo

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Todas las órdenes se marcarán con el nombre de la estrategia
        CommentMode = StrategyCommentModes.Name;

        // O con el identificador para una vinculación exacta
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

Con el valor `Name` y una estrategia llamada "Cruce de SMA", cada orden recibirá el comentario "Cruce de SMA", lo que permite filtrar las órdenes de esta estrategia en el diario de operaciones.

## WorkingTime -- horario de trabajo

La propiedad `WorkingTime` establece el horario durante el cual la estrategia está activa. Fuera de los intervalos de tiempo especificados, la estrategia puede restringir automáticamente su actividad.

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configurar horario de trabajo
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // Operar de 10:00 a 18:00
                        new Range<TimeSpan>(
                            TimeSpan.FromHours(10),
                            TimeSpan.FromHours(18))
                    }
                }
            }
        };
    }
}
```

La propiedad `TotalWorkingTime` (solo lectura) muestra el tiempo total de trabajo de la estrategia desde su inicio. Se calcula automáticamente al detener y reiniciar la estrategia.

## RiskFreeRate -- tasa libre de riesgo

La propiedad `RiskFreeRate` establece la tasa anual libre de riesgo usada en cálculos estadísticos, principalmente el ratio Sharpe y el ratio Sortino.

```csharp
var strategy = new MyStrategy();

// Tasa libre de riesgo del 5% anual
strategy.RiskFreeRate = 0.05m;
```

El valor se pasa automáticamente a todos los parámetros estadísticos que implementan `IRiskFreeRateStatisticParameter` cuando se inicializa el gestor de estadísticas de la estrategia.

## IndicatorSource -- fuente de datos para indicadores

La propiedad `IndicatorSource` establece el valor predeterminado de la propiedad `IIndicator.Source` para todos los indicadores de la estrategia que no tienen una fuente especificada explícitamente. Define qué campo de `Level1Fields` se usará como datos de entrada del indicador.

```csharp
var strategy = new MyStrategy();

// Todos los indicadores usarán por defecto el precio de la última operación
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// O el precio medio
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

Si la propiedad es `null` (valor predeterminado), los indicadores usan su propia fuente de datos.

## HistoryCalculated -- periodo histórico calculado

La propiedad virtual `HistoryCalculated` permite que una estrategia determine programáticamente el periodo de datos históricos requerido para el warm-up de indicadores. Devuelve `TimeSpan?`: la duración del periodo histórico, o `null` si no se especifica ningún periodo.

```csharp
public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _longPeriod;

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public SmaCrossStrategy()
    {
        _longPeriod = Param(nameof(LongPeriod), 50);
    }

    // Cálculo automático del periodo histórico requerido
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` es la versión calculada por código de la propiedad `HistorySize`. La diferencia es que `HistorySize` la establece el usuario como parámetro de estrategia, mientras que `HistoryCalculated` se calcula programáticamente en función de parámetros de la estrategia (por ejemplo, periodos de indicadores).

## Ejemplo: estrategia con todos los ajustes avanzados

```csharp
public class AdvancedStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<int> _smaPeriod;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public int SmaPeriod
    {
        get => _smaPeriod.Value;
        set => _smaPeriod.Value = value;
    }

    public AdvancedStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _smaPeriod = Param(nameof(SmaPeriod), 20);
    }

    // Cálculo automático del periodo histórico
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Comentarios de órdenes -- nombre de la estrategia
        CommentMode = StrategyCommentModes.Name;

        // Tasa libre de riesgo para el cálculo de Sharpe
        RiskFreeRate = 0.05m;

        // Fuente de datos para indicadores
        IndicatorSource = Level1Fields.LastTradePrice;

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Lógica de negociación...
    }
}
```

En este ejemplo, la estrategia usa todas las funciones descritas: comenta órdenes automáticamente, establece la tasa libre de riesgo para estadísticas, define la fuente de datos para indicadores y calcula el periodo histórico requerido.
