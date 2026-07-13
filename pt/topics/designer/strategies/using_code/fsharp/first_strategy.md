# Exemplo de estratégia F#

A criação de uma estratégia a partir de código-fonte será demonstrada usando o exemplo da estratégia SMA, semelhante ao exemplo da estratégia SMA montada a partir de cubos na secção [Criar algoritmo a partir de cubos](../../using_visual_designer/first_strategy.md).

Esta secção não descreve construções da linguagem F# (nem a [Strategy](../../../../api/strategies.md), que é usada como base para criar estratégias), mas menciona funcionalidades específicas para trabalhar com código no **Designer**.

> [!TIP]
> As estratégias criadas no **Designer** são compatíveis com estratégias criadas na [API](../../../../api.md), devido à utilização da classe base comum [Strategy](../../../../api/strategies.md). Isto torna a execução dessas estratégias fora do **Designer** significativamente mais simples do que executar [esquemas](../../../live_execution/running_strategies_outside_of_designer.md).

1. Os parâmetros da estratégia são criados usando uma abordagem especial:

```fsharp
// --- Parâmetros da estratégia: CandleType, Long, Short, TakeValue, StopValue ---

// Parâmetro para o tipo de vela
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Tipo de vela", "Tipo de vela para cálculo da estratégia.", "Configurações gerais")

// Parâmetro para a SMA longa
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// Parâmetro para a SMA curta
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// Parâmetro para take profit
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// Parâmetro para stop loss
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// Indicador para acompanhar o cruzamento das SMA
let mutable isShortLessThenLong : bool option = None

// --------------------- Propriedades públicas ---------------------

/// <summary>O tipo de vela usado pela estratégia.</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>O período do indicador SMA longo.</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>O período do indicador SMA curto.</summary>
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

Ao usar a classe [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1), a abordagem de guardar e restaurar definições é aplicada automaticamente.

2. Ao criar indicadores e subscrever dados de mercado, é necessário ligá-los para que os dados recebidos da subscrição possam atualizar os valores dos indicadores:

```fsharp
// ---------- Criar indicadores ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ Subscrever o fluxo de velas e ligar indicadores ------
let subscription = this.SubscribeCandles(this.CandleType)

// Ligar os nossos indicadores à subscrição e atribuir a função de processamento
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. Ao trabalhar com gráficos, é importante considerar que, ao executar a estratégia [fora do Designer](../../../live_execution/running_strategies_outside_of_designer.md), o objeto do gráfico pode não existir.

```fsharp
// ------------- Configurar gráfico -------------
let area = this.CreateChartArea()

// area pode ser null caso não exista GUI (por exemplo, Runner ou aplicação de consola)
if not (isNull area) then
	// Desenhar velas
	this.DrawCandles(area, subscription) |> ignore

	// Desenhar indicadores
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// Desenhar negócios próprios
	this.DrawOwnTrades(area) |> ignore
```

4. Inicie a proteção da posição através de [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), se exigido pela lógica da estratégia:

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. A lógica da estratégia propriamente dita, implementada no método OnProcess. O método é chamado pela subscrição criada no passo 1:

```fsharp
member private this.OnProcess
	(
		candle: ICandleMessage,
		longValue: decimal,
		shortValue: decimal
	) =
	// Registar informações da vela
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

	// Ignorar se a vela não estiver finalizada
	if candle.State <> CandleStates.Finished then
		()
	else
		// Determinar se a SMA curta é menor do que a SMA longa
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// Primeira vez: apenas memorizar a relação atual
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// Ocorreu um cruzamento
			// Se short < long, isso significa Sell; caso contrário, Buy
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// Calcular o volume para abrir uma nova posição ou inverter
			// Se não existir posição, usar Volume; caso contrário, duplicar
			// o mínimo entre o tamanho absoluto da posição e Volume
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// Obter o passo de preço para o preço limite
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// Definir o preço da ordem limite ligeiramente acima/abaixo do preço de fecho atual
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // não deverá ocorrer

			// Enviar ordem limite
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// Atualizar o indicador de acompanhamento
			isShortLessThenLong <- Some shortLess
		| _ ->
			// Não fazer nada se não houver cruzamento
			()
```
