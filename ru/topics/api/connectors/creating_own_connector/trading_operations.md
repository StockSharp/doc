# Торговые операции

При создании собственного адаптера для работы с биржей необходимо реализовать методы для выполнения торговых операций, таких как регистрация, замена и отмена заявок. Эти методы вызываются при получении соответствующих сообщений от ядра StockSharp.

## Регистрация заявки

Для регистрации новой заявки реализуется метод **RegisterOrderAsync**. Этот метод вызывается при получении сообщения [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage).

Основные шаги при регистрации заявки:

1. Проверка типа заявки и дополнительных условий.
2. Преобразование параметров заявки в формат, понятный бирже.
3. Отправка запроса на регистрацию заявки через API биржи.
4. Обработка ответа от биржи и отправка соответствующего сообщения [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage).

```cs
public override async ValueTask RegisterOrderAsync(OrderRegisterMessage regMsg, CancellationToken cancellationToken)
{
	var condition = (CoinbaseOrderCondition)regMsg.Condition;

	switch (regMsg.OrderType)
	{
		case null:
		case OrderTypes.Limit:
		case OrderTypes.Market:
			break;
		case OrderTypes.Conditional:
		{
			// Обработка условных заявок, например, вывод средств
			if (!condition.IsWithdraw)
				break;

			var withdrawId = await _restClient.Withdraw(regMsg.SecurityId.SecurityCode, regMsg.Volume, condition.WithdrawInfo, cancellationToken);

			await SendOutMessageAsync(new ExecutionMessage
			{
				DataTypeEx = DataType.Transactions,
				OrderStringId = withdrawId,
				ServerTime = CurrentTime.ConvertToUtc(),
				OriginalTransactionId = regMsg.TransactionId,
				OrderState = OrderStates.Done,
				HasOrderInfo = true,
			}, cancellationToken);

			await PortfolioLookupAsync(null, cancellationToken);
			return;
		}
		default:
			throw new NotSupportedException(LocalizedStrings.OrderUnsupportedType.Put(regMsg.OrderType, regMsg.TransactionId));
	}

	// Определение типа заявки (рыночная или лимитная)
	var isMarket = regMsg.OrderType == OrderTypes.Market;
	var price = isMarket ? (decimal?)null : regMsg.Price;
	
	// Отправка заявки на биржу
	var result = await _restClient.RegisterOrder(
		regMsg.TransactionId.To<string>(), regMsg.SecurityId.ToSymbol(),
		regMsg.OrderType.ToNative(), regMsg.Side.ToNative(), price,
		condition?.StopPrice, regMsg.Volume, regMsg.TimeInForce,
		regMsg.TillDate.EnsureToday(), regMsg.Leverage, cancellationToken);

	var orderState = result.Status.ToOrderState();

	// Обработка результата регистрации заявки
	if (orderState == OrderStates.Failed)
	{
		await SendOutMessageAsync(new ExecutionMessage
		{
			DataTypeEx = DataType.Transactions,
			ServerTime = result.CreationTime,
			OriginalTransactionId = regMsg.TransactionId,
			OrderState = OrderStates.Failed,
			Error = new InvalidOperationException(),
			HasOrderInfo = true,
		}, cancellationToken);
	}
}
```

## Замена заявки

Для замены существующей заявки реализуется метод **ReplaceOrderAsync**. Этот метод вызывается при получении сообщения [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage).

Основные шаги при замене заявки:

1. Проверка возможности замены заявки на бирже.
2. Отправка запроса на замену заявки через API биржи.
3. Обработка ответа от биржи и отправка соответствующего сообщения [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage).

```cs
public override async ValueTask ReplaceOrderAsync(OrderReplaceMessage replaceMsg, CancellationToken cancellationToken)
{
	// Отправка запроса на замену заявки
	await _restClient.EditOrder(
		replaceMsg.OldOrderId.To<string>(), 
		replaceMsg.Price, 
		replaceMsg.Volume, 
		cancellationToken);
	
	// Примечание: Обработка результата замены заявки обычно происходит
	// в отдельном методе, который вызывается при получении обновления от биржи
}
```

### Особенности замены заявок

При реализации метода замены заявок важно учитывать особенности протокола биржи. Для этого в StockSharp предусмотрено свойство [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent).

Если протокол биржи предполагает изменение заявки с сохранением за ней старого идентификатора, необходимо переопределить это свойство и вернуть `true`. Это указывает StockSharp, что при замене заявки не нужно ожидать новый идентификатор от биржи.

```cs
public override bool IsReplaceCommandEditCurrent => true;
```

Если же при изменении заявки происходит отмена старой и регистрация новой с новым биржевым идентификатором, то это свойство не нужно переопределять. По умолчанию оно возвращает `false`, что соответствует поведению большинства бирж.

## Отмена заявки

Для отмены существующей заявки реализуется метод **CancelOrderAsync**. Этот метод вызывается при получении сообщения [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage).

Основные шаги при отмене заявки:

1. Проверка наличия идентификатора заявки.
2. Отправка запроса на отмену заявки через API биржи.
3. Обработка ответа от биржи и отправка соответствующего сообщения [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage).

```cs
public override async ValueTask CancelOrderAsync(OrderCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	// Проверка наличия идентификатора заявки
	if (cancelMsg.OrderStringId.IsEmpty())
		throw new InvalidOperationException(LocalizedStrings.OrderNoExchangeId.Put(cancelMsg.OriginalTransactionId));

	// Отправка запроса на отмену заявки
	await _restClient.CancelOrder(cancelMsg.OrderStringId, cancellationToken);

	// Примечание: Обработка результата отмены заявки обычно происходит
	// в отдельном методе, который вызывается при получении обновления от биржи
}
```

## Массовая отмена заявок

Некоторые биржи поддерживают функцию массовой отмены заявок, которая позволяет отменить сразу несколько или все активные заявки одним запросом. Это может быть полезно для быстрого закрытия позиций или очистки книги заявок в определенных рыночных условиях.

Для реализации массовой отмены заявок в адаптере обычно используется метод **CancelOrderGroupAsync**. Этот метод вызывается при получении сообщения [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage).

Стоит отметить, что не все биржи поддерживают эту функцию. Например, Coinbase не предоставляет API для массовой отмены заявок. В таких случаях может потребоваться реализация последовательной отмены отдельных заявок.

Ниже приведен пример реализации метода массовой отмены заявок, взятый из коннектора [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp), который поддерживает эту функцию:

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

Важно не забыть убрать удаление поддержки этого типа команды из конструкторе адаптера:

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## Отслеживание состояния заявок

В случае с Coinbase, как и с некоторыми другими современными биржами, обновления состояния заявок транслируются через WebSocket соединение. Это означает, что после выполнения торговых операций (регистрации, замены или отмены заявки) нет необходимости немедленно запрашивать новое состояние заявки через REST API. Вместо этого, адаптер будет получать обновления автоматически через установленное WebSocket соединение.

Обработка этих обновлений происходит в методе, подобном `SessionOnOrderReceived`, который был рассмотрен в разделе о [запросе текущего состояния портфеля и заявок](portfolio_and_orders_state.md). Этот метод вызывается каждый раз, когда биржа отправляет обновление о состоянии заявки, независимо от того, было ли это обновление вызвано действиями пользователя или изменениями на самой бирже.

Такой подход позволяет более эффективно отслеживать состояние заявок, уменьшает нагрузку на API биржи и обеспечивает получение обновлений в режиме реального времени. Однако при реализации собственного адаптера необходимо внимательно изучить документацию API используемой биржи, чтобы правильно настроить и обработать эти WebSocket обновления.

## Обработка ошибок

При выполнении торговых операций важно корректно обрабатывать возможные ошибки и исключения. В случае возникновения ошибки, необходимо отправить сообщение [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) с установленным свойством [Error](xref:StockSharp.Messages.ExecutionMessage.Error).

## Особенности реализации

При реализации методов для работы с торговыми операциями необходимо учитывать особенности конкретной биржи:

- Поддерживаемые типы заявок (рыночные, лимитные, стоп-заявки и т.д.).
- Формат идентификаторов заявок.
- Особенности API биржи для работы с заявками.
- Возможные ограничения на частоту отправки запросов.