# Synthetics

To create the synthetic positions by options (or, vice versa, option positions by the base instrument) you can use the special [Synthetic](xref:StockSharp.Algo.Derivatives.Synthetic) class. This class through [Synthetic.Buy](xref:StockSharp.Algo.Derivatives.Synthetic.Buy) and [Synthetic.Sell](xref:StockSharp.Algo.Derivatives.Synthetic.Sell) methods returns a combination of synthetic instruments to determine their possible position. 

The synthetic combination can be used together with the degree of liquidity by the option determination (when it is impossible to get the required position). To do this you can use the order book liquidity analysis methods [TraderHelper.GetTheoreticalTrades](xref:StockSharp.Algo.TraderHelper.GetTheoreticalTrades(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Sides,System.Decimal))**(**[StockSharp.BusinessEntities.MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) depth, [StockSharp.Messages.Sides](xref:StockSharp.Messages.Sides) orderDirection, [System.Decimal](xref:System.Decimal) volume **)**: 

```cs
// order book of the option
var depth = _connector.GetMarketDepth(option);
// calc theoretical price for 100 contracts
var trades = depth.GetTheoreticalTrades(Sides.Buy, 100);
// calc matched size
var matchedVolume = trades.Sum(t => t.Trade.Volume);
// a new order for the option
_connector.RegisterOrder(new Order
{
	Security = option,
	Volume = matchedVolume,
	Direction = Sides.Buy,
	// using max price
	Price = trades.Max(t => t.Trade.Price),
});
// calc elapsed size
var elapsedVolume = 100 - matchedVolume;
if (elapsedVolume > 0)
{
	// creating synthetic instruments
	var syntheticBuy = new Synthetic(option).Buy();
	// registering orders with elapsed volumes
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

Similarly to options, you can also get the option position for the base instrument through [Synthetic.Buy](xref:StockSharp.Algo.Derivatives.Synthetic.Buy(System.Decimal))**(**[System.Decimal](xref:System.Decimal) strike **)** and [Synthetic.Sell](xref:StockSharp.Algo.Derivatives.Synthetic.Sell(System.Decimal))**(**[System.Decimal](xref:System.Decimal) strike **)** methods. 
