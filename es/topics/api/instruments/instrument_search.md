# Búsqueda de instrumentos

La mayoría de los conectores a bolsas de acciones de EE. UU. (por ejemplo, [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), [PolygonIO](../connectors/stock_market/polygonio.md) y otros) no transfieren todos los instrumentos disponibles al cliente después de establecer una conexión mediante el método [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect). Esto se debe al gran número de instrumentos negociados en las bolsas estadounidenses y se hace para reducir la carga sobre los servidores de brokers y las fuentes de datos.

## Fundamentos de la búsqueda de instrumentos

Para buscar instrumentos en S#, se usa un mecanismo de suscripciones, similar a la recepción de datos de mercado. Este enfoque permite usar código uniforme para todos los tipos de datos, incluidos los instrumentos.

### Creación de una suscripción para búsqueda de instrumentos

Para buscar instrumentos, necesita crear una instancia de la clase [Subscription](xref:StockSharp.BusinessEntities.Subscription) basada en el mensaje [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage), que contiene parámetros de filtrado:

```csharp
// Crear un objeto de filtro para la búsqueda
var lookupMessage = new SecurityLookupMessage
{
	// Establecer criterios de búsqueda
	SecurityId = new SecurityId
	{
		// Buscar por código de instrumento (puede usar una máscara como "AAPL*")
		SecurityCode = "AAPL",
		// Opcionalmente, puede especificar el código de plaza
		BoardCode = "NASDAQ"
	},
	// Puede especificar el tipo de instrumento
	SecurityType = SecurityTypes.Stock,
	// Establecer ID de transacción
	TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// Crear una suscripción para búsqueda de instrumentos
var subscription = new Subscription(lookupMessage);
```

### Parámetros de filtrado posibles

El mensaje [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) permite establecer los siguientes criterios de búsqueda:

- **SecurityId** — identificador del instrumento, que contiene:
  - **SecurityCode** — código o máscara del código del instrumento (por ejemplo, "AAPL" o "MS*")
  - **BoardCode** — código de la plaza bursátil (por ejemplo, [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq))
- **SecurityType** — tipo de instrumento ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future), etc.)
- **SecurityTypes** — array de tipos de instrumentos para búsqueda avanzada
- **Currency** — divisa de trading del instrumento
- **ExpiryDate** — fecha de vencimiento (para derivados)
- **Strike** — precio strike (para opciones)
- **OptionType** — tipo de opción (para opciones)
- **Name** — nombre del instrumento o parte de él
- **Class** — clase del instrumento

### Procesamiento de resultados de búsqueda

Después de crear la suscripción, debe suscribirse a los eventos para recibir instrumentos y enviar la solicitud:

```csharp
// Manejador del evento de recepción de instrumento
private void OnSecurityReceived(Subscription subscription, Security security)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Found instrument: {security.Id} - {security.Name}, Type: {security.Type}");
	
	// Aquí puede agregar el instrumento a una colección o realizar otras acciones
	Securities.Add(security);
}

// Manejador del evento de finalización de búsqueda
private void OnSubscriptionFinished(Subscription subscription)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Search completed. Instruments found: {Securities.Count}");
}

// Manejador de errores de suscripción
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Instrument search error: {error.Message}");
}

// Suscribirse a eventos
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// Enviar la solicitud de búsqueda de instrumentos
Connector.Subscribe(subscription);
```

### Ejemplo completo de búsqueda de instrumentos

A continuación se muestra un ejemplo completo de un método para buscar instrumentos:

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
	// Crear un objeto para búsqueda de instrumentos
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId
		{
			SecurityCode = searchCode,
			// Si necesita buscar en una plaza específica
			// BoardCode = ExchangeBoard.Nyse.Code,
		},
		SecurityType = securityType,
		TransactionId = Connector.TransactionIdGenerator.GetNextId()
	};
	
	// Crear una suscripción
	var subscription = new Subscription(lookupMessage);
	
	// Limpiar la colección para los resultados de búsqueda
	_searchResults.Clear();
	
	// Colección temporal para acumular resultados
	var foundSecurities = new List<Security>();
	
	// Suscripción para recibir instrumentos
	void OnSecurityReceived(Subscription sub, Security security)
	{
		if (sub != subscription)
			return;
			
		// Agregar el instrumento encontrado a la colección
		foundSecurities.Add(security);
		Console.WriteLine($"Found: {security.Id}, {security.Name}");
	}
	
	// Suscripción para finalización de búsqueda
	void OnSubscriptionFinished(Subscription sub)
	{
		if (sub != subscription)
			return;
			
		// Copiar los resultados a la colección principal
		_searchResults.AddRange(foundSecurities);
		
		Console.WriteLine($"Search completed. Instruments found: {foundSecurities.Count}");
		
		// Cancelar suscripción a eventos
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// Manejo de errores de suscripción
	void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
	{
		if (sub != subscription)
			return;
			
		Console.WriteLine($"Instrument search error: {error.Message}");
		
		// Cancelar suscripción a eventos
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// Suscribirse a eventos
	Connector.SecurityReceived += OnSecurityReceived;
	Connector.SubscriptionFinished += OnSubscriptionFinished;
	Connector.SubscriptionFailed += OnSubscriptionFailed;
	
	// Enviar la solicitud de búsqueda
	Connector.Subscribe(subscription);
}
```

### Ejemplo de uso en una aplicación WPF

En una aplicación gráfica, la búsqueda de instrumentos a menudo se llama desde un manejador de clic de botón:

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
	// Obtener criterios de búsqueda del campo de texto
	var searchText = SearchTextBox.Text;
	
	if (string.IsNullOrWhiteSpace(searchText))
	{
		MessageBox.Show("Enter a search criterion");
		return;
	}
	
	// Crear y enviar una suscripción de búsqueda
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId { SecurityCode = searchText },
		// Si se selecciona un tipo en la interfaz
		SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
	};
	
	var subscription = new Subscription(lookupMessage);
	
	// Aquí puede mostrar un indicador de carga
	LoadingIndicator.Visibility = Visibility.Visible;
	
	// Enviar la solicitud
	Connector.Subscribe(subscription);
}
```

## Uso de SecurityLookupWindow

StockSharp también proporciona un diálogo ya preparado para búsqueda de instrumentos: [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow):

```csharp
private void ShowSecurityLookupWindow_Click(object sender, RoutedEventArgs e)
{
	var lookupWindow = new SecurityLookupWindow
	{
		// Especificar la capacidad de buscar todos los instrumentos
		// (si el conector admite esta función)
		ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),
		
		// Establecer criterios de búsqueda iniciales
		CriteriaMessage = new SecurityLookupMessage
		{
			SecurityId = new SecurityId { SecurityCode = "AAPL" },
			SecurityType = SecurityTypes.Stock
		}
	};
	
	// Mostrar la ventana como diálogo modal
	if (lookupWindow.ShowModal(this))
	{
		// Si el usuario confirmó la selección, enviar la solicitud
		Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
	}
}
```

## Conclusión

El mecanismo de suscripciones en StockSharp proporciona una forma unificada de obtener datos, incluida la búsqueda de instrumentos. Esto permite usar el mismo enfoque para trabajar con distintos conectores y tipos de datos, lo que simplifica de forma significativa el desarrollo de aplicaciones de trading.
