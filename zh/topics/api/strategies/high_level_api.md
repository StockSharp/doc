# 策略中的高级 API

StockSharp 提供了一组高级 API，用于简化在交易策略中处理常见任务的工作。这些接口允许编写更简洁的代码，专注于交易逻辑而不是技术细节。

## 简化的订阅管理

用于处理订阅的高级方法隐藏了管理订阅生命周期和数据处理的复杂性。

### 订阅K线方法

你可以使用 [SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)) 方法，而不是手动创建订阅和设置事件处理程序：

```cs
// 用一行创建并配置 K线订阅
var subscription = SubscribeCandles(CandleType);
```

此方法返回一个类型为 [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1) 的对象，该对象提供了一个便捷的接口以进行进一步的订阅配置。

### 指标与订阅的自动绑定

高级 API 使将指标绑定到数据订阅变得容易：

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
	// 将指标绑定到 K线订阅
	.Bind(longSma, shortSma, OnProcess)
	// 开始处理
	.Start();
```

#### 自动向 Strategy.Indicators 集合添加指标

需要注意的是，当使用 [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) 方法将指标与订阅链接时，**不需要**额外将这些指标添加到 [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中，这在传统方法中通常是需要的（如[指标文档](indicators.md)中所述）。系统会自动：

1. 将指标添加到 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中
2. 跟踪指标的形成状态
3. 更新策略的 [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) 状态

这显著简化了代码并减少了错误的可能性。

如果需要在某些指标尚无
数据时仍然接收指标值（`IIndicatorValue.IsEmpty` 为 `true`），请使用
`BindWithEmpty` 方法。在这种情况下，处理程序参数必须为
`decimal?` 类型。你也可以使用 `BindEx` 直接检查原始
`IIndicatorValue` 对象。

#### 使用 BindEx 处理原始指标值

如果一个指标返回非标准值（不仅仅是数字），你可以使用 [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) 方法，该方法提供对原始 [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) 对象的访问：

```cs
subscription
	.BindEx(indicator, OnProcessWithRawValue)
	.Start();

// 处理器接收原始 IIndicatorValue
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
	// 访问 IIndicatorValue 属性
	if (value.IsFinal)
	{
		// 对于返回布尔值的指标
		var boolValue = value.GetValue<bool>();
		
		// 或特定指标专用的其他数据类型
		// ...
	}
}
```

[BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) 方法在以下情况下特别有用：

- 使用返回布尔值的指标（例如， [Fractals](xref:StockSharp.Algo.Indicators.Fractals)）
- 访问指标值类型的附加属性（例如，[IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal)标志）
- 使用返回结构化数据的指标

#### 使用复杂指标（IComplexIndicator）

对于包含多个内部指标的复杂指标（例如， [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), [MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence)），API 提供了 `Bind` 和 `BindEx` 方法的特殊重载：

```cs
// 创建复杂指标
var bollinger = new BollingerBands 
{ 
	Length = 20, 
	Deviation = 2 
};

// 将复杂指标绑定到订阅
subscription
	.BindEx(bollinger, OnProcessBollinger)
	.Start();

// 处理器接收 BollingerBandsValue 实例
private void OnProcessBollinger(ICandleMessage candle, IIndicatorValue value)
{
	var typed = (BollingerBandsValue)value;

		// 使用布林带值
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
}
```

为了更灵活的工作，您可以使用 [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) 来直接访问复杂指标值：

```cs
subscription.BindEx(bollinger, (candle, indicatorValue) =>
{
	var typed = (BollingerBandsValue)indicatorValue;

	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
});
```

[BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) 方法用于复杂指标自动：

1. 通过复杂指标处理输入数据
2. 将生成的 `IIndicatorValue` 传递给指定的处理程序

将该值转换为指示器专用的**值类型**，以便使用其各个字段。

### `Bind` 方法建立了订阅数据与指标之间的连接。当收到新的K线时：

1. K线会自动发送到指标进行处理
2. 处理结果会传递给指定的处理器（在示例中，是 `OnProcess` 方法）
3. 所有同步和状态管理代码对开发者来说都是隐藏的

处理器接收作为简单 `decimal` 类型的可直接使用的值。只有当所有绑定的指示器返回数据时才会调用该方法：

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// 直接使用现成指标值
	var isShortLessThenLong = shortValue < longValue;
	
	// 交易逻辑使用干净的数值
	// 无需从 IIndicatorValue 中提取
	// ...
}
```

这显著简化了代码并使其更易读，因为开发者不需要：
- 手动处理收到K线的事件
- 手动将数据传递给指标
- 从指标结果中提取数值

## 简化图表管理

### 自动可视化

高级 API 提供了将订阅和指标绑定到图表元素的简单方法：

```cs
var area = CreateChartArea();

// 无 GUI 运行时 area 可以为 null
if (area != null)
{
	// 将 K线自动绑定到图表区域
	DrawCandles(area, subscription);

	// 以自定义颜色绘制指标
	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);
	
	// 绘制自有成交
	DrawOwnTrades(area);
	
	// 绘制订单
	DrawOrders(area);
}
```

#### 绘制K线方法

[DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) 方法会自动将K线订阅链接到图表K线显示元素：

```cs
// 创建用于显示 K线的图表元素
IChartCandleElement candles = DrawCandles(area, subscription);

// 可以配置元素的附加参数
candles.DrawOpenClose = true;  // 显示开盘/收盘线
candles.DrawHigh = true;       // 显示最高价
candles.DrawLow = true;        // 显示最低价
```

该方法返回一个 [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) 图表元素，可以进一步自定义。

#### 绘制指标方法

[DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) 方法创建并配置用于显示指标值的图表元素：

```cs
// 使用默认颜色将指标简单添加到图表
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// 添加带指定主颜色的指标
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// 添加带指定主色和辅色的指标
IChartIndicatorElement bollingerElem = DrawIndicator(
	area, 
	bollinger, 
	System.Drawing.Color.Blue,    // 主颜色
	System.Drawing.Color.Gray     // 次颜色（用于第二条线）
);

// 元素附加配置
smaElem.DrawStyle = DrawStyles.Line;           // 绘制样式：线
rsiFast.DrawStyle = DrawStyles.Dot;            // 绘制样式：点
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // 绘制样式：点划线
```

该方法返回一个 [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) 图表元素，该元素可以自定义。对于具有多个值的指标（例如，[BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)），主要颜色应用于第一个值，次要颜色应用于第二个值。

#### DrawOwnTrades 方法

[DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) 方法在图表上创建一个用于显示策略自身交易的元素：

```cs
// 创建用于显示成交的元素
IChartTradeElement trades = DrawOwnTrades(area);

// 元素配置
trades.BuyColor = System.Drawing.Color.Green;   // 买入成交颜色
trades.SellColor = System.Drawing.Color.Red;    // 卖出成交颜色
trades.FullTitle = "我的策略成交";              // 元素标题
```

此方法会自动设置策略执行的所有交易的显示。交易会作为标记显示在图表上，标记出交易执行的点，并考虑交易方向（买入/卖出）。

#### DrawOrders 方法

[DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) 方法创建一个用于在图表上显示订单的元素：

```cs
// 创建用于显示订单的元素
IChartOrderElement orders = DrawOrders(area);

// 元素配置
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // 活跃买入订单颜色
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // 活跃卖出订单颜色
orders.BuyColor = System.Drawing.Color.Green;              // 已成交买入订单颜色
orders.SellColor = System.Drawing.Color.Red;               // 已成交卖出订单颜色
orders.CancelColor = System.Drawing.Color.Gray;            // 已撤销订单颜色
```

此方法会自动设置策略所下的所有订单的显示。订单会以其价格水平的标记显示，不同的订单状态会有不同的颜色编码。

#### 创建图表区域方法

[CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) 方法在策略图上创建一个新区域：

```cs
// 创建 K线和指标的第一个区域
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// 为单独的指标创建第二个区域（例如 RSI）
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

将图表划分为不同区域可以更直观地显示不同类型的数据。例如，数值范围与价格不同的指标（RSI、随机指标等）最好在单独的区域中显示。

高级可视化方法的优点：
- 无需手动创建 `ChartDrawData` 对象
- 无需按时间管理数据分组
- 无需拨打 `chart.Draw()` 来更新图表
- 订阅与图表元素之间的自动数据同步
- 图形元素外观的简化管理

当接收到新数据时，系统会自动更新图表，使开发者可以避免关注技术可视化的细节。

## 持仓保护

### 启动保护方法

为了保护未平仓持仓，StockSharp 提供了高级 [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) 方法：

```cs
// 使用止盈和止损水平启动持仓保护
StartProtection(TakeValue, StopValue);
```

此方法会自动为所有未平仓持仓设置保护：
- 跟踪价格变化
- 当达到止盈或止损水平时，自动创建平仓订单
- 支持多种类型的测量单位（绝对值、百分比、点）
- 可以使用跟踪止损进行自适应持仓保护

带有附加参数的示例：

```cs
// 使用跟踪止损和市价订单启动保护
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute), // 止盈
	stopLoss: new Unit(2, UnitTypes.Percent),     // 百分比止损
	isStopTrailing: true,                         // 启用跟踪止损
	useMarketOrders: true                         // 使用市价单
);
```

## 高级 API 的优势

StockSharp 策略中的高级 API 提供以下优势：

1. **代码量减少** - 执行常见任务所需的代码行数更少

2. **职责分离** - 交易逻辑与数据处理和可视化的技术细节分开

3. **可读性提升** - 代码变得更易理解和表达清晰，更专注于业务逻辑

4. **降低错误概率** - 通过自动化常规任务可以消除许多典型错误

5. **处理干净的数据类型** - 与其处理复杂对象，你可以操作简单的数据类型 (例如， `decimal`)

## 使用高级 API 的示例策略

下面是一个展示使用高级 API 的策略的完整示例：

```cs
public class SmaStrategy : Strategy
{
	private bool? _isShortLessThenLong;

	public SmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
		_long = Param(nameof(Long), 80);
		_short = Param(nameof(Short), 30);
		_takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
		_stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
	}

	private readonly StrategyParam<DataType> _candleType;
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}

	private readonly StrategyParam<int> _long;
	public int Long
	{
		get => _long.Value;
		set => _long.Value = value;
	}

	private readonly StrategyParam<int> _short;
	public int Short
	{
		get => _short.Value;
		set => _short.Value = value;
	}

	private readonly StrategyParam<Unit> _takeValue;
	public Unit TakeValue
	{
		get => _takeValue.Value;
		set => _takeValue.Value = value;
	}

	private readonly StrategyParam<Unit> _stopValue;
	public Unit StopValue
	{
		get => _stopValue.Value;
		set => _stopValue.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// 创建指标
		var longSma = new SMA { Length = Long };
		var shortSma = new SMA { Length = Short };

		// 创建 K线订阅并绑定到指标
		var subscription = SubscribeCandles(CandleType);
		subscription
			.Bind(longSma, shortSma, OnProcess)
			.Start();

		// 配置可视化
		var area = CreateChartArea();
		if (area != null)
		{
			DrawCandles(area, subscription);
			DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
			DrawIndicator(area, longSma);
			DrawOwnTrades(area);
		}

		// 启动仓位保护
		StartProtection(TakeValue, StopValue);
	}

	private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
	{
		// 只处理已完成的 K线
		if (candle.State != CandleStates.Finished)
			return;

		// 基于指标交叉的交易逻辑
		var isShortLessThenLong = shortValue < longValue;

		if (_isShortLessThenLong == null)
		{
			_isShortLessThenLong = isShortLessThenLong;
		}
		else if (_isShortLessThenLong != isShortLessThenLong)
		{
			// 发生交叉
			var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
			var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
			var priceStep = GetSecurity().PriceStep ?? 1;
			var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

			// 下单
			if (direction == Sides.Buy)
				BuyLimit(price, volume);
			else
				SellLimit(price, volume);

			// 保存当前指标位置
			_isShortLessThenLong = isShortLessThenLong;
		}
	}
}
```

## 结论

StockSharp 中的高层 API 显著简化了交易策略的开发，使开发者能够专注于交易逻辑而不是技术细节。对于不需要对数据处理或可视化进行精细调整的典型用例，它尤其有用。

结合策略参数系统、事件模型和持仓保护机制，高层 API 使 StockSharp 成为一个功能强大且便捷的算法交易品种，适合初学者和有经验的开发者。
