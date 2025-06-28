# F# Strategy Example

Creating a strategy from source code will be demonstrated using the SMA strategy example - similar to the SMA strategy example assembled from cubes in the [Creating Algorithm from Cubes](../../using_visual_designer/first_strategy.md) section.

This section will not describe F# language constructs (or [Strategy](../../../../api/strategies.md), which is used as the base for creating strategies), but will mention specific features for working with code in the **Designer**.

> [!TIP]
> Strategies created in the **Designer** are compatible with strategies created in the [API](../../../../api.md) due to using the common base class [Strategy](../../../../api/strategies.md). This makes running such strategies outside the **Designer** significantly easier than running [schemes](../../../live_execution/running_strategies_outside_of_designer.md).

1. Strategy parameters are created using a special approach:

```fsharp
// --- Strategy parameters: CandleType, Long, Short, TakeValue, StopValue ---

// Parameter for the candle type
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

// Parameter for the long SMA
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// Parameter for the short SMA
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// Parameter for take profit
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// Parameter for stop loss
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// Flag to track SMA crossing
let mutable isShortLessThenLong : bool option = None

// --------------------- Public properties ---------------------

/// <summary>The candle type used by the strategy.</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>The period for the long SMA indicator.</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>The period for the short SMA indicator.</summary>
member this.Short
	with get () = shortParam.Value
	and set value = shortParam.Value <- value

/// <summary>Take profit value.</summary>
member this.TakeValue
	with get () = takeValueParam.Value
	and set value = takeValueParam.Value <- value

/// <summary>Stop loss value.</summary>
member this.StopValue
	with get () = stopValueParam.Value
	and set value = stopValueParam.Value <- value
```

When using the [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) class, the settings save and restore approach is automatically used.

2. When creating indicators and subscribing to market data, you need to bind them so that incoming data from the subscription can update indicator values:

```fsharp
// ---------- Create indicators ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ Subscribe to the candle flow and bind indicators ------
let subscription = this.SubscribeCandles(this.CandleType)

// Bind our indicators to the subscription and assign the processing function
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. When working with charts, it's important to consider that when running the strategy [outside the Designer](../../../live_execution/running_strategies_outside_of_designer.md), the chart object might be absent.

```fsharp
// ------------- Configure chart -------------
let area = this.CreateChartArea()

// area can be null in case there is no GUI (e.g., Runner or console app)
if not (isNull area) then
	// Draw candles
	this.DrawCandles(area, subscription) |> ignore

	// Draw indicators
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// Draw own trades
	this.DrawOwnTrades(area) |> ignore
```

4. Start position protection through [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) if required by the strategy logic:

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. The strategy logic itself, implemented in the OnProcess method. The method is called by the subscription created in step 1:

```fsharp
member private this.OnProcess
	(
		candle: ICandleMessage,
		longValue: decimal,
		shortValue: decimal
	) =
	// Log candle information
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

	// Skip if the candle is not finished
	if candle.State <> CandleStates.Finished then
		()
	else
		// Determine if short SMA is less than long SMA
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// First time: just remember the current relation
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// A crossing has occurred
			// If short < long, that means Sell, otherwise Buy
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// Calculate volume for opening a new position or reversing
			// If there is no position, use Volume; otherwise, double
			// the minimum of the absolute position size and Volume
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// Get price step for the limit price
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// Set the limit order price slightly higher/lower than the current close price
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // should not occur

			// Send limit order
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// Update the tracking flag
			isShortLessThenLong <- Some shortLess
		| _ ->
			// Do nothing if no crossing
			()
```