# Ejemplo de estrategia F#

La creación de una estrategia desde código fuente se demostrará con el ejemplo de la estrategia SMA, similar al ejemplo de estrategia SMA ensamblada desde cubos en la sección [Creación de un algoritmo a partir de cubos](../../using_visual_designer/first_strategy.md).

Esta sección no describirá las construcciones del lenguaje F# (ni [Strategy](../../../../api/strategies.md), que se usa como base para crear estrategias), pero mencionará características específicas para trabajar con código en **Designer**.

> [!TIP]
> Las estrategias creadas en **Designer** son compatibles con las estrategias creadas en la [API](../../../../api.md) gracias al uso de la clase base común [Strategy](../../../../api/strategies.md). Esto facilita mucho más la ejecución de dichas estrategias fuera de **Designer** que la ejecución de [esquemas](../../../live_execution/running_strategies_outside_of_designer.md).

1. Los parámetros de la estrategia se crean usando un enfoque especial:

```fsharp
// --- Parámetros de estrategia: CandleType, Long, Short, TakeValue, StopValue ---

// Parámetro para el tipo de vela
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

// Parámetro para la SMA larga
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// Parámetro para la SMA corta
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// Parámetro para take profit
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// Parámetro para stop loss
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// Bandera para seguir el cruce de SMA
let mutable isShortLessThenLong : bool option = None

// --------------------- Propiedades públicas ---------------------

/// <summary>Tipo de vela usado por la estrategia.</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>Período para el indicador SMA largo.</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>Período para el indicador SMA corto.</summary>
member this.Short
	with get () = shortParam.Value
	and set value = shortParam.Value <- value

/// <summary>Valor de take profit.</summary>
member this.TakeValue
	with get () = takeValueParam.Value
	and set value = takeValueParam.Value <- value

/// <summary>Valor de stop loss.</summary>
member this.StopValue
	with get () = stopValueParam.Value
	and set value = stopValueParam.Value <- value
```

Al usar la clase [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1), se aplica automáticamente el enfoque de guardar y restaurar configuración.

2. Al crear indicadores y suscribirse a datos de mercado, debe vincularlos para que los datos entrantes de la suscripción puedan actualizar los valores de los indicadores:

```fsharp
// ---------- Crear indicadores ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ Suscribirse al flujo de velas y vincular indicadores ------
let subscription = this.SubscribeCandles(this.CandleType)

// Vincular nuestros indicadores a la suscripción y asignar la función de procesamiento
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. Al trabajar con gráficos, es importante tener en cuenta que al ejecutar la estrategia [fuera de Designer](../../../live_execution/running_strategies_outside_of_designer.md), el objeto de gráfico puede estar ausente.

```fsharp
// ------------- Configurar gráfico -------------
let area = this.CreateChartArea()

// area puede ser null si no hay GUI (por ejemplo, Runner o app de consola)
if not (isNull area) then
	// Dibujar velas
	this.DrawCandles(area, subscription) |> ignore

	// Dibujar indicadores
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// Dibujar operaciones propias
	this.DrawOwnTrades(area) |> ignore
```

4. Inicie la protección de posición mediante [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) si la lógica de la estrategia lo requiere:

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. La propia lógica de la estrategia, implementada en el método OnProcess. El método es llamado por la suscripción creada en el paso 1:

```fsharp
member private this.OnProcess
	(
		candle: ICandleMessage,
		longValue: decimal,
		shortValue: decimal
	) =
	// Registrar información de la vela
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

	// Omitir si la vela no está finalizada
	if candle.State <> CandleStates.Finished then
		()
	else
		// Determinar si la SMA corta es menor que la SMA larga
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// Primera vez: solo recordar la relación actual
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// Se produjo un cruce
			// Si short < long, significa vender; en caso contrario, comprar
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// Calcular volumen para abrir una nueva posición o revertir
			// Si no hay posición, usar Volume; en caso contrario, duplicar
			// el mínimo entre el tamaño absoluto de la posición y Volume
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// Obtener paso de precio para el precio límite
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// Establecer el precio de la orden límite ligeramente por encima/debajo del cierre actual
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // no debería ocurrir

			// Enviar orden limitada
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// Actualizar la bandera de seguimiento
			isShortLessThenLong <- Some shortLess
		| _ ->
			// No hacer nada si no hay cruce
			()
```
