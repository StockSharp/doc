# 策略与StockSharp平台的兼容性

在StockSharp中开发交易策略时，重要的是要考虑它们与各种平台的兼容性：[Designer](../../designer.md)、[Shell](../../shell.md)、[Runner](../../runner.md) 和 [云回测](../../designer/backtesting/cloud_backtesting.md)。遵循以下建议，您将创建在所有环境中都能正确运行的策略。

## 策略构造器参数

### 避免在构造函数中使用参数

为了确保与 StockSharp 平台的兼容性，特别是在云回测时，**你不应该在策略构造函数中添加参数**：

```cs
// 正确：无参数构造函数
public class SmaStrategy : Strategy
{
	public SmaStrategy()
	{
		// 参数初始化
	}
}

// 错误：带参数的构造函数
public class SmaStrategy : Strategy
{
	public SmaStrategy(int longLength, int shortLength) // 不要使用这种方式
	{
		// ...
	}
}
```

StockSharp 平台使用无参数构造函数创建策略实例。如果你的策略需要带参数的构造函数，则不会被正确初始化。

## 使用 StrategyParam 而不是常规属性

### StrategyParam 的好处

不要创建常规的 C# 属性然后覆盖 `Save` 和 `Load` 方法，而是对所有可自定义参数使用 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1)：

```cs
// 正确：使用 StrategyParam
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
	get => _longSmaLength.Value;
	set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
	_longSmaLength = Param(nameof(LongSmaLength), 80)
						.SetDisplay("长期 SMA 周期", string.Empty, "基本设置");
}

// 错误：使用普通属性
private int _longSmaLength = 80; // 不要使用这种方式

public int LongSmaLength
{
	get => _longSmaLength;
	set => _longSmaLength = value;
}
```

通过 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) 创建的参数将自动：
- 在平台用户界面中显示
- 被保存和加载而不覆盖 `Save` 和 `Load` 方法
- 用于优化
- 发送到云回测时应正确序列化

## 与用户界面合作

### 使用抽象而不是直接访问用户界面

不要直接访问用户界面元素，而是使用 StockSharp 提供的抽象。

```cs
// 正确方式：使用 IChart
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// 获取运行环境提供的图表
	_chart = GetChart();
	
	if (_chart != null)
	{
		// 图表可用（例如在 Designer 或 Shell 中）
		InitChart();
	}
	else
	{
		// 图表不可用（例如在 Runner 或云端回测中）
		// 策略在没有可视化时仍继续运行
	}
}

private void InitChart()
{
	// 通过抽象接口配置图表
	_chart.ClearAreas();
	var area = _chart.AddArea();
	_chartCandleElement = area.AddCandles();
	// ...
}
```

[Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) 方法在当前运行时环境中，如果有图表可用，将返回 [IChart](xref:StockSharp.Charting.IChart) 接口。如果策略在控制台 [Runner](../../runner.md) 或云回测中运行，而没有图形界面，该方法将返回 `null`。

[IChart](xref:StockSharp.Charting.IChart) 接口提供用于操作图表的方法：
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - 向图表添加一个区域
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - 移除一个区域
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - 向图表中添加一个元素
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - 移除一个元素
- [重置](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - 重置元素值

### 检查图表可用性

在使用图表之前，请始终检查其可用性：

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
	if (_chart == null) return; // 重要检查
	
	var data = _chart.CreateData();
	data.Group(candle.OpenTime)
		.Add(_chartCandleElement, candle)
		.Add(_longSmaIndicatorElement, longSma)
		.Add(_shortSmaIndicatorElement, shortSma);
	_chart.Draw(data);
}
```

## 线程与同步

### 避免创建额外的线程

在 StockSharp 中，**你不需要为数据处理创建额外的线程**。所有事件（市场数据、交易）都在单个线程中进行：

```cs
// 正确：使用标准事件处理器
private void ProcessCandle(ICandleMessage candle)
{
	// 在主线程中处理 K线
	var longSmaIsFormedPrev = _longSma.IsFormed;
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	
	// ...
}

// 错误：创建额外线程
private void ProcessCandle(ICandleMessage candle)
{
	// 不要这样做
	Task.Run(() => {
		var longSmaIsFormedPrev = _longSma.IsFormed;
		// ...
	});
}
```

### 避免使用同步对象

由于所有事件都在单线程中处理，**无需使用同步对象**：

```cs
// 正确：无需同步的常规处理
private void ProcessCandle(ICandleMessage candle)
{
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	// ...
}

// 错误：不必要的同步
private readonly object _syncLock = new object(); // Not needed

private void ProcessCandle(ICandleMessage candle)
{
	lock (_syncLock) // Not needed
	{
		var ls = _longSma.Process(candle);
		// ...
	}
}
```

## 外部资源

### 使用 StockSharp 基础设施

不要直接访问外部资源（文件、数据库、网络），而应使用 StockSharp 平台提供的功能：

```cs
// 正确：使用内置机制保存数据
protected override void OnStopped()
{
	// 数据会通过策略参数自动保存
	base.OnStopped();
}

// 错误：直接访问外部资源
protected override void OnStopped()
{
	// 不要这样做
	File.WriteAllText("results.txt", $"PnL: {PnL}");
	
	// 或者这样
	using (var connection = new SqlConnection("..."))
	{
		// ...
	}
	
	base.OnStopped();
}
```

### 数据存储

要保存策略结果，请使用：

- [策略参数](parameters.md) 用于设置和配置
- [Designer](../../designer.md) 和 [Shell](../../shell.md) 中的内置存储机制
- [统计](xref:StockSharp.Algo.Statistics.StatisticManager) 用于收集交易指标

### 保存与加载方法

[Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) 和 [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) 方法专门用于保存不是设置或参数的额外策略数据。这是保存恢复策略状态所需数据的理想位置：

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings); // 先保存策略参数
	
	// 然后保存自定义数据
	settings.SetValue("CustomState", _customState);
	settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings); // 先加载策略参数
	
	// 然后加载自定义数据
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
	
	if (settings.Contains("LastSignalTime"))
		_lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

然而，主要的可配置参数仍应通过上述描述的 [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) 实现，因为这将确保它们在用户界面中自动显示。

## 市场数据订阅

### 使用规则而不是直接订阅

对于市场数据处理，建议使用 [事件模型](event_model.md) 和规则：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	var subscription = new Subscription(序列, Security);

	// 正确：使用规则处理数据
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Connector.Subscribe(subscription);
}
```

规则相比普通事件处理程序有几个重要的优势：

1. **自动退订** - 当策略停止或不再需要时，规则会自动从事件中退订。你无需手动管理订阅。

2. **高级 API** - 规则提供了比标准事件处理程序更易理解和方便的接口。例如，`WhenCandlesFinished` 比订阅 `CandleReceived` 事件并随后检查K线状态要清晰得多。

3. **条件组合** - 规则可以使用 `And`、`Or` 等运算符组合，从而创建复杂的激活条件：

```cs
// 组合规则示例
Security
	.WhenNewTrade()
	.And(Portfolio.WhenMoneyChanged())
	.Do(() => {
		// 仅当有新成交
		// 且投资组合余额发生变化时执行的代码
	})
	.Apply(this);
```

4. **生命周期管理** - 规则可以设置为一次性（`Once()`）、设定取消条件（`Until()`）、添加延迟操作等。

## 兼容策略示例

下面是一个遵循所有建议并能在所有 StockSharp 平台上正确运行的策略示例：

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType 序列
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("长期 SMA 周期", string.Empty, "基本设置")
							.SetCanOptimize(true);
							
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetDisplay("短期 SMA 周期", string.Empty, "基本设置")
							.SetCanOptimize(true);
							
		_series = Param(nameof(序列), DataType.TimeFrame(TimeSpan.FromMinutes(15)))
					.SetDisplay("序列", string.Empty, "基本设置");
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);
		
		// 如果可用则初始化图表
		_chart = GetChart();
		if (_chart != null)
			InitChart();
		
		var subscription = new Subscription(序列, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Connector.Subscribe(subscription);
	}

	private void InitChart()
	{
		_chart.ClearAreas();
		var area = _chart.AddArea();
		
		_chartCandleElement = area.AddCandles();
		
		_longSmaIndicatorElement = area.AddIndicator(_longSma);
		_longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
		_longSmaIndicatorElement.DrawStyle = DrawStyles.Line;
		
		_shortSmaIndicatorElement = area.AddIndicator(_shortSma);
		_shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
		_shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		var ls = _longSma.Process(candle);
		var ss = _shortSma.Process(candle);
		
		// 如果可用则在图表上绘制
		if (_chart != null)
		{
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_longSmaIndicatorElement, ls)
				.Add(_shortSmaIndicatorElement, ss);
			_chart.Draw(data);
		}
		
		if (!_longSma.IsFormed)
			return;
			
		var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		if (isShortLessCurrent == isShortLessPrev)
			return;
			
		// 交易逻辑
		var volume = Volume + Math.Abs(Position);

		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}
}
```

## 另请参阅

- [策略参数](parameters.md)
- [事件模型](event_model.md)
- [策略记录](logging.md)
