# Cliente REST

Ao desenvolver um conector para diversas exchanges, um componente importante é o cliente HTTP, que fornece a interação com a API REST da exchange. No StockSharp, para esse propósito, uma classe `HttpClient` é frequentemente criada ao desenvolver um conector, construída com base na biblioteca RestSharp.

## Recursos do RestSharp

[RestSharp](https://www.nuget.org/packages/RestSharp) é uma biblioteca .NET popular para trabalhar com APIs REST. Para simplificar o trabalho com o RestSharp dentro do framework do StockSharp, foram desenvolvidos métodos de extensão que estão disponíveis no [repositório Ecng](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs).

## Estrutura do HttpClient

O `HttpClient` geralmente inclui os seguintes elementos-chave:

1. **Construtor**
  - Inicializa o RestClient
  - Configura a URL base e os cabeçalhos

2. **Métodos para diferentes tipos de solicitações**
  - Por exemplo, `GetProducts`, `GetCandles`, `GetTrades`

3. **Métodos para trabalhar com operações de negociação**
  - Por exemplo, `GetOrders`, `RegisterOrder`, `CancelOrder`

4. **Métodos auxiliares**
  - Para formar solicitações
  - Para processar respostas

```cs
class HttpClient : BaseLogReceiver
{
	private readonly RestClient _restClient;
	private readonly Authenticator _authenticator;

	// O construtor inicializa RestClient e configura a URL base
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

	// Método para obter uma lista de produtos (instrumentos) da bolsa
	public async Task<IEnumerable<Product>> GetProducts(string type, CancellationToken cancellationToken)
	{
		var request = new RestRequest("products", Method.Get)
			.AddParameter("type", type);

		// Usar o método de extensão ExecuteAsync para executar o pedido
		var response = await _restClient.ExecuteAsync<List<Product>>(request, cancellationToken);
		return response.Data;
	}

	// Método para obter velas históricas
	public async Task<IEnumerable<Candle>> GetCandles(string symbol, long start, long end, string granularity, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"products/{symbol}/candles", Method.Get)
			.AddParameter("start", start)
			.AddParameter("end", end)
			.AddParameter("granularity", granularity);

		var response = await _restClient.ExecuteAsync<List<Candle>>(request, cancellationToken);
		return response.Data;
	}

	// Método para registrar uma nova ordem
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

		// Aplicar autenticação antes de executar a solicitação
		var response = await _restClient.ExecuteAsync<Order>(ApplyAuth(request), cancellationToken);
		return response.Data;
	}

	// Método para cancelar uma ordem existente
	public async Task<bool> CancelOrder(string orderId, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"orders/{orderId}", Method.Delete);

		var response = await _restClient.ExecuteAsync(ApplyAuth(request), cancellationToken);
		return response.IsSuccessful;
	}

	// Método auxiliar para aplicar autenticação a uma solicitação
	private RestRequest ApplyAuth(RestRequest request)
	{
		_authenticator.ApplyAuthentication(request);
		return request;
	}
}
```

## Recomendações de Implementação

- Use métodos assíncronos para uma operação de rede eficiente.
- Implemente o tratamento de erros e novas tentativas para conexões instáveis.
- Leve em conta os limites de taxa da API da exchange específica.
- Use os métodos de extensão do RestSharp para simplificar o trabalho com solicitações e respostas.

Lembre-se de que a implementação específica pode variar dependendo dos requisitos e das especificidades da API de uma exchange em particular.
