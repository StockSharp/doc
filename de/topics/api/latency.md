# Latenzmessung

[S#](../api.md) misst die Latenz der Orderregistrierung und Orderstornierung über den [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager). Der Manager bestimmt, wie viel Zeit zwischen dem Senden einer Order und dem Empfang der Bestätigung von der Börse vergeht.

## Interface ILatencyManager

Das Interface [ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) definiert den Basiskontrakt:

- **LatencyRegistration** - gesamte Registrierungslatenz über alle Orders (TimeSpan).
- **LatencyCancellation** - gesamte Stornierungslatenz über alle Orders (TimeSpan).
- **Reset()** - setzt den Zustand des Managers zurück.
- **ProcessMessage(Message)** - verarbeitet eine Nachricht; gibt die Latenz für die angegebene Operation oder `null` zurück.

## Funktionsweise

Der Latenzmanager arbeitet nach dem Request-Response-Prinzip:

### 1. Orderregistrierung

Wenn eine [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) empfangen wird, speichert der Manager das Paar (`TransactionId`, `LocalTime`) - den Zeitpunkt, zu dem die Order gesendet wurde.

### 2. Orderstornierung

Wenn eine [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) empfangen wird, speichert der Manager das Paar (`TransactionId`, `LocalTime`) - den Zeitpunkt, zu dem die Stornierung gesendet wurde.

### 3. Orderersetzung

Wenn eine [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) empfangen wird, registriert der Manager gleichzeitig sowohl eine Stornierung (der alten Order) als auch eine Registrierung (der neuen Order).

### 4. Bestätigung

Wenn eine [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) mit Orderinformationen empfangen wird (nicht im Zustand `Pending` und nicht `Failed`), berechnet der Manager die Latenz:

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

Das Ergebnis wird je nach Operationstyp zu `LatencyRegistration` oder `LatencyCancellation` addiert.

## Zustand: ILatencyManagerState

Das Interface [ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) speichert den internen Zustand des Managers:

- Ausstehende Registrierungen: `AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- Ausstehende Stornierungen: `AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- Akkumulierte Latenzen: `LatencyRegistration`, `LatencyCancellation`
- Methoden zum Hinzufügen: `AddLatencyRegistration(TimeSpan)`, `AddLatencyCancellation(TimeSpan)`

Die Standardimplementierung ist [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState).

## Fehlerbehandlung

Wenn eine Order fehlschlägt (`OrderState == Failed`), wird die Latenz nicht gezählt - der Eintrag wird lediglich aus dem Zustandsspeicher entfernt.

## Integration über Adapter

Die Klasse [LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) umschließt einen inneren Adapter und misst automatisch die Latenz für alle Orderoperationen.

## Integration mit Strategy

Die Strategie ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) stellt die Eigenschaft `Latency` zur Verfolgung der Latenz bereit.

## Verwendungsbeispiel

```cs
// Manager mit Zustandsspeicher erstellen
var manager = new LatencyManager(new LatencyManagerState());

// Orderregistrierung verarbeiten (Sendezeit speichern)
manager.ProcessMessage(orderRegisterMsg);

// Bestätigung verarbeiten (Latenz berechnen)
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Latenz: {latency.Value.TotalMilliseconds} ms");
}

// Gesamtlatenzen
Console.WriteLine($"Registrierungslatenz: {manager.LatencyRegistration.TotalMilliseconds} ms");
Console.WriteLine($"Stornierungslatenz: {manager.LatencyCancellation.TotalMilliseconds} ms");
```

## Zustand zurücksetzen

Die Methode `Reset()` löscht alle ausstehenden Einträge und setzt akkumulierte Latenzen auf null zurück:

```cs
manager.Reset();
```

