# Optimización de estrategias

## Resumen

StockSharp proporciona un mecanismo integrado para optimizar parámetros de estrategias sobre datos históricos. La optimización recorre automáticamente distintas combinaciones de parámetros de la estrategia, como longitudes de indicadores, marcos temporales, valores umbral y otros ajustes, y ayuda a encontrar el mejor resultado para el criterio seleccionado, como beneficio, drawdown, número de operaciones, etc.

Hay dos modos de optimización disponibles:

- **Fuerza bruta** -- la clase `BruteForceOptimizer`. Recorre todas las combinaciones posibles de parámetros, o un subconjunto aleatorio de ellas.
- **Algoritmo genético** -- la clase `GeneticOptimizer`. Usa un enfoque evolutivo para encontrar parámetros óptimos, lo que es mucho más eficiente para espacios grandes de parámetros.

Ambos optimizadores heredan de `BaseOptimizer` y funcionan de forma asíncrona, devolviendo un `IAsyncEnumerable` con resultados a medida que se completa cada iteración.

## Preparar una estrategia

### Definir parámetros con rangos de optimización

Para optimizar una estrategia, defina sus parámetros mediante `StrategyParam<T>` y especifique rangos de valores. El método `SetOptimize(from, to, step)` establece el rango que se recorrerá, y `SetCanOptimize(true)` habilita la optimización para el parámetro:

```csharp
class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _longSma = Param(nameof(LongSma), 80)
            .SetCanOptimize(true)
            .SetOptimize(50, 100, 5);      // de 50 a 100 con paso de 5

        _shortSma = Param(nameof(ShortSma), 30)
            .SetCanOptimize(true)
            .SetOptimize(20, 40, 1);        // de 20 a 40 con paso de 1

        _candleTimeFrame = Param<TimeSpan?>(nameof(CandleTimeFrame))
            .SetCanOptimize(true)
            .SetOptimize(
                TimeSpan.FromMinutes(5),    // desde 5 minutos
                TimeSpan.FromMinutes(15),   // hasta 15 minutos
                TimeSpan.FromMinutes(5));   // con paso de 5 minutos

        _candleType = Param(nameof(CandleType),
            TimeSpan.FromMinutes(1).TimeFrame()).SetRequired();
    }

    private readonly StrategyParam<int> _longSma;
    public int LongSma
    {
        get => _longSma.Value;
        set => _longSma.Value = value;
    }

    private readonly StrategyParam<int> _shortSma;
    public int ShortSma
    {
        get => _shortSma.Value;
        set => _shortSma.Value = value;
    }

    private readonly StrategyParam<TimeSpan?> _candleTimeFrame;
    public TimeSpan? CandleTimeFrame
    {
        get => _candleTimeFrame.Value;
        set => _candleTimeFrame.Value = value;
    }

    private readonly StrategyParam<DataType> _candleType;
    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var dt = CandleTimeFrame is null
            ? CandleType
            : DataType.Create(CandleType.MessageType, CandleTimeFrame);

        var subscription = new Subscription(dt, Security)
        {
            MarketData =
            {
                IsFinishedOnly = true,
            }
        };

        var longSma = new SMA { Length = LongSma };
        var shortSma = new SMA { Length = ShortSma };

        SubscribeCandles(subscription)
            .Bind(longSma, shortSma, OnProcess)
            .Start();
    }

    private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
    {
        if (candle.State != CandleStates.Finished)
            return;

        var isShortLessThenLong = shortValue < longValue;

        if (_isShortLessThenLong == null)
        {
            _isShortLessThenLong = isShortLessThenLong;
        }
        else if (_isShortLessThenLong != isShortLessThenLong)
        {
            var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
            var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
            var price = candle.ClosePrice;

            if (direction == Sides.Buy)
                BuyLimit(price, volume);
            else
                SellLimit(price, volume);

            _isShortLessThenLong = isShortLessThenLong;
        }
    }

    protected override void OnReseted()
    {
        base.OnReseted();
        _isShortLessThenLong = null;
    }
}
```

### Tipos de parámetros compatibles

La optimización admite los siguientes tipos de parámetros:

| Tipo | Ejemplo de `SetOptimize` |
|------|--------------------------|
| `int`, `long` y otros tipos enteros | `.SetOptimize(10, 100, 5)` |
| `decimal`, `double`, `float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

Para parámetros con un conjunto discreto de valores, como `DataType`, use `SetOptimizeValues`:

```csharp
_candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame())
    .SetCanOptimize(true)
    .SetOptimizeValues(new[]
    {
        TimeSpan.FromMinutes(5).TimeFrame(),
        TimeSpan.FromMinutes(15).TimeFrame(),
        TimeSpan.FromMinutes(30).TimeFrame(),
    });
```

## Optimización por fuerza bruta

La clase `BruteForceOptimizer` recorre todas las combinaciones posibles de valores de parámetros. Este modo es adecuado para espacios de parámetros pequeños.

### Crear y configurar el optimizador

```csharp
// Instrumento y cartera.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Almacenamiento de datos históricos.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// Crear el optimizador.
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// Configurar parámetros de emulación.
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // iteraciones máximas (0 = ilimitado)
settings.CommissionRules = new[]                       // comisión
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // número de hilos paralelos
                                                       // predeterminado = CPU * 2

// Cachear datos de mercado entre iteraciones para acelerar la optimización.
optimizer.AdapterCache = new();
```

### Ejecutar optimización por fuerza bruta

```csharp
// Estrategia base con rangos de optimización.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Seleccionar parámetros para optimizar.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// Generar todas las combinaciones de parámetros.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// Ejecutar la optimización.
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s es la estrategia con resultados después del backtesting.
    Console.WriteLine($"PnL={s.PnL}, LongSma={s.Parameters["LongSma"].Value}, " +
                      $"ShortSma={s.Parameters["ShortSma"].Value}");
}
```

### Muestreo aleatorio

Si recorrer todas las combinaciones toma demasiado tiempo, use muestreo aleatorio. El método `ToBruteForceRandom` genera el número especificado de combinaciones aleatorias:

```csharp
var randomCount = 50; // número de combinaciones aleatorias

var strategies = strategy.ToBruteForceRandom(
    optimizeParams,
    randomCount,
    out _,
    out var totalCount);

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Optimización genética

La clase `GeneticOptimizer` implementa un algoritmo genético que es mucho más eficiente que la fuerza bruta cuando el número de parámetros es grande. El algoritmo converge automáticamente hacia valores óptimos en menos iteraciones.

### Crear y configurar el optimizador

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // sistema de archivos para la fórmula de fitness

optimizer.AdapterCache = new();

// Configurar el algoritmo genético.
optimizer.Settings.Population = 8;            // tamaño de población
optimizer.Settings.PopulationMax = 16;        // tamaño máximo de población
optimizer.Settings.GenerationsMax = 20;       // generaciones máximas
optimizer.Settings.GenerationsStagnation = 5; // detener tras N generaciones sin mejora
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // fórmula de fitness (PnL por defecto)

optimizer.EmulationSettings.MaxIterations = 100;
```

### Parámetros del algoritmo genético

| Propiedad | Descripción | Predeterminado |
|-----------|-------------|----------------|
| `Population` | Tamaño inicial de población | 8 |
| `PopulationMax` | Tamaño máximo de población | 16 |
| `GenerationsMax` | Número máximo de generaciones (0 = ilimitado) | 20 |
| `GenerationsStagnation` | Detener tras N generaciones estancadas (0 = desactivado) | 5 |
| `MutationProbability` | Probabilidad de mutación (0..1) | 0.1 |
| `CrossoverProbability` | Probabilidad de cruce (0..1) | 0.75 |
| `Fitness` | Fórmula de la función de fitness | `"PnL"` |
| `Selection` | Operador de selección | `TournamentSelection` |
| `Crossover` | Operador de cruce | `OnePointCrossover` |
| `Mutation` | Operador de mutación | `UniformMutation` |
| `Reinsertion` | Estrategia de reemplazo de generación | `ElitistReinsertion` |

### Fórmula de fitness

La propiedad `Fitness` define la fórmula usada para evaluar una estrategia. Las estadísticas de estrategia están disponibles mediante abreviaturas:

| Abreviatura | Estadística de la estrategia |
|-------------|------------------------------|
| `PnL` | Beneficio neto |
| `MaxDD` | Drawdown máximo |
| `MaxRelDD` | Drawdown relativo máximo |
| `WinTrades` | Operaciones ganadoras |
| `LosTrades` | Operaciones perdedoras |
| `Recovery` | Factor de recuperación |
| `Ret` | Rentabilidad |
| `TCount` | Número de operaciones |
| `AvgTPnL` | Beneficio medio por operación |

Las fórmulas pueden combinarse, por ejemplo: `"PnL - MaxDD"` o `"Recovery"`.

### Ejecutar optimización genética

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Preparar parámetros para el optimizador genético.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// ToGeneticParameters convierte parámetros de estrategia al formato del optimizador genético.
// Para parámetros con un conjunto discreto de valores, como TimeSpan?, pase una lista
// explícita mediante una tupla (param, values):
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = usar el rango de SetOptimize
    (shortParam, null),
});

// Ejecutar la optimización.
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Configuración común del optimizador

La clase `OptimizerSettings`, disponible mediante `optimizer.EmulationSettings`, contiene ajustes comunes para ambos tipos de optimización:

| Propiedad | Descripción | Predeterminado |
|-----------|-------------|----------------|
| `BatchSize` | Número de estrategias probadas en paralelo | `CPU * 2` |
| `MaxIterations` | Número máximo de iteraciones (0 = ilimitado) | 0 |
| `MaxMessageCount` | Número máximo de mensajes procesados (-1 = ilimitado) | -1 |
| `CommissionRules` | Reglas de cálculo de comisiones | `null` |

## Caché de datos

La propiedad `AdapterCache` cachea los datos de mercado entre iteraciones. Esto acelera significativamente la optimización, porque los datos se cargan desde el almacenamiento una sola vez:

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## Eventos del optimizador

| Evento | Descripción |
|--------|-------------|
| `SingleProgressChanged` | Se llama cuando cambia el progreso de una sola iteración. Parámetros: `(Strategy, IStrategyParam[], int progress)`. Progreso `100` significa que la iteración está completa. |
| `StrategyInitialized` | Se llama después de inicializar la estrategia y antes de iniciar el backtest. |
| `ConnectorInitialized` | Se llama después de crear el conector y antes de que se conecte. Permite configurar parámetros de `HistoryEmulationConnector`. |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteration complete: PnL={strategy.PnL}");
};
```

## Pausa y detención

La optimización puede pausarse y reanudarse:

```csharp
// Pausar. Las iteraciones actuales finalizarán, no se iniciarán nuevas.
optimizer.Pause();

// Reanudar.
optimizer.Resume();

// Comprobar estado.
bool isPaused = optimizer.IsPaused;
```

Para detener la optimización por completo, cancele el `CancellationToken`:

```csharp
cts.Cancel();
```

## Ejemplo completo (aplicación de consola)

```csharp
using System;
using System.Linq;
using System.Threading;

using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Algo.Strategies;
using StockSharp.Algo.Strategies.Optimization;
using StockSharp.Algo.Commissions;
using StockSharp.BusinessEntities;
using StockSharp.Configuration;
using StockSharp.Messages;

// Configurar el instrumento y la cartera.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Almacenamiento de datos.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(Paths.HistoryDataPath)
};

// Crear el optimizador (fuerza bruta).
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

optimizer.EmulationSettings.MaxIterations = 100;
optimizer.EmulationSettings.CommissionRules = new[]
{
    new CommissionTradeRule { Value = 0.01m },
};
optimizer.AdapterCache = new();

// Configurar la estrategia.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Parámetros para optimizar.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// Generar combinaciones.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Total iterations: {totalCount}");

// Ejecutar la optimización.
var startTime = Paths.HistoryBeginDate;
var stopTime = Paths.HistoryEndDate;
var cts = new CancellationTokenSource();

var bestPnL = decimal.MinValue;
Strategy bestStrategy = null;

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    var pnl = s.PnL;
    var paramStr = string.Join(", ", parameters.Select(p => $"{p.Id}={p.Value}"));
    Console.WriteLine($"[{paramStr}] PnL={pnl:F2}");

    if (pnl > bestPnL)
    {
        bestPnL = pnl;
        bestStrategy = s;
    }
}

if (bestStrategy != null)
{
    Console.WriteLine($"\nBest result: PnL={bestPnL:F2}");
    foreach (var p in bestStrategy.Parameters)
        Console.WriteLine($"  {p.Id} = {p.Value}");
}
```

## Véase también

- [Pruebas con datos históricos](historical_data.md)
- Ejemplo: `Samples/07_Testing/02_Optimization`
