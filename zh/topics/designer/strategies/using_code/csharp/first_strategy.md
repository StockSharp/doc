# C# 策略示例

下面以 SMA 策略为例，演示如何通过源代码创建策略。该示例与[通过模块创建算法](../../using_visual_designer/first_strategy.md)中使用模块组装的 SMA 策略相似。

本节不介绍 C# 语言结构，也不详细说明作为策略基类的 [Strategy](../../../../api/strategies.md)，而是重点介绍在 **Designer** 中使用代码时需要注意的特性。

> [!TIP]
> 在 **Designer** 中创建的策略与通过 [API](../../../../api.md) 创建的策略兼容，因为二者使用相同的基类 [Strategy](../../../../api/strategies.md)。因此，与运行[策略图](../../../live_execution/running_strategies_outside_of_designer.md)相比，在 **Designer** 外部运行此类策略要简单得多。

1. 使用专门的方式创建策略参数：

```cs
_candleTypeParam = this.Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);


private readonly StrategyParam<DataType> _candleTypeParam;

public DataType CandleType
{
	get => _candleTypeParam.Value;
	set => _candleTypeParam.Value = value;
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
```

使用 [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) 类后，系统会自动处理设置的保存和恢复。

2. 创建指标并订阅市场数据时，需要将二者绑定，使订阅收到的数据能够更新指标值：

```cs
// ---------- 创建指标 -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- 绑定K线集和指标 ----

var subscription = SubscribeCandles(CandleType);

subscription
	// 将指标绑定到K线
	.Bind(longSma, shortSma, OnProcess)
	// 开始处理
	.Start();
```

3. 使用图表时需要注意：在 [Designer](../../../live_execution/running_strategies_outside_of_designer.md) 外部运行策略时，图表对象可能不存在。

```cs
var area = CreateChartArea();

// 如果没有 GUI，area 可以为 null（策略运行在 Runner 或自己的控制台应用中）
if (area != null)
{
	DrawCandles(area, subscription);

	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	DrawOwnTrades(area);
}
```

4. 如果策略逻辑需要，请通过 [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) 启动持仓保护：

```cs
// 按止盈和/或止损启动保护
StartProtection(TakeValue, StopValue);
```

5. 策略逻辑本身在 OnProcess 方法中实现。该方法由步骤 1 中创建的订阅调用：

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// 如果我们只订阅了未完成的K线
	if (candle.State != CandleStates.Finished)
		return;

	// 计算 short 和 long 的新值
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong)
	{
		// 发生交叉

		// short 小于 long 时卖出，否则买入
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// 计算开仓或反转的数量
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		var priceStep = GetSecurity().PriceStep ?? 1;

		// 将订单价格计算为收盘价 + 偏移
		var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		// 保存 short 和 long 的当前值
		_isShortLessThenLong = isShortLessThenLong;
	}
}
```
