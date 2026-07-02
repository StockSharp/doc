# Erstellen eines eigenen Connectors

Der Nachrichtenmechanismus ist eine interne logische Schicht der [StockSharp](https://github.com/StockSharp/StockSharp)-Architektur, die die Interaktion zwischen verschiedenen Plattformelementen mithilfe eines Standardprotokolls ermöglicht.

Es gibt zwei Hauptklassen:

- [Message](xref:StockSharp.Messages.Message) - eine Nachricht, die Informationen trägt.
- [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) - ein Nachrichtenadapter (=Konverter).

Eine **Nachricht** fungiert als Agent, der Informationen übermittelt. Nachrichten haben ihren eigenen Typ [MessageTypes](xref:StockSharp.Messages.MessageTypes). Jeder Nachrichtentyp entspricht einer bestimmten Klasse. Alle Nachrichtenklassen wiederum erben von der abstrakten Klasse [Message](xref:StockSharp.Messages.Message), die den Nachfolgern Eigenschaften wie den Nachrichtentyp [Message.Type](xref:StockSharp.Messages.Message.Type) und [Message.LocalTime](xref:StockSharp.Messages.Message.LocalTime) - die lokale Zeit der Erstellung/des Empfangs der Nachricht - verleiht.

Nachrichten können *eingehend* und *ausgehend* sein:

- *Eingehende* Nachrichten - Nachrichten, die an ein externes System gesendet werden. Dies sind in der Regel vom Programm generierte Befehle, zum Beispiel die Nachricht [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) - ein Befehl, der eine Verbindung zum Server anfordert.
- *Ausgehende* Nachrichten - Nachrichten, die von einem externen System kommen. Dies sind Nachrichten, die Informationen über Marktdaten, Transaktionen, Portfolios, Verbindungsereignisse usw. übertragen. Zum Beispiel überträgt die Nachricht [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) Informationen über Änderungen im Orderbuch.

Der **Nachrichtenadapter** spielt die Rolle eines Vermittlers zwischen dem Handelssystem und dem Programm. Für jeden Connector-Typ gibt es eine separate Adapterklasse, die von der abstrakten Klasse [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) erbt.

Der Adapter erfüllt zwei Hauptfunktionen:

1. Konvertiert eingehende Nachrichten in Befehle eines bestimmten Handelssystems.
2. Konvertiert Informationen, die vom Handelssystem empfangen werden (Verbindung, Marktdaten, Transaktionen usw.), in ausgehende Nachrichten.

Im Folgenden wird der Prozess der Erstellung eines eigenen Adapters für [Coinbase](https://github.com/StockSharp/StockSharp/tree/master/Connectors/Coinbase) beschrieben (alle Connectoren mit Quellcode sind im [StockSharp-Repository](https://github.com/StockSharp/StockSharp/tree/master/Connectors) verfügbar und werden als Tutorial bereitgestellt).

## Beispiel für das Erstellen eines Coinbase-Nachrichtenadapters

### 1. Erstellen einer Adapterklasse

Zunächst erstellen wir die Nachrichtenadapterklasse **CoinbaseMessageAdapter**, die von der abstrakten Klasse [AsyncMessageAdapter](xref:StockSharp.Messages.AsyncMessageAdapter) erbt.

```cs
public partial class CoinbaseMessageAdapter : AsyncMessageAdapter
{
	private Authenticator _authenticator;
	private HttpClient _restClient;
	private SocketClient _socketClient;

	// Other adapter fields and properties
}
```

### 2. Adapter-Konstruktor

Im Adapter-Konstruktor müssen die folgenden Aktionen durchgeführt werden:

1. Übergeben Sie den Transaktions-ID-Generator, der zur Erstellung von Nachrichten-IDs verwendet wird.

2. Geben Sie die unterstützten Nachrichtentypen mithilfe der folgenden Methoden an:
 - [AddMarketDataSupport](xref:StockSharp.Messages.Extensions.AddMarketDataSupport(StockSharp.Messages.MessageAdapter)) - Unterstützung für Nachrichten zum Abonnieren von Marktdaten.
 - [AddTransactionalSupport](xref:StockSharp.Messages.Extensions.AddTransactionalSupport(StockSharp.Messages.MessageAdapter)) - Unterstützung für Transaktionsnachrichten.

3. Geben Sie die spezifischen, vom Adapter unterstützten Marktdatentypen mithilfe der Methode [AddSupportedMarketDataType](xref:StockSharp.Messages.Extensions.AddSupportedMarketDataType(StockSharp.Messages.MessageAdapter,StockSharp.Messages.DataType)) an.

```cs
public CoinbaseMessageAdapter(IdGenerator transactionIdGenerator)
	: base(transactionIdGenerator)
{
	HeartbeatInterval = TimeSpan.FromSeconds(5);

	// Add support for market data and transactions
	this.AddMarketDataSupport();
	this.AddTransactionalSupport();

	// Remove unsupported message types
	this.RemoveSupportedMessage(MessageTypes.Portfolio);
	this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);

	// Add supported market data types
	this.AddSupportedMarketDataType(DataType.Ticks);
	this.AddSupportedMarketDataType(DataType.MarketDepth);
	this.AddSupportedMarketDataType(DataType.Level1);
	this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
}
```

### 3. Verbinden und Trennen des Adapters

Um den Adapter mit dem Handelssystem zu verbinden, wird die Methode [AsyncMessageAdapter.ConnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ConnectAsync(StockSharp.Messages.ConnectMessage,System.Threading.CancellationToken)) aufgerufen. Ihr wird die eingehende Nachricht [ConnectMessage](xref:StockSharp.Messages.ConnectMessage) übergeben. Wenn die Verbindung erfolgreich ist, sendet der Adapter eine ausgehende Nachricht [ConnectMessage](xref:StockSharp.Messages.ConnectMessage).

```cs
public override async ValueTask ConnectAsync(ConnectMessage connectMsg, CancellationToken cancellationToken)
{
	// Check the presence of keys for transactional mode
	if (this.IsTransactional())
	{
		if (Key.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.KeyNotSpecified);

		if (Secret.IsEmpty())
			throw new InvalidOperationException(LocalizedStrings.SecretNotSpecified);
	}

	// Initialize the authenticator
	_authenticator = new(this.IsTransactional(), Key, Secret, Passphrase);

	// Check that clients are not yet created
	if (_restClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	if (_socketClient != null)
		throw new InvalidOperationException(LocalizedStrings.NotDisconnectPrevTime);

	// Create REST client
	_restClient = new(_authenticator) { Parent = this };

	// Create and configure WebSocket client
	_socketClient = new(_authenticator, ReConnectionSettings.ReAttemptCount) { Parent = this };
	SubscribePusherClient();

	// Connect WebSocket client
	await _socketClient.Connect(cancellationToken);

	// Send successful connection message
	SendOutMessage(new ConnectMessage());
}
```

Um den Adapter vom Handelssystem zu trennen, wird die Methode [AsyncMessageAdapter.DisconnectAsync](xref:StockSharp.Messages.AsyncMessageAdapter.DisconnectAsync(StockSharp.Messages.DisconnectMessage,System.Threading.CancellationToken)) aufgerufen. Wenn die Trennung erfolgreich ist, sendet der Adapter eine ausgehende Nachricht [DisconnectMessage](xref:StockSharp.Messages.DisconnectMessage).

```cs
public override ValueTask DisconnectAsync(DisconnectMessage disconnectMsg, CancellationToken cancellationToken)
{
	// Check that clients are created
	if (_restClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	if (_socketClient == null)
		throw new InvalidOperationException(LocalizedStrings.ConnectionNotOk);

	// Free REST client resources
	_restClient.Dispose();
	_restClient = null;

	// Disconnect WebSocket client
	_socketClient.Disconnect();

	// Send disconnection message
	SendOutDisconnectMessage(true);
	return default;
}
```

Darüber hinaus stellt der Adapter die Methode [AsyncMessageAdapter.ResetAsync](xref:StockSharp.Messages.AsyncMessageAdapter.ResetAsync(StockSharp.Messages.ResetMessage,System.Threading.CancellationToken)) bereit, um den Zustand zurückzusetzen, wodurch die Verbindung geschlossen wird und der Adapter in seinen ursprünglichen Zustand zurückversetzt wird.

```cs
public override ValueTask ResetAsync(ResetMessage resetMsg, CancellationToken cancellationToken)
{
	// Free REST client resources
	if (_restClient != null)
	{
		try
		{
			_restClient.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_restClient = null;
	}

	// Disconnect and clear WebSocket client
	if (_socketClient != null)
	{
		try
		{
			UnsubscribePusherClient();
			_socketClient.Disconnect();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_socketClient = null;
	}

	// Free authenticator resources
	if (_authenticator != null)
	{
		try
		{
			_authenticator.Dispose();
		}
		catch (Exception ex)
		{
			SendOutError(ex);
		}

		_authenticator = null;
	}

	// Clear additional data
	_candlesTransIds.Clear();

	// Send reset message
	SendOutMessage(new ResetMessage());
	return default;
}
```

### Nach Abschluss der Entwicklung

Sobald der Connector implementiert ist, gibt es zwei Möglichkeiten, ihn zu verwenden:

1. Veröffentlichen Sie ihn im [StockSharp Store](https://stocksharp.com/store) entweder als kostenpflichtiges oder als kostenloses Produkt. In diesem Fall installieren die Benutzer den Connector automatisch über den [Installer](../../installer/setup.md).
2. Für den persönlichen Gebrauch kopieren Sie die erstellte Connector-*.dll*-Datei in den Ordner Ihrer Anwendung (oder eines beliebigen StockSharp-Produkts). Beim Start durchsucht die Anwendung das aktuelle Verzeichnis nach Adaptern anhand der folgenden Kriterien:

   - Es werden nur Dateien mit der Erweiterung **.dll** berücksichtigt, deren Namen mit `StockSharp.` beginnen.
   - Jede verbleibende Datei wird überprüft, um sicherzustellen, dass es sich um eine gültige .NET-Assembly handelt.
   - Die Assembly wird geladen, und alle Typen, die `IMessageAdapter` implementieren, werden gesammelt.
   - Alle Fehler, die während des Scannens oder Ladens auftreten, werden im Protokoll festgehalten und stoppen die Suche nicht. Wenn das Laden fehlschlägt, öffnen Sie das Protokollfenster oder die Protokolldatei der Anwendung, um die Fehlerdetails anzuzeigen.

Dieses Dokument beschreibt die allgemeinen Prinzipien der Funktionsweise des Adapters, seiner Erstellung und der Verwaltung der Verbindung mit dem Handelssystem. Die folgenden Dokumente widmen sich der Implementierung der Funktionalität des Adapters:

- [Instrumentensuche](creating_own_connector/instrument_lookup.md)
- [Arbeiten mit Marktdaten](creating_own_connector/market_data.md)
- [Abfragen des aktuellen Status von Portfolio und Orders](creating_own_connector/portfolio_and_orders_state.md)
- [Arbeiten mit Handelsoperationen](creating_own_connector/trading_operations.md)
- [Speichern von Einstellungen](creating_own_connector/settings.md)
- [Erweiterte Orderbedingungen](creating_own_connector/order_extended.md)
