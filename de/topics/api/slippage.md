# Slippage-Messung

[S#](../api.md) berechnet Slippage über den [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). Slippage ist die Differenz zwischen dem erwarteten Ausführungspreis einer Order und dem tatsächlichen Trade-Preis.

## ISlippageManager-Interface

Das Interface [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) definiert den Basisvertrag:

- **Slippage** - insgesamt akkumulierte Slippage (decimal).
- **Reset()** - setzt den Zustand des Managers zurück.
- **ProcessMessage(Message)** - verarbeitet eine Nachricht; gibt die Slippage für die angegebene Ausführung oder `null` zurück.

## Funktionsweise

Der Slippage-Manager arbeitet in drei Stufen:

### 1. Aktualisieren von Marktpreisen

Wenn eine [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) oder [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) empfangen wird, speichert der Manager die besten Bid- und Ask-Preise für jedes Instrument.

### 2. Speichern des geplanten Preises

Wenn eine [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) empfangen wird, speichert der Manager den "geplanten Preis" - den besten Marktpreis zum Zeitpunkt der Orderregistrierung:

- Für einen Kauf (`Buy`) wird das beste Ask verwendet.
- Für einen Verkauf (`Sell`) wird das beste Bid verwendet.

### 3. Berechnen der Slippage

Wenn eine [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) mit einem Trade empfangen wird, berechnet der Manager die Slippage:

- Für einen Kauf: `(TradePrice - PlannedPrice) * TradeVolume`
- Für einen Verkauf: `(PlannedPrice - TradePrice) * TradeVolume`

Ein positiver Wert bedeutet ungünstige Slippage (Preis schlechter als erwartet), während ein negativer Wert günstige Slippage bedeutet (Preis besser als erwartet).

## Zustand: ISlippageManagerState

Das Interface [ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) speichert den internen Zustand des Managers:

- Beste Bid-/Ask-Preise für jedes Instrument ([SecurityId](xref:StockSharp.Messages.SecurityId)).
- Geplante Preise und Richtungen für jede Transaktion (`TransactionId`).
- Insgesamt akkumulierte Slippage.

Die Standardimplementierung ist [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState).

## Einstellungen

| Eigenschaft | Standard | Beschreibung |
|-------------|:--------:|--------------|
| `CalculateNegative` | `true` | Günstige Slippage berücksichtigen. Wenn `false`, werden negative Werte durch null ersetzt. |

## Integration über Adapter

Die Klasse [SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) kapselt einen inneren Adapter und berechnet automatisch die Slippage für alle Trades.

## Integration mit Strategy

Die Strategie ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) stellt die Eigenschaft `Slippage` zur Verfolgung der gesamten Slippage bereit.

## Verwendungsbeispiel

```cs
// Manager mit Zustandsspeicher erstellen
var manager = new SlippageManager(new SlippageManagerState());

// Nur ungünstige Slippage berücksichtigen
manager.CalculateNegative = false;

// Marktdaten verarbeiten (beste Preise aktualisieren)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Orderregistrierung verarbeiten (geplanten Preis speichern)
manager.ProcessMessage(orderRegisterMsg);

// Trade verarbeiten (Slippage berechnen)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Slippage: {slippage.Value}");
}

// Insgesamt akkumulierte Slippage
Console.WriteLine($"Total slippage: {manager.Slippage}");
```

## Zustand zurücksetzen

Die Methode `Reset()` löscht den internen Zustand vollständig: beste Preise, geplante Preise und akkumulierte Slippage:

```cs
manager.Reset();
```
