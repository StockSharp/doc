# Otimização de estratégias

## Visão geral

O StockSharp fornece um mecanismo incorporado para otimizar parâmetros de estratégias em dados históricos. A otimização percorre automaticamente diferentes combinações de parâmetros da estratégia, como comprimentos de indicadores, períodos, valores de limiar e outras definições, e ajuda a encontrar o melhor resultado para o critério selecionado, como lucro, drawdown, número de transações, e assim por diante.

Estão disponíveis dois modos de otimização:

- **Força bruta** -- a classe `BruteForceOptimizer`. Percorre todas as combinações possíveis de parâmetros, ou um subconjunto aleatório delas.
- **Algoritmo genético** -- a classe `GeneticOptimizer`. Usa uma abordagem evolutiva para encontrar parâmetros ótimos, o que é muito mais eficiente para espaços de parâmetros grandes.

Ambos os otimizadores herdam de `BaseOptimizer` e trabalham de forma assíncrona, devolvendo um `IAsyncEnumerable` com resultados à medida que cada iteração termina.

## Preparar uma estratégia

### Definir parâmetros com intervalos de otimização

Para otimizar uma estratégia, defina os seus parâmetros através de `StrategyParam<T>` e especifique intervalos de valores. O método `SetOptimize(from, to, step)` define o intervalo a percorrer, e `SetCanOptimize(true)` ativa a otimização para o parâmetro:

```csharp
class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _longSma = Param(nameof(LongSma), 80)
            .SetCanOptimize(true)
            .SetOptimize(50, 100, 5);      // de 50 a 100 com passo de 5

        _shortSma = Param(nameof(ShortSma), 30)
            .SetCanOptimize(true)
            .SetOptimize(20, 40, 1);        // de 20 a 40 com passo de 1

        _candleTimeFrame = Param<TimeSpan?>(nameof(CandleTimeFrame))
            .SetCanOptimize(true)
            .SetOptimize(
                TimeSpan.FromMinutes(5),    // a partir de 5 minutos
                TimeSpan.FromMinutes(15),   // até 15 minutos
                TimeSpan.FromMinutes(5));   // com passo de 5 minutos

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

### Tipos de parâmetros suportados

A otimização suporta os seguintes tipos de parâmetros:

| Tipo | Exemplo de `SetOptimize` |
|------|--------------------------|
| `int`, `long` e outros tipos inteiros | `.SetOptimize(10, 100, 5)` |
| `decimal`, `double`, `float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

Para parâmetros com um conjunto discreto de valores, como `DataType`, use `SetOptimizeValues`:

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

## Otimização por força bruta

A classe `BruteForceOptimizer` percorre todas as combinações possíveis de valores dos parâmetros. Este modo é adequado para espaços de parâmetros pequenos.

### Criar e configurar o otimizador

```csharp
// Instrumento e carteira.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Armazenamento de dados históricos.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// Criar o otimizador.
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// Configurar parâmetros de emulação.
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // máximo de iterações (0 = ilimitado)
settings.CommissionRules = new[]                       // comissão
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // número de threads paralelas
                                                       // predefinição = CPU * 2

// Colocar dados de mercado em cache entre iterações para acelerar a otimização.
optimizer.AdapterCache = new();
```

### Executar otimização por força bruta

```csharp
// Estratégia base com intervalos de otimização.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Selecionar parâmetros a otimizar.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// Gerar todas as combinações de parâmetros.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// Executar otimização.
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s é a estratégia com resultados após os testes históricos.
    Console.WriteLine($"PnL={s.PnL}, SMA longa={s.Parameters["LongSma"].Value}, " +
                      $"SMA curta={s.Parameters["ShortSma"].Value}");
}
```

### Amostragem aleatória

Se percorrer todas as combinações demorar demasiado tempo, use amostragem aleatória. O método `ToBruteForceRandom` gera o número especificado de combinações aleatórias:

```csharp
var randomCount = 50; // número de combinações aleatórias

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

## Otimização genética

A classe `GeneticOptimizer` implementa um algoritmo genético que é muito mais eficiente do que a força bruta quando o número de parâmetros é grande. O algoritmo converge automaticamente para valores ótimos em menos iterações.

### Criar e configurar o otimizador

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // sistema de ficheiros para a fórmula de fitness

optimizer.AdapterCache = new();

// Configurar o algoritmo genético.
optimizer.Settings.Population = 8;            // tamanho da população
optimizer.Settings.PopulationMax = 16;        // tamanho máximo da população
optimizer.Settings.GenerationsMax = 20;       // máximo de gerações
optimizer.Settings.GenerationsStagnation = 5; // parar após N gerações sem melhoria
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // fórmula de fitness (PnL por predefinição)

optimizer.EmulationSettings.MaxIterations = 100;
```

### Parâmetros do algoritmo genético

| Propriedade | Descrição | Predefinição |
|-------------|-----------|--------------|
| `Population` | Tamanho inicial da população | 8 |
| `PopulationMax` | Tamanho máximo da população | 16 |
| `GenerationsMax` | Número máximo de gerações (0 = ilimitado) | 20 |
| `GenerationsStagnation` | Parar após N gerações estagnadas (0 = desativado) | 5 |
| `MutationProbability` | Probabilidade de mutação (0..1) | 0.1 |
| `CrossoverProbability` | Probabilidade de crossover (0..1) | 0.75 |
| `Fitness` | Fórmula da função de fitness | `"PnL"` |
| `Selection` | Operador de seleção | `TournamentSelection` |
| `Crossover` | Operador de crossover | `OnePointCrossover` |
| `Mutation` | Operador de mutação | `UniformMutation` |
| `Reinsertion` | Estratégia de substituição de geração | `ElitistReinsertion` |

### Fórmula de fitness

A propriedade `Fitness` define a fórmula usada para avaliar uma estratégia. As estatísticas da estratégia estão disponíveis através de abreviaturas:

| Abreviatura | Estatística da estratégia |
|-------------|---------------------------|
| `PnL` | Lucro líquido |
| `MaxDD` | Drawdown máximo |
| `MaxRelDD` | Drawdown relativo máximo |
| `WinTrades` | Transações vencedoras |
| `LosTrades` | Transações perdedoras |
| `Recovery` | Fator de recuperação |
| `Ret` | Retorno |
| `TCount` | Número de transações |
| `AvgTPnL` | Lucro médio por transação |

As fórmulas podem ser combinadas, por exemplo: `"PnL - MaxDD"` ou `"Recovery"`.

### Executar otimização genética

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Preparar parâmetros para o otimizador genético.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// ToGeneticParameters converte parâmetros da estratégia para o formato do otimizador genético.
// Para parâmetros com um conjunto discreto de valores, como TimeSpan?, passe uma lista
// explícita através de um tuplo (param, values):
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = usar o intervalo de SetOptimize
    (shortParam, null),
});

// Executar otimização.
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Definições comuns do otimizador

A classe `OptimizerSettings`, disponível através de `optimizer.EmulationSettings`, contém definições comuns aos dois tipos de otimização:

| Propriedade | Descrição | Predefinição |
|-------------|-----------|--------------|
| `BatchSize` | Número de estratégias testadas em paralelo | `CPU * 2` |
| `MaxIterations` | Número máximo de iterações (0 = ilimitado) | 0 |
| `MaxMessageCount` | Número máximo de mensagens processadas (-1 = ilimitado) | -1 |
| `CommissionRules` | Regras de cálculo de comissão | `null` |

## Cache de dados

A propriedade `AdapterCache` coloca dados de mercado em cache entre iterações. Isto acelera significativamente a otimização, porque os dados são carregados do armazenamento apenas uma vez:

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## Eventos do otimizador

| Evento | Descrição |
|--------|-----------|
| `SingleProgressChanged` | Chamado quando o progresso muda para uma única iteração. Parâmetros: `(Strategy, IStrategyParam[], int progress)`. Progresso `100` significa que a iteração está concluída. |
| `StrategyInitialized` | Chamado depois de a estratégia ser inicializada e antes de o teste histórico começar. |
| `ConnectorInitialized` | Chamado depois de o conector ser criado e antes de se ligar. Permite configurar parâmetros de `HistoryEmulationConnector`. |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteração concluída: PnL={strategy.PnL}");
};
```

## Pausa e paragem

A otimização pode ser pausada e retomada:

```csharp
// Pausar. As iterações atuais terminarão; novas iterações não serão iniciadas.
optimizer.Pause();

// Retomar.
optimizer.Resume();

// Verificar estado.
bool isPaused = optimizer.IsPaused;
```

Para parar completamente a otimização, cancele o `CancellationToken`:

```csharp
cts.Cancel();
```

## Exemplo completo (aplicação de consola)

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

// Configurar o instrumento e a carteira.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Armazenamento de dados.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(Paths.HistoryDataPath)
};

// Criar o otimizador (força bruta).
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

// Configurar a estratégia.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Parâmetros a otimizar.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// Gerar combinações.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Total de iterações: {totalCount}");

// Executar otimização.
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
    Console.WriteLine($"\nMelhor resultado: PnL={bestPnL:F2}");
    foreach (var p in bestStrategy.Parameters)
        Console.WriteLine($"  {p.Id} = {p.Value}");
}
```

## Ver também

- [Teste com dados históricos](historical_data.md)
- Exemplo: `Samples/07_Testing/02_Optimization`
