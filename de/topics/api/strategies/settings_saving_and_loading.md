# Speichern und Laden von Einstellungen

In StockSharp ist der Mechanismus zum Speichern und Laden von Strategieeinstellungen über die Methoden [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) und [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) implementiert.

## Automatische Parameterverarbeitung

In den meisten Fällen ist es **nicht erforderlich**, die Methoden `Save` und `Load` zu überschreiben, da die Basisklasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) Strategieparameter, die mit [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) erstellt wurden, automatisch speichert und lädt.

Der empfohlene Ansatz besteht darin, den im Abschnitt [Strategieparameter](parameters.md) ausführlich beschriebenen Mechanismus für Strategieparameter zu verwenden. Bei diesem Ansatz werden alle Parameter automatisch gespeichert und geladen:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Long SMA length", string.Empty, "Base settings");
	}
}
```

## Überschreiben für Sonderfälle

Das Überschreiben der Methoden `Save` und `Load` ist nur in Sonderfällen erforderlich, wenn Daten gespeichert oder geladen werden müssen, die nicht zum Standardsatz der Strategieparameter gehören. Beispiele sind das Speichern eines internen Zustands, nicht standardmäßiger Datenstrukturen oder zwischengespeicherter Werte.

Wenn Sie diese Methoden überschreiben, **müssen Sie die Methoden der Basisklasse aufrufen**:

```cs
public override void Save(SettingsStorage settings)
{
	// Zuerst Basismethode aufrufen, um Standardparameter zu speichern
	base.Save(settings);
	
	// Dann eigene Speicherlogik hinzufügen
	settings.SetValue("CustomState", _customState);
}
	
public override void Load(SettingsStorage settings)
{
	// Zuerst Basismethode aufrufen, um Standardparameter zu laden
	base.Load(settings);
	
	// Dann eigene Ladelogik hinzufügen
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
}
```

## Speichern in und Laden aus einer Datei

Um Einstellungen in einer Datei zu speichern oder aus einer Datei zu laden, können Sie die in StockSharp implementierte Serialisierung und Deserialisierung verwenden:

```cs
// Einstellungen in einer Datei speichern
var settingsStorage = new SettingsStorage();
strategy.Save(settingsStorage);
new JsonSerializer<SettingsStorage>().Serialize(settingsStorage, "strategy.json");

// Einstellungen aus einer Datei laden
var newStrategy = new SmaStrategy();
if (File.Exists("strategy.json"))
{
	var loadedSettings = new JsonSerializer<SettingsStorage>().Deserialize("strategy.json");
	newStrategy.Load(loadedSettings);
}
```

## Empfehlungen

1. Verwenden Sie nach Möglichkeit [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) für alle konfigurierbaren Strategieparameter.
2. Überschreiben Sie die Methoden `Save` und `Load` nur, wenn Sie nicht standardmäßige Daten speichern oder laden müssen.
3. Rufen Sie beim Überschreiben immer die Basismethoden `base.Save()` und `base.Load()` auf.
4. Verwenden Sie die Standard-Serialisierungstools von StockSharp, um Einstellungen in einer Datei zu speichern oder aus einer Datei zu laden.

## Siehe auch

[Strategieparameter](parameters.md)

