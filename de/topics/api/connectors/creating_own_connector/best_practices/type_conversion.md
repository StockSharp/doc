# Typkonvertierung

Die Typkonvertierungskomponente spielt eine wichtige Rolle bei der Sicherstellung der Kompatibilität zwischen den in StockSharp verwendeten Datentypen und den für eine bestimmte Börse spezifischen Formaten.

## Hauptfunktionen

1. Konvertierung von StockSharp-Typen (z. B. [Sides](xref:StockSharp.Messages.Sides), [OrderTypes](xref:StockSharp.Messages.OrderTypes), [TimeInForce](xref:StockSharp.Messages.TimeInForce)) in die von der Börse verwendeten String-Darstellungen.
2. Umgekehrte Konvertierung von von der Börse empfangenen Daten in StockSharp-Typen.
3. Konvertierung von Instrumentenkennungen zwischen den Formaten von StockSharp und der Börse.
4. Konvertierung von Zeitformaten und Zeitrahmen.

## Implementierungsbeispiel

Nachfolgend finden Sie ein Beispiel für eine Klasse mit Erweiterungsmethoden zur Typkonvertierung:

```cs
static class Extensions
{
	// Converting StockSharp order side to exchange string representation
	public static string ToNative(this Sides side)
	{
		return side switch
		{
			Sides.Buy => "buy",
			Sides.Sell => "sell",
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};
	}

	// Converting exchange order side string representation to StockSharp type
	public static Sides ToSide(this string side)
		=> side?.ToLowerInvariant() switch
		{
			"buy" or "bid" => Sides.Buy,
			"sell" or "ask" or "offer" => Sides.Sell,
			_ => throw new ArgumentOutOfRangeException(nameof(side), side, LocalizedStrings.InvalidValue),
		};

	// Converting StockSharp order type to exchange string representation
	public static string ToNative(this OrderTypes? type)
	{
		return type switch
		{
			null => null,
			OrderTypes.Limit => "limit",
			OrderTypes.Market => "market",
			OrderTypes.Conditional => "stop",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};
	}

	// Converting exchange order type string representation to StockSharp type
	public static OrderTypes ToOrderType(this string type)
		=> type?.ToLowerInvariant() switch
		{
			"limit" => OrderTypes.Limit,
			"market" => OrderTypes.Market,
			"stop" or "stop limit" => OrderTypes.Conditional,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, LocalizedStrings.InvalidValue),
		};

	// Weitere Konvertierungsmethoden...

	// Dictionary for mapping StockSharp timeframes to exchange string representations
	public static readonly PairSet<TimeSpan, string> TimeFrames = new()
	{
		{ TimeSpan.FromMinutes(1), "ONE_MINUTE" },
		{ TimeSpan.FromMinutes(5), "FIVE_MINUTE" },
		// Weitere Timeframes...
	};

	// Converting StockSharp timeframe to exchange string representation
	public static string ToNative(this TimeSpan timeFrame)
		=> TimeFrames.TryGetValue(timeFrame) ?? throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, LocalizedStrings.InvalidValue);

	// Stringdarstellung des Börsen-Timeframes in TimeSpan konvertieren
	public static TimeSpan ToTimeFrame(this string name)
		=> TimeFrames.TryGetKey2(name) ?? throw new ArgumentOutOfRangeException(nameof(name), name, LocalizedStrings.InvalidValue);
}
```

## Empfehlungen

- Verwenden Sie Erweiterungsmethoden für die bequeme Nutzung von Konvertierungsfunktionen.
- Behandeln Sie alle möglichen Aufzählungswerte, einschließlich `null` und unbekannter Werte.
- Verwenden Sie `switch`-Ausdrücke (C# 8.0+) für saubereren und besser lesbaren Code.
- Fügen Sie Prüfungen für ungültige Werte hinzu und werfen Sie Ausnahmen mit klaren Fehlermeldungen.
- Erwägen Sie die Verwendung von Dictionaries für die Zuordnung von Werten, insbesondere bei komplexen oder häufig wechselnden Zuordnungen (z. B. für Zeitrahmen).

Die richtige Implementierung der Typkonvertierung vereinfacht die Arbeit mit Daten in anderen Teilen des Connectors erheblich und verringert die Wahrscheinlichkeit von Fehlern im Zusammenhang mit Formatabweichungen.
