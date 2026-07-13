# Informes de estrategia

## Descripción general

StockSharp proporciona un sistema de generación de informes para resultados de negociación de estrategias. El sistema se basa en dos componentes clave:

- **`IReportSource`** -- una interfaz que describe la fuente de datos para el informe (parámetros de estrategia, órdenes, operaciones, posiciones, estadísticas).
- **`IReportGenerator`** -- una interfaz de generador de informes que admite varios formatos (CSV, JSON, XML, Excel).

La clase `Strategy` implementa la interfaz `IReportSource`, por lo que una estrategia se puede pasar directamente al generador de informes.

## Interfaz IReportSource

La interfaz `IReportSource` proporciona todos los datos necesarios para generar un informe:

| Propiedad | Tipo | Descripción |
|----------|------|-------------|
| `Name` | `string` | Nombre de la estrategia |
| `TotalWorkingTime` | `TimeSpan` | Tiempo total de trabajo |
| `Commission` | `decimal?` | Comisión total |
| `Position` | `decimal` | Posición actual |
| `PnL` | `decimal` | Beneficio/pérdida total |
| `Slippage` | `decimal?` | Deslizamiento total |
| `Latency` | `TimeSpan?` | Latencia total |
| `Parameters` | `IEnumerable<(string, object)>` | Parámetros de estrategia |
| `StatisticParameters` | `IEnumerable<(string, object)>` | Parámetros estadísticos |
| `Orders` | `IEnumerable<ReportOrder>` | Órdenes |
| `OwnTrades` | `IEnumerable<ReportTrade>` | Operaciones propias |
| `Positions` | `IEnumerable<ReportPosition>` | Ciclos completos de posición |

Antes de leer los datos, se llama al método `Prepare()` para sincronizar el estado interno de la fuente.

## Clase ReportSource

`ReportSource` es una implementación independiente de `IReportSource`, no vinculada a la clase `Strategy`. Permite construir manualmente la fuente de datos para un informe:

```csharp
var source = new ReportSource();
source.Name = "Mi estrategia";
source.PnL = 15000m;
source.TotalWorkingTime = TimeSpan.FromHours(8);

source.AddParameter("Marco temporal", "5 minutos");
source.AddStatisticParameter("Ratio de Sharpe", 1.85);

source.AddOrder(new ReportOrder(
    Id: 123,
    TransactionId: 456,
    SecurityId: securityId,
    Side: Sides.Buy,
    Time: DateTime.UtcNow,
    Price: 100m,
    State: OrderStates.Done,
    Balance: 0,
    Volume: 10,
    Type: OrderTypes.Limit
));
```

### Agregación de datos

Con una gran cantidad de órdenes y operaciones, `ReportSource` agrega automáticamente datos para reducir el tamaño del informe:

```csharp
// Umbral de agregación automática (el valor predeterminado es 10000)
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// Intervalo de agrupación (el valor predeterminado es 1 hora)
source.AggregationInterval = TimeSpan.FromMinutes(30);

// Agregación manual
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

Durante la agregación, las órdenes y operaciones se agrupan por intervalo de tiempo, instrumento y dirección. Los volúmenes se suman y los precios se calculan como promedios ponderados.

## PositionLifecycleTracker

`PositionLifecycleTracker` sigue el ciclo de vida de posiciones y genera ciclos completos: registros de apertura y cierre de posición.

Un ciclo completo se registra cuando:
- Una posición se cierra por completo (el valor se vuelve cero)
- Se produce una reversión de posición (cambio de signo)

En la clase `Strategy`, el tracker se integra automáticamente: los ciclos completos finalizados se agregan a `ReportSource` mediante el evento `RoundTripClosed`.

```csharp
var tracker = new PositionLifecycleTracker();

// Evento al cerrar un ciclo completo
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"Posición cerrada: {roundTrip.SecurityId}, " +
        $"Apertura: {roundTrip.OpenTime} a {roundTrip.OpenPrice}, " +
        $"Cierre: {roundTrip.CloseTime} a {roundTrip.ClosePrice}, " +
        $"Volumen máximo: {roundTrip.MaxPosition}");
};

// Procesar actualización de posición
tracker.ProcessPosition(position);

// Acceder al historial de ciclos completos
IReadOnlyList<ReportPosition> history = tracker.History;
```

## Generadores de informes

Están disponibles los siguientes generadores:

| Generador | Formato | Descripción |
|-----------|--------|-------------|
| `CsvReportGenerator` | CSV | Formato de texto con delimitadores |
| `JsonReportGenerator` | JSON | JSON estructurado |
| `XmlReportGenerator` | XML | Formato XML |
| `ExcelReportGenerator` | Excel | Formato Excel (requiere `IExcelWorkerProvider`) |

Todos los generadores heredan de `BaseReportGenerator` y admiten la configuración de secciones incluidas:

```csharp
var generator = new CsvReportGenerator();

// Configurar secciones del informe
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## Generar un informe desde una estrategia

Como `Strategy` implementa `IReportSource`, se puede generar un informe directamente:

```csharp
// La propia estrategia es la fuente de datos
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

Para una fuente de datos separada:

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// Agregar posiciones desde el tracker
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## Ejemplo: estrategia con generación de informe al detenerse

```csharp
public class ReportingStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public ReportingStrategy()
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
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Lógica de negociación...
    }

    protected override void OnStopped()
    {
        // Generar informe cuando se detiene la estrategia
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

En este ejemplo, la estrategia crea automáticamente un informe CSV cuando se detiene. El informe incluye parámetros de estrategia, estadísticas, órdenes, operaciones y ciclos completos de posición.
