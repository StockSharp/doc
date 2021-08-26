# Подписки

Начиная с версии 5.0, [S\#.API](StockSharpAbout.md) предлагаем новую модель получения данных (маркет\-данные и транзакционные). Модель основана на подписках, и обладает преимуществами перед обычными запросами на подписку:

- Подписки изолированы друг от друга, поэтому можно параллельно запускать произвольное количество подписков (с запросом истории или нет). 
- Подписки имею состояния, позволяющие понять, идут ли в данный момент исторические данные или подписка перешла в online. 
- Подписки имеют универсальный подход, и код их одинаков, не зависимо от запрашиваемых типов данных. 

Для работа с подписками необходимо использовать класс [Subscription](../api/StockSharp.Algo.Subscription.html). Ниже показан пример подписки на свечи через новую модель:

```cs
...
var subscription \= new Subscription(new MarketDataMessage
{
	DataType2 \= DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	From \= DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
	\/\/ null означает что подписка после истории перейдет в online для получения данных реального времени
	To \= null,
}, (SecurityMessage)sec);
\/\/ подписка на события
\_connector.CandleReceived +\= (sub, candle) \=\>
{
	if (sub \!\= subscription)
		return;
	Console.WriteLine(candle);
};
\_connector.SubscriptionOnline +\= (sub) \=\>
{
	if (sub \!\= subscription)
		return;
	Console.WriteLine("Online");
};
\_connector.SubscriptionFailed +\= (sub, error, isSubscribe) \=\>
{
	if (sub \!\= subscription)
		return;
	Console.WriteLine(error);
};
\/\/ запуск подписки
\_connector.Subscribe(subscription);
...
			
```

Состояния подписок:

- [Stopped](../api/StockSharp.Algo.SubscriptionStates.Stopped.html)

   \- подписка неактивна (остановлена или не запускалась). 
- [Active](../api/StockSharp.Algo.SubscriptionStates.Active.html)

   \- подписка активна, и она может передавать исторические данные до тех пор, пока не перейдет в online или не будет завершена. 
- [Error](../api/StockSharp.Algo.SubscriptionStates.Error.html)

   \- подписка неактивна и находится в состоянии ошибки. 
- [Finished](../api/StockSharp.Algo.SubscriptionStates.Finished.html)

   \- подписка закончила свою работу (все данные получены). 
- [Online](../api/StockSharp.Algo.SubscriptionStates.Online.html)

   \- подписка перешла в состояние online и передает только данные в реальном времени. 
