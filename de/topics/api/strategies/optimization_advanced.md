# Erweiterte Optimierung

## Überblick

StockSharp bietet erweiterte Werkzeuge zur Feinabstimmung des Strategieoptimierungsprozesses. Dieser Abschnitt beschreibt erweiterte Komponenten: Konfiguration genetischer Algorithmen über [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings), Kompilierung benutzerdefinierter Fitnessformeln über [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) und Fortschrittsüberwachung über [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker).

## GeneticSettings

Die Klasse [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) verwaltet alle Aspekte des genetischen Optimierungsalgorithmus. Sie befindet sich im Namespace `StockSharp.Algo.Strategies.Optimization`.

### Populations- und Generationsparameter

| Eigenschaft | Typ | Standard | Beschreibung |
|-------------|-----|----------|--------------|
| `Population` | int | 8 | Größe der Anfangspopulation |
| `PopulationMax` | int | 16 | Maximale Populationsgröße |
| `GenerationsMax` | int | 20 | Maximale Anzahl von Generationen |
| `GenerationsStagnation` | int | 5 | Stopp bei Stagnation (Generationen ohne Verbesserung) |

### Wahrscheinlichkeitsparameter

| Eigenschaft | Typ | Standard | Beschreibung |
|-------------|-----|----------|--------------|
| `MutationProbability` | decimal | 0.1 | Mutationswahrscheinlichkeit (0-1) |
| `CrossoverProbability` | decimal | 0.8 | Crossover-Wahrscheinlichkeit (0-1) |

### Genetische Operatoren

| Eigenschaft | Standard | Beschreibung |
|-------------|----------|--------------|
| `Reinsertion` | ElitistReinsertion | Strategie zum Ersetzen von Generationen |
| `Mutation` | UniformMutation | Mutationsoperator |
| `Crossover` | OnePointCrossover | Crossover-Operator |
| `Selection` | TournamentSelection | Selektionsoperator |

## FitnessFormulaProvider

Die Klasse [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) kompiliert C#-Stringausdrücke in Funktionen vom Typ `Func<Strategy, decimal>`, die zur Bewertung von Strategien während der Optimierung verwendet werden.

### Compile-Methode

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### Verfügbare Variablen

Die folgenden Variablen stehen in Formeln der Fitnessfunktion zur Verfügung und entsprechen statistischen Kennzahlen der Strategie:

| Variable | Beschreibung |
|----------|--------------|
| PnL | Nettogewinn |
| WinTrades | Gewinntrades |
| LosTrades | Verlusttrades |
| TCount | Gesamtzahl der Trades |
| RTrip | Round-Trips |
| AvgTPnL | Durchschnittlicher Gewinn pro Trade |
| AvgWTrades | Durchschnittlicher Gewinntrade |
| AvgLTrades | Durchschnittlicher Verlusttrade |
| MaxLong | Maximale Long-Position |
| MaxShort | Maximale Short-Position |
| MaxPnL | Maximaler Gewinn |
| MaxDD | Maximaler Drawdown |
| MaxRelDD | Maximaler relativer Drawdown |
| Ret | Rendite |
| Recovery | Recovery-Faktor |
| MaxLatReg | Maximale Registrierungslatenz |
| MaxLatCan | Maximale Stornierungslatenz |
| MinLatReg | Minimale Registrierungslatenz |
| MinLatCan | Minimale Stornierungslatenz |
| OrdCount | Anzahl der Orders |
| OrdRegErrCount | Registrierungsfehler |
| OrdCancelErrCount | Stornierungsfehler |
| OrdFundErrCount | Fehler wegen unzureichender Mittel |

Variablen können in beliebigen mathematischen Ausdrücken kombiniert werden: `"PnL - MaxDD * 2"`, `"Recovery * WinTrades"`, `"PnL / (MaxDD + 1)"`.

## OptimizationProgressTracker

Die Klasse [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) bietet eine bequeme Überwachung des Optimierungsprozesses.

### Eigenschaften

| Eigenschaft | Typ | Beschreibung |
|-------------|-----|--------------|
| `TotalIterations` | int | Gesamtzahl der Iterationen |
| `CompletedIterations` | int | Anzahl der abgeschlossenen Iterationen |
| `TotalProgress` | double | Gesamtfortschritt (0-100) |
| `StartedAt` | DateTimeOffset | Startzeit der Optimierung |
| `Elapsed` | TimeSpan | Verstrichene Zeit |
| `Remaining` | TimeSpan | Geschätzte verbleibende Zeit |

### Methoden

| Methode | Beschreibung |
|---------|--------------|
| `IterationCompleted()` | Markiert eine Iteration als abgeschlossen |
| `Reset(totalIterations)` | Setzt den Tracker für einen neuen Lauf zurück |

## Optimizer-Klassen

StockSharp stellt zwei Optimierer bereit, die von `BaseOptimizer` erben:

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** -- vollständige Enumeration aller Parameterkombinationen. Geeignet für kleine Parameterräume.
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- genetischer Algorithmus. Effizient bei einer großen Anzahl von Parametern und konvergiert automatisch zur optimalen Lösung.

## Verwendungsbeispiel

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

// Benutzerdefinierte Formel für die Fitnessfunktion
var fitnessProvider = new FitnessFormulaProvider();
var fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");

// Genetischen Optimierer erstellen
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

// Fortschrittsüberwachung
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

## Siehe auch

- [Strategieoptimierung](../testing/optimization.md)
- [Strategiestatistiken](statistics.md)
- [Genetische Optimierung im Designer](../../designer/optimization/genetic.md)

