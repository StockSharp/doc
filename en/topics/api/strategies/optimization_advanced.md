# Advanced Optimization

## Overview

StockSharp provides advanced tools for fine-tuning the strategy optimization process. This section describes advanced components: genetic algorithm configuration via [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings), custom fitness formula compilation via [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider), and progress monitoring via [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker).

## GeneticSettings

The [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) class manages all aspects of the genetic optimization algorithm. It is located in the `StockSharp.Algo.Strategies.Optimization` namespace.

### Population and Generation Parameters

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Population` | int | 8 | Initial population size |
| `PopulationMax` | int | 16 | Maximum population size |
| `GenerationsMax` | int | 20 | Maximum number of generations |
| `GenerationsStagnation` | int | 5 | Stop on stagnation (generations without improvement) |

### Probability Parameters

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `MutationProbability` | decimal | 0.1 | Mutation probability (0-1) |
| `CrossoverProbability` | decimal | 0.8 | Crossover probability (0-1) |

### Genetic Operators

| Property | Default | Description |
|----------|---------|-------------|
| `Reinsertion` | ElitistReinsertion | Generation replacement strategy |
| `Mutation` | UniformMutation | Mutation operator |
| `Crossover` | OnePointCrossover | Crossover operator |
| `Selection` | TournamentSelection | Selection operator |

## FitnessFormulaProvider

The [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) class compiles C# string expressions into `Func<Strategy, decimal>` functions used for evaluating strategies during optimization.

### Compile Method

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### Available Variables

The following variables are available in fitness function formulas, corresponding to strategy statistical indicators:

| Variable | Description |
|----------|-------------|
| PnL | Net profit |
| WinTrades | Winning trades |
| LosTrades | Losing trades |
| TCount | Total trades |
| RTrip | Round-trips |
| AvgTPnL | Average profit per trade |
| AvgWTrades | Average winning trade |
| AvgLTrades | Average losing trade |
| MaxLong | Max long position |
| MaxShort | Max short position |
| MaxPnL | Max profit |
| MaxDD | Max drawdown |
| MaxRelDD | Max relative drawdown |
| Ret | Return |
| Recovery | Recovery factor |
| MaxLatReg | Max registration latency |
| MaxLatCan | Max cancellation latency |
| MinLatReg | Min registration latency |
| MinLatCan | Min cancellation latency |
| OrdCount | Order count |
| OrdRegErrCount | Registration errors |
| OrdCancelErrCount | Cancellation errors |
| OrdFundErrCount | Insufficient funds errors |

Variables can be combined in arbitrary mathematical expressions: `"PnL - MaxDD * 2"`, `"Recovery * WinTrades"`, `"PnL / (MaxDD + 1)"`.

## OptimizationProgressTracker

The [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) class provides convenient monitoring of the optimization process.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `TotalIterations` | int | Total number of iterations |
| `CompletedIterations` | int | Number of completed iterations |
| `TotalProgress` | double | Overall progress (0-100) |
| `StartedAt` | DateTimeOffset | Optimization start time |
| `Elapsed` | TimeSpan | Elapsed time |
| `Remaining` | TimeSpan | Estimated remaining time |

### Methods

| Method | Description |
|--------|-------------|
| `IterationCompleted()` | Marks one iteration as completed |
| `Reset(totalIterations)` | Resets the tracker for a new run |

## Optimizer Classes

StockSharp provides two optimizers that inherit from `BaseOptimizer`:

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** -- exhaustive enumeration of all parameter combinations. Suitable for small parameter spaces.
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- genetic algorithm. Efficient with a large number of parameters, automatically converges to the optimal solution.

## Usage Example

```cs
var geneticSettings = new GeneticSettings
{
	Population = 16,
	PopulationMax = 32,
	GenerationsMax = 50,
	GenerationsStagnation = 10,
	MutationProbability = 0.15m,
	CrossoverProbability = 0.75m,
};

// Custom fitness function formula
var fitnessProvider = new FitnessFormulaProvider();
var fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");

// Create the genetic optimizer
var optimizer = new GeneticOptimizer(
	new CollectionSecurityProvider(new[] { security }),
	new CollectionPortfolioProvider(new[] { portfolio }),
	storageRegistry,
	Paths.FileSystem);

optimizer.Settings.Population = geneticSettings.Population;
optimizer.Settings.PopulationMax = geneticSettings.PopulationMax;
optimizer.Settings.GenerationsMax = geneticSettings.GenerationsMax;
optimizer.Settings.GenerationsStagnation = geneticSettings.GenerationsStagnation;
optimizer.Settings.MutationProbability = geneticSettings.MutationProbability;
optimizer.Settings.CrossoverProbability = geneticSettings.CrossoverProbability;

// Progress monitoring
var tracker = new OptimizationProgressTracker();
tracker.Reset(totalIterations: 100);

optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
	if (progress == 100)
	{
		tracker.IterationCompleted();
		Console.WriteLine($"Progress: {tracker.TotalProgress:F1}%, " +
			$"Remaining: {tracker.Remaining:hh\\:mm\\:ss}");
	}
};
```

## See Also

- [Strategy Optimization](../testing/optimization.md)
- [Strategy Statistics](statistics.md)
- [Genetic Optimization in Designer](../../designer/optimization/genetic.md)
