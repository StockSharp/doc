# Marktdaten

Beim Erstellen eines eigenen Adapters für die Arbeit mit einer Börse müssen Sie Methoden zum Abonnieren verschiedener Arten von Marktdaten implementieren. Diese Methoden werden aufgerufen, wenn eine Nachricht [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) empfangen wird, und sorgen **für** den Empfang und die Verarbeitung von Daten von der Börse.

Schematisch sieht der Algorithmus zur Verarbeitung einer Abonnement- oder Kündigungsanfrage wie folgt aus:

1. Sendet eine Bestätigung über den Empfang der Abonnementanfrage mithilfe der Methode [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Prüft, ob es sich bei der Anfrage um ein Abonnement oder eine Kündigung handelt, mithilfe der Eigenschaft [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe).
3. Im Falle eines Abonnements wird ein Abonnement zum Empfang von Echtzeitdaten über WebSocket oder einen anderen Mechanismus (spezifisch für jede Börse) eingerichtet.
4. Im Falle einer Kündigung wird das entsprechende Abonnement gekündigt (spezifisch für jede Börse).
5. Sendet eine Nachricht über das Abonnementergebnis mithilfe der Methoden [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) oder [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset})), abhängig vom Abonnementtyp und dem Operationsergebnis.

## Kerzendaten

Bei der Implementierung eines Abonnements für Kerzendaten in Ihrem eigenen Adapter berücksichtigen Sie, wie die Börse mit diesem Datentyp arbeitet. Bei Coinbase wurden die folgenden Methoden und Eigenschaften überschrieben:

### Unterstützte Zeitrahmen

Die Eigenschaft `TimeFrames` definiert die Liste der vom Adapter unterstützten Zeitrahmen für Kerzen. Dies ermöglicht es StockSharp zu wissen, welche Zeitrahmen über diesen Adapter angefragt werden können.

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### Unterstützung für Kerzen-Updates

Die Methode `IsSupportCandlesUpdates` bestimmt, ob der Adapter Echtzeit-Kerzen-Updates für eine bestimmte Abonnementanfrage unterstützt. Im Fall von Coinbase werden nur Updates für 5-Minuten-Kerzen unterstützt.

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase unterstützt Aktualisierungen per WebSocket nur für 5-Minuten-Kerzen
	// Daher werden andere Zeitrahmen aus Ticks erstellt (automatisch durch den StockSharp-Kern)
	return subscription.DataType2 == _tf5min;
}
```

Das Überschreiben dieser Methoden und Eigenschaften ermöglicht es dem Adapter, Anfragen zum Abonnieren von Kerzendaten unter Berücksichtigung der Besonderheiten der Coinbase-API korrekt zu behandeln. Wenn beispielsweise ein anderer Zeitrahmen als 5 Minuten angefragt wird, weiß StockSharp, dass Tick-Daten verwendet werden müssen, um Kerzen anderer Zeitrahmen zu erstellen.

### Abonnieren von Kerzendaten

Um Kerzendaten zu abonnieren, wird die Methode **OnTFCandlesSubscriptionAsync** implementiert. Diese Methode kann, ähnlich wie die Methode zum Abonnieren von Tick-Daten, historische Daten anfordern sowie ein Abonnement zum Empfang neuer Kerzen in Echtzeit einrichten.

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Bestätigung über den Empfang der Abonnementanfrage senden
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		var tf = mdMsg.GetTimeFrame();

		// Wenn historische Daten angefordert werden
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix();
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix();
			var left = mdMsg.Count ?? long.MaxValue;
			var step = (long)tf.Multiply(200).TotalSeconds;
			var granularity = mdMsg.GetTimeFrame().ToNative();

			while (from < to)
			{
				// Historische Kerzen anfordern
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

					// Informationen zu jeder historischen Kerze senden
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// Bei Identifikation der Daten über das Abonnement müssen Instrumentinformationen nicht ausgefüllt werden
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
			// Neue Kerzen in Echtzeit abonnieren
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// Melden, dass das Abonnement in den Online-Status gewechselt ist
			await SendSubscriptionResultAsync(mdMsg, cancellationToken);
		}
		else
		{
			// Eine Antwort senden, dass das Abonnement abgeschlossen ist (nicht online)
			await SendSubscriptionFinishedAsync(mdMsg.TransactionId, cancellationToken);
		}
	}
	else
	{
		// Kerzenempfang abbestellen
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### Verarbeitung von Kerzendaten

Um Kerzendaten zu verarbeiten, die in Echtzeit von der Börse empfangen werden, wird üblicherweise eine Methode mit Code wie in der Methode **SessionOnCandleReceived** implementiert. Diese Methode konvertiert die empfangenen Daten in eine Nachricht [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) und sendet sie mithilfe der Methode SendOutMessageAsync.

```cs
private async ValueTask SessionOnCandleReceived(Ohlc candle, CancellationToken cancellationToken)
{
	// Prüfen, ob für dieses Instrument ein aktives Kerzenabonnement vorhanden ist
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// Nachricht über eine neue Kerze erstellen und senden
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // Die Kerze gilt als aktiv, da sie sich noch ändern kann

		// Bei Identifikation der Daten über das Abonnement müssen Instrumentinformationen nicht ausgefüllt werden
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## Level 1 (beste Geld- und Briefkurse, letzter Preis)

### Abonnieren von Level-1-Daten

Um Level-1-Änderungen zu abonnieren, wird die Methode **OnLevel1SubscriptionAsync** implementiert. Diese Methode führt in der Regel die folgenden Aktionen aus:

```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Bestätigung über den Empfang der Abonnementanfrage senden
	// Dies informiert das System, dass die Anfrage empfangen wurde und verarbeitet wird
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Instrumentkennung in ein von der Börse verstandenes Symbol umwandeln
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Wenn es sich um eine Abonnementanfrage handelt
		// Level1-Daten per WebSocket abonnieren
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// Nachricht über erfolgreiches Abonnement senden
		// Dies informiert das System, dass das Abonnement eingerichtet ist und Daten empfangen werden
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Wenn es sich um eine Kündigungsanfrage handelt
		// Abonnement für Level1-Daten stornieren
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### Verarbeitung von Level-1-Daten

Um Level-1-Daten zu verarbeiten, die in Echtzeit von der Börse empfangen werden, wird üblicherweise eine Methode mit Code wie im Beispiel **SessionOnTickerChanged** implementiert. Diese Methode konvertiert die empfangenen Daten in eine Nachricht [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) und sendet sie mithilfe der Methode SendOutMessageAsync.

```cs
private async ValueTask SessionOnTickerChanged(Ticker ticker, CancellationToken cancellationToken)
{
	// Nachricht mit Level1-Datenänderungen erstellen
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// Instrumentkennung angeben
		SecurityId = ticker.Product.ToStockSharp(),
		// Zeitpunkt des Datenempfangs festlegen
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// Verschiedene Level1-Felder hinzufügen, falls sie in den Börsendaten vorhanden sind
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

## Orderbuch

### Unterstützung für inkrementelle Orderbuch-Updates

Bei der Implementierung der Orderbuch-Funktionalität in Ihrem eigenen Adapter prüfen Sie, ob die Börse inkrementelle Orderbuch-Updates unterstützt. Der Coinbase-Adapter überschreibt dafür die Eigenschaft `IsSupportOrderBookIncrements`:

```cs
public override bool IsSupportOrderBookIncrements => true;
```

Die Eigenschaft `IsSupportOrderBookIncrements` gibt an, ob der Adapter inkrementelle Orderbuch-Updates unterstützt. Wenn diese Eigenschaft auf `true` gesetzt ist, bedeutet dies, dass die Börse bei jeder Änderung partielle Orderbuch-Updates anstelle eines vollständigen Snapshots senden kann.

Das Überschreiben dieser Eigenschaft ermöglicht es StockSharp, die Verarbeitung von Orderbuchdaten zu optimieren. Wenn die Eigenschaft auf `true` gesetzt ist, erwartet das System inkrementelle Updates und behandelt sie korrekt.

### Abonnieren von Orderbuchdaten

Um Orderbuch-Änderungen zu abonnieren, wird die Methode **OnMarketDepthSubscriptionAsync** implementiert. Diese Methode führt Aktionen aus, die der Methode OnLevel1SubscriptionAsync ähneln, jedoch für Orderbuchdaten.

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Bestätigung über den Empfang der Abonnementanfrage senden
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Instrumentkennung in ein von der Börse verstandenes Symbol umwandeln
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Wenn es sich um eine Abonnementanfrage handelt
		// Orderbuchdaten per WebSocket abonnieren
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// Nachricht über erfolgreiches Abonnement senden
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Wenn es sich um eine Kündigungsanfrage handelt
		// Abonnement für Orderbuchdaten stornieren
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### Verarbeitung von Orderbuchdaten

Um Orderbuchdaten zu verarbeiten, die in Echtzeit von der Börse empfangen werden, wird üblicherweise eine Methode mit Code wie in der Methode **SessionOnOrderBookReceived** implementiert. Diese Methode konvertiert die empfangenen Daten in eine Nachricht [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) und sendet sie mithilfe der Methode SendOutMessageAsync.

```cs
private async ValueTask SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes, CancellationToken cancellationToken)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// Änderungen nach Bids und Asks aufteilen
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// Nachricht mit Orderbuchänderungen erstellen und senden
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// Bestimmen, ob dies ein vollständiger Orderbuch-Snapshot oder ein inkrementelles Update ist.
		// Wenn die Börse immer nur vollständige Orderbücher sendet und keine inkrementellen Updates unterstützt,
		// dann muss diese Eigenschaft überhaupt nicht gesetzt werden
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## Tick-Daten (Trades)

### Abonnieren von Tick-Daten

Um Tick-Daten zu abonnieren, wird die Methode **OnTicksSubscriptionAsync** implementiert. Diese Methode kann zusätzlich zu Aktionen, die den vorherigen Abonnementmethoden ähneln, auch historische Daten anfordern, sofern dies in der Anfrage angegeben ist.

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Bestätigung über den Empfang der Abonnementanfrage senden
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Wenn historische Daten angefordert werden
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix(false);
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix(false);
			var left = mdMsg.Count ?? long.MaxValue;

			while (from < to)
			{
				// Historische Trades anfordern
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

					// Informationen zu jedem historischen Trade senden
					await SendOutMessageAsync(new ExecutionMessage
					{
						// Festlegen, dass die Nachricht Informationen über einen Tick-Trade enthält
						// (keine Transaktion wie eine Order oder ein eigener Trade)
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// Für Historie immer den Abonnementbezeichner setzen,
						// damit externer Code erkennt, für welches Abonnement die Daten empfangen wurden.
						// Bei Identifikation der Daten über das Abonnement müssen Instrumentinformationen nicht ausgefüllt werden
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
			// Neue Trades in Echtzeit abonnieren
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// Nachricht über erfolgreiches Abonnement senden
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Trades in Echtzeit abbestellen
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### Verarbeitung von Tick-Daten

Um Tick-Daten zu verarbeiten, die in Echtzeit von der Börse empfangen werden, wird üblicherweise eine Methode mit Code wie in der Methode **SessionOnTradeReceived** implementiert. Diese Methode konvertiert die empfangenen Daten in eine Nachricht [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) vom Typ [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) und sendet sie mithilfe der Methode SendOutMessageAsync.

```cs
private async ValueTask SessionOnTradeReceived(Trade trade, CancellationToken cancellationToken)
{
	// Nachricht über einen neuen Trade erstellen und senden
	await SendOutMessageAsync(new ExecutionMessage
	{
		// Festlegen, dass die Nachricht Informationen über einen Tick-Trade enthält
		// (keine Transaktion wie eine Order oder ein eigener Trade)
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

## Abonnieren des Orderprotokolls

Das Orderprotokoll enthält detaillierte Informationen über alle Änderungen im Orderbuch, einschließlich des Hinzufügens, Änderns und Löschens von Orders. Diese Daten sind spezifisch und werden nicht von allen Datenquellen bereitgestellt. Coinbase unterstützt beispielsweise die Bereitstellung eines Orderprotokolls nicht.

Um ein Abonnement eines Orderprotokolls in einem Adapter zu implementieren, wird die Methode **OnOrderLogSubscriptionAsync** verwendet. Diese Methode wird aufgerufen, wenn eine Nachricht [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) mit dem Datentyp [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog) empfangen wird.

Nachfolgend finden Sie ein Beispiel für die Implementierung dieser Methode aus dem Connector [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp), der das Orderprotokoll unterstützt:

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Bestätigung über den Empfang der Abonnementanfrage senden
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Instrumentkennung in ein Währungspaar umwandeln
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// Orderprotokoll in Echtzeit abonnieren
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// Nachricht über erfolgreiches Abonnement senden
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// Orderprotokoll abbestellen
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

Bei der Verarbeitung von Orderprotokolldaten, die von der Börse empfangen werden, wird üblicherweise eine separate Methode verwendet, die die empfangenen Daten in Nachrichten [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) vom Typ [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog) konvertiert:

```cs
private async ValueTask SessionOnNewOrderLog(string symbol, OrderStates state, Order order, CancellationToken cancellationToken)
{
	// Nachricht mit Informationen über einen neuen Orderprotokolleintrag erstellen und senden
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

Fügen Sie im Adapter-Konstruktor die Unterstützung für diesen Datentyp hinzu:

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## Besonderheiten bei der Verarbeitung von historischen und Live-Daten

Beachten Sie bei der Implementierung von Anfragen für historische Daten und der Verarbeitung von Live-Daten in Ihrem eigenen Adapter die folgenden Punkte:

### Historische Daten

Beim Senden historischer Daten als Antwort auf eine Anfrage:

1. Das Setzen von [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) ist obligatorisch. Dies ermöglicht es dem System, die empfangenen Daten mit der ursprünglichen Anfrage zu verknüpfen.

2. Das Setzen von [SecurityId](xref:StockSharp.Messages.SecurityId) oder [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) (im Falle von Kerzen) ist nicht erforderlich, aber auch nicht verboten. Der StockSharp-Kern füllt diese Felder automatisch mit den erforderlichen Werten aus der ursprünglichen Anfrage.

### Live-Daten

Bei der Verarbeitung von Live-Daten, die beispielsweise über WebSocket empfangen werden:

1. Das Setzen von [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) ist optional. Wenn die Transaktions-ID nicht gesetzt ist, verteilt das System die Daten an alle aktiven Abonnements für das entsprechende Instrument und den Datentyp.

2. Das Setzen von [SecurityId](xref:StockSharp.Messages.SecurityId) und anderen spezifischen Feldern (zum Beispiel [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) für Kerzen) ist obligatorisch, da diese Informationen für die korrekte Weiterleitung der Daten im System erforderlich sind.
