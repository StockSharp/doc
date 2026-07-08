# Ligação através do Protocolo FIX

O [Hydra](../../hydra.md) pode ser utilizado em modo de servidor, o que permite a ligação remota ao [Hydra](../../hydra.md) para aceder aos dados no armazenamento. A ativação do modo de servidor do [Hydra](../../hydra.md) é descrita na secção [Definições](settings.md).

Para ligar através do [Protocolo FIX](../../api/connectors/common/fix_protocol.md), é necessário criar e configurar uma ligação Fix ([Inicialização do adaptador FIX](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)).

```cs
// Criar uma instância do conector
private readonly Connector _connector = new Connector();

// Configurar o adaptador para dados de mercado através do protocolo FIX
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// Configurar o adaptador para dados transacionais
var transactionDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(transactionDataAdapter);
```

Subscreva eventos e configure os manipuladores de dados:

```cs
// Evento de ligação bem-sucedida
_connector.Connected += () =>
{
	Console.WriteLine("Connection established");
	
	// Criar uma subscrição para procurar instrumentos
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// Evento de perda de ligação
_connector.Disconnected += () =>
{
	Console.WriteLine("Connection lost");
};

// Evento de instrumento recebido
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"Instrument received: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);
	
	// Se este for o instrumento alvo, subscrever os respetivos dados
	if (security.Id == targetSecurityId)
	{
		// Subscrição do livro de ordens
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// Subscrição de transações tick
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);
		
		// Subscrição de candles
		var candleSubscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			security)
		{
			MarketData =
			{
				From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
				To = DateTime.Now
			}
		};
		_connector.Subscribe(candleSubscription);
	}
};

// Evento de transação tick recebida
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"Trade received: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// Evento de alteração do livro de ordens
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"Order book received: {depth.SecurityId}, Best bid: {depth.BestBid()?.Price}, Best ask: {depth.BestAsk()?.Price}");
};

// Evento de candle recebido
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"Candle received: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// Evento de erro de ligação
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Connection error: {error.Message}");
};

// Evento de erro geral
_connector.Error += error =>
{
	Console.WriteLine($"Error: {error.Message}");
};

// Evento de erro na subscrição de dados de mercado
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"Subscription error {subscription.DataType} for {subscription.SecurityId}: {error}");
};

// Ligar ao servidor
_connector.Connect();
```

## Utilizar Serviços Hydra

O Hydra em modo de servidor fornece acesso a vários tipos de dados. Vejamos exemplos de obtenção de dados históricos:

```cs
// Obter candles históricos
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// Criar uma subscrição para candles históricos
	var candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		security)
	{
		MarketData =
		{
			From = from,
			To = to
		}
	};
	
	// Subscrever para processar candles recebidos
	_connector.CandleReceived += OnCandleReceived;
	
	// Iniciar a subscrição
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Verificar se o candle pertence à nossa subscrição
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;
		
	Console.WriteLine($"Historical candle: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");
	
	// Processar os candles recebidos, por exemplo, guardar no armazenamento local
	// ou utilizar para análise/visualização
}
```

## Desligar do Servidor Hydra

```cs
// Encerramento correto da ligação
private void DisconnectFromServer()
{
	// Cancelar a subscrição de todas as subscrições
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}
	
	// Desligar do servidor
	_connector.Disconnect();
}
```
