# REST クライアント

多くの取引所向けコネクターを開発する場合、重要なコンポーネントの 1 つが HTTP クライアントです。これは取引所の REST API との相互作用を提供します。StockSharp では、この目的のために、コネクター開発時に RestSharp ライブラリを基盤とした `HttpClient` クラスが作成されることがよくあります。

## RestSharp の機能

[RestSharp](https://www.nuget.org/packages/RestSharp) は、REST API を扱うための人気のある .NET ライブラリです。StockSharp の枠組み内で RestSharp を扱いやすくするために、[Ecng リポジトリ](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/RestSharpHelper.cs) で利用できる拡張メソッドが開発されています。

## HttpClient の構造

`HttpClient` には通常、次の主要要素が含まれます。

1. **コンストラクター**
  - RestClient を初期化します
  - ベース URL とヘッダーを設定します

2. **さまざまな種類のリクエスト用メソッド**
  - 例: `GetProducts`、`GetCandles`、`GetTrades`

3. **取引操作を扱うためのメソッド**
  - 例: `GetOrders`、`RegisterOrder`、`CancelOrder`

4. **ヘルパーメソッド**
  - リクエストの形成用
  - レスポンスの処理用

```cs
class HttpClient : BaseLogReceiver
{
	private readonly RestClient _restClient;
	private readonly Authenticator _authenticator;

	// コンストラクターは RestClient を初期化し、ベース URL を設定します
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

	// 取引所からプロダクト（銘柄）の一覧を取得するメソッド
	public async Task<IEnumerable<Product>> GetProducts(string type, CancellationToken cancellationToken)
	{
		var request = new RestRequest("products", Method.Get)
			.AddParameter("type", type);

		// ExecuteAsync 拡張メソッドを使用してリクエストを実行
		var response = await _restClient.ExecuteAsync<List<Product>>(request, cancellationToken);
		return response.Data;
	}

	// 履歴キャンドルを取得するメソッド
	public async Task<IEnumerable<Candle>> GetCandles(string symbol, long start, long end, string granularity, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"products/{symbol}/candles", Method.Get)
			.AddParameter("start", start)
			.AddParameter("end", end)
			.AddParameter("granularity", granularity);

		var response = await _restClient.ExecuteAsync<List<Candle>>(request, cancellationToken);
		return response.Data;
	}

	// 新規注文を登録するメソッド
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

		// リクエストを実行する前に認証を適用
		var response = await _restClient.ExecuteAsync<Order>(ApplyAuth(request), cancellationToken);
		return response.Data;
	}

	// 既存注文をキャンセルするメソッド
	public async Task<bool> CancelOrder(string orderId, CancellationToken cancellationToken)
	{
		var request = new RestRequest($"orders/{orderId}", Method.Delete);

		var response = await _restClient.ExecuteAsync(ApplyAuth(request), cancellationToken);
		return response.IsSuccessful;
	}

	// リクエストに認証を適用するヘルパーメソッド
	private RestRequest ApplyAuth(RestRequest request)
	{
		_authenticator.ApplyAuthentication(request);
		return request;
	}
}
```

## 実装に関する推奨事項

- 効率的なネットワーク処理のために非同期メソッドを使用してください。
- 不安定な接続に備えて、エラー処理とリトライを実装してください。
- 対象取引所の API レート制限を考慮してください。
- リクエストとレスポンスの処理を簡素化するために、RestSharp 拡張メソッドを使用してください。

具体的な実装は、特定の取引所 API の要件や仕様によって異なる場合があることに留意してください。
