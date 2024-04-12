# Исторические данные

В случае запроса истории, в адаптер приходит подписка, с инициализированными значениями [MarketDataMessage.From](xref:StockSharp.Messages.MarketDataMessage.From) и [MarketDataMessage.To](xref:StockSharp.Messages.MarketDataMessage.To). Адаптер должен обработать полученный запрос, вернув запрашиваемые данные. Признаком окончания запроса служит отправка сообщения [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage). Таким образом, внешний код понимает, что все запрошенные данные были получены. Если данных нет (подключение не поддерживает историю или же запрашиваемый период недоступен), то так же требуется результирующее значение [SubscriptionFinishedMessage](xref:StockSharp.Messages.SubscriptionFinishedMessage). 

Некоторые сообщения, реализующие [ISubscriptionMessage](xref:StockSharp.Messages.ISubscriptionMessage) (например, [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage)), имеют всегда инициализированными [ISubscriptionMessage.From](xref:StockSharp.Messages.ISubscriptionMessage.From) и [ISubscriptionMessage.To](xref:StockSharp.Messages.ISubscriptionMessage.To). Для таких сообщение подписка на online данные невозможна, и по ним отправляются только исторические данные (или же идет запрос мета\-данных, как инструменты, площадки и т.д.). 

При обработке результатов истории, у исторические сообщений необходимо инициализировать свойство [IOriginalTransactionIdMessage.OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId). Таким образом внешний код поймет, что возвращаемые адаптером данные относятся к конкретной исторический подписке:

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

Специальные адаптер [PartialDownloadMessageAdapter](xref:StockSharp.Algo.PartialDownloadMessageAdapter) дробит подписки с большим диапазоном данных (подробнее [Вспомогательные адаптеры](adapters_chain.md)) на множество запросов с мелкими диапазонами. Каждый адаптер может передать, какой диапазон времени максимально он поддерживает через перегрузку метода [IMessageAdapter.GetHistoryStepSize](xref:StockSharp.Messages.IMessageAdapter.GetHistoryStepSize(StockSharp.Messages.DataType,System.TimeSpan@))**(**[StockSharp.Messages.DataType](xref:StockSharp.Messages.DataType) dataType, **out** [System.TimeSpan](xref:System.TimeSpan) iterationInterval **)**:

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

В случае online данных заполнение [IOriginalTransactionIdMessage.OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) не требуется, так как адаптер [SubscriptionOnlineMessageAdapter](xref:StockSharp.Algo.SubscriptionOnlineMessageAdapter) передает в адаптер только одну подписку с online данными.
