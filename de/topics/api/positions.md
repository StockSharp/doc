# Positionsverwaltung

StockSharp stellt ein flexibles Positionsverwaltungssystem bereit, mit dem Sie den aktuellen Positionszustand verfolgen, Positionen auf Basis von Orders oder Trades berechnen und eine Lebenszyklus-Historie (Eröffnung, Schließung, Umkehrungen) führen können.

## PositionManager

Die Klasse [PositionManager](xref:StockSharp.Algo.Positions.PositionManager) implementiert das Interface [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) und dient als primäre Komponente zur Berechnung aktueller Positionen auf Basis eingehender Nachrichten.

### Manager erstellen

Der Konstruktor akzeptiert zwei Parameter:

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` - die Position wird auf Basis von Änderungen des Order-Saldos berechnet. Geeignet, wenn das Handelssystem Orderzustandsaktualisierungen, aber keine einzelnen Trades empfängt.
- `byOrders = false` - die Position wird auf Basis von Trade-Volumina berechnet (empfohlener Modus). Bietet eine genauere Abrechnung ausgeführter Operationen.

### Nachrichten verarbeiten

Die Methode `ProcessMessage` akzeptiert eine eingehende Nachricht ([Message](xref:StockSharp.Messages.Message)) und gibt eine [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) zurück, wenn sich die Position ändert, oder `null`, wenn sich die Position nicht geändert hat:

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"Position: {posChange.CurrentValue}");
}
```

## IPositionManagerState

Das Interface [IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) beschreibt den internen Zustand des Positionsmanagers. Die Implementierung [PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) speichert Informationen zu aktuellen Orders und Positionen.

### Hauptmethoden

| Methode | Beschreibung |
|---------|--------------|
| `AddOrGetOrder` | Registriert eine neue Order oder gibt eine bestehende anhand von `transactionId` zurück |
| `TryGetOrder` | Ruft Orderparameter ab (Instrument, Portfolio, Richtung, Saldo) |
| `UpdateOrderBalance` | Aktualisiert den aktuellen Order-Saldo nach Teilausführung |
| `RemoveOrder` | Entfernt eine abgeschlossene Order aus dem Tracking |
| `UpdatePosition` | Aktualisiert die Position nach Instrument und Portfolio und gibt den neuen Wert zurück |
| `Clear` | Setzt den gesamten Managerzustand zurück |

### Beispiel für die Arbeit mit State

```cs
var state = new PositionManagerState();

// Order registrieren
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// Nach Teilausführung aktualisieren
state.UpdateOrderBalance(12345, newBalance: 60);

// Position direkt aktualisieren
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"Current position: {newPosition}");

// Leeren
state.Clear();
```

## PositionLifecycleTracker

Die Klasse [PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) verfolgt den vollständigen Lebenszyklus von Positionen - von der Eröffnung bis zur Schließung (Round-Trip). Dies ist nützlich für die Analyse einzelner Trades, die Gewinnberechnung pro Position und die Erstellung von Berichten.

### Hauptmerkmale

- **History**: Die Eigenschaft `History` (`IReadOnlyList<ReportPosition>`) enthält alle abgeschlossenen Round-Trip-Positionen.
- **Ereignis `RoundTripClosed`**: Wird ausgelöst, wenn eine Position geschlossen wird (Wert erreicht null) oder umgekehrt wird (Positionsvorzeichen ändert sich).
- **Methode `ProcessPosition`**: Akzeptiert ein Objekt [Position](xref:StockSharp.BusinessEntities.Position) und aktualisiert den internen Zustand.

### Erkannte Zustände

| Zustand | Beschreibung |
|---------|--------------|
| Opening | Position wechselt von null zu einem Wert ungleich null |
| Closing | Positionswert erreicht null |
| Reversal | Positionsvorzeichen ändert sich (z. B. von Long zu Short) |

### Verwendungsbeispiel

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"Round-trip completed:");
    Console.WriteLine($"  Opened: {report.OpenTime}");
    Console.WriteLine($"  Closed: {report.CloseTime}");
};

// Positionenaktualisierungen verarbeiten
tracker.ProcessPosition(position);

// Historie anzeigen
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## PositionMessageAdapter

Die Klasse [PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) ist ein Wrapper um einen Nachrichtenadapter, der Positionen automatisch aus dem Nachrichtenstrom berechnet. Sie wird innerhalb der internen Connector-Infrastruktur verwendet.

### Funktionsweise

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

Der Adapter fängt Orderausführungs- und Trade-Nachrichten ab, ruft `PositionManager.ProcessMessage` auf und erzeugt entsprechende `PositionChangeMessage`-Instanzen für Upstream-Handler.

## Positionen in Strategien

In der Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) wird über die Eigenschaft `Position` auf die aktuelle Position zugegriffen:

```cs
// Aktuelle Position für das primäre Instrument
decimal currentPosition = Position;

// Position schließen
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// Oder über eine integrierte Methode
ClosePosition();
```

Weitere Details zu Handelsoperationen in Strategien finden Sie im Abschnitt [Handelsoperationen](strategies/trading_operations.md).

## Siehe auch

- [Handelsoperationen](strategies/trading_operations.md)
- [Positionsschutz](strategies/take_profit_and_stop_loss.md)
- [Verwaltung der Zielposition](strategies/target_position_management.md)
- [Strategieberichte](strategies/reporting.md)
