# Маркет данные

При создании собственного адаптера для работы с биржей необходимо реализовать методы для подписки на различные виды рыночных данных. Эти методы вызываются при получении сообщения [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) и обеспечивают получение и обработку данных от биржи.

Схематично алгоритм обработки запроса на подписку или отписку выглядит так:

1. Отправляет подтверждение о получении запроса на подписку с помощью метода [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Threading.CancellationToken,System.Exception)).
2. Проверяет, является ли запрос подпиской или отпиской, используя свойство [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe).
3. В случае подписки устанавливает подписку на получение данных в реальном времени через WebSocket или другой механизм (специфично для каждой биржи).
4. В случае отписки отменяет соответствующую подписку (специфично для каждой биржи).
5. Отправляет сообщение о результате подписки с помощью методов [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage,System.Threading.CancellationToken)) или [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Threading.CancellationToken,System.Nullable{System.DateTime})), в зависимости от типа подписки и результата операции.

## Свечные данные

При реализации подписки на свечные данные в собственном адаптере важно учитывать особенности работы конкретной биржи с этим типом данных. В случае с Coinbase были переопределены следующие методы и свойства:

### Поддерживаемые таймфреймы

Свойство `TimeFrames` определяет список поддерживаемых адаптером таймфреймов для свечей. Это позволяет StockSharp знать, какие таймфреймы можно запрашивать через данный адаптер.

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### Поддержка обновлений свечей

Метод `IsSupportCandlesUpdates` определяет, поддерживает ли адаптер обновления свечей в реальном времени для конкретного запроса подписки. В случае с Coinbase, поддерживаются только обновления для 5-минутных свечей.

```cs
private static readonly DataType _tf5min = TimeSpan.FromMinutes(5).TimeFrame();

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
        // Coinbase поддерживает только 5-минутки для обновления через веб сокеты
        // Поэтому другие ТФ будут строиться из тиков (автоматически ядром StockSharp)
	return subscription.DataType2 == _tf5min;
}
```

Переопределение этих методов и свойств позволяет адаптеру корректно обрабатывать запросы на подписку на свечные данные, учитывая особенности API Coinbase. Например, если запрашивается таймфрейм, отличный от 5 минут, StockSharp будет знать, что нужно использовать тиковые данные для построения свечей других таймфреймов.

### Подписка на свечные данные

Для подписки на свечные данные реализуется метод **OnTFCandlesSubscriptionAsync**. Этот метод, как и метод подписки на тиковые данные, может запрашивать исторические данные, а также устанавливать подписку на получение новых свечей в реальном времени.

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Отправляем подтверждение о получении запроса на подписку
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		var tf = mdMsg.GetTimeFrame();

		// Если запрошены исторические данные
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix();
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix();
			var left = mdMsg.Count ?? long.MaxValue;
			var step = (long)tf.Multiply(200).TotalSeconds;
			var granularity = mdMsg.GetTimeFrame().ToNative();

			while (from < to)
			{
				// Запрашиваем исторические свечи
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

					// Отправляем информацию о каждой исторической свече
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// в случае идентификации данных по подписке заполнение информации об инструменте не требуется
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
			// Подписываемся на получение новых свечей в реальном времени
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// извещаем что подписка перешла в статус online
			await SendSubscriptionResultAsync(mdMsg, cancellationToken);
		}
		else
		{
			// отправляем ответ что подписка завершена (не онлайн)
			await SendSubscriptionFinishedAsync(mdMsg.TransactionId, cancellationToken);
		}
	}
	else
	{
		// Отписываемся от получения свечей
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### Обработка свечных данных

Для обработки свечных данных, полученных от биржи в режиме реального времени, обычно реализуется метод с кодом как в методе **SessionOnCandleReceived**. Этот метод преобразует полученные данные в сообщение [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) и отправляет его с помощью метода SendOutMessageAsync.

```cs
private void SessionOnCandleReceived(Ohlc candle)
{
	// Проверяем, есть ли активная подписка на свечи для данного инструмента
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// Создаем и отправляем сообщение о новой свече
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // Свеча считается активной, так как она может еще измениться

		// в случае идентификации данных по подписке заполнение информации об инструменте не требуется
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## Level 1 (лучшие цены покупки и продажи, последняя цена)

### Подписка на данные Level 1

Для подписки на изменения Level 1 реализуется метод **OnLevel1SubscriptionAsync**. Этот метод обычно выполняет следующие действия:


```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Отправляем подтверждение о получении запроса на подписку
	// Это информирует систему о том, что запрос получен и обрабатывается
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Преобразуем идентификатор инструмента в символ, понятный бирже
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Если это запрос на подписку
		// Подписываемся на получение данных Level 1 через WebSocket
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// Отправляем сообщение об успешной подписке
		// Это информирует систему о том, что подписка установлена и данные будут поступать
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Если это запрос на отписку
		// Отменяем подписку на получение данных Level 1
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### Обработка данных Level 1

Для обработки данных Level 1, полученных от биржи в режиме реального времени, обычно реализуется метод с кодом, как в примере **SessionOnTickerChanged**. Этот метод преобразует полученные данные в сообщение [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) и отправляет его с помощью метода SendOutMessageAsync.

```cs
private void SessionOnTickerChanged(Ticker ticker)
{
	// Создаем сообщение с изменениями данных Level 1
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// Указываем идентификатор инструмента
		SecurityId = ticker.Product.ToStockSharp(),
		// Устанавливаем время получения данных
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// Добавляем различные поля Level 1, если они присутствуют в данных от биржи
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

## Стакан котировок

### Поддержка инкрементальных обновлений стакана

При реализации работы со стаканом котировок в собственном адаптере важно учитывать, поддерживает ли биржа инкрементальные обновления стакана. Для этого в адаптере Coinbase было переопределено `IsSupportOrderBookIncrements` свойство:

```cs
public override bool IsSupportOrderBookIncrements => true;
```

Свойство `IsSupportOrderBookIncrements` указывает, поддерживает ли адаптер инкрементальные обновления стакана. Установка этого свойства в `true` означает, что биржа может отправлять частичные обновления стакана, а не полный снапшот при каждом изменении.

Переопределение этого свойства позволяет StockSharp оптимизировать обработку данных стакана. Если свойство установлено в `true`, система будет ожидать и корректно обрабатывать инкрементальные обновления.

### Подписка на данные стакана

Для подписки на изменения стакана котировок реализуется метод **OnMarketDepthSubscriptionAsync**. Этот метод выполняет действия, аналогичные методу OnLevel1SubscriptionAsync, но для данных стакана.

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Отправляем подтверждение о получении запроса на подписку
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Преобразуем идентификатор инструмента в символ, понятный бирже
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Если это запрос на подписку
		// Подписываемся на получение данных стакана через WebSocket
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// Отправляем сообщение об успешной подписке
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Если это запрос на отписку
		// Отменяем подписку на получение данных стакана
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### Обработка данных стакана

Для обработки данных стакана, полученных от биржи в режиме реального времени, обычно реализуется метод с кодом как в методе **SessionOnOrderBookReceived**. Этот метод преобразует полученные данные в сообщение [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) и отправляет его с помощью метода SendOutMessageAsync.

```cs
private void SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// Распределяем изменения по спросу и предложению
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// Создаем и отправляем сообщение с изменениями в стакане
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// Определяем, является ли это полным снимком стакана или инкрементальным обновлением.
		// Если же бирже передает всегда только целые стаканы и не поддерживает инкрементальность,
		// то установка этого свойства не требуется вообще
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## Тиковые данные (сделки)

### Подписка на тиковые данные

Для подписки на тиковые данные реализуется метод **OnTicksSubscriptionAsync**. Этот метод, помимо действий, аналогичных предыдущим методам подписки, также может запрашивать исторические данные, если это указано в запросе.

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Отправляем подтверждение о получении запроса на подписку
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Если запрошены исторические данные
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix(false);
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix(false);
			var left = mdMsg.Count ?? long.MaxValue;

			while (from < to)
			{
				// Запрашиваем исторические сделки
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

					// Отправляем информацию о каждой исторической сделке
					await SendOutMessageAsync(new ExecutionMessage
					{
						// устанавливаем что сообщение несет информацию о тиковой сделке
						// (а не о транзакции как заявка или собственная сделка)
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// для истории всегда устанавливает идентификатор подписки,
						// чтобы внешний код смог понять, к какой подписке были получены данные.
						// в случае идентификации данных по подписке заполнение информации об инструменте не требуется
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
			// Подписываемся на получение новых сделок в реальном времени
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// Отправляем сообщение об успешной подписке
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Отписываемся от получения сделок в реальном времени
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### Обработка тиковых данных

Для обработки тиковых данных, полученных от биржи в режиме реального времени, обычно реализуется метод с кодом как в методе **SessionOnTradeReceived**. Этот метод преобразует полученные данные в сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) с типом [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) и отправляет его с помощью метода SendOutMessageAsync.

```cs
private void SessionOnTradeReceived(Trade trade)
{
	// Создаем и отправляем сообщение о новой сделке
	await SendOutMessageAsync(new ExecutionMessage
	{
		// устанавливаем что сообщение несет информацию о тиковой сделке
		// (а не о транзакции как заявка или собственная сделка)
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

## Подписка на лог заявок (Order Log)

Лог заявок - это детальная информация о всех изменениях в книге заявок, включая добавление, изменение и удаление заявок. Эти данные специфичны и предоставляются не всеми источниками данных. Например, Coinbase не поддерживает предоставление лога заявок.

Для реализации подписки на лог заявок в адаптере используется метод **OnOrderLogSubscriptionAsync**. Этот метод вызывается при получении сообщения [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) с типом данных [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog).

Ниже приведен пример реализации этого метода, взятый из коннектора [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp), поддерживающего лог заявок:

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Отправляем подтверждение о получении запроса на подписку
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Преобразуем идентификатор инструмента в валютную пару
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// Подписываемся на получение лога заявок в реальном времени
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// Отправляем сообщение об успешной подписке
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// Отписываемся от получения лога заявок
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

При обработке данных лога заявок, полученных от биржи, обычно используется отдельный метод, который преобразует полученные данные в сообщения [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) с типом [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog):

```cs
private void SessionOnNewOrderLog(string symbol, OrderStates state, Order order)
{
	// Создаем и отправляем сообщение с информацией о новой записи в логе заявок
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

Важно не забыть добавить поддержку этого типа данных в конструкторе адаптера:

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## Особенности обработки исторических и live данных

При реализации запросов на исторические данные и обработке live данных в собственном адаптере важно учитывать следующие моменты:

### Исторические данные

При отправке исторических данных в ответ на запрос:

1. Установка [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) является обязательной. Это позволяет системе связать полученные данные с исходным запросом.

2. Установка [SecurityId](xref:StockSharp.Messages.SecurityId) или [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) (в случае свечей) не требуется, но и не запрещена. Ядро StockSharp автоматически заполнит эти поля нужными значениями из исходного запроса.

### Live данные

При обработке live данных, например, получаемых через WebSocket:

1. Установка [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) является опциональной. Если ID транзакции не установлен, система будет распространять данные на все активные подписки для соответствующего инструмента и типа данных.

2. Установка [SecurityId](xref:StockSharp.Messages.SecurityId) и других специфичных полей (например, [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) для свечей) обязательна, так как эта информация необходима для правильной маршрутизации данных в системе.