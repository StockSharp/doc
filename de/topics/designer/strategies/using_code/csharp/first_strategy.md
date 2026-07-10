# Beispiel einer C#-Strategie

Das Erstellen einer Strategie aus Quellcode wird anhand des Beispiels einer SMA-Strategie gezeigt, ähnlich wie im Beispiel der aus Würfeln zusammengesetzten SMA-Strategie im Abschnitt [Algorithmus aus Würfeln erstellen](../../using_visual_designer/first_strategy.md).

Dieser Abschnitt beschreibt keine C#-Sprachkonstrukte oder die Klasse [Strategy](../../../../api/strategies.md), die als Basis für die Erstellung von Strategien verwendet wird, sondern erwähnt spezielle Aspekte der Arbeit mit Code im **Designer**.

> [!TIP]
> Strategien, die im **Designer** erstellt wurden, sind durch die gemeinsame Basisklasse [Strategy](../../../../api/strategies.md) mit Strategien aus der [API](../../../../api.md) kompatibel. Dadurch ist das Ausführen solcher Strategien außerhalb des **Designer** deutlich einfacher als das Ausführen von [Schemata](../../../live_execution/running_strategies_outside_of_designer.md).

1. Strategieparameter werden mit einem speziellen Ansatz erstellt:

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

Bei Verwendung der Klasse [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1) wird automatisch der Ansatz zum Speichern und Wiederherstellen von Einstellungen verwendet.

2. Beim Erstellen von Indikatoren und Abonnieren von Marktdaten müssen Sie diese binden, damit eingehende Daten aus dem Abonnement die Indikatorwerte aktualisieren können:

```cs
// ---------- Indikatoren erstellen -----------

var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

// ----------------------------------------

// --- Kerzensatz und Indikatoren binden ----

var subscription = SubscribeCandles(CandleType);

subscription
	// Indikatoren an die Kerzen binden
	.Bind(longSma, shortSma, OnProcess)
	// Verarbeitung starten
	.Start();
```

3. Bei der Arbeit mit Charts ist es wichtig zu berücksichtigen, dass das Chart-Objekt fehlen kann, wenn die Strategie [außerhalb des Designer](../../../live_execution/running_strategies_outside_of_designer.md) ausgeführt wird.

```cs
var area = CreateChartArea();

// area kann null sein, wenn keine GUI vorhanden ist (Strategie läuft in Runner oder in eigener Konsolen-App).
if (area != null)
{
	DrawCandles(area, subscription);

	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	DrawOwnTrades(area);
}
```

4. Starten Sie bei Bedarf den Positionsschutz über [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)), wenn die Strategielogik dies erfordert:

```cs
// Schutz über Take Profit und/oder Stop Loss starten
StartProtection(TakeValue, StopValue);
```

5. Die Strategielogik selbst ist in der Methode OnProcess implementiert. Die Methode wird durch das in Schritt 1 erstellte Abonnement aufgerufen:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// Falls wir nicht nur abgeschlossene Kerzen abonniert haben
	if (candle.State != CandleStates.Finished)
		return;

	// Neue Werte für short und long berechnen
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong)
	{
		// Kreuzung ist aufgetreten

		// Wenn short kleiner als long ist, verkaufen, andernfalls kaufen
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// Größe für offene Position oder Umkehr berechnen
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		var priceStep = GetSecurity().PriceStep ?? 1;

		// Orderpreis als Schlusskurs + Offset berechnen
		var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		// Aktuelle Werte für short und long speichern
		_isShortLessThenLong = isShortLessThenLong;
	}
}
```
