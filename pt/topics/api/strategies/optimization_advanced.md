# Otimização Avançada

## Visão Geral

StockSharp fornece ferramentas avançadas para afinar o processo de otimização de estratégias. Esta secção descreve componentes avançados: configuração do algoritmo genético através de [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings), compilação de fórmulas de fitness personalizadas através de [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) e monitorização do progresso através de [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker).

## GeneticSettings

A classe [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) gere todos os aspetos do algoritmo de otimização genética. Está localizada no namespace `StockSharp.Algo.Strategies.Optimization`.

### Parâmetros de População e Geração

| Propriedade | Tipo | Por defeito | Descrição |
|-------------|------|-------------|-----------|
| `Population` | int | 8 | Tamanho inicial da população |
| `PopulationMax` | int | 16 | Tamanho máximo da população |
| `GenerationsMax` | int | 20 | Número máximo de gerações |
| `GenerationsStagnation` | int | 5 | Parar em estagnação (gerações sem melhoria) |

### Parâmetros de Probabilidade

| Propriedade | Tipo | Por defeito | Descrição |
|-------------|------|-------------|-----------|
| `MutationProbability` | decimal | 0.1 | Probabilidade de mutação (0-1) |
| `CrossoverProbability` | decimal | 0.8 | Probabilidade de crossover (0-1) |

### Operadores Genéticos

| Propriedade | Por defeito | Descrição |
|-------------|-------------|-----------|
| `Reinsertion` | ElitistReinsertion | Estratégia de substituição de geração |
| `Mutation` | UniformMutation | Operador de mutação |
| `Crossover` | OnePointCrossover | Operador de crossover |
| `Selection` | TournamentSelection | Operador de seleção |

## FitnessFormulaProvider

A classe [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) compila expressões C# em string para funções `Func<Strategy, decimal>` usadas para avaliar estratégias durante a otimização.

### Método Compile

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### Variáveis Disponíveis

As seguintes variáveis estão disponíveis em fórmulas de função de fitness, correspondendo aos indicadores estatísticos da estratégia:

| Variável | Descrição |
|----------|-----------|
| PnL | Lucro líquido |
| WinTrades | Negócios vencedores |
| LosTrades | Negócios perdedores |
| TCount | Total de negócios |
| RTrip | Round-trips |
| AvgTPnL | Lucro médio por negócio |
| AvgWTrades | Negócio vencedor médio |
| AvgLTrades | Negócio perdedor médio |
| MaxLong | Posição longa máxima |
| MaxShort | Posição curta máxima |
| MaxPnL | Lucro máximo |
| MaxDD | Drawdown máximo |
| MaxRelDD | Drawdown relativo máximo |
| Ret | Retorno |
| Recovery | Fator de recuperação |
| MaxLatReg | Latência máxima de registo |
| MaxLatCan | Latência máxima de cancelamento |
| MinLatReg | Latência mínima de registo |
| MinLatCan | Latência mínima de cancelamento |
| OrdCount | Número de ordens |
| OrdRegErrCount | Erros de registo |
| OrdCancelErrCount | Erros de cancelamento |
| OrdFundErrCount | Erros de fundos insuficientes |

As variáveis podem ser combinadas em expressões matemáticas arbitrárias: `"PnL - MaxDD * 2"`, `"Recovery * WinTrades"`, `"PnL / (MaxDD + 1)"`.

## OptimizationProgressTracker

A classe [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) fornece monitorização conveniente do processo de otimização.

### Propriedades

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `TotalIterations` | int | Número total de iterações |
| `CompletedIterations` | int | Número de iterações concluídas |
| `TotalProgress` | double | Progresso global (0-100) |
| `StartedAt` | DateTimeOffset | Hora de início da otimização |
| `Elapsed` | TimeSpan | Tempo decorrido |
| `Remaining` | TimeSpan | Tempo restante estimado |

### Métodos

| Método | Descrição |
|--------|-----------|
| `IterationCompleted()` | Marca uma iteração como concluída |
| `Reset(totalIterations)` | Repõe o rastreador para uma nova execução |

## Classes de Otimizador

StockSharp fornece dois otimizadores que herdam de `BaseOptimizer`:

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** -- enumeração exaustiva de todas as combinações de parâmetros. Adequado para espaços de parâmetros pequenos.
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- algoritmo genético. Eficiente com um grande número de parâmetros, converge automaticamente para a solução ótima.

## Exemplo de Utilização

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

## Ver Também

- [Otimização de Estratégias](../testing/optimization.md)
- [Estatísticas da Estratégia](statistics.md)
- [Otimização Genética no Designer](../../designer/optimization/genetic.md)
