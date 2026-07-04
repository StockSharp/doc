# Working Securities

## Beschreibung

Die Methode `GetWorkingSecurities()` in der Basisklasse `Strategy` wird verwendet, um eine Liste der Instrumente und Datentypen zu erhalten, die die Strategie in ihrer Arbeit verwendet. Diese Methode spielt bei der Arbeit mit dem [Designer](../../designer.md) eine wichtige Rolle.

## Zweck

Der Hauptzweck der Methode besteht darin, dem Designer Informationen darüber bereitzustellen, welche Instrumente und Datentypen für die Arbeit der Strategie erforderlich sind. Dadurch kann der Designer:

1. Vor dem Start des Tests prüfen, ob die erforderlichen historischen Daten im Speicher vorhanden sind.
2. Die erforderlichen Daten automatisch laden, wenn sie verfügbar sind.
3. Abonnements beim Start der Strategie korrekt einrichten.

## Implementierung

In der Basisklasse `Strategy` gibt die Methode eine leere Sammlung zurück. Für die korrekte Arbeit mit dem Designer wird empfohlen, sie in Ihrer Strategie zu überschreiben:

```cs
public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
{
	// Liste der von der Strategie verwendeten Paare (Instrument, Datentyp) zurückgeben
	return new[] 
	{ 
		(Security, CandleType),
		// Weitere Instrument-Datentyp-Paare, wenn die Strategie mehrere verwendet
	};
}
```

## Bedeutung des Überschreibens der Methode

Wenn die Methode `GetWorkingSecurities()` in Ihrer Strategie nicht überschrieben wird:

- Der Designer kann die erforderlichen Daten nicht automatisch prüfen.
- Wenn die erforderlichen historischen Daten im Speicher fehlen, gibt der Designer keine Warnungen aus.
- Die Strategie kann für Tests gestartet werden, aber es werden keine Ergebnisse angezeigt.
- Der Benutzer erhält keine Informationen über den Grund für das Fehlen von Ergebnissen.

## Verwendungsbeispiel

```cs
public class MySmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
	
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}
	
	public MySmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
	}
	
	// Methode für die korrekte Arbeit mit dem Designer überschreiben
	public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
	{
		return new[] { (Security, CandleType) };
	}
	
	// Restlicher Strategiecode...
}
```

## Fazit

Obwohl die Methode `GetWorkingSecurities()` für die Implementierung der Grundfunktionalität einer Strategie nicht zwingend erforderlich ist, wird ihr Überschreiben für die korrekte Arbeit mit dem StockSharp Designer dringend empfohlen. Dies hilft, Situationen zu vermeiden, in denen eine Strategie zum Testen gestartet wird, aber aufgrund fehlender historischer Daten keine Ergebnisse anzeigt.
