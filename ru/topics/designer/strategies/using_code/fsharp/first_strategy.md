# Пример стратегии на F#

Создание стратегии из исходного кода будет рассмотрено на примере стратегии SMA \- аналогичный пример\-стратегии SMA, собранной из кубиков в пункте [Создание алгоритма из кубиков](../../using_visual_designer/first_strategy.md).

Данный раздел не будет описывать конструкции F# языка (как и [Strategy](../../../../api/strategies.md), на базе чего создаются стратегии), но будут упомянуты особенности для работы кода в **Дизайнере**.

> [!TIP]
> Стратегии, создающиеся в **Дизайнере** совместимы со стратегиями, создающиеся в [API](../../../../api.md) за счет использования общего базового класса [Strategy](../../../../api/strategies.md). Это делает возможность запуска таких стратегий вне **Дизайнера** значительно проще, чем [схем](../../../live_execution/running_strategies_outside_of_designer.md).

1. Параметры стратегии создаются через специальный подход:

```fsharp
// --- Параметры стратегии: CandleType, Long, Short, TakeValue, StopValue ---

// Параметр типа свечей
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Тип свечи", "Тип свечи для расчета стратегии.", "Общие")

// Параметр длинной SMA
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// Параметр короткой SMA
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// Параметр take profit
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// Параметр stop loss
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// Флаг для отслеживания пересечения SMA
let mutable isShortLessThenLong : bool option = None

// --------------------- Открытые свойства ---------------------

/// <summary>Тип свечей, используемый стратегией.</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>Период длинной SMA.</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>Период короткой SMA.</summary>
member this.Short
	with get () = shortParam.Value
	and set value = shortParam.Value <- value

/// <summary>Значение take profit.</summary>
member this.TakeValue
	with get () = takeValueParam.Value
	and set value = takeValueParam.Value <- value

/// <summary>Значение stop loss.</summary>
member this.StopValue
	with get () = stopValueParam.Value
	and set value = stopValueParam.Value <- value
```

При использовании класса [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) автоматически используется подход сохранения и восстановления настроек.

2. При создании индикаторов и подписки на маркет-данные необходимо связать их, чтобы поступающие данные из подписки могли обновлять значения индикаторов:

```fsharp
// ---------- Создать индикаторы ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ Подписаться на поток свечей и связать индикаторы ------
let subscription = this.SubscribeCandles(this.CandleType)

// Связать индикаторы с подпиской и назначить функцию обработки
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. При работе с графиком необходимо учитывать, что в случае запуска стратегии [вне Дизайнера](../../../live_execution/running_strategies_outside_of_designer.md) объект графика может быть отсутствовать.

```fsharp
let area = this.CreateChartArea()

	// area может быть null, если GUI отсутствует (например, Runner или консольное приложение)
if not (isNull area) then
	// Нарисовать свечи
	this.DrawCandles(area, subscription) |> ignore

	// Нарисовать индикаторы
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// Нарисовать собственные сделки
	this.DrawOwnTrades(area) |> ignore
```

4. Запустить защиту позиций через [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), если этого требует логика стратегии:

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
	// Записать информацию о свече в лог
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

	// Пропустить, если свеча не завершена
	if candle.State <> CandleStates.Finished then
		()
	else
		// Определить, меньше ли короткая SMA длинной SMA
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// Первый раз: просто запомнить текущее соотношение
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// Произошло пересечение
		// Если short < long, это означает продажу, иначе покупку
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// Рассчитать объём для открытия новой позиции или разворота
			// If there is no position, use Volume; otherwise, double
			// минимум из абсолютного размера позиции и Volume
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// Получить шаг цены для лимитной цены
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// Установить цену лимитной заявки немного выше/ниже текущей цены закрытия
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // не должно происходить

			// Отправить лимитную заявку
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// Обновить флаг отслеживания
			isShortLessThenLong <- Some shortLess
		| _ ->
			// Ничего не делать, если пересечения нет
			()
```
