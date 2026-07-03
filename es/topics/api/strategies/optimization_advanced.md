# Optimización avanzada

## Descripción general

StockSharp proporciona herramientas avanzadas para ajustar con precisión el proceso de optimización de estrategias. Esta sección describe componentes avanzados: configuración de algoritmo genético mediante [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings), compilación de fórmulas de fitness personalizadas mediante [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) y supervisión del progreso mediante [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker).

## GeneticSettings

La clase [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) gestiona todos los aspectos del algoritmo de optimización genética. Se encuentra en el espacio de nombres `StockSharp.Algo.Strategies.Optimization`.

### Parámetros de población y generaciones

| Propiedad | Tipo | Predeterminado | Descripción |
|----------|------|----------------|-------------|
| `Population` | int | 8 | Tamaño de población inicial |
| `PopulationMax` | int | 16 | Tamaño máximo de población |
| `GenerationsMax` | int | 20 | Número máximo de generaciones |
| `GenerationsStagnation` | int | 5 | Detener por estancamiento (generaciones sin mejora) |

### Parámetros de probabilidad

| Propiedad | Tipo | Predeterminado | Descripción |
|----------|------|----------------|-------------|
| `MutationProbability` | decimal | 0.1 | Probabilidad de mutación (0-1) |
| `CrossoverProbability` | decimal | 0.8 | Probabilidad de crossover (0-1) |

### Operadores genéticos

| Propiedad | Predeterminado | Descripción |
|----------|----------------|-------------|
| `Reinsertion` | ElitistReinsertion | Estrategia de reemplazo de generación |
| `Mutation` | UniformMutation | Operador de mutación |
| `Crossover` | OnePointCrossover | Operador de crossover |
| `Selection` | TournamentSelection | Operador de selección |

## FitnessFormulaProvider

La clase [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) compila expresiones de string C# en funciones `Func<Strategy, decimal>` usadas para evaluar estrategias durante la optimización.

### Método Compile

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### Variables disponibles

Las siguientes variables están disponibles en fórmulas de función fitness, correspondientes a indicadores estadísticos de estrategia:

| Variable | Descripción |
|----------|-------------|
| PnL | Beneficio neto |
| WinTrades | Operaciones ganadoras |
| LosTrades | Operaciones perdedoras |
| TCount | Operaciones totales |
| RTrip | Round-trips |
| AvgTPnL | Beneficio medio por operación |
| AvgWTrades | Operación ganadora media |
| AvgLTrades | Operación perdedora media |
| MaxLong | Posición larga máxima |
| MaxShort | Posición corta máxima |
| MaxPnL | Beneficio máximo |
| MaxDD | Drawdown máximo |
| MaxRelDD | Drawdown relativo máximo |
| Ret | Rentabilidad |
| Recovery | Factor de recuperación |
| MaxLatReg | Latencia máxima de registro |
| MaxLatCan | Latencia máxima de cancelación |
| MinLatReg | Latencia mínima de registro |
| MinLatCan | Latencia mínima de cancelación |
| OrdCount | Número de órdenes |
| OrdRegErrCount | Errores de registro |
| OrdCancelErrCount | Errores de cancelación |
| OrdFundErrCount | Errores de fondos insuficientes |

Las variables se pueden combinar en expresiones matemáticas arbitrarias: `"PnL - MaxDD * 2"`, `"Recovery * WinTrades"`, `"PnL / (MaxDD + 1)"`.

## OptimizationProgressTracker

La clase [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) proporciona una supervisión cómoda del proceso de optimización.

### Propiedades

| Propiedad | Tipo | Descripción |
|----------|------|-------------|
| `TotalIterations` | int | Número total de iteraciones |
| `CompletedIterations` | int | Número de iteraciones completadas |
| `TotalProgress` | double | Progreso general (0-100) |
| `StartedAt` | DateTimeOffset | Hora de inicio de la optimización |
| `Elapsed` | TimeSpan | Tiempo transcurrido |
| `Remaining` | TimeSpan | Tiempo restante estimado |

### Métodos

| Método | Descripción |
|--------|-------------|
| `IterationCompleted()` | Marca una iteración como completada |
| `Reset(totalIterations)` | Restablece el tracker para una nueva ejecución |

## Clases de optimizador

StockSharp proporciona dos optimizadores que heredan de `BaseOptimizer`:

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** -- enumeración exhaustiva de todas las combinaciones de parámetros. Adecuado para espacios de parámetros pequeños.
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- algoritmo genético. Eficiente con una gran cantidad de parámetros, converge automáticamente hacia la solución óptima.

## Ejemplo de uso

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

// Fórmula de función fitness personalizada
var fitnessProvider = new FitnessFormulaProvider();
var fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");

// Crear el optimizador genético
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

// Supervisión del progreso
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

## Ver también

- [Optimización de estrategias](../testing/optimization.md)
- [Estadísticas de estrategia](statistics.md)
- [Optimización genética en Designer](../../designer/optimization/genetic.md)
