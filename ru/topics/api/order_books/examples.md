# Примеры со стаканом

## Получение лучших цен

Для получения лучших цен по стакану заявок важно сосредоточиться на первых элементах списков заявок на покупку ([Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)) и продажу ([Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)), так как они представляют собой наиболее выгодные доступные цены для совершения сделок:

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Лучшая цена покупки: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Лучшая цена продажи: {bestAsk.Price}");
}
```

Или использовать готовые методы-расширения [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) и [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage)):

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Лучшая цена покупки: {bestBid.Price}, объем: {bestBid.Volume}");
}
else
{
	Console.WriteLine("Лучшие заявки на покупку отсутствуют.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Лучшая цена продажи: {bestAsk.Price}, объем: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("Лучшие заявки на продажу отсутствуют.");
}
```

## Получение данных в глубину стакана

Для анализа глубины стакана заявок можно перебирать элементы в списках [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) и [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), начиная с начала списка. Это даст представление о распределении заявок на различных уровнях цен и поможет определить потенциальные уровни поддержки и сопротивления:

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Цена покупки: {bid.Price}, объем: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Цена продажи: {ask.Price}, объем: {ask.Volume}");
}
```

## Поиск объемов по стакану

Алгоритм поиска значительных объемов в стакане заявок помогает выявить уровни, на которых накапливаются крупные заявки. Это может указывать на интерес крупных игроков и служить дополнительным сигналом при принятии торговых решений.

Алгоритм:

1. Определите пороговое значение объема, которое будет считаться значительным.
2. Переберите заявки в списке [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) и [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), сравнивая объем каждой заявки с пороговым значением.
3. Запишите уровни цен, на которых были найдены заявки с объемом выше порога.

```cs
double significantVolumeThreshold = 10000; // Пример порогового значения

Console.WriteLine("Значительные объемы в стакане заявок:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Покупка: Цена {bid.Price}, объем {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Продажа: Цена {ask.Price}, объем {ask.Volume}");
	}
}
```

Этот алгоритм поможет выделить уровни со значительными объемами, которые могут играть ключевую роль в формировании ценовых движений на рынке.
