# Синтетика

Для построения синтетических позиций по опционам (или, наоборот, опционных позиций по базовому инструменту) можно воспользоваться специальным классом [Synthetic](xref:StockSharp.Algo.Derivatives.Synthetic). Данный класс через методы [Synthetic.Buy](xref:StockSharp.Algo.Derivatives.Synthetic.Buy) и [Synthetic.Sell](xref:StockSharp.Algo.Derivatives.Synthetic.Sell) возвращает комбинацию из синтетических инструментов для определения их возможной позиции. 

Синтетическую комбинацию можно использовать совместно с определением степени ликвидности по опциону (когда нет возможности реализовать необходимую позицию). Для этого можно воспользоваться методами анализа ликвидности стакана [Overload:StockSharp.Algo.TraderHelper.GetTheoreticalTrades](xref:Overload:StockSharp.Algo.TraderHelper.GetTheoreticalTrades): 

```cs
// получить стакан опциона
var depth = _connector.GetMarketDepth(option);
// получить теоретические сделки на покупку 100 контрактов
var trades = depth.GetTheoreticalTrades(Sides.Buy, 100);
// рассчитать реализованный объем
var matchedVolume = trades.Sum(t => t.Trade.Volume);
// регистрируем заявку по основному опциону
_connector.RegisterOrder(new Order
{
	Security = option,
	Volume = matchedVolume,
	Direction = Sides.Buy,
	// максимальная цена, чтобы реализовать требуемый объем
	Price = trades.Max(t => t.Trade.Price),
});
// определяем оставшийся объем
var elapsedVolume = 100 - matchedVolume;
// если реализованный объем меньше планируемого
if (elapsedVolume > 0)
{
	// получаем синтетические инструменты
	var syntheticBuy = new Synthetic(option).Buy();
	// регистрируем оставшийся объем по синтетическим инструментам
	foreach (var pair in syntheticBuy)
	{
		_connector.RegisterOrder(new Order
		{
			Security = pair.Key,
			Volume = elapsedVolume,
			Direction = pair.Value,
			Price = pair.Key.LastTrade.Price,
		});
	}
}
```

Аналогично опционам также можно получить опционную позицию для базового инструмента через методы [Synthetic.Buy](xref:StockSharp.Algo.Derivatives.Synthetic.Buy) и [Synthetic.Sell](xref:StockSharp.Algo.Derivatives.Synthetic.Sell). 
