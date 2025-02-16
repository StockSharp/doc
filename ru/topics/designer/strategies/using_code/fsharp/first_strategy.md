# Пример стратегии на F#

Создание стратегии из исходного кода будет рассмотрено на примере стратегии SMA \- аналогичный пример\-стратегии SMA, собранной из кубиков в пункте [Создание алгоритма из кубиков](../../using_visual_designer/first_strategy.md).

Данный раздел не будет описывать конструкции F# языка (как и [Strategy](../../../../api/strategies.md), на базе чего создаются стратегии), но будут упомянуты особенности для работы кода в **Дизайнере**.

> [!TIP]
> Стратегии, создающиеся в **Дизайнере** совместимы со стратегиями, создающиеся в [API](../../../../api.md) за счет использования общего базового класса [Strategy](../../../../api/strategies.md). Это делает возможность запуска таких стратегий вне **Дизайнера** значительно проще, чем [схем](../../../live_execution/running_strategies_outside_of_designer.md).

1. Параметры стратегии создаются через специальный подход:

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

При использовании класса [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) автоматически используется подход сохранения и восстановления настроек.

2. При создании индикаторов и подписки на маркет-данные необходимо связать их, чтобы поступающие данные из подписки могли обновлять значения индикаторов:

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

3. При работе с графиком необходимо учитывать, что в случае запуска стратегии [вне Дизайнера](../../../live_execution/running_strategies_outside_of_designer.md) объект графика может быть отсутствовать.

```fsharp
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

4. Запустить защиту позиций через [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection), если такое требует логика стратегии:

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. Сама логика стратегии, реализованная в методе OnProcess. Метод вызывается подпиской, созданной на этапе 1:

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
