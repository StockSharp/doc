# REST 客户端

在为多个交易所开发连接器时，一个重要的组成部分是 HTTP 客户端，它负责与交易所的 REST API 进行交互。在 StockSharp 中，为此目的，开发连接器时通常会创建一个基于 RestSharp 库构建的 `HttpClient` 类。

## RestSharp 功能

[RestSharp](https://www.nuget.org/packages/RestSharp) 是一个用于处理 REST API 的流行 .NET 库。为了简化在 StockSharp 框架内使用 RestSharp，已经开发了一系列扩展方法，这些方法可以在 [Ecng 仓库](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs) 中找到。

## HttpClient 结构

`HttpClient` 通常包含以下关键要素：

1. **构造函数**
  - 初始化 RestClient
  - 配置基础 URL 和请求头

2. **不同类型请求的方法**
  - 例如 `GetProducts`、`GetCandles`、`GetTrades`

3. **交易操作相关方法**
  - 例如 `GetOrders`、`RegisterOrder`、`CancelOrder`

4. **辅助方法**
  - 用于构造请求
  - 用于处理响应

```cs
class HttpClient : BaseLogReceiver
{
	private readonly RestClient _restClient;
	private readonly Authenticator _authenticator;

	// 构造函数初始化 RestClient 并配置基础 URL
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

	// 从交易所获取产品（交易品种）列表的方法
	public async Task<IEnumerable<Product>> GetProducts(string type, CancellationToken cancellationToken)
	{
		var request = new RestRequest("products", Method.Get)
			.AddParameter("type", type);

		// 使用 ExecuteAsync 扩展方法执行请求
		var response = await _restClient.ExecuteAsync<List<Product>>(request, cancellationToken);
		return response.Data;
	}

	// 获取历史 K线的方法
	public async Task<IEnumerable<Candle>> GetCandles(string symbol, long start, long end, string granularity, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"products/{symbol}/candles", Method.Get)
			.AddParameter("start", start)
			.AddParameter("end", end)
			.AddParameter("granularity", granularity);

		var response = await _restClient.ExecuteAsync<List<Candle>>(request, cancellationToken);
		return response.Data;
	}

	// 注册新订单的方法
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

		// 执行请求前应用认证
		var response = await _restClient.ExecuteAsync<Order>(ApplyAuth(request), cancellationToken);
		return response.Data;
	}

	// 撤销现有订单的方法
	public async Task<bool> CancelOrder(string orderId, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"orders/{orderId}", Method.Delete);

		var response = await _restClient.ExecuteAsync(ApplyAuth(request), cancellationToken);
		return response.IsSuccessful;
	}

	// 将认证应用到请求的辅助方法
	private RestRequest ApplyAuth(RestRequest request)
	{
		_authenticator.ApplyAuthentication(request);
		return request;
	}
}
```

## 实现建议

- 使用异步方法以提高网络操作的效率。
- 针对不稳定的连接实现错误处理和重试机制。
- 考虑特定交易所的 API 速率限制。
- 使用 RestSharp 扩展方法简化请求和响应的处理。

请注意，具体实现可能会因特定交易所 API 的要求和细节而有所不同。
