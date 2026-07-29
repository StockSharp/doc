# Instrumentensuche

Die meisten Connectors zu US-Aktienbörsen (zum Beispiel [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), [PolygonIO](../connectors/stock_market/polygonio.md) und andere) übertragen nach dem Herstellen einer Verbindung über die Methode [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect) nicht alle verfügbaren Instrumente an den Client. Grund dafür ist die große Anzahl von Instrumenten, die an amerikanischen Börsen gehandelt werden; außerdem wird so die Last auf Broker-Servern und Datenquellen reduziert.

## Grundlagen der Instrumentensuche

Für die Suche nach Instrumenten in S# wird ein Abonnementmechanismus verwendet, ähnlich wie beim Empfang von Marktdaten. Dieser Ansatz ermöglicht einheitlichen Code für alle Datentypen, einschließlich Instrumenten.

### Erstellen eines Abonnements für die Instrumentensuche

Um nach Instrumenten zu suchen, müssen Sie eine Instanz der Klasse [Subscription](xref:StockSharp.BusinessEntities.Subscription) auf Basis der Nachricht [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) erstellen, die Filterparameter enthält:

```csharp
// Filterobjekt für die Suche erstellen
var lookupMessage = new SecurityLookupMessage
{
	// Suchkriterien festlegen
	SecurityId = new SecurityId
	{
		// Suche nach Instrumentencode (Sie können eine Maske wie "AAPL*" verwenden)
		SecurityCode = "AAPL",
		// Optional kann der Handelsplatzcode angegeben werden
		BoardCode = "NASDAQ"
	},
	// Der Instrumententyp kann angegeben werden
	SecurityType = SecurityTypes.Stock,
	// Transaktions-ID setzen
	TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// Abonnement für die Instrumentensuche erstellen
var subscription = new Subscription(lookupMessage);
```

### Mögliche Filterparameter

Die Nachricht [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) erlaubt das Festlegen folgender Suchkriterien:

- **SecurityId** - Instrumentenbezeichner, bestehend aus:
  - `SecurityCode` - Code oder Maske des Instrumentencodes (zum Beispiel "AAPL" oder "MS*")
  - `BoardCode` - Börsen-Handelsplatzcode (zum Beispiel [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq))
- **SecurityType** - Instrumententyp ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future) usw.)
- **SecurityTypes** - Array von Instrumententypen für erweiterte Suche
- **Currency** - Handelswährung des Instruments
- **ExpiryDate** - Verfallsdatum (für Derivate)
- **Strike** - Ausübungspreis (für Optionen)
- **OptionType** - Optionstyp (für Optionen)
- **Name** - Instrumentenname oder ein Teil davon
- **Class** - Instrumentenklasse

### Verarbeitung von Suchergebnissen

Nach dem Erstellen des Abonnements müssen Sie Ereignisse für den Empfang von Instrumenten abonnieren und die Anfrage senden:

```csharp
// Handler für das Instrumentenempfangsereignis
private void OnSecurityReceived(Subscription subscription, Security security)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;

	Console.WriteLine($"Instrument gefunden: {security.Id} - {security.Name}, Typ: {security.Type}");

	// Hier können Sie das Instrument zu einer Sammlung hinzufügen oder andere Aktionen ausführen
	Securities.Add(security);
}

// Handler für das Ereignis zum Abschluss der Suche
private void OnSubscriptionFinished(Subscription subscription)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;

	Console.WriteLine($"Suche abgeschlossen. Gefundene Instrumente: {Securities.Count}");
}

// Handler für Abonnementfehler
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;

	Console.WriteLine($"Fehler bei der Instrumentsuche: {error.Message}");
}

// Ereignisse abonnieren
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// Anfrage zur Instrumentensuche senden
Connector.Subscribe(subscription);
```

### Vollständiges Beispiel für die Instrumentensuche

Unten sehen Sie ein vollständiges Beispiel einer Methode zur Suche nach Instrumenten:

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
	// Objekt für die Instrumentensuche erstellen
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId
		{
			SecurityCode = searchCode,
			// Wenn auf einem bestimmten Board gesucht werden soll
			// BoardCode = ExchangeBoard.Nyse.Code,
		},
		SecurityType = securityType,
		TransactionId = Connector.TransactionIdGenerator.GetNextId()
	};

	// Abonnement erstellen
	var subscription = new Subscription(lookupMessage);

	// Sammlung für Suchergebnisse leeren
	_searchResults.Clear();

	// Temporäre Sammlung zum Sammeln der Ergebnisse
	var foundSecurities = new List<Security>();

	// Abonnement für den Empfang von Instrumenten
	void OnSecurityReceived(Subscription sub, Security security)
	{
		if (sub != subscription)
			return;

		// Gefundenes Instrument zur Sammlung hinzufügen
		foundSecurities.Add(security);
		Console.WriteLine($"Gefunden: {security.Id}, {security.Name}");
	}

	// Abonnement für den Abschluss der Suche
	void OnSubscriptionFinished(Subscription sub)
	{
		if (sub != subscription)
			return;

		// Ergebnisse in die Hauptsammlung kopieren
		_searchResults.AddRange(foundSecurities);

		Console.WriteLine($"Suche abgeschlossen. Gefundene Instrumente: {foundSecurities.Count}");

		// Ereignisse abbestellen
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}

	// Behandlung von Abonnementfehlern
	void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
	{
		if (sub != subscription)
			return;

		Console.WriteLine($"Fehler bei der Instrumentsuche: {error.Message}");

		// Ereignisse abbestellen
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}

	// Ereignisse abonnieren
	Connector.SecurityReceived += OnSecurityReceived;
	Connector.SubscriptionFinished += OnSubscriptionFinished;
	Connector.SubscriptionFailed += OnSubscriptionFailed;

	// Suchanfrage senden
	Connector.Subscribe(subscription);
}
```

### Verwendungsbeispiel in einer WPF-Anwendung

In einer grafischen Anwendung wird die Instrumentensuche häufig aus dem Klick-Handler einer Schaltfläche aufgerufen:

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
	// Suchkriterium aus dem Textfeld abrufen
	var searchText = SearchTextBox.Text;

	if (string.IsNullOrWhiteSpace(searchText))
	{
		MessageBox.Show("Suchkriterium eingeben");
		return;
	}

	// Suchabonnement erstellen und senden
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId { SecurityCode = searchText },
		// Wenn im Interface ein Typ ausgewählt ist
		SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
	};

	var subscription = new Subscription(lookupMessage);

	// Hier kann ein Ladeindikator angezeigt werden
	LoadingIndicator.Visibility = Visibility.Visible;

	// Anfrage senden
	Connector.Subscribe(subscription);
}
```

## Verwenden von SecurityLookupWindow

StockSharp stellt außerdem einen fertigen Dialog für die Instrumentensuche bereit - [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow):

```csharp
private void ShowSecurityLookupWindow_Click(object sender, RoutedEventArgs e)
{
	var lookupWindow = new SecurityLookupWindow
	{
		// Möglichkeit zur Suche nach allen Instrumenten angeben
		// (wenn der Connector diese Funktion unterstützt)
		ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),

		// Anfangs-Suchkriterien festlegen
		CriteriaMessage = new SecurityLookupMessage
		{
			SecurityId = new SecurityId { SecurityCode = "AAPL" },
			SecurityType = SecurityTypes.Stock
		}
	};

	// Fenster als modalen Dialog anzeigen
	if (lookupWindow.ShowModal(this))
	{
		// Wenn der Benutzer die Auswahl bestätigt hat, Anfrage senden
		Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
	}
}
```

## Fazit

Der Abonnementmechanismus in StockSharp bietet einen einheitlichen Weg zum Abrufen von Daten, einschließlich der Instrumentensuche. Dadurch kann derselbe Ansatz für die Arbeit mit verschiedenen Connectors und Datentypen verwendet werden, was die Entwicklung von Handelsanwendungen deutlich vereinfacht.

