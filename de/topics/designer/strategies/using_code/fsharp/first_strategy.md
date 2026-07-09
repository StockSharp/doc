# Beispiel einer F#-Strategie

Das Erstellen einer Strategie aus Quellcode wird anhand des Beispiels einer SMA-Strategie gezeigt, ähnlich wie im Beispiel der aus Würfeln zusammengesetzten SMA-Strategie im Abschnitt [Creating Algorithm from Cubes](../../using_visual_designer/first_strategy.md).

Dieser Abschnitt beschreibt keine F#-Sprachkonstrukte (oder [Strategy](../../../../api/strategies.md), die als Basis für die Erstellung von Strategien verwendet wird), sondern erwähnt spezielle Aspekte der Arbeit mit Code im **Designer**.

> [!TIP]
> Strategien, die im **Designer** erstellt wurden, sind durch die gemeinsame Basisklasse [Strategy](../../../../api/strategies.md) mit Strategien aus der [API](../../../../api.md) kompatibel. Dadurch ist das Ausführen solcher Strategien außerhalb des **Designer** deutlich einfacher als das Ausführen von [Schemata](../../../live_execution/running_strategies_outside_of_designer.md).

1. Strategieparameter werden mit einem speziellen Ansatz erstellt:

```fsharp
// --- Strategieparameter: CandleType, Long, Short, TakeValue, StopValue ---

// Parameter für den Kerzentyp
let candleTypeParam =
	this.Param<DataType>(nameof(this.CandleType), DataType.TimeFrame(TimeSpan.FromMinutes 1.0))
		.SetDisplay("Candle type", "Candle type for strategy calculation.", "General")

// Parameter für den langen SMA
let longParam =
	this.Param<int>(nameof(this.Long), 80)

// Parameter für den kurzen SMA
let shortParam =
	this.Param<int>(nameof(this.Short), 30)

// Parameter für Take Profit
let takeValueParam =
	this.Param<Unit>(nameof(this.TakeValue), Unit(0m, UnitTypes.Absolute))

// Parameter für Stop Loss
let stopValueParam =
	this.Param<Unit>(nameof(this.StopValue), Unit(2m, UnitTypes.Percent))

// Flag zur Verfolgung der SMA-Kreuzung
let mutable isShortLessThenLong : bool option = None

// --------------------- Öffentliche Eigenschaften ---------------------

/// <summary>Der Kerzentyp, der von der Strategie verwendet wird.</summary>
member this.CandleType
	with get () = candleTypeParam.Value
	and set value = candleTypeParam.Value <- value

/// <summary>Die Periode für den langen SMA-Indikator.</summary>
member this.Long
	with get () = longParam.Value
	and set value = longParam.Value <- value

/// <summary>Die Periode für den kurzen SMA-Indikator.</summary>
member this.Short
	with get () = shortParam.Value
	and set value = shortParam.Value <- value

/// <summary>Take-Profit-Wert.</summary>
member this.TakeValue
	with get () = takeValueParam.Value
	and set value = takeValueParam.Value <- value

/// <summary>Stop-Loss-Wert.</summary>
member this.StopValue
	with get () = stopValueParam.Value
	and set value = stopValueParam.Value <- value
```

Bei Verwendung der Klasse [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) wird automatisch der Ansatz zum Speichern und Wiederherstellen von Einstellungen verwendet.

2. Beim Erstellen von Indikatoren und Abonnieren von Marktdaten müssen Sie diese binden, damit eingehende Daten aus dem Abonnement die Indikatorwerte aktualisieren können:

```fsharp
// ---------- Indikatoren erstellen ----------
let longSma = SMA()
longSma.Length <- this.Long

let shortSma = SMA()
shortSma.Length <- this.Short
// ---------------------------------------

// ------ Kerzenfluss abonnieren und Indikatoren binden ------
let subscription = this.SubscribeCandles(this.CandleType)

// Unsere Indikatoren an das Abonnement binden und die Verarbeitungsfunktion zuweisen
subscription
	.Bind(longSma, shortSma, fun candle longV shortV -> this.OnProcess(candle, longV, shortV))
	.Start() |> ignore
```

3. Bei der Arbeit mit Charts ist es wichtig zu berücksichtigen, dass das Chart-Objekt fehlen kann, wenn die Strategie [außerhalb des Designer](../../../live_execution/running_strategies_outside_of_designer.md) ausgeführt wird.

```fsharp
// ------------- Chart konfigurieren -------------
let area = this.CreateChartArea()

// area kann null sein, wenn keine GUI vorhanden ist (z. B. Runner oder Konsolen-App)
if not (isNull area) then
	// Kerzen zeichnen
	this.DrawCandles(area, subscription) |> ignore

	// Indikatoren zeichnen
	this.DrawIndicator(area, shortSma, System.Drawing.Color.Coral) |> ignore
	this.DrawIndicator(area, longSma) |> ignore

	// Eigene Trades zeichnen
	this.DrawOwnTrades(area) |> ignore
```

4. Starten Sie bei Bedarf den Positionsschutz über [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), wenn die Strategielogik dies erfordert:

```fsharp
this.StartProtection(this.TakeValue, this.StopValue)
```

5. Die Strategielogik selbst ist in der Methode OnProcess implementiert. Die Methode wird durch das in Schritt 1 erstellte Abonnement aufgerufen:

```fsharp
member private this.OnProcess
	(
		candle: ICandleMessage,
		longValue: decimal,
		shortValue: decimal
	) =
	// Kerzeninformationen protokollieren
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

	// Überspringen, wenn die Kerze nicht abgeschlossen ist
	if candle.State <> CandleStates.Finished then
		()
	else
		// Bestimmen, ob short SMA kleiner als long SMA ist
		let shortLess = shortValue < longValue

		match isShortLessThenLong with
		| None ->
			// Beim ersten Mal nur das aktuelle Verhältnis merken
			isShortLessThenLong <- Some shortLess
		| Some prevValue when prevValue <> shortLess ->
			// Eine Kreuzung ist aufgetreten
			// Wenn short < long ist, bedeutet das Sell, andernfalls Buy
			let direction =
				if shortLess then
					Sides.Sell
				else
					Sides.Buy

			// Volumen zum Öffnen einer neuen Position oder zur Umkehr berechnen
			// Wenn keine Position vorhanden ist, Volume verwenden, andernfalls
			// das Minimum aus absoluter Positionsgröße und Volume verdoppeln
			let vol =
				if this.Position = 0m then
					this.Volume
				else
					(abs this.Position |> min this.Volume) * 2m

			// Preisschritt für den Limitpreis abrufen
			let priceStep =
				let step = this.GetSecurity().PriceStep
				if step.HasValue then step.Value else 1m

			// Limit-Orderpreis etwas höher/niedriger als den aktuellen Schlusskurs setzen
			let limitPrice =
				match direction with
				| Sides.Buy  -> candle.ClosePrice + priceStep
				| Sides.Sell -> candle.ClosePrice - priceStep
				| _          -> candle.ClosePrice // sollte nicht auftreten

			// Limit-Order senden
			match direction with
			| Sides.Buy  -> this.BuyLimit(limitPrice, vol) |> ignore
			| Sides.Sell -> this.SellLimit(limitPrice, vol) |> ignore
			| _          -> ()

			// Tracking-Flag aktualisieren
			isShortLessThenLong <- Some shortLess
		| _ ->
			// Nichts tun, wenn keine Kreuzung vorliegt
			()
```
