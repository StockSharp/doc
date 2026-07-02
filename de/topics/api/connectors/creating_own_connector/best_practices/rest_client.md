# REST-Client

Bei der Entwicklung eines Connectors für eine Reihe von Börsen ist eine wichtige Komponente der HTTP-Client, der die Interaktion mit der REST-API der Börse ermöglicht. In StockSharp wird zu diesem Zweck bei der Entwicklung eines Connectors häufig eine Klasse `HttpClient` erstellt, die auf der Grundlage der RestSharp-Bibliothek aufgebaut ist.

## RestSharp-Funktionen

[RestSharp](https://www.nuget.org/packages/RestSharp) ist eine beliebte .NET-Bibliothek für die Arbeit mit REST-APIs. Um die Arbeit mit RestSharp im Rahmen von StockSharp zu vereinfachen, wurden Erweiterungsmethoden entwickelt, die im [Ecng-Repository](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs) verfügbar sind.

## Struktur von HttpClient

`HttpClient` umfasst in der Regel die folgenden Schlüsselelemente:

1. **Konstruktor**
  - Initialisiert RestClient
  - Konfiguriert die Basis-URL und die Header

2. **Methoden für verschiedene Arten von Anfragen**
  - Zum Beispiel `GetProducts`, `GetCandles`, `GetTrades`

3. **Methoden für die Arbeit mit Handelsoperationen**
  - Zum Beispiel `GetOrders`, `RegisterOrder`, `CancelOrder`

4. **Hilfsmethoden**
  - Zum Bilden von Anfragen
  - Zum Verarbeiten von Antworten

```cs
class HttpClient : BaseLogReceiver
{
	private readonly RestClient _restClient;
	private readonly Authenticator _authenticator;

	// The constructor initializes RestClient and configures the base URL
	public HttpClient(Authenticator authenticator)
	{
		_authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));

		var options = new RestClientOptions
		{
			BaseUrl = new Uri("https://api.example.com"),
			UserAgent = "YourAppName/1.0"
		};

		_restClient = new RestClient(options);
	}

	// Method for getting a list of products (instruments) from the exchange
	public async Task<IEnumerable<Product>> GetProducts(string type, CancellationToken cancellationToken)
	{
		var request = new RestRequest("products", Method.Get)
			.AddParameter("type", type);

		// Using the ExecuteAsync extension method to execute the request
		var response = await _restClient.ExecuteAsync<List<Product>>(request, cancellationToken);
		return response.Data;
	}

	// Method for getting historical candles
	public async Task<IEnumerable<Candle>> GetCandles(string symbol, long start, long end, string granularity, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"products/{symbol}/candles", Method.Get)
			.AddParameter("start", start)
			.AddParameter("end", end)
			.AddParameter("granularity", granularity);

		var response = await _restClient.ExecuteAsync<List<Candle>>(request, cancellationToken);
		return response.Data;
	}

	// Method for registering a new order
	public async Task<Order> RegisterOrder(string clientOrderId, string symbol, string type, string side, decimal? price, decimal volume, CancellationToken cancellationToken)
	{
		var request = new RestRequest("orders", Method.Post)
			.AddJsonBody(new
			{
				client_order_id = clientOrderId,
				symbol,
				type,
				side,
				price,
				volume
			});

		// Applying authentication before executing the request
		var response = await _restClient.ExecuteAsync<Order>(ApplyAuth(request), cancellationToken);
		return response.Data;
	}

	// Method for canceling an existing order
	public async Task<bool> CancelOrder(string orderId, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"orders/{orderId}", Method.Delete);

		var response = await _restClient.ExecuteAsync(ApplyAuth(request), cancellationToken);
		return response.IsSuccessful;
	}

	// Helper method for applying authentication to a request
	private RestRequest ApplyAuth(RestRequest request)
	{
		_authenticator.ApplyAuthentication(request);
		return request;
	}
}
```

## Implementierungsempfehlungen

- Verwenden Sie asynchrone Methoden für einen effizienten Netzwerkbetrieb.
- Implementieren Sie Fehlerbehandlung und Wiederholungsversuche für instabile Verbindungen.
- Berücksichtigen Sie die API-Ratenlimits der jeweiligen Börse.
- Verwenden Sie RestSharp-Erweiterungsmethoden, um die Arbeit mit Anfragen und Antworten zu vereinfachen.

Denken Sie daran, dass sich die konkrete Implementierung je nach Anforderungen und Besonderheiten der API einer bestimmten Börse unterscheiden kann.
