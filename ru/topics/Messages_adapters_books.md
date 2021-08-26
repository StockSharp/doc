# Стаканы (инкрементальные и обычные)

В случае транслирование внешней торговой системой полных стаканов (стакан присылается целиком при каждом изменении), необходимо отправлять его ввиде сообщения:

```cs
// получаем стакана от торговой системы
private void SessionOnOrderBook(string pair, OrderBook book)
{
		SendOutMessage(new QuoteChangeMessage
		{
			SecurityId = pair.ToStockSharp(),
			Bids = book.Bids.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			Asks = book.Asks.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			ServerTime = book.Time,
		});
}
```

В случае транслирование внешней торговой системой инкрементальных стаканов (транслируются только изменения ценовых уровне, а не весь стакан целиком), в адаптере необходимо прописать логику как построения снэпшота стакана (если он не транслируется), так и отдачи изменений стакана. Для этого необходимо использовать свойство [QuoteChangeMessage.State](../api/StockSharp.Messages.QuoteChangeMessage.State.html):

```cs
// получаем снэпшот стакана от торговой системы
private void SessionOnOrderBookSnapshot(string pair, OrderBook book)
{
		SendOutMessage(new QuoteChangeMessage
		{
			SecurityId = pair.ToStockSharp(),
			Bids = book.Bids.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			Asks = book.Asks.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			ServerTime = book.Time,
			State = QuoteChangeStates.SnapshotComplete, // <- указываем, что текущее сообщение является снэпшотом,
			// и необходимо сбросить состояние стакана новым снимком
		});
}
```

Для отправки инкрементальных сообщений код аналогичен, но устанавливается признак изменения стакана. Если у котировки [QuoteChange.Volume](../api/StockSharp.Messages.QuoteChange.Volume.html) значение равно 0, то это является признаком для удаления ценового уровня:

```cs
// получаем изменения стакана стакана
private void SessionOnOrderBookIncrement(string pair, OrderBook book)
{
		SendOutMessage(new QuoteChangeMessage
		{
			SecurityId = pair.ToStockSharp(),
			Bids = book.Bids.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(), // <- с нулевым объемом котировки интепретируются как удаляемые
			Asks = book.Asks.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			ServerTime = book.Time,
			State = QuoteChangeStates.Increment, // <- указываем, что текущее сообщение является инкрементальным
		});
}
```

Последним шагом является переопределение свойства [IMessageAdapter.IsSupportOrderBookIncrements](../api/StockSharp.Messages.IMessageAdapter.IsSupportOrderBookIncrements.html), которое укажет, что в цепочке адаптеров необходимо при подключении добавить [OrderBookIncrementMessageAdapter](../api/StockSharp.Algo.OrderBookIncrementMessageAdapter.html) (подробнее [Вспомогательные адаптеры](Messages_adapters_chain.md)) :

```cs
public partial class MyOwnMessageAdapter : MessageAdapter
{
	// ...
	
	/// <inheritdoc />
	public override bool IsSupportOrderBookIncrements => true;
}
```
