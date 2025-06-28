# REST Client

When developing a connector for a number of exchanges, an important component is the HTTP client, which provides interaction with the exchange's REST API. In StockSharp, for this purpose, a `HttpClient` class is often created when developing a connector, built on the basis of the RestSharp library.

## RestSharp Features

[RestSharp](https://www.nuget.org/packages/RestSharp) is a popular .NET library for working with REST APIs. To simplify working with RestSharp within the framework of StockSharp, extension methods have been developed that are available in the [Ecng repository](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs).

## HttpClient Structure

`HttpClient` usually includes the following key elements:

1. **Constructor**
  - Initializes RestClient
  - Configures the base URL and headers

2. **Methods for different types of requests**
  - For example, `GetProducts`, `GetCandles`, `GetTrades`

3. **Methods for working with trading operations**
  - For example, `GetOrders`, `RegisterOrder`, `CancelOrder`

4. **Helper methods**
  - For forming requests
  - For processing responses

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

## Implementation Recommendations

- Use asynchronous methods for efficient network operation.
- Implement error handling and retries for unstable connections.
- Take into account the API rate limits of the specific exchange.
- Use RestSharp extension methods to simplify working with requests and responses.

Remember that the specific implementation may differ depending on the requirements and specifics of the API of a particular exchange.