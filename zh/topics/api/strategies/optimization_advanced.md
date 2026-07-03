# 高级优化

## 概览

StockSharp 提供用于微调策略优化过程的高级工具。本节描述高级组件：通过 [GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) 配置遗传算法，通过 [FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) 编译自定义适应度公式，以及通过 [OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) 监控进度。

## 遗传设置

[GeneticSettings](xref:StockSharp.Algo.Strategies.Optimization.GeneticSettings) 类管理遗传优化算法的所有方面。它位于 `StockSharp.Algo.Strategies.Optimization` 命名空间中。

### 人口与代参数

| 属性 | 类型 | 默认值 | 描述 |
|----------|------|---------|-------------|
| `Population` | 整数 | 8 | 初始种群规模 |
| `PopulationMax` | 整数 | 16 | 最大人口规模 |
| `GenerationsMax` | 整数 | 20 | 最大世代数 |
| `GenerationsStagnation` | 整数 | 5 | 在停滞时停止（几代没有改进） |

### 概率参数

| 属性 | 类型 | 默认值 | 描述 |
|----------|------|---------|-------------|
| `MutationProbability` | 十进制 | 0.1 | 突变概率（0-1） |
| `CrossoverProbability` | 十进制 | 0.8 | 交叉概率（0-1） |

### 遗传算子

| 属性 | 默认值 | 描述 |
|----------|---------|-------------|
| `Reinsertion` | 精英再插入 | 代替生成策略 |
| `Mutation` | 统一变异 | 变异算子 |
| `Crossover` | 单点交叉 | 交叉算子 |
| `Selection` | 锦标赛选择 | 选择算子 |

## 健身公式提供者

[FitnessFormulaProvider](xref:StockSharp.Algo.Strategies.Optimization.FitnessFormulaProvider) 类将 C# 字符串表达式编译为 `Func<Strategy, decimal>` 函数，用于在优化过程中评估策略。

### 编译方法

```cs
var fitnessProvider = new FitnessFormulaProvider();
Func<Strategy, decimal> fitness = fitnessProvider.Compile("PnL / (MaxDD + 1)");
```

### 可用变量

在适应度函数公式中，可以使用以下变量，对应于策略统计指标：

| 变量 | 描述 |
|----------|-------------|
| 盈亏 | 净利润 |
| 赢利交易 | 赢利交易 |
| 亏损交易 | 亏损的交易 |
| TCount | 总交易 |
| RTrip | 往返旅行 |
| 平均每笔交易利润 | 每笔交易的平均利润 |
| AvgWTrades | 平均获利交易 |
| AvgLTrades | 平均亏损交易 |
| 最大多头 | 最大多头持仓 |
| 最大空头 | 最大空头头寸 |
| 最大盈利 | 最大利润 |
| 最大回撤 | 最大回撤 |
| 最大相对回撤 | 最大相对回撤 |
| Ret | 返回 |
| 回收 | 回收因子 |
| MaxLatReg | 最大注册延迟 |
| MaxLatCan | 最大取消延迟 |
| MinLatReg | 最小注册延迟 |
| MinLatCan | 最小取消延迟 |
| 订单数量 | 订单计数 |
| OrdRegErrCount | 注册错误 |
| OrdCancelErrCount | 取消错误 |
| OrdFundErrCount | 资金不足错误 |

变量可以组合成任意数学表达式：`"PnL - MaxDD * 2"`, `"Recovery * WinTrades"`, `"PnL / (MaxDD + 1)"`。

## 优化进度跟踪器

[OptimizationProgressTracker](xref:StockSharp.Algo.Strategies.Optimization.OptimizationProgressTracker) 类提供了对优化过程的便捷监控。

### 属性

| 属性 | 类型 | 描述 |
|----------|------|-------------|
| `TotalIterations` | 整数 | 迭代总次数 |
| `CompletedIterations` | int | 完成的迭代次数 |
| `TotalProgress` | 双精度 | 总体进度 (0-100) |
| `StartedAt` | 日期时间偏移 | 优化开始时间 |
| `Elapsed` | 时间间隔 | 已用时间 |
| `Remaining` | 时间间隔 | 预计剩余时间 |

### 方法

| 方法 | 描述 |
|--------|-------------|
| `IterationCompleted()` | 标记一次迭代为已完成 |
| `Reset(totalIterations)` | 重置追踪器以进行新运行 |

## 优化器类

StockSharp 提供了两种继承自 `BaseOptimizer` 的优化器：

- **[BruteForceOptimizer](xref:StockSharp.Algo.Strategies.Optimization.BruteForceOptimizer)** —— 对所有参数组合进行穷举枚举。适用于小型参数空间。
- **[GeneticOptimizer](xref:StockSharp.Algo.Strategies.Optimization.GeneticOptimizer)** -- 遗传算法。对大量参数有效，能够自动收敛到最优解。

## 使用示例

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

## 另请参阅

- [策略优化](../testing/optimization.md)
- [策略统计](statistics.md)
- [Designer中的遗传优化](../../designer/optimization/genetic.md)
