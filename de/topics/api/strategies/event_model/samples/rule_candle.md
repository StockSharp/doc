# Regel für eine einzelne Kerze

## Überblick

`SimpleCandleRulesStrategy` ist eine Strategie, die die Verwendung von Regeln für Kerzen in StockSharp demonstriert. Sie verfolgt Kerzenvolumina und protokolliert Informationen, wenn bestimmte Bedingungen erfüllt sind.

## Hauptkomponenten

```cs
// Hauptkomponenten
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## OnStarted-Methode

Wird aufgerufen, wenn die Strategie startet:

- Initialisiert ein Abonnement für 5-Minuten-Kerzen
- Erstellt Regeln zur Verarbeitung von Kerzen

```cs
// OnStarted-Methode
protected override void OnStarted2(DateTime time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// Fertige Kerzen sind viel schneller als Kompression im On-the-fly-Modus
		// Kompression deaktivieren, um den Optimizer zu beschleunigen (!!! stellen Sie sicher, dass Sie Kerzen haben)

		//MarketData =
		//{
		//    BuildMode = MarketDataBuildModes.Build,
		//    BuildFrom = DataType.Ticks,
		//}
	};
	Subscribe(subscription);

	var i = 0;
	var diff = "10%".ToUnit();

	this.WhenCandlesStarted(subscription)
		.Do((candle) =>
		{
			i++;

			this
				.WhenTotalVolumeMore(candle, diff)
				.Do((candle1) =>
				{
	LogInfo($"Regel WhenCandlesStarted und WhenTotalVolumeMore Kerze={candle1}");
	LogInfo($"Regel WhenCandlesStarted und WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted2(time);
}
```

## Logik

- Die Strategie abonniert 5-Minuten-Kerzen.
- Wenn die Bildung jeder Kerze beginnt, wird eine Regel erstellt.
- Die Regel löst aus, wenn das Gesamtvolumen der Kerze 10 % überschreitet (über einen Prozentwert).
- Wenn die Regel ausgelöst wird, werden Informationen zur Kerze und zum Zähler ins Log geschrieben.
- Nach dem ersten Auslösen beendet die Regel dank der Methode `Once()` ihre Arbeit.

## Funktionen

- Demonstriert die Verwendung der Regeln `WhenCandlesStarted` und `WhenTotalVolumeMore`.
- Verwendet den Mechanismus für Kerze-Abonnements.
- Zeigt ein Beispiel für das Erstellen eines Prozentwerts über `"10%".ToUnit()`.
- Zeigt ein Beispiel für das Protokollieren von Informationen in einer Strategie mit der Methode `LogInfo`.
- Enthält auskommentierten Code zum Konfigurieren der Kerzenerstellung aus Ticks.
