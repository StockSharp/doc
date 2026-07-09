# Positionsschutz

## Einführung

Diese Modifikation der SMA-Strategie implementiert einen Mechanismus zum Schutz offener Positionen mithilfe eines lokalen Schutzcontrollers. Dieser Ansatz ermöglicht flexibles Risikomanagement und automatisches Schließen von Positionen, wenn bestimmte Bedingungen erfüllt sind.

## Schlüsselkomponenten des Positionsschutzes

### Schutzcontroller

Die Strategie verwendet zwei Schlüsselobjekte für den Positionsschutz:

```cs
// Deklaration von Schutzcontrollern
private readonly ProtectiveController _protectiveController = new();
private IProtectivePositionController _posController;

// Dieser Code initialisiert den Haupt-Schutzcontroller und erstellt einen Platzhalter für
// einen spezifischen Positionscontroller. ProtectiveController verwaltet alle Positionen,
// während IProtectivePositionController für eine spezifische Position verantwortlich ist.
```

- `_protectiveController`: Der Hauptcontroller, der den Schutz für alle Positionen verwaltet.
- `_posController`: Controller für eine spezifische Position.

### Initialisierung des Schutzes

Beim Öffnen einer neuen Position oder beim Ändern einer bestehenden Position wird der Schutzcontroller initialisiert:

```cs
// Initialisierung des Schutzcontrollers für eine neue Position
this.WhenOwnTradeReceived()
	.Do(t =>
	{
		// ... (anderer Code)

		if (TakeValue.IsSet() || StopValue.IsSet())
		{
			_posController ??= _protectiveController.GetController(
				security.ToSecurityId(),
				portfolio.Name,
				new LocalProtectiveBehaviourFactory(security.PriceStep, security.Decimals),
				TakeValue, StopValue, true, default, default, true);
		}

		var info = _posController?.Update(t.Trade.Price, t.GetPosition());

		if (info is not null)
			ActiveProtection(info.Value);
	})
	.Apply(this);

// Dieser Code erstellt und initialisiert einen Schutzcontroller für eine neue Position
// beim Empfang von Informationen über einen neuen Trade. Außerdem aktualisiert er die
// Positioneninformationen im Controller und aktiviert bei Bedarf den Schutz.
```

Dadurch wird ein Controller für eine spezifische Position mit den angegebenen Take-Profit- und Stop-Loss-Parametern erstellt.

### Aktualisierung der Positionsinformationen

```cs
var info = _posController?.Update(t.Trade.Price, t.GetPosition());

if (info is not null)
	ActiveProtection(info.Value);
```

Dadurch kann der Controller den aktuellen Zustand der Position verfolgen und Schutzorders bei Bedarf anpassen.

### Prüfung der Aktivierungsbedingungen für den Schutz

In der Methode, die neue Daten verarbeitet, beispielsweise beim Empfang einer neuen Kerze, werden die Bedingungen für die Aktivierung von Schutzorders geprüft:

```cs
// Prüfung der Schutzaktivierungsbedingungen in der Methode ProcessCandle
var info = _posController?.TryActivate(candle.ClosePrice, CurrentTime);

if (info is not null)
	ActiveProtection(info.Value);

// Dieser Code prüft, ob eine Schutzorder auf Basis des aktuellen Preises
// (in diesem Fall des Schlusskurses der Kerze) und der Zeit aktiviert werden muss.
// Wenn die Bedingungen erfüllt sind, wird die Methode ActiveProtection aufgerufen.
```

Hier wird der Schlusskurs der Kerze als aktueller Preis verwendet. Es kann jedoch jeder relevante Preiswert sein, beispielsweise der Preis des letzten Trades oder der aktuelle Spread im Orderbuch.

### Aktivierung einer Schutzorder

Wenn die Bedingungen für die Aktivierung einer Schutzorder erfüllt sind, wird die entsprechende Logik ausgelöst:

```cs
// Methode zur Aktivierung einer Schutzorder
private void ActiveProtection((bool isTake, Sides side, decimal price, decimal volume, OrderCondition condition) info)
{
	// Senden einer Schutzorder (positionsschließend) als reguläre Order
	RegisterOrder(this.CreateOrder(info.side, info.price, info.volume));
}

// Diese Methode erstellt und registriert eine Order zum Schließen der Position
// auf Basis der Informationen, die vom Schutzcontroller empfangen wurden.
```

Diese Methode erstellt und registriert eine Order zum Schließen der Position entsprechend den Parametern, die der Schutzcontroller zurückgibt.

## Vergleich mit serverseitigen Stop-Orders

### Vorteile serverseitiger Stop-Orders

1. Stop-Orders (Stop Loss und Take Profit) werden direkt an den Broker gesendet.
2. Der Broker überwacht selbstständig, ob die Stop-Bedingungen erreicht werden.
3. Wenn ein Stop ausgelöst wird, platziert der Broker automatisch eine Market- oder Limit-Order.

### Vorteile des lokalen Ansatzes

1. **Flexibilität**: Möglichkeit, komplexe Schutzlogik zu implementieren, die in standardmäßigen serverseitigen Stops nicht verfügbar ist.
2. **Vertraulichkeit**: Informationen über Stop-Niveaus werden nicht an den Broker übertragen, was in manchen Märkten wichtig sein kann.
3. **Reaktionsgeschwindigkeit**: Potenziell schnellere Reaktion auf veränderte Marktbedingungen.
4. **Anpassungsfähigkeit**: Möglichkeit, Schutzniveaus dynamisch anhand von Marktdaten oder Strategielogik anzupassen.
5. **Unabhängigkeit von Broker-/Börsenimplementierung**: Der lokale Ansatz funktioniert gleich, unabhängig davon, ob der Broker oder die Börse alle notwendigen Arten von Schutzorders unterstützt.
6. **Tests mit historischen Daten**: Möglichkeit, die Strategie mit Positionsschutz vollständig auf historischen Daten zu testen, was mit serverseitigen Stops nicht möglich ist.

### Nachteile des lokalen Ansatzes

1. **Abhängigkeit von der Funktionalität des Handelsterminals**: Wenn das Terminal getrennt ist, funktioniert der Schutz nicht.
2. **Systemlast**: Erfordert ständige Berechnungen auf Clientseite.
3. **Verzögerungen**: Mögliche Verzögerungen beim Platzieren einer Order, nachdem Schutzbedingungen ausgelöst wurden.

### Nachteile serverseitiger Stop-Orders

1. **Abhängigkeit von Broker-/Börsenimplementierung**: Nicht alle Broker oder Börsen unterstützen alle Arten von Schutzorders, was die Strategiefunktionalität einschränken kann.
2. **Keine vollständigen Tests mit historischen Daten möglich**: Serverseitige Stops können beim Testen auf historischen Daten nicht präzise modelliert werden, wodurch die Bewertung der tatsächlichen Effektivität der Strategie erschwert wird.
3. **Eingeschränkte Flexibilität**: Üblicherweise sind nur grundlegende Stop-Order-Typen verfügbar, wodurch die Möglichkeiten zur Implementierung komplexer Schutzmechanismen begrenzt sind.

## Fazit

Die Verwendung eines lokalen Schutzcontrollers in der SMA-Strategie ermöglicht ein effektives Risikomanagement offener Positionen. Dieser Ansatz bietet Flexibilität bei der Einstellung von Schutzparametern und eine schnelle Reaktion auf Änderungen der Marktsituation, was für erfolgreichen Handel entscheidend ist.
