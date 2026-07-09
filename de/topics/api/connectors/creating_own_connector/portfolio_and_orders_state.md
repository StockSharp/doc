# Informationen über Portfolios und Aufträge

Beim Erstellen eines eigenen Adapters für die Arbeit mit einer Börse müssen Sie Methoden zur Abfrage des aktuellen Zustands des Portfolios und der Aufträge implementieren. Diese Methoden werden beim Empfang der Nachrichten [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) bzw. [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) aufgerufen.

## Abfrage des Portfoliozustands

Zur Abfrage des Portfoliozustands wird die Methode **PortfolioLookupAsync** implementiert. Diese Methode führt in der Regel folgende Aktionen aus:

1. Sendet eine Bestätigung des Empfangs der Anfrage mit [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Prüft, ob es sich bei der Anfrage um ein Abonnement oder eine Kündigung handelt, anhand der Eigenschaft [IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe).
3. Im Falle eines Abonnements:
  - Sendet eine [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage)-Nachricht mit Informationen über das Portfolio.
  - Fordert die aktuellen Kontosalden von der Börse an.
  - Erstellt und sendet für jedes Konto eine [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage)-Nachricht mit Informationen über die Position.
4. Sendet eine Nachricht über das Ergebnis des Abonnements mit [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	var transId = lookupMsg.TransactionId;

	// Bestätigung über den Empfang der Anfrage senden
	await SendSubscriptionReplyAsync(transId, cancellationToken);

	if (!lookupMsg.IsSubscribe)
		return;

	// Nachricht mit Portfolioinformationen senden
	await SendOutMessageAsync(new PortfolioMessage
	{
		PortfolioName = PortfolioName,
		BoardCode = BoardCodes.Coinbase,
		OriginalTransactionId = transId,
	}, cancellationToken);

	// Aktuelle Kontostände anfordern
	var accounts = await _restClient.GetAccounts(cancellationToken);

	foreach (var account in accounts)
	{
		// Für jedes Konto eine Nachricht mit Positionsinformationen erstellen und senden
		await SendOutMessageAsync(new PositionChangeMessage
		{
			PortfolioName = PortfolioName,
			SecurityId = new SecurityId
			{
				SecurityCode = account.Currency,
				BoardCode = BoardCodes.Coinbase,
			},
			ServerTime = CurrentTime.ConvertToUtc(),
		}
		.TryAdd(PositionChangeTypes.CurrentValue, (decimal)account.Available, true)
		.TryAdd(PositionChangeTypes.BlockedValue, (decimal)account.Hold, true), cancellationToken);
	}

	// Nachricht über den erfolgreichen Abschluss des Abonnements senden
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

## Abfrage des Auftragszustands

Zur Abfrage des Auftragszustands wird die Methode **OrderStatusAsync** implementiert. Diese Methode führt in der Regel folgende Aktionen aus:

1. Sendet eine Bestätigung des Empfangs der Anfrage mit [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Prüft, ob es sich bei der Anfrage um ein Abonnement oder eine Kündigung handelt, anhand der Eigenschaft [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe).
3. Im Falle eines Abonnements:
  - Fordert die Liste der aktuellen Aufträge von der Börse an.
  - Erstellt und sendet für jeden Auftrag eine [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)-Nachricht mit Informationen über den Auftrag.
  - Richtet bei Bedarf ein Abonnement ein, um Auftragsaktualisierungen in Echtzeit zu erhalten.
4. Sendet eine Nachricht über das Ergebnis des Abonnements mit [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
	// Bestätigung über den Empfang der Anfrage senden
	await SendSubscriptionReplyAsync(statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsSubscribe)
		return;

	// Liste aktueller Orders anfordern
	var orders = await _restClient.GetOrders(cancellationToken);

	foreach (var order in orders)
		await ProcessOrder(order, statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsHistoryOnly())
	{
		// Abonnement für Orderaktualisierungen in Echtzeit einrichten
		await _socketClient.SubscribeOrders(cancellationToken);
	}

	// Nachricht über den erfolgreichen Abschluss des Abonnements senden
	await SendSubscriptionResultAsync(statusMsg, cancellationToken);
}

private async ValueTask ProcessOrder(Order order, long originTransId, CancellationToken cancellationToken)
{
	if (!long.TryParse(order.ClientOrderId, out var transId))
		return;

	var state = order.Status.ToOrderState();

	// Nachricht mit Orderinformationen erstellen und senden
	await SendOutMessageAsync(new ExecutionMessage
	{
		ServerTime = originTransId == 0 ? CurrentTime.ConvertToUtc() : order.CreationTime,
		DataTypeEx = DataType.Transactions,
		SecurityId = order.Product.ToStockSharp(),
		TransactionId = originTransId == 0 ? 0 : transId,
		OriginalTransactionId = originTransId,
		OrderState = state,
		Error = state == OrderStates.Failed ? new InvalidOperationException() : null,
		OrderType = order.Type.ToOrderType(),
		Side = order.Side.ToSide(),
		OrderStringId = order.Id,
		OrderPrice = order.Price?.ToDecimal() ?? 0,
		OrderVolume = order.Size?.ToDecimal(),
		TimeInForce = order.TimeInForce.ToTimeInForce(),
		Balance = (decimal?)order.LeavesQuantity,
		HasOrderInfo = true,
	}, cancellationToken);
}
```

## Verarbeitung von Echtzeit-Aktualisierungen

Zur Verarbeitung von Echtzeit-Aktualisierungen des Auftragszustands wird in der Regel eine separate Methode implementiert, die beim Empfang entsprechender Ereignisse vom WebSocket-Client aufgerufen wird:

```cs
private async ValueTask SessionOnOrderReceived(Order order, CancellationToken cancellationToken)
{
	// Empfangenes Orderupdate verarbeiten
	// OriginTransId = 0, since this is a real-time update, not a response to a specific request
	await ProcessOrder(order, 0, cancellationToken);
}
```
