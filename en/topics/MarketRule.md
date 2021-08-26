# Rule

For the [IMarketRule](../api/StockSharp.Algo.IMarketRule.html) the [S\#](StockSharpAbout.md) already has a number of predefined conditions and actions for the most common scenarios. The [MarketRuleHelper](../api/StockSharp.Algo.MarketRuleHelper.html) class conditions lists grouped by trading objects are listed below: 

For the [Security](../api/StockSharp.BusinessEntities.Security.html)

- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- the instrument change event rule. 
- [MarketRuleHelper.WhenNewTrade](../api/StockSharp.Algo.MarketRuleHelper.WhenNewTrade.html)

   \- the rule when instrument has the new trade occurrence event. 
- [MarketRuleHelper.WhenMarketDepthChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenMarketDepthChanged.html)

   \- the rule when instrument has the order book change event. 
- [MarketRuleHelper.WhenBestBidPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore.html)

   \- the rule for the event of the best bid increase above a specified level. 
- [MarketRuleHelper.WhenBestBidPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess.html)

   \- the rule for the event of the best bid decrease below a specified level. 
- [MarketRuleHelper.WhenBestAskPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore.html)

   \- the rule for the event of the best offer increase above a specified level. 
- [MarketRuleHelper.WhenBestAskPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess.html)

   \- the rule for the event of the best offer decrease below a specified level. 
- [MarketRuleHelper.WhenLastTradePriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceMore.html)

   \- the rule for the event of the last trade price increase above a specified level. 
- [MarketRuleHelper.WhenLastTradePriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceLess.html)

   \- the rule for the event of the last trade price decrease below a specified level. 

For the [MarketDepth](../api/StockSharp.BusinessEntities.MarketDepth.html)

- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- the order book change event rule. 
- [MarketRuleHelper.WhenSpreadMore](../api/StockSharp.Algo.MarketRuleHelper.WhenSpreadMore.html)

   \- the rule for the event of the book order spread size more then specified value. 
- [MarketRuleHelper.WhenSpreadLess](../api/StockSharp.Algo.MarketRuleHelper.WhenSpreadLess.html)

   \- the rule for the event of the book order spread size less then specified value. 
- [MarketRuleHelper.WhenBestBidPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore.html)

   \- the rule for the event of the best bid increase above a specified level. 
- [MarketRuleHelper.WhenBestBidPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess.html)

   \- the rule for the event of the best bid decrease below a specified level. 
- [MarketRuleHelper.WhenBestAskPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore.html)

   \- the rule for the event of the best offer increase above a specified level. 
- [MarketRuleHelper.WhenBestAskPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess.html)

   \- the rule for the event of the best offer decrease below a specified level. 

For the [Order](../api/StockSharp.BusinessEntities.Order.html)

- [MarketRuleHelper.WhenRegistered](../api/StockSharp.Algo.MarketRuleHelper.WhenRegistered.html)

   \- the rule for the event of the successful order registration on exchange. 
- [MarketRuleHelper.WhenPartiallyMatched](../api/StockSharp.Algo.MarketRuleHelper.WhenPartiallyMatched.html)

   \- the rule for the event of the partially matched order. 
- [MarketRuleHelper.WhenRegisterFailed](../api/StockSharp.Algo.MarketRuleHelper.WhenRegisterFailed.html)

   \- the rule for the event of the failed order registration on exchange. 
- [MarketRuleHelper.WhenCancelFailed](../api/StockSharp.Algo.MarketRuleHelper.WhenCancelFailed.html)

   \- the rule for the event of the failed order cancel on exchange. 
- [MarketRuleHelper.WhenCanceled](../api/StockSharp.Algo.MarketRuleHelper.WhenCanceled.html)

   \- the rule for the event of the order cancel on exchange. 
- [MarketRuleHelper.WhenMatched](../api/StockSharp.Algo.MarketRuleHelper.WhenMatched.html)

   \- the rule for the event of the fully matched order on exchange. 
- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- the rule for the event of the order change. 
- [MarketRuleHelper.WhenNewTrade](../api/StockSharp.Algo.MarketRuleHelper.WhenNewTrade.html)

   \- the rule for the event of the trade occurrence by the order. 

For the [Portfolio](../api/StockSharp.BusinessEntities.Portfolio.html)

- [MarketRuleHelper.WhenMoneyLess](../api/StockSharp.Algo.MarketRuleHelper.WhenMoneyLess.html)

   \- the rule for the event of the money decrease in the portfolio below a specified level. 
- [MarketRuleHelper.WhenMoneyMore](../api/StockSharp.Algo.MarketRuleHelper.WhenMoneyMore.html)

   \- the rule for the event of the money increase in the portfolio above a specified level. 

For [Position](../api/StockSharp.BusinessEntities.Position.html)

- [MarketRuleHelper.WhenLess](../api/StockSharp.Algo.MarketRuleHelper.WhenLess.html)

   \- the rule for the event of the position decrease below a specified level. 
- [MarketRuleHelper.WhenMore](../api/StockSharp.Algo.MarketRuleHelper.WhenMore.html)

   \- the rule for the event of the position increase above a specified level. 
- [MarketRuleHelper.Changed](../api/StockSharp.Algo.MarketRuleHelper.Changed.html)

   \- the rule for the event of the position change. 

For the [IPnLManager](../api/StockSharp.Algo.PnL.IPnLManager.html)

- [StrategyHelper.WhenPnLLess](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPnLLess.html)

   \- the rule for the event of the profit decrease below a specified level. 
- [StrategyHelper.WhenPnLMore](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPnLMore.html)

   \- the rule for the event of the profit increase above a specified level. 
- [StrategyHelper.WhenPositionChanged](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged.html)

   \- the rule for the event of the profit change. 

For the [Candle](../api/StockSharp.Algo.Candles.Candle.html)

- [MarketRuleHelper.WhenClosePriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenClosePriceMore.html)

   \- the rule for the event of the candle closing price increase above a specified level. 
- [MarketRuleHelper.WhenClosePriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenClosePriceLess.html)

   \- the rule for the event of the candle closing price decrease below a specified level. 
- [MarketRuleHelper.WhenTotalVolumeMore](../api/StockSharp.Algo.MarketRuleHelper.WhenTotalVolumeMore.html)

   \- the rule for the event of the candle total volume increase above a specified level. 
- [MarketRuleHelper.WhenCurrentCandleTotalVolumeMore](../api/StockSharp.Algo.MarketRuleHelper.WhenCurrentCandleTotalVolumeMore.html)

   \- the rule for the event of the current candle total volume increase above a specified level. 
- [MarketRuleHelper.WhenCandlesStarted](../api/StockSharp.Algo.MarketRuleHelper.WhenCandlesStarted.html)

   \- the rule for the event of the new candles occurrence. 
- [MarketRuleHelper.WhenCandlesChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenCandlesChanged.html)

   \- the rule for the event of the candles change. 
- [MarketRuleHelper.WhenCandlesFinished](../api/StockSharp.Algo.MarketRuleHelper.WhenCandlesFinished.html)

   \- the rule for the event of the candles end. 
- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- the rule for the event of the candle change. 
- [MarketRuleHelper.WhenFinished](../api/StockSharp.Algo.MarketRuleHelper.WhenFinished.html)

   \- the rule for the event of the candle finish. 
- [MarketRuleHelper.WhenPartiallyFinished](../api/StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinished.html)

   \- the rule for the event of the candle partial finish. 
- [MarketRuleHelper.WhenPartiallyFinishedCandles](../api/StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinishedCandles.html)

   \- the rule for the event of the candles partial finish. 

For the [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html)

- [StrategyHelper.WhenNewMyTrade](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenNewMyTrade.html)

   \- the rule for the event of the strategy new trades occurrence. 
- [StrategyHelper.WhenPositionChanged](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged.html)

   \- the rule for the event of the strategy position change. 
- [StrategyHelper.WhenStarted](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenStarted.html)

   \- the rule for the event of the strategy work start. 
- [StrategyHelper.WhenStopping](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenStopping.html)

   \- the rule for the event of the strategy work stopping. 
- [StrategyHelper.WhenStopped](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenStopped.html)

   \- the rule for the event of the strategy work stopped. 
- [StrategyHelper.WhenError](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenError.html)

   \- the rule for the event of the strategy error. 
- [StrategyHelper.WhenWarning](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenWarning.html)

   \- the rule for the event of the strategy warning. 

For the [IConnector](../api/StockSharp.BusinessEntities.IConnector.html)

- [MarketRuleHelper.WhenIntervalElapsed](../api/StockSharp.Algo.MarketRuleHelper.WhenIntervalElapsed.html)

   \- the rule for the event of the 

  [IConnector.MarketTimeChanged](../api/StockSharp.BusinessEntities.IConnector.MarketTimeChanged.html)

   change on value, which greater than or equal to the parameter. 
- [MarketRuleHelper.WhenTimeCome](../api/StockSharp.Algo.MarketRuleHelper.WhenTimeCome.html)

   \- a rule that is activated upon the exact time occurrence. 
- [MarketRuleHelper.WhenNewMyTrade](../api/StockSharp.Algo.MarketRuleHelper.WhenNewMyTrade.html)

   \- the rule for the event of the new trade occurrence. 
- [MarketRuleHelper.WhenNewOrder](../api/StockSharp.Algo.MarketRuleHelper.WhenNewOrder.html)

   \- the rule for the event of the new order occurrence. 

Similarly to the conditions there are predefined actions: 

- [StrategyHelper.Register](../api/StockSharp.Algo.Strategies.StrategyHelper.Register.html)

   \- the action which registers the order. 
- [StrategyHelper.ReRegister](../api/StockSharp.Algo.Strategies.StrategyHelper.ReRegister.html)

   \- the action which reregisters the order. 
- [StrategyHelper.Cancel](../api/StockSharp.Algo.Strategies.StrategyHelper.Cancel.html)

   \- the action which cancels the order. 
- [Protect](../api/Overload:StockSharp.Algo.Strategies.Extensions.Protect.html)

   \- the action which protects trades simultaneously with 

  [TakeProfitStrategy](../api/StockSharp.Algo.Strategies.Protective.TakeProfitStrategy.html)

   and 

  [StopLossStrategy](../api/StockSharp.Algo.Strategies.Protective.StopLossStrategy.html)

   strategies. 

If you need to create your own unique rule (on any event, which is not provided as standard), you must create the derived [MarketRule\`2](../api/StockSharp.Algo.MarketRule`2.html) class, which will work with a prerequisite. Below is the [WhenMoneyMore](../api/StockSharp.Algo.MarketRuleHelper.WhenMoneyMore.html) method implementation: 

```cs
		
private sealed class PortfolioRule : MarketRule\<Portfolio, Portfolio\>
{
	private readonly Func\<Portfolio, bool\> \_changed;
	private readonly Portfolio \_portfolio;
	private readonly IConnector \_connector;
	public PortfolioRule(Portfolio portfolio, IConnector connector, Func\<Portfolio, bool\> changed) : base(portfolio)
	{
		if (portfolio \=\= null)
			throw new ArgumentNullException("portfolio");
		if (changed \=\= null)
			throw new ArgumentNullException("changed");
		\_changed \= changed;
		\_portfolio \= portfolio;
		\_connector \= connector;
		\_connector.PortfolioChanged +\= OnPortfolioChanged;
	}
	private void OnPortfolioChanged(Portfolio portfolio)
	{
		if ((portfolio\=\=\_portfolio) && \_changed(\_portfolio))
			Activate(\_portfolio);
	}
	protected override void DisposeManaged()
	{
		\_connector.PortfolioChanged \-\= OnPortfolioChanged;
		base.DisposeManaged();
	}
}
		
public static MarketRule\<Portfolio, Portfolio\> WhenMoneyMore(this Portfolio portfolio, Unit money)
{
	if (portfolio \=\= null)
		throw new ArgumentNullException("portfolio");
	if (money \=\= null)
		throw new ArgumentNullException("money");
	var finishMoney \= money.Type \=\= UnitTypes.Limit ? money : portfolio.CurrentValue + money;
	return new PortfolioRule(portfolio, pf \=\> pf.CurrentValue \> finishMoney)
	{
		Name \= "Money increase of portfolio {0} above {1}".Put(portfolio, finishMoney)
	};
}
		
```

The *PortfolioRule* rule subscribes to the [IPortfolioProvider.PortfolioChanged](../api/StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged.html) event, and as soon as it is called, then the condition is checked to exceed the current level of money in the portfolio above a specified limit. If the condition returns **true**, then the rule is activated through the [MarketRule\`2.Activate](../api/StockSharp.Algo.MarketRule`2.Activate.html) method. 
