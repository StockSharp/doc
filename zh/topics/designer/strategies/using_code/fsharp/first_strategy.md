# F# 策略示例

下面以 SMA 策略为例，演示如何通过源代码创建策略。该示例与[通过模块创建算法](../../using_visual_designer/first_strategy.md)中使用模块组装的 SMA 策略相似。

本节不介绍 F# 语言结构，也不详细说明作为策略基类的 [Strategy](../../../../api/strategies.md)，而是重点介绍在 **Designer** 中使用代码时需要注意的特性。

> [!TIP]
> 在 **Designer** 中创建的策略与通过 [API](../../../../api.md) 创建的策略兼容，因为二者使用相同的基类 [Strategy](../../../../api/strategies.md)。因此，与运行[策略图](../../../live_execution/running_strategies_outside_of_designer.md)相比，在 **Designer** 外部运行此类策略要简单得多。

1. 使用专门的方式创建策略参数：

```fsharp
// --- 策略参数：CandleType、Long、Short、TakeValue、StopValue ---

// 蜡烛类型参数
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

// 长周期 SMA 参数
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// 短周期 SMA 参数
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// take profit 参数
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// stop loss 参数
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// 用于跟踪 SMA 交叉的标志
let mutable isShortLessThenLong : bool option = None

// --------------------- 公共属性 ---------------------

/// <summary>策略使用的蜡烛类型。</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>长周期 SMA 指标的周期。</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>短周期 SMA 指标的周期。</summary>
member this.Short
	with get () = shortParam.Value
	and set value = shortParam.Value <- value

/// <summary>take profit 值。</summary>
member this.TakeValue
	with get () = takeValueParam.Value
	and set value = takeValueParam.Value <- value

/// <summary>stop loss 值。</summary>
member this.StopValue
	with get () = stopValueParam.Value
	and set value = stopValueParam.Value <- value
```

使用 [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) 类后，系统会自动处理设置的保存和恢复。

2. 创建指标并订阅市场数据时，需要将二者绑定，使订阅收到的数据能够更新指标值：

```fsharp
// ---------- 创建指标 ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ 订阅蜡烛流并绑定指标 ------
let subscription = this.SubscribeCandles(this.CandleType)

// 将指标绑定到订阅并指定处理函数
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. 使用图表时需要注意：在 [Designer](../../../live_execution/running_strategies_outside_of_designer.md) 外部运行策略时，图表对象可能不存在。

```fsharp
// ------------- 配置图表 -------------
let area = this.CreateChartArea()

// area can be null in case there is no GUI (e.g., Runner or console app)
if not (isNull area) then
	// 绘制蜡烛
	this.DrawCandles(area, subscription) |> ignore

	// 绘制指标
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// 绘制自身成交
	this.DrawOwnTrades(area) |> ignore
```

4. 如果策略逻辑需要，请通过 [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) 启动持仓保护：

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. 策略逻辑本身在 OnProcess 方法中实现。该方法由步骤 1 中创建的订阅调用：

```fsharp
member private this.OnProcess
	(
		candle: ICandleMessage,
		longValue: decimal,
		shortValue: decimal
	) =
	// 记录蜡烛信息
	this.LogInfo(
		LocalizedStrings.SmaNewCandleLog,
		candle.OpenTime,
		candle.OpenPrice,
		candle.HighPrice,
		candle.LowPrice,
		candle.ClosePrice,
		candle.TotalVolume,
		candle.SecurityId
	)

	// 如果蜡烛未完成则跳过
	if candle.State <> CandleStates.Finished then
		()
	else
		// 判断短周期 SMA 是否小于长周期 SMA
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// 第一次：只记住当前关系
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// 发生了交叉
			// If short < long, that means Sell, otherwise Buy
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// 计算开新仓或反转持仓的数量
			// If there is no position, use Volume; otherwise, double
			// 取绝对持仓大小和 Volume 中的较小值
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// 获取限价价格步长
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// 将限价单价格设置为略高/略低于当前收盘价
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // should not occur

			// 发送限价单
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// 更新跟踪标志
			isShortLessThenLong <- Some shortLess
		| _ ->
			// 没有交叉则不执行操作
			()
```
