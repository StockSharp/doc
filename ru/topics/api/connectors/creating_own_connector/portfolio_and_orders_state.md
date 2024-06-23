# Информация о портфелях и заявках

При создании собственного адаптера для работы с биржей необходимо реализовать методы для запроса текущего состояния портфеля и заявок. Эти методы вызываются при получении сообщений [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) и [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) соответственно.

## Запрос состояния портфеля

Для запроса состояния портфеля реализуется метод **PortfolioLookupAsync**. Этот метод обычно выполняет следующие действия:

1. Отправляет подтверждение о получении запроса с помощью [SendSubscriptionReply](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReply(System.Int64,System.Exception)).
2. Проверяет, является ли запрос подпиской или отпиской, используя свойство [IsSubscribe](xref:StockSharp.Messages.PortfolioMessage.IsSubscribe).
3. В случае подписки:
  - Отправляет сообщение [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) с информацией о портфеле.
  - Запрашивает текущие балансы счетов у биржи.
  - Для каждого счета создает и отправляет сообщение [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) с информацией о позиции.
4. Отправляет сообщение о результате подписки с помощью [SendSubscriptionResult](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResult(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
    var transId = lookupMsg.TransactionId;

    // Отправляем подтверждение о получении запроса
    SendSubscriptionReply(transId);

    if (!lookupMsg.IsSubscribe)
        return;

    // Отправляем сообщение с информацией о портфеле
    SendOutMessage(new PortfolioMessage
    {
        PortfolioName = PortfolioName,
        BoardCode = BoardCodes.Coinbase,
        OriginalTransactionId = transId,
    });

    // Запрашиваем текущие балансы счетов
    var accounts = await _restClient.GetAccounts(cancellationToken);

    foreach (var account in accounts)
    {
        // Для каждого счета создаем и отправляем сообщение с информацией о позиции
        SendOutMessage(new PositionChangeMessage
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
        .TryAdd(PositionChangeTypes.BlockedValue, (decimal)account.Hold, true));
    }

    // Отправляем сообщение об успешном завершении подписки
    SendSubscriptionResult(lookupMsg);
}
```

## Запрос состояния заявок

Для запроса состояния заявок реализуется метод **OrderStatusAsync**. Этот метод обычно выполняет следующие действия:

1. Отправляет подтверждение о получении запроса с помощью [IMessageAdapter.SendSubscriptionReply](xref:StockSharp.Messages.IMessageAdapter.SendSubscriptionReply(long)).
2. Проверяет, является ли запрос подпиской или отпиской, используя свойство [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe).
3. В случае подписки:
  - Запрашивает список текущих заявок у биржи.
  - Для каждой заявки создает и отправляет сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) с информацией о заявке.
  - Если требуется, устанавливает подписку на получение обновлений по заявкам в реальном времени.
4. Отправляет сообщение о результате подписки с помощью [IMessageAdapter.SendSubscriptionResult](xref:StockSharp.Messages.IMessageAdapter.SendSubscriptionResult(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
    // Отправляем подтверждение о получении запроса
    SendSubscriptionReply(statusMsg.TransactionId);

    if (!statusMsg.IsSubscribe)
        return;

    // Запрашиваем список текущих заявок
    var orders = await _restClient.GetOrders(cancellationToken);

    foreach (var order in orders)
        ProcessOrder(order, statusMsg.TransactionId);

    if (!statusMsg.IsHistoryOnly())
    {
        // Устанавливаем подписку на получение обновлений по заявкам в реальном времени
        await _socketClient.SubscribeOrders(cancellationToken);
    }

    // Отправляем сообщение об успешном завершении подписки
    SendSubscriptionResult(statusMsg);
}

private void ProcessOrder(Order order, long originTransId)
{
    if (!long.TryParse(order.ClientOrderId, out var transId))
        return;

    var state = order.Status.ToOrderState();

    // Создаем и отправляем сообщение с информацией о заявке
    SendOutMessage(new ExecutionMessage
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
    });
}
```

## Обработка обновлений в реальном времени

Для обработки обновлений состояния заявок в реальном времени обычно реализуется отдельный метод, который вызывается при получении соответствующих событий от WebSocket клиента:

```cs
private void SessionOnOrderReceived(Order order)
{
    // Обрабатываем полученное обновление по заявке
    // OriginTransId = 0, так как это обновление в реальном времени, а не ответ на конкретный запрос
    ProcessOrder(order, 0);
}
```