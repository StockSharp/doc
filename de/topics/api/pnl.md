# Gewinn- und Verlustverwaltung

[S#](../api.md) implementiert die Gewinn- und Verlustberechnung (PnL) über den [PnLManager](xref:StockSharp.Algo.PnL.PnLManager). Der Manager verarbeitet einen Nachrichtenstrom (Trades, Marktdaten) und berechnet realisierten und unrealisierten Gewinn.

## IPnLManager-Interface

Das Interface [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) definiert den Basisvertrag:

- **Realisierter Gewinn oder Verlust** - realisierter Gewinn/Verlust (decimal). Wird angesammelt, wenn Positionen geschlossen werden.
- **Nicht realisierter Gewinn oder Verlust** - unrealisierter Gewinn/Verlust (decimal). Wird anhand aktueller Marktpreise neu berechnet.
- **Reset()** - setzt den Zustand des Managers zurück.
- **UpdateSecurity(Level1ChangeMessage)** - aktualisiert Instrumentparameter (Preisschritt, Schrittpreis, Lot-Multiplikator).
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** - verarbeitet eine Nachricht; gibt [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) zurück, wenn eine Position geschlossen wird, andernfalls `null`.

## Architektur

Das PnL-System besitzt eine dreistufige Hierarchie:

```
PnLManager
  └── PortfolioPnLManager (nach Portfolioname)
        └── PnLQueue (nach SecurityId)
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) - oberste Ebene, verwaltet ein Dictionary von Portfolio-Managern.
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) - PnL-Manager für ein bestimmtes Portfolio, verwaltet Queues nach Instrument.
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) - FIFO-Queue zum Abgleichen von Trades für ein einzelnes Instrument.

### PnLQueue - Berechnungsqueue

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) ist für das Abgleichen eröffnender und schließender Trades verantwortlich:

- **PriceStep** - Preisschritt des Instruments.
- **StepPrice** - Wert eines Preisschritts (für Futures).
- **Hebel** - Hebel.
- **Losgrößenmultiplikator** - Lot-Multiplikator.

Der Gewinnmultiplikator wird mit folgender Formel berechnet:

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

Für reguläre Aktien (bei denen `StepPrice` nicht gesetzt ist) entspricht der Multiplikator `1 * Leverage * LotMultiplier`.

## PnLInfo - Ergebnis der Trade-Verarbeitung

Die Klasse [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) enthält das Ergebnis des Schließens einer Position:

- **Serverzeit** - Trade-Zeit.
- **Geschlossenes Volumen** - Volumen der geschlossenen Position.
- **PnL** - realisierter Gewinn aus diesem Trade.

Wenn die Position beispielsweise +2 war und ein Trade über -5 Kontrakte eingetroffen ist, dann gilt `ClosedVolume = 2` (2 Kontrakte aus der Position wurden geschlossen).

## Konfigurieren von Datenquellen

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) ermöglicht die Auswahl von Marktdatenquellen für die Berechnung des unrealisierten Gewinns:

| Eigenschaft | Standard | Beschreibung |
|-------------|:--------:|--------------|
| `UseTick` | `true` | Tick-Trades verwenden. |
| `UseOrderBook` | `false` | Orderbuch verwenden (bestes Bid/Ask). |
| `UseLevel1` | `false` | Level1-Daten verwenden. |
| `UseOrderLog` | `false` | Orderprotokoll verwenden. |
| `UseCandles` | `true` | Kerzen verwenden (Schlusskurs). |

## Integration über Adapter

Die Klasse [PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) kapselt einen inneren Adapter und verarbeitet automatisch alle Nachrichten für die PnL-Berechnung.

## Integration mit Strategy

Die Strategie ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) stellt Folgendes bereit:

- Eigenschaft `PnLManager` - die Manager-Instanz.
- Eigenschaft `PnL` - Gesamtgewinn (`RealizedPnL + UnrealizedPnL`).
- Ereignis `PnLChanged` - Benachrichtigung über Gewinnänderungen.
- Ereignis `PnLReceived2` - Benachrichtigung, wenn neue PnL-Daten empfangen werden.

## Verwendungsbeispiel

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// Nachrichten verarbeiten
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"Geschlossen: {info.ClosedVolume}, PnL: {info.PnL}");
}

// Gesamtgewinn/-verlust
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Realisierter PnL: {realizedPnL}");
Console.WriteLine($"Nicht realisierter PnL: {unrealizedPnL}");
Console.WriteLine($"Gesamt-PnL: {totalPnL}");
```

## Zustand zurücksetzen

Die Methode `Reset()` löscht alle Portfolio-Manager und Berechnungsqueues und setzt den realisierten PnL auf null zurück:

```cs
pnlManager.Reset();
```
