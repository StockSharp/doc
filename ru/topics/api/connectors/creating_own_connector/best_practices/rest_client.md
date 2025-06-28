# REST клиент

При разработке коннектора для ряда бирж важным компонентом является HTTP клиент, который обеспечивает взаимодействие с REST API биржи. В StockSharp для этой цели часто при разработке коннектора создается класс `HttpClient`, построенный на основе библиотеки RestSharp.

## Особенности RestSharp

[RestSharp](https://www.nuget.org/packages/RestSharp) - это популярная .NET библиотека для работы с REST API. Для упрощения работы с RestSharp в рамках StockSharp разработаны методы-расширения, которые доступны в [репозитории Ecng](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs).

## Структура HttpClient

`HttpClient` обычно включает следующие ключевые элементы:

1. **Конструктор**
  - Инициализирует RestClient
  - Настраивает базовый URL и заголовки

2. **Методы для различных типов запросов**
  - Например, `GetProducts`, `GetCandles`, `GetTrades`

3. **Методы для работы с торговыми операциями**
  - Например, `GetOrders`, `RegisterOrder`, `CancelOrder`

4. **Вспомогательные методы**
  - Для формирования запросов
  - Для обработки ответов

```cs
class HttpClient : BaseLogReceiver
{
	private readonly RestClient _restClient;
	private readonly Authenticator _authenticator;

	// Конструктор инициализирует RestClient и настраивает базовый URL
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

	// Метод для получения списка продуктов (инструментов) с биржи
	public async Task<IEnumerable<Product>> GetProducts(string type, CancellationToken cancellationToken)
	{
		var request = new RestRequest("products", Method.Get)
			.AddParameter("type", type);

		// Используем метод-расширение ExecuteAsync для выполнения запроса
		var response = await _restClient.ExecuteAsync<List<Product>>(request, cancellationToken);
		return response.Data;
	}

	// Метод для получения исторических свечей
	public async Task<IEnumerable<Candle>> GetCandles(string symbol, long start, long end, string granularity, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"products/{symbol}/candles", Method.Get)
			.AddParameter("start", start)
			.AddParameter("end", end)
			.AddParameter("granularity", granularity);

		var response = await _restClient.ExecuteAsync<List<Candle>>(request, cancellationToken);
		return response.Data;
	}

	// Метод для регистрации новой заявки
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

		// Применяем аутентификацию перед выполнением запроса
		var response = await _restClient.ExecuteAsync<Order>(ApplyAuth(request), cancellationToken);
		return response.Data;
	}

	// Метод для отмены существующей заявки
	public async Task<bool> CancelOrder(string orderId, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"orders/{orderId}", Method.Delete);

		var response = await _restClient.ExecuteAsync(ApplyAuth(request), cancellationToken);
		return response.IsSuccessful;
	}

	// Вспомогательный метод для применения аутентификации к запросу
	private RestRequest ApplyAuth(RestRequest request)
	{
		_authenticator.ApplyAuthentication(request);
		return request;
	}
}
```

## Рекомендации по реализации

- Используйте асинхронные методы для эффективной работы с сетью.
- Реализуйте обработку ошибок и повторные попытки для нестабильных соединений.
- Учитывайте ограничения API (rate limits) конкретной биржи.
- Используйте методы-расширения RestSharp для упрощения работы с запросами и ответами.

Помните, что конкретная реализация может отличаться в зависимости от требований и особенностей API конкретной биржи.