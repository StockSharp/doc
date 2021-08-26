# Исторические данные

В случае запроса истории, в адаптер приходит подписка, с инициализированными значениями [MarketDataMessage.From](../api/StockSharp.Messages.MarketDataMessage.From.html) и [MarketDataMessage.To](../api/StockSharp.Messages.MarketDataMessage.To.html). Адаптер должен обработать полученный запрос, вернув запрашиваемые данные. Признаком окончания запроса служит отправка сообщения [SubscriptionFinishedMessage](../api/StockSharp.Messages.SubscriptionFinishedMessage.html). Таким образом, внешний код понимает, что все запрошенные данные были получены. Если данных нет (подключение не поддерживает историю или же запрашиваемый период недоступен), то так же требуется результирующее значение [SubscriptionFinishedMessage](../api/StockSharp.Messages.SubscriptionFinishedMessage.html). 

Некоторые сообщения, реализующие [ISubscriptionMessage](../api/StockSharp.Messages.ISubscriptionMessage.html) (например, [SecurityLookupMessage](../api/StockSharp.Messages.SecurityLookupMessage.html)), имеют всегда инициализированными [ISubscriptionMessage.From](../api/StockSharp.Messages.ISubscriptionMessage.From.html) и [ISubscriptionMessage.To](../api/StockSharp.Messages.ISubscriptionMessage.To.html). Для таких сообщение подписка на online данные невозможна, и по ним отправляются только исторические данные (или же идет запрос мета\-данных, как инструменты, площадки и т.д.). 

При обработке результатов истории, у исторические сообщений необходимо инициализировать свойство [IOriginalTransactionIdMessage.OriginalTransactionId](../api/StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId.html). Таким образом внешний код поймет, что возвращаемые адаптером данные относятся к конкретной исторический подписке:

```cs
      		SendOutMessage(new TimeFrameCandleMessage
			{
				OpenPrice = (decimal)candle.Open,
				ClosePrice = (decimal)candle.Close,
				HighPrice = (decimal)candle.High,
				LowPrice = (decimal)candle.Low,
				TotalVolume = (decimal)candle.AssetVolume,
				OpenTime = candle.StartTime,
				State = candle.IsFormed ? CandleStates.Finished : CandleStates.Active,
				TotalTicks = candle.TradesCount,
				OriginalTransactionId = _candleTransactions.TryGetValue(Tuple.Create(secId, tf)), // <- заполняем идентификато подписки, по которому внешний код определит, какой инструмент и тайм-фрейм был в подписке
			});
      
```

Специальные адаптер [PartialDownloadMessageAdapter](../api/StockSharp.Algo.PartialDownloadMessageAdapter.html) дробит подписки с большим диапазоном данных (подробнее [Вспомогательные адаптеры](Messages_adapters_chain.md)) на множество запросов с мелкими диапазонами. Каждый адаптер может передать, какой диапазон времени максимально он поддерживает через перегрузку метода [IMessageAdapter.GetHistoryStepSize](../api/StockSharp.Messages.IMessageAdapter.GetHistoryStepSize.html):

```cs
public partial class MyOwnMessageAdapter : MessageAdapter
{
	// ...
	
	/// <inheritdoc />
	public override TimeSpan GetHistoryStepSize(DataType dataType, out TimeSpan iterationInterval)
	{
		var step = base.GetHistoryStepSize(dataType, out iterationInterval);
			
		if (dataType == DataType.Ticks) // тиковые данные максимально поддерживает диапазон в один день
			step = TimeSpan.FromDays(1);
		return step;
	}
}
```

В случае online данных заполнение [IOriginalTransactionIdMessage.OriginalTransactionId](../api/StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId.html) не требуется, так как адаптер [SubscriptionOnlineMessageAdapter](../api/StockSharp.Algo.SubscriptionOnlineMessageAdapter.html) передает в адаптер только одну подписку с online данными.
