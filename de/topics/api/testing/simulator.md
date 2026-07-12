# Tests mit Echtzeit-Marktdaten

Tests mit Echtzeit-Marktdaten bedeuten Handel über eine echte Verbindung zur Börse ("Live"-Kurse), jedoch ohne reale Orders an der Börse zu platzieren. Alle registrierten Orders werden abgefangen und ihre Ausführung wird auf Basis von Markt-Orderbüchern emuliert. Ein solcher Test kann zum Beispiel bei der Entwicklung eines Handelssimulators oder bei der Überprüfung eines Handelsalgorithmus über einen kurzen Zeitraum mit echten Kursen nützlich sein.

Um Handel mit realen Daten zu emulieren, verwenden Sie [RealTimeEmulationTrader\<TAdapter\>](xref:StockSharp.Algo.Testing.RealTimeEmulationTrader`1). Diese Klasse umschließt einen Connector zu einem bestimmten Handelssystem ([Binance](../connectors/crypto_exchanges/binance.md), [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) usw.).

## Emulations-Connector erstellen

Um einen Emulations-Connector zu erstellen, erzeugen Sie zunächst einen normalen Connector zum Empfangen von Marktdaten und anschließend darauf basierend den Emulations-Connector:

```csharp
// Normalen Connector zum Empfangen von Marktdaten erstellen
private readonly Connector _realConnector = new();

// Emulations-Connector erstellen
_emuConnector = new RealTimeEmulationTrader<IMessageAdapter>(_realConnector.Adapter, _realConnector, _emuPf, false);

// Emulationsparameter konfigurieren
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;
settings.TimeZone = TimeHelper.Est;
settings.ConvertTime = true;
```

Für die Handelsemulation müssen Sie ein spezielles Portfolio verwenden:

```csharp
private readonly Portfolio _emuPf = Portfolio.CreateSimulator();
```

## Ereignisse abonnieren

Wie ein normaler Connector erzeugt auch der Emulations-Connector Ereignisse beim Empfang von Marktdaten und bei der Ausführung von Transaktionen:

```csharp
// Connector-Ereignisse abonnieren
_emuConnector.Connected += () =>
{
	// GUI-Labels aktualisieren
	this.GuiAsync(() => { ChangeConnectStatus(true); });
};

_emuConnector.Disconnected += () =>
{
	// GUI-Labels aktualisieren
	this.GuiAsync(() => { ChangeConnectStatus(false); });
};

_emuConnector.ConnectionError += error => this.GuiAsync(() =>
{
	// GUI-Labels aktualisieren
	ChangeConnectStatus(false);
	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
});

_emuConnector.OrderBookReceived += OnDepth;
_emuConnector.PositionReceived += (sub, p) => PortfolioGrid.Positions.TryAdd(p);
_emuConnector.OwnTradeReceived += (s, t) => TradeGrid.Trades.TryAdd(t);
_emuConnector.OrderReceived += (s, o) =>
{
	if (!_fistTimeOrders.Add(o))
		return;

	_bufferOrders.Add(o);
	OrderGrid.Orders.Add(o);
};

// Fehler bei der Orderregistrierung abonnieren
_emuConnector.OrderRegisterFailReceived += (s, f) => OrderGrid.AddRegistrationFail(f);

_emuConnector.CandleReceived += (s, candle) =>
{
	if (s == _candlesSubscription)
		_buffer.Add(candle);
};
```

## Marktdaten abonnieren

Für die Arbeit mit Marktdaten müssen Sie die entsprechenden Datentypen abonnieren:

```csharp
// Orderbücher, Ticks und Level1 für den Emulations-Connector abonnieren
_emuConnector.Subscribe(new(DataType.MarketDepth, security));
_emuConnector.Subscribe(new(DataType.Ticks, security));
_emuConnector.Subscribe(new(DataType.Level1, security));

// Orderbücher für den realen Connector abonnieren (für die Emulation erforderlich)
_realConnector.Subscribe(new(DataType.MarketDepth, security));

// Kerzen abonnieren
_candlesSubscription = new(CandleDataTypeEdit.DataType, security)
{
	From = DateTimeOffset.UtcNow - TimeSpan.FromDays(10),
};
_emuConnector.Subscribe(_candlesSubscription);
```

## Orderregistrierung und Verwaltung

Orders werden über den Emulations-Connector ähnlich wie über einen normalen Connector registriert:

```csharp
// Orderregistrierung
_emuConnector.RegisterOrder(order);

// Aufträgetornierung
_emuConnector.CancelOrder(order);

// Orderänderung
_emuConnector.ReRegisterOrder(order, newPrice, order.Balance);
```

## Emulationsparameter konfigurieren

Sie können die Eigenschaft [MarketEmulatorSettings](xref:StockSharp.Algo.Testing.MarketEmulatorSettings) verwenden, um Emulationsparameter zu konfigurieren:

```csharp
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;

// Zeitzone festlegen
settings.TimeZone = TimeHelper.Est;

// Zeit konvertieren
settings.ConvertTime = true;

// Aufträge bei Preisberührung matchen
settings.MatchOnTouch = false;

// Latenz der Orderausführung emulieren
settings.Latency = TimeSpan.FromMilliseconds(100);
```

## Beispiel für die Oberfläche

Das Beispiel SampleRealTimeEmulation zeigt, wie Daten vom realen Connector und vom Emulations-Connector gleichzeitig angezeigt werden können:

![Beispiel Echtzeit-Emulation](../../../images/sample_realtime_emulation.png)

Die Anwendungsoberfläche enthält die folgenden Elemente:
- Charts zur Anzeige von Kerzen und Orders
- Tabellen für Orders und eigene Trades
- Reales Markt-Orderbuch und Emulations-Orderbuch
- Steuerelemente zum Erstellen und Stornieren von Orders

## Vorteile und Einschränkungen

Tests mit Echtzeit-Marktdaten haben folgende Vorteile:
- Nutzung realer Marktdaten ohne finanzielle Risiken
- Testen von Algorithmen unter Bedingungen, die dem realen Handel sehr nahekommen
- Möglichkeit, Ergebnisse in Echtzeit mit dem realen Markt zu vergleichen

Einschränkungen:
- Die Testgeschwindigkeit ist durch die Geschwindigkeit der realen Daten begrenzt
- Historische Zeiträume können nicht getestet werden
- Abhängigkeit von Qualität und Vollständigkeit der empfangenen Marktdaten
