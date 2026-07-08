# Cliente REST

Al desarrollar un conector para una serie de exchanges, un componente importante es el cliente HTTP, que proporciona la interacción con la API REST del exchange. En StockSharp, para este propósito, a menudo se crea una clase `HttpClient` al desarrollar un conector, construida sobre la base de la librería RestSharp.

## Características de RestSharp

[RestSharp](https://www.nuget.org/packages/RestSharp) es una popular librería de .NET para trabajar con API REST. Para simplificar el trabajo con RestSharp dentro del marco de StockSharp, se han desarrollado métodos de extensión que están disponibles en el [repositorio Ecng](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs).

## Estructura de HttpClient

`HttpClient` generalmente incluye los siguientes elementos clave:

1. **Constructor**
  - Inicializa RestClient
  - Configura la URL base y los encabezados

2. **Métodos para diferentes tipos de solicitudes**
  - Por ejemplo, `GetProducts`, `GetCandles`, `GetTrades`

3. **Métodos para trabajar con operaciones de trading**
  - Por ejemplo, `GetOrders`, `RegisterOrder`, `CancelOrder`

4. **Métodos auxiliares**
  - Para formar las solicitudes
  - Para procesar las respuestas

```cs
class HttpClient : BaseLogReceiver
{
	private readonly RestClient _restClient;
	private readonly Authenticator _authenticator;

	// El constructor inicializa RestClient y configura la URL base
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

	// Método para obtener velas históricas
	public async Task<IEnumerable<Candle>> GetCandles(string symbol, long start, long end, string granularity, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"products/{symbol}/candles", Method.Get)
			.AddParameter("start", start)
			.AddParameter("end", end)
			.AddParameter("granularity", granularity);

		var response = await _restClient.ExecuteAsync<List<Candle>>(request, cancellationToken);
		return response.Data;
	}

	// Método para registrar una nueva orden
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

		// Aplicar autenticación antes de ejecutar la solicitud
		var response = await _restClient.ExecuteAsync<Order>(ApplyAuth(request), cancellationToken);
		return response.Data;
	}

	// Método para cancelar una orden existente
	public async Task<bool> CancelOrder(string orderId, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"orders/{orderId}", Method.Delete);

		var response = await _restClient.ExecuteAsync(ApplyAuth(request), cancellationToken);
		return response.IsSuccessful;
	}

	// Método auxiliar para aplicar autenticación a una solicitud
	private RestRequest ApplyAuth(RestRequest request)
	{
		_authenticator.ApplyAuthentication(request);
		return request;
	}
}
```

## Recomendaciones de Implementación

- Utilice métodos asíncronos para un funcionamiento eficiente de la red.
- Implemente el manejo de errores y reintentos para conexiones inestables.
- Tenga en cuenta los límites de tasa de la API del exchange específico.
- Utilice los métodos de extensión de RestSharp para simplificar el trabajo con solicitudes y respuestas.

Recuerde que la implementación específica puede diferir según los requisitos y las particularidades de la API de un exchange en particular.
</content>
