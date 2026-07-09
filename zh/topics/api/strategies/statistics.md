# 策略统计

## 概览

StockSharp平台提供了一个用于交易策略统计分析的综合系统，帮助交易员评估有效性、优化参数并做出明智决策。统计系统收集并处理来自交易各个方面的数据，包括订单、交易、持仓以及盈亏指标。

## 目的与好处

交易策略中的统计分析有几个重要功能：

1. **绩效测量**：使用净利润、最大回撤和恢复因子等指标对您的策略成功进行定量评估。

2. **风险管理**：通过最大回撤百分比和持仓规模统计等指标来了解策略的风险状况。

3. **优化**：通过比较不同参数集的统计指标来寻找最佳策略参数。

4. **交易质量分析**：分析交易分布、盈利与亏损交易的比例、每笔交易的平均利润。

5. **运营指标**：跟踪运营指标，例如延迟统计和订单错误率，以识别执行问题。

## 可用统计指标

StockSharp 中的 [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) 接口提供对多个按类别组织的统计参数的访问：

### 损益统计

- 净利润
- 净利润 (%)
- 最大利润
- 最大回撤
- 最大回撤 (%)
- 最大相对回撤
- 采收率

### 贸易统计

- 盈利交易数量
- 亏损交易次数
- 交易总数
- 每笔交易平均利润
- 平均盈利交易
- 平均亏损交易
- 每月/每日交易次数

### 持仓统计

- 最大多头持仓
- 最大空头持仓

### 序统计量

- 订单数量
- 订单错误数量
- 最大/最小注册延迟
- 最大/最小取消延迟

## 与策略类的集成

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类在执行过程中会自动收集和计算统计信息。统计管理器可以通过 `StatisticManager` 属性访问，该属性实现了 [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) 接口。

关键的统计值也直接作为 Strategy 类的属性表示：

- `PnL`：盈亏值
- `Commission`：已支付总佣金
- `Slippage`：总滑点
- `Latency`：平均订单操作延迟

## 可视化

StockSharp 提供了一个用于可视化策略统计的特殊图形组件，称为 `StatisticParameterGrid`，可在 `StockSharp.Xaml` 命名空间中使用。该网格以用户友好的格式显示所有统计参数。

有关该图形组件的更多信息，请参阅 [统计](../graphical_user_interface/strategies/statistics.md) 文档。

## 使用示例

下面是一个在代码中使用策略统计的示例：

```csharp
// 创建策略
var strategy = new SmaStrategy
{
	// 配置策略参数
	Security = security,
	Portfolio = portfolio,
	Volume = 1,
	// 设置 SMA 参数
	LongSma = 200,
	ShortSma = 50,
};

// 将策略连接到图表以便可视化
var chart = new ChartPanel();
strategy.SetChart(chart);

// 访问统计管理器
var statisticManager = strategy.StatisticManager;

// 在用户界面中显示策略统计
// 假设你在 XAML 中定义了名为 'StatisticsGrid' 的 StatisticParameterGrid
StatisticsGrid.Parameters.Clear();
StatisticsGrid.Parameters.AddRange(statisticManager.Parameters);

// 启动策略
strategy.Start();

// 当需要响应统计变化时
strategy.PnLChanged += () =>
{
	Console.WriteLine($"Current PnL: {strategy.PnL}");
	
	// 也可以访问单个统计参数
	var netProfit = statisticManager.Parameters
		.OfType<NetProfitParameter>()
		.FirstOrDefault();
		
	if (netProfit != null)
	{
		Console.WriteLine($"Net Profit: {netProfit.Value}");
	}
};

// 用于跟踪持仓统计
strategy.PositionChanged += () =>
{
	Console.WriteLine($"Current Position: {strategy.Position}");
};
```

## 自定义统计

你也可以通过实现适当的接口来创建自己的统计参数：

- [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter)：所有统计参数的基础接口
- [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter)：与利润/损失相关的参数
- [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter)：与交易相关的参数
- [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter)：与持仓相关的参数
- [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter)：与订单相关的参数

这里有一个自定义统计参数的简单例子：

```csharp
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = "My Custom Indicator",
	Description = "Description of my custom indicator",
	GroupName = "Custom Parameters",
	Order = 1000
)]
public class MyCustomParameter : BasePnLStatisticParameter<decimal>
{
	public MyCustomParameter()
		: base(StatisticParameterTypes.Custom)
	{
	}

	public override void Add(DateTimeOffset marketTime, decimal pnl, decimal? commission)
	{
		// 自定义计算逻辑
		Value = /* your custom calculation */;
	}
}

// 然后将其添加到策略的 StatisticManager
strategy.StatisticManager.Parameters.Add(new MyCustomParameter());
```

## 结论

StockSharp中的统计分析系统为交易者提供了评估和优化交易策略的强大工具。通过使用这些统计数据，您可以深入了解策略的表现，识别需要改进的领域，并做出数据驱动的决策以提升交易结果。
