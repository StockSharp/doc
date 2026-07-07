# Relatórios de Estratégias

## Visão Geral

StockSharp fornece um sistema de geração de relatórios para os resultados de negociação de estratégias. O sistema é construído sobre dois componentes essenciais:

- **`IReportSource`** -- uma interface que descreve a fonte de dados para o relatório (parâmetros da estratégia, ordens, transações, posições, estatísticas).
- **`IReportGenerator`** -- uma interface de gerador de relatórios que suporta vários formatos (CSV, JSON, XML, Excel).

A classe `Strategy` implementa a interface `IReportSource`, pelo que uma estratégia pode ser passada diretamente para o gerador de relatórios.

## Interface IReportSource

A interface `IReportSource` fornece todos os dados necessários para gerar um relatório:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Name` | `string` | Nome da estratégia |
| `TotalWorkingTime` | `TimeSpan` | Tempo total de funcionamento |
| `Commission` | `decimal?` | Comissão total |
| `Position` | `decimal` | Posição atual |
| `PnL` | `decimal` | Lucro/perda total |
| `Slippage` | `decimal?` | Slippage total |
| `Latency` | `TimeSpan?` | Latência total |
| `Parameters` | `IEnumerable<(string, object)>` | Parâmetros da estratégia |
| `StatisticParameters` | `IEnumerable<(string, object)>` | Parâmetros estatísticos |
| `Orders` | `IEnumerable<ReportOrder>` | Ordens |
| `OwnTrades` | `IEnumerable<ReportTrade>` | Transações próprias |
| `Positions` | `IEnumerable<ReportPosition>` | Ciclos completos de posição |

Antes de ler os dados, o método `Prepare()` é chamado para sincronizar o estado interno da fonte.

## Classe ReportSource

`ReportSource` é uma implementação autónoma de `IReportSource`, não associada à classe `Strategy`. Permite construir manualmente a fonte de dados para um relatório:

```csharp
var source = new ReportSource();
source.Name = "My strategy";
source.PnL = 15000m;
source.TotalWorkingTime = TimeSpan.FromHours(8);

source.AddParameter("Timeframe", "5 minutes");
source.AddStatisticParameter("Sharpe Ratio", 1.85);

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

### Agregação de Dados

Com um grande número de ordens e transações, `ReportSource` agrega automaticamente os dados para reduzir o tamanho do relatório:

```csharp
// Limite de agregação automática (predefinição 10000)
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// Intervalo de agrupamento (predefinição 1 hora)
source.AggregationInterval = TimeSpan.FromMinutes(30);

// Agregação manual
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

Durante a agregação, ordens e transações são agrupadas por intervalo de tempo, instrumento e direção. Os volumes são somados e os preços são calculados como médias ponderadas.

## PositionLifecycleTracker

`PositionLifecycleTracker` acompanha o ciclo de vida das posições e gera ciclos completos -- registos de abertura e fecho de posição.

Um ciclo completo é registado quando:
- Uma posição é totalmente fechada (o valor torna-se zero)
- Ocorre uma inversão de posição (mudança de sinal)

Na classe `Strategy`, o tracker é integrado automaticamente: ciclos completos concluídos são adicionados ao `ReportSource` através do evento `RoundTripClosed`.

```csharp
var tracker = new PositionLifecycleTracker();

// Evento ao fechar um ciclo completo
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"Position closed: {roundTrip.SecurityId}, " +
        $"Open: {roundTrip.OpenTime} at {roundTrip.OpenPrice}, " +
        $"Close: {roundTrip.CloseTime} at {roundTrip.ClosePrice}, " +
        $"Max volume: {roundTrip.MaxPosition}");
};

// Processar atualização de posição
tracker.ProcessPosition(position);

// Aceder ao histórico de ciclos completos
IReadOnlyList<ReportPosition> history = tracker.History;
```

## Geradores de Relatórios

Estão disponíveis os seguintes geradores:

| Gerador | Formato | Descrição |
|---------|---------|-----------|
| `CsvReportGenerator` | CSV | Formato de texto com delimitadores |
| `JsonReportGenerator` | JSON | JSON estruturado |
| `XmlReportGenerator` | XML | Formato XML |
| `ExcelReportGenerator` | Excel | Formato Excel (requer `IExcelWorkerProvider`) |

Todos os geradores herdam de `BaseReportGenerator` e suportam configuração das secções incluídas:

```csharp
var generator = new CsvReportGenerator();

// Configurar secções do relatório
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## Gerar um Relatório a Partir de uma Estratégia

Como `Strategy` implementa `IReportSource`, um relatório pode ser gerado diretamente:

```csharp
// A própria estratégia é a fonte de dados
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

Para uma fonte de dados separada:

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// Adicionar posições a partir do tracker
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## Exemplo: Estratégia com Geração de Relatório ao Parar

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

        // Lógica de negociação...
    }

    protected override void OnStopped()
    {
        // Gerar relatório quando a estratégia para
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

Neste exemplo, a estratégia cria automaticamente um relatório CSV quando para. O relatório inclui parâmetros da estratégia, estatísticas, ordens, transações e ciclos completos de posição.
