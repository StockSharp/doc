# Regeln für Orderbücher und Trades

## Überblick

`SimpleRulesStrategy` ist eine Strategie, die verschiedene Möglichkeiten zum Erstellen und Anwenden von Regeln in StockSharp demonstriert. Sie abonniert Trades und Orderbücher und erstellt anschließend verschiedene Regeln zur Verarbeitung der empfangenen Daten.

## Hauptkomponenten

```cs
// Hauptkomponenten
public class SimpleRulesStrategy : Strategy
{
}
```

## OnStarted-Methode

Wird aufgerufen, wenn die Strategie startet:

- Erstellt Abonnements für Trades und Orderbücher
- Demonstriert verschiedene Möglichkeiten zum Erstellen und Anwenden von Regeln

```cs
// OnStarted-Methode
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	//-----------------------Regel erstellen. Methode Nr. 1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived в„–1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//-----------------------Regel erstellen. Methode Nr. 2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived в„–2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//----------------------Regel innerhalb einer Regel-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived в„–3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		//----------------------keine Once-Regel-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"The rule WhenOrderBookReceived в„–4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// Anfragen zum Abonnieren von Marktdaten senden.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Logik

### Methode #1: Regel erstellen

- Erstellt eine Regel, die beim Empfang eines Orderbuchs auslöst.
- Protokolliert das beste Bid und Ask.
- Die Regel löst nur einmal aus (`Once()`).

### Methode #2: Regel erstellen

- Demonstriert eine alternative Möglichkeit zum Erstellen einer Regel.
- Funktional identisch mit Methode #1.

### Regel innerhalb einer Regel

- Erstellt eine Regel, die beim Empfang eines Orderbuchs auslöst.
- Innerhalb dieser Regel wird eine weitere Regel erstellt.
- Die äußere Regel löst einmal aus, die innere Regel bei jedem Empfang eines Orderbuchs.

## Funktionen

- Demonstriert verschiedene Möglichkeiten zum Erstellen und Anwenden von Regeln in StockSharp.
- Verwendet Abonnements für Trades und Orderbücher.
- Zeigt ein Beispiel für das Protokollieren von Informationen in einer Strategie mit der Methode `LogInfo`.
- Veranschaulicht die Verwendung von `Once()`, um das Auslösen von Regeln zu begrenzen.
