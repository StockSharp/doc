# Strategie erstellen

Die in [Shell](../shell.md) enthaltenen Strategien liegen in `Shell.Avalonia/Shell.Core/Strategies/`. Eine eigene Strategie wird ebenfalls dort hinzugefügt und durchläuft drei Schritte: die Strategieklasse, einen Eintrag im Katalog und - falls nötig - ein eigenes Panel.

## Strategieklasse

Eine Strategie ist ein gewöhnlicher Nachfahre von `Strategy` aus StockSharp. Shell-spezifisches enthält sie nicht:

```cs
public class MyStrategy : Strategy
{
	private readonly StrategyParam<int> _length;

	public MyStrategy()
	{
		Name = "Meine Strategie";

		_length = Param(nameof(Length), 20)
			.SetGreaterThanZero()
			// Die Gruppe wird aus den Ressourcen genommen, damit der Parameter in dieselbe Gruppe wie die Standardparameter gelangt.
			.SetDisplay("Länge", "Länge des gleitenden Durchschnitts", LocalizedStrings.Parameters)
			.SetOptimize(10, 50, 5);
	}

	public int Length
	{
		get => _length.Value;
		set => _length.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Datenabonnements und Handelslogik
	}
}
```

Der Einstiegspunkt ist `OnStarted2(DateTime)` und nicht `OnStarted`: Zum Zeitpunkt seines Aufrufs hat die Strategie bereits das Instrument, das Portfolio und die Verbindung erhalten, deshalb kann man Daten direkt hier abonnieren.

Ein Parameter, der mit `Param(...)` und einem angegebenen `SetOptimize(...)` deklariert ist, erscheint automatisch auf der Registerkarte [Optimierung](user_interface/optimization.md): Die Liste der zu durchlaufenden Parameter und die Grenzen des Durchlaufs werden von der Strategie selbst gelesen und nicht separat festgelegt.

## Strategiekatalog

Damit die Strategie im Auswahlfenster erscheint, muss sie an zwei Stellen hinzugefügt werden.

Die erste ist die Fabrik in `BuiltInStrategies`, die eine Instanz anhand der Typkennung erzeugt:

```cs
public static Strategy TryCreate(string templateTypeId)
{
	...
	if (templateTypeId.Equals(MyTypeId, StringComparison.OrdinalIgnoreCase))
		return new MyStrategy();
	...
}
```

Die zweite ist die Beschreibung der Vorlage in `LegacyStrategyTemplates`, aus der der Name der Strategie in der Liste und die Menge der Parameter mit ihren Typen und Standardwerten genommen werden:

```cs
new StrategyTemplateDefinition(MyTypeId, "Meine Strategie", StrategyTemplateKind.Generic, true,
[
	.. CommonParameters(),
	new("Length", "Länge", StrategyParameterKind.Integer, "20", true, 1),
])
```

`CommonParameters()` gibt das Instrument, das Portfolio und das Volumen zurück — jede Strategie hat sie; der Rest sind ihre eigenen Parameter. Die Felder der Beschreibung folgen in dieser Reihenfolge: Schlüssel, Beschriftung, Typ, Standardwert, ob er erforderlich ist, Minimum und Maximum.

`StrategyTemplateKind` unterscheidet nur die Arten, für die die Shell ein eigenes Panel hat (`Sma`, `SmaIndex`, `StairsTrend`, `StairsCountertrend`, `Designer`); eine Strategie ohne eigenes Panel ist `Generic`.

Die Werte, die der Benutzer im Panel **Strategie** festlegt, werden in `StrategyMaterializer` auf die Instanz angewendet: Dort wird auch das Einlesen neuer Parameter ergänzt.

Die Einstellungen der Strategie werden von der Shell gespeichert und wiederhergestellt: Eigenen Code zum Speichern müssen Sie nicht schreiben.

## Eigenes Panel

Wenn das Standardpanel mit den Parametern ausreicht, muss nichts getan werden — Shell zeigt es von selbst an. Ein eigenes Panel ist nötig, wenn die Strategie über die Parameter hinaus etwas zu zeigen hat: Wie man es einrichtet und wie man es beobachtet, ist im Abschnitt [Strategiepanel erstellen](create_strategy_panel.md) beschrieben.

## Siehe auch

[Strategiepanel erstellen](create_strategy_panel.md)

[Strategien aus dem Designer ausführen](run_strategies_from_designer.md)
