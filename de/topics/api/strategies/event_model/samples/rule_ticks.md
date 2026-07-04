# Regeln für Tick-Trades

## Überblick

`SimpleTradeRulesStrategy` ist eine Strategie, die die Verwendung kombinierter Regeln zur Analyse von Trade-Preisen in StockSharp demonstriert. Sie abonniert Trades und erstellt eine Regel, die unter bestimmten Preisbedingungen auslöst.

## Hauptkomponenten

```cs
// Hauptkomponenten
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## OnStarted-Methode

Wird aufgerufen, wenn die Strategie startet:

- Erstellt ein Abonnement für Ticks
- Erstellt eine kombinierte Regel zur Analyse von Trade-Preisen

```cs
// OnStarted-Methode
protected override void OnStarted2(DateTime time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // Diese Regel nur einmal aufrufen
	.Apply(this);

	// Anfrage zum Abonnieren von Marktdaten senden.
	Subscribe(sub);

	base.OnStarted2(time);
}
```

## Logik

- Beim Empfang des ersten Ticks wird eine kombinierte Regel erstellt.
- Sie basiert auf dem Preis des empfangenen Ticks: Erstellt wird eine Regel, die auslöst, wenn sich der Preis um +/- 2 ändert.
- Die Regel löst aus, wenn der Preis des letzten Trades größer als aktueller Preis + 2 oder kleiner als aktueller Preis - 2 wird.
- Beim Auslösen der Regel werden Informationen zum Tick ins Log geschrieben.
- Die äußere Regel löst nur einmal aus (`Once()`).

## Funktionen

- Demonstriert das Erstellen kombinierter Regeln mit `Or()`.
- Verwendet `WhenLastTradePriceMore` und `WhenLastTradePriceLess` für die Preisanalyse.
- Zeigt ein Beispiel für das Protokollieren von Informationen zu Trades mit der Methode `LogInfo`.
- Veranschaulicht die Verwendung von `Once()`, um das Auslösen von Regeln zu begrenzen.
- Übergibt den Tick-Parameter an den Ereignishandler (anders als im Dokumentationsbeispiel).
