# Dados de Mercado

Ao criar seu próprio adaptador para trabalhar com uma exchange, você precisa implementar métodos para assinar vários tipos de dados de mercado. Esses métodos são chamados quando uma mensagem [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) é recebida e fornecem **para** o recebimento e processamento de dados da exchange.

Esquematicamente, o algoritmo de processamento de uma solicitação de assinatura ou cancelamento de assinatura tem a seguinte aparência:

1. Envia uma confirmação de recebimento da solicitação de assinatura usando o método [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Verifica se a solicitação é de assinatura ou cancelamento de assinatura usando a propriedade [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe).
3. Em caso de assinatura, configura uma assinatura para receber dados em tempo real via WebSocket ou outro mecanismo (específico de cada exchange).
4. Em caso de cancelamento de assinatura, cancela a assinatura correspondente (específico de cada exchange).
5. Envia uma mensagem sobre o resultado da assinatura usando os métodos [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) ou [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset})), dependendo do tipo de assinatura e do resultado da operação.

## Dados de velas

Ao implementar uma assinatura para dados de candles em seu próprio adaptador, leve em conta como a exchange trabalha com esse tipo de dado. No Coinbase, os seguintes métodos e propriedades foram sobrescritos:

### Períodos suportados

A propriedade `TimeFrames` define a lista de períodos suportados pelo adaptador para candles. Isso permite que o StockSharp saiba quais períodos podem ser solicitados através desse adaptador.

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### Suporte para atualizações de velas

O método `IsSupportCandlesUpdates` determina se o adaptador suporta atualizações de candles em tempo real para uma solicitação de assinatura específica. No caso do Coinbase, apenas atualizações para candles de 5 minutos são suportadas.

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase só suporta velas de 5 minutos para atualização via WebSocket
	// Portanto, outros períodos serão construídos a partir de ticks (automaticamente pelo núcleo do StockSharp)
	return subscription.DataType2 == _tf5min;
}
```

A sobrescrita desses métodos e propriedades permite que o adaptador trate corretamente as solicitações de assinatura de dados de candles, levando em conta as especificidades da API do Coinbase. Por exemplo, se um período diferente de 5 minutos for solicitado, o StockSharp saberá que precisa usar dados de ticks para construir candles de outros períodos.

### Subscrever dados de velas

Para assinar dados de candles, o método **OnTFCandlesSubscriptionAsync** é implementado. Esse método, assim como o método de assinatura de dados de ticks, pode solicitar dados históricos, bem como configurar uma assinatura para receber novos candles em tempo real.

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmação de recebimento da solicitação de assinatura
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		var tf = mdMsg.GetTimeFrame();

		// Se forem solicitados dados históricos
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix();
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix();
			var left = mdMsg.Count ?? long.MaxValue;
			var step = (long)tf.Multiply(200).TotalSeconds;
			var granularity = mdMsg.GetTimeFrame().ToNative();

			while (from < to)
			{
				// Solicitar velas históricas
				var candles = await _restClient.GetCandles(symbol, from, from + step, granularity, cancellationToken);
				var needBreak = true;
				var last = from;

				foreach (var candle in candles.OrderBy(t => t.Time))
				{
					cancellationToken.ThrowIfCancellationRequested();

					var time = (long)candle.Time.ToUnix();

					if (time < from)
						continue;

					if (time > to)
					{
						needBreak = true;
						break;
					}

					// Enviar informações sobre cada vela histórica
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// Ao identificar dados pela assinatura, não é necessário preencher as informações do instrumento
						OriginalTransactionId = mdMsg.TransactionId,
					}, cancellationToken);

					if (--left <= 0)
					{
						needBreak = true;
						break;
					}

					last = time;
					needBreak = false;
				}

				if (needBreak)
					break;

				from = last;
			}
		}

		if (!mdMsg.IsHistoryOnly() && mdMsg.DataType2 == _tf5min)
		{
			// Assinar recebimento de novas velas em tempo real
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// Notificar que a assinatura passou para o status online
			await SendSubscriptionResultAsync(mdMsg, cancellationToken);
		}
		else
		{
			// Send a response that the subscription is finished (not online)
			await SendSubscriptionFinishedAsync(mdMsg.TransactionId, cancellationToken);
		}
	}
	else
	{
		// Cancelar recebimento de velas
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### Processar dados de velas

Para processar dados de candles recebidos da exchange em tempo real, um método com um código semelhante ao do método **SessionOnCandleReceived** geralmente é implementado. Esse método converte os dados recebidos em uma mensagem [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) e a envia usando o método SendOutMessageAsync.

```cs
private async ValueTask SessionOnCandleReceived(Ohlc candle, CancellationToken cancellationToken)
{
	// Verificar se há uma assinatura ativa de velas para este instrumento
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// Criar e enviar mensagem sobre uma nova vela
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // The candle is considered active as it may still change

		// Ao identificar dados pela assinatura, não é necessário preencher as informações do instrumento
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## Nível 1 (Melhores Preços de Compra e Venda, Último Preço)

### Assinando Dados de Nível 1

Para assinar mudanças de Nível 1, o método **OnLevel1SubscriptionAsync** é implementado. Esse método geralmente executa as seguintes ações:

```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmação de recebimento da solicitação de assinatura
	// Isso informa ao sistema que a solicitação foi recebida e está sendo processada
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Converter o identificador do instrumento em um símbolo compreendido pela exchange
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Se for um pedido de subscrição
		// Assinar recebimento de dados Level1 via WebSocket
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// Enviar mensagem de assinatura bem-sucedida
		// Isso informa ao sistema que a assinatura foi configurada e os dados serão recebidos
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Se for um pedido de cancelamento da subscrição
		// Cancelar assinatura de recebimento de dados Level1
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### Processando Dados de Nível 1

Para processar dados de Nível 1 recebidos da exchange em tempo real, um método com um código semelhante ao do exemplo **SessionOnTickerChanged** geralmente é implementado. Esse método converte os dados recebidos em uma mensagem [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) e a envia usando o método SendOutMessageAsync.

```cs
private async ValueTask SessionOnTickerChanged(Ticker ticker, CancellationToken cancellationToken)
{
	// Criar mensagem com alterações de dados Level1
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// Especificar identificador do instrumento
		SecurityId = ticker.Product.ToStockSharp(),
		// Definir hora de recebimento dos dados
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// Adicionar vários campos Level1 se estiverem presentes nos dados da exchange
	.TryAdd(Level1Fields.LastTradeId, ticker.LastTradeId)
	.TryAdd(Level1Fields.LastTradePrice, ticker.LastTradePrice?.ToDecimal())
	.TryAdd(Level1Fields.LastTradeVolume, ticker.LastTradePrice?.ToDecimal())
	.TryAdd(Level1Fields.LastTradeOrigin, ticker.LastTradeSide?.ToSide())
	.TryAdd(Level1Fields.HighPrice, ticker.High?.ToDecimal())
	.TryAdd(Level1Fields.LowPrice, ticker.Low?.ToDecimal())
	.TryAdd(Level1Fields.Volume, ticker.Volume?.ToDecimal())
	.TryAdd(Level1Fields.Change, ticker.Change?.ToDecimal())
	.TryAdd(Level1Fields.BestBidPrice, ticker.Bid?.ToDecimal())
	.TryAdd(Level1Fields.BestAskPrice, ticker.Ask?.ToDecimal())
	.TryAdd(Level1Fields.BestBidVolume, ticker.BidSize?.ToDecimal())
	.TryAdd(Level1Fields.BestAskVolume, ticker.AskSize?.ToDecimal())
	, cancellationToken);
}
```

## Livro de Ofertas

### Suporte para Atualizações Incrementais do Livro de Ofertas

Ao implementar a funcionalidade do livro de ofertas em seu próprio adaptador, verifique se a exchange suporta atualizações incrementais do livro de ofertas. O adaptador Coinbase sobrescreve a propriedade `IsSupportOrderBookIncrements` para isso:

```cs
public override bool IsSupportOrderBookIncrements => true;
```

A propriedade `IsSupportOrderBookIncrements` indica se o adaptador suporta atualizações incrementais do livro de ofertas. Definir essa propriedade como `true` significa que a exchange pode enviar atualizações parciais do livro de ofertas em vez de um snapshot completo a cada mudança.

A sobrescrita dessa propriedade permite que o StockSharp otimize o processamento dos dados do livro de ofertas. Se a propriedade for definida como `true`, o sistema esperará e tratará corretamente as atualizações incrementais.

### Assinando Dados do Livro de Ofertas

Para assinar mudanças do livro de ofertas, o método **OnMarketDepthSubscriptionAsync** é implementado. Esse método executa ações semelhantes ao método OnLevel1SubscriptionAsync, mas para dados do livro de ofertas.

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmação de recebimento da solicitação de assinatura
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Converter o identificador do instrumento em um símbolo compreendido pela exchange
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Se for um pedido de subscrição
		// Assinar recebimento de dados do livro de ofertas via WebSocket
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// Enviar mensagem de assinatura bem-sucedida
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Se for um pedido de cancelamento da subscrição
		// Cancelar assinatura de recebimento de dados do livro de ofertas
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### Processando Dados do Livro de Ofertas

Para processar dados do livro de ofertas recebidos da exchange em tempo real, um método com um código semelhante ao do método **SessionOnOrderBookReceived** geralmente é implementado. Esse método converte os dados recebidos em uma mensagem [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) e a envia usando o método SendOutMessageAsync.

```cs
private async ValueTask SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes, CancellationToken cancellationToken)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// Distribuir alterações por bids e asks
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// Criar e enviar mensagem com alterações no livro de ofertas
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// Determinar se é um snapshot completo do livro de ofertas ou uma atualização incremental.
		// Se a bolsa enviar sempre apenas livros de ofertas completos e não suportar incrementalidade,
		// então não é necessário definir esta propriedade
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## Dados de Ticks (Negociações)

### Assinando Dados de Ticks

Para assinar dados de ticks, o método **OnTicksSubscriptionAsync** é implementado. Esse método, além de executar ações semelhantes aos métodos de assinatura anteriores, também pode solicitar dados históricos, caso especificado na solicitação.

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmação de recebimento da solicitação de assinatura
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Se forem solicitados dados históricos
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix(false);
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix(false);
			var left = mdMsg.Count ?? long.MaxValue;

			while (from < to)
			{
				// Solicitar negociações históricas
				var trades = await _restClient.GetTrades(symbol, from, to, cancellationToken);
				var needBreak = true;
				var last = from;

				foreach (var trade in trades.OrderBy(t => t.Time))
				{
					cancellationToken.ThrowIfCancellationRequested();

					var time = (long)trade.Time.ToUnix();

					if (time < from)
						continue;

					if (time > to)
					{
						needBreak = true;
						break;
					}

					// Enviar informações sobre cada negociação histórica
					await SendOutMessageAsync(new ExecutionMessage
					{
						// Definir que a mensagem contém informações sobre uma negociação tick
						// (not a transaction like an order or own trade)
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// For history, always set the subscription identifier,
						// para que o código externo entenda para qual assinatura os dados foram recebidos.
						// Ao identificar dados pela assinatura, não é necessário preencher as informações do instrumento
						OriginalTransactionId = mdMsg.TransactionId,
					}, cancellationToken);

					if (--left <= 0)
					{
						needBreak = true;
						break;
					}

					last = time;
					needBreak = false;
				}

				if (needBreak)
					break;

				from = last;
			}
		}

		if (!mdMsg.IsHistoryOnly())
		{
			// Assinar recebimento de novas negociações em tempo real
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// Enviar mensagem de assinatura bem-sucedida
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Cancelar recebimento de negociações em tempo real
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### Processando Dados de Ticks

Para processar dados de ticks recebidos da exchange em tempo real, um método com um código semelhante ao do método **SessionOnTradeReceived** geralmente é implementado. Esse método converte os dados recebidos em uma mensagem [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) com o tipo [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) e a envia usando o método SendOutMessageAsync.

```cs
private async ValueTask SessionOnTradeReceived(Trade trade, CancellationToken cancellationToken)
{
	// Criar e enviar mensagem sobre uma nova negociação
	await SendOutMessageAsync(new ExecutionMessage
	{
		// Definir que a mensagem contém informações sobre uma negociação tick
		// (not a transaction like an order or own trade)
		DataTypeEx = DataType.Ticks,

		SecurityId = trade.ProductId.ToStockSharp(),
		TradeId = trade.TradeId,
		TradePrice = (decimal)trade.Price,
		TradeVolume = (decimal)trade.Size,
		ServerTime = trade.Time,
		OriginSide = trade.Side.ToSide(),
	}, cancellationToken);
}
```

## Subscrição ao log de ordens

O order log é uma informação detalhada sobre todas as mudanças no livro de ofertas, incluindo a adição, modificação e exclusão de ordens. Esses dados são específicos e não são fornecidos por todas as fontes de dados. Por exemplo, o Coinbase não suporta o fornecimento de order log.

Para implementar uma assinatura de order log em um adaptador, o método **OnOrderLogSubscriptionAsync** é usado. Esse método é chamado quando uma mensagem [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) com o tipo de dado [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog) é recebida.

Abaixo está um exemplo de implementação desse método retirado do conector [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp), que suporta order log:

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmação de recebimento da solicitação de assinatura
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Converter identificador do instrumento em par de moedas
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// Assinar recebimento do log de ordens em tempo real
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// Enviar mensagem de assinatura bem-sucedida
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// Cancelar recebimento do log de ordens
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

Ao processar dados de order log recebidos da exchange, um método separado geralmente é usado, que converte os dados recebidos em mensagens [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) com o tipo [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog):

```cs
private async ValueTask SessionOnNewOrderLog(string symbol, OrderStates state, Order order, CancellationToken cancellationToken)
{
	// Criar e enviar mensagem com informações sobre uma nova entrada no log de ordens
	await SendOutMessageAsync(new ExecutionMessage
	{
		DataTypeEx = DataType.OrderLog,
		SecurityId = symbol.ToStockSharp(),
		ServerTime = order.Time,
		OrderVolume = (decimal)order.Amount,
		OrderPrice = (decimal)order.Price,
		OrderId = order.Id,
		Side = order.Type.ToSide(),
		OrderState = state,
	}, cancellationToken);
}
```

Adicione suporte para esse tipo de dado no construtor do adaptador:

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## Especificidades do Processamento de Dados Históricos e ao Vivo

Ao implementar solicitações de dados históricos e o processamento de dados ao vivo em seu próprio adaptador, tenha em mente os seguintes pontos:

### Dados Históricos

Ao enviar dados históricos em resposta a uma solicitação:

1. Definir [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) é obrigatório. Isso permite que o sistema associe os dados recebidos à solicitação original.

2. Definir [SecurityId](xref:StockSharp.Messages.SecurityId) ou [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) (no caso de candles) não é obrigatório, mas também não é proibido. O núcleo do StockSharp preencherá automaticamente esses campos com os valores necessários a partir da solicitação original.

### Dados ao Vivo

Ao processar dados ao vivo, por exemplo, recebidos via WebSocket:

1. Definir [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) é opcional. Se o ID da transação não for definido, o sistema distribuirá os dados para todas as assinaturas ativas do instrumento e tipo de dado correspondentes.

2. Definir [SecurityId](xref:StockSharp.Messages.SecurityId) e outros campos específicos (por exemplo, [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) para candles) é obrigatório, pois essa informação é necessária para o roteamento correto dos dados no sistema.
