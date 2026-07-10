# Until-Regel

## Überblick

`SimpleRulesUntilStrategy` ist eine Strategie, die die Verwendung einer Regel mit Beendigungsbedingung (`Until`) in StockSharp demonstriert. Sie abonniert Trades und Orderbücher und erstellt anschließend eine Regel, die ausgeführt wird, bis eine bestimmte Bedingung erfüllt ist.

## Hauptkomponenten

```cs
// Hauptkomponenten
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## OnStarted-Methode

Wird aufgerufen, wenn die Strategie startet:

- Erstellt Abonnements für Ticks und Orderbücher
- Erstellt eine Regel, die beim Empfang von Orderbuchdaten ausgeführt wird, bis eine bestimmte Bedingung erfüllt ist

```cs
// OnStarted-Methode
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"Regel WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"Regel WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// Anfragen zum Abonnieren von Marktdaten senden.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Logik

- Beim Start erstellt die Strategie Abonnements für Ticks und Orderbücher.
- Es wird eine Regel erstellt, die jedes Mal auslöst, wenn Orderbuchdaten empfangen werden.
- Wenn die Regel auslöst:
  - Der Zähler `i` wird erhöht.
  - Informationen zu den besten Bid- und Ask-Preisen werden ins Log geschrieben.
  - Der aktuelle Wert des Zählers `i` wird ins Log geschrieben.
- Die Regel wird ausgeführt, bis der Wert des Zählers `i` 10 erreicht oder überschreitet.
- Nach Erfüllung der Bedingung beendet die Regel ihre Arbeit automatisch.

## Funktionen

- Demonstriert die Verwendung der Methode `Until()`, um die Regelausführung zu begrenzen.
- Verwendet Abonnements für Trades und Orderbücher.
- Zeigt ein Beispiel für das Protokollieren von Informationen zum Orderbuch und zum Zählerzustand mit der Methode `LogInfo`.
- Veranschaulicht, wie die Anzahl der Regelausführungen anhand einer bestimmten Bedingung begrenzt wird.
