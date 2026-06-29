# REST 客户端

在为多个交易所开发连接器时，一个重要的组成部分是HTTP客户端，它提供与交易所的REST API的交互。在StockSharp中，为此目的，在开发连接器时通常会创建一个`HttpClient`类，该类基于RestSharp库构建。

## RestSharp 功能

[RestSharp](https://www.nuget.org/packages/RestSharp) 是一个用于处理 REST API 的流行 .NET 库。为了简化在 StockSharp 框架中使用 RestSharp，已经开发了扩展方法，这些方法可以在 [Ecng 仓库](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs) 中获取。

## HttpClient 结构

`HttpClient` 通常包括以下关键要素：

1. **构造函数**
  - 初始化 RestClient
  - 配置基础 URL 和头信息

2. **不同类型请求的方法**
  - 例如，`GetProducts`，`GetCandles`，`GetTrades`

3. **交易操作方法**
  - 例如，`GetOrders`，`RegisterOrder`，`CancelOrder`

4. **辅助方法**
  - 用于提出请求
  - 用于处理响应

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

## 实施建议

- 使用异步方法以提高网络操作效率。
- 为不稳定的连接实现错误处理和重试机制。
- 考虑特定交易所的 API 速率限制。
- 使用 RestSharp 扩展方法简化请求和响应的处理。

请记住，具体实现可能会根据特定交易所 API 的要求和细节有所不同。