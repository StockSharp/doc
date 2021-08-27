# Rule

For the [IMarketRule](xref:StockSharp.Algo.IMarketRule) the [S\#](StockSharpAbout.md) already has a number of predefined conditions and actions for the most common scenarios. The [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper) class conditions lists grouped by trading objects are listed below: 

For the [Security](xref:StockSharp.BusinessEntities.Security)

- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged)

   \- the instrument change event rule. 
- [MarketRuleHelper.WhenNewTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewTrade)

   \- the rule when instrument has the new trade occurrence event. 
- [MarketRuleHelper.WhenMarketDepthChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenMarketDepthChanged)

   \- the rule when instrument has the order book change event. 
- [MarketRuleHelper.WhenBestBidPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore)

   \- the rule for the event of the best bid increase above a specified level. 
- [MarketRuleHelper.WhenBestBidPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess)

   \- the rule for the event of the best bid decrease below a specified level. 
- [MarketRuleHelper.WhenBestAskPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore)

   \- the rule for the event of the best offer increase above a specified level. 
- [MarketRuleHelper.WhenBestAskPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess)

   \- the rule for the event of the best offer decrease below a specified level. 
- [MarketRuleHelper.WhenLastTradePriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceMore)

   \- the rule for the event of the last trade price increase above a specified level. 
- [MarketRuleHelper.WhenLastTradePriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceLess)

   \- the rule for the event of the last trade price decrease below a specified level. 

For the [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth)

- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged)

   \- the order book change event rule. 
- [MarketRuleHelper.WhenSpreadMore](xref:StockSharp.Algo.MarketRuleHelper.WhenSpreadMore)

   \- the rule for the event of the book order spread size more then specified value. 
- [MarketRuleHelper.WhenSpreadLess](xref:StockSharp.Algo.MarketRuleHelper.WhenSpreadLess)

   \- the rule for the event of the book order spread size less then specified value. 
- [MarketRuleHelper.WhenBestBidPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore)

   \- the rule for the event of the best bid increase above a specified level. 
- [MarketRuleHelper.WhenBestBidPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess)

   \- the rule for the event of the best bid decrease below a specified level. 
- [MarketRuleHelper.WhenBestAskPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore)

   \- the rule for the event of the best offer increase above a specified level. 
- [MarketRuleHelper.WhenBestAskPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess)

   \- the rule for the event of the best offer decrease below a specified level. 

For the [Order](xref:StockSharp.BusinessEntities.Order)

- [MarketRuleHelper.WhenRegistered](xref:StockSharp.Algo.MarketRuleHelper.WhenRegistered)

   \- the rule for the event of the successful order registration on exchange. 
- [MarketRuleHelper.WhenPartiallyMatched](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyMatched)

   \- the rule for the event of the partially matched order. 
- [MarketRuleHelper.WhenRegisterFailed](xref:StockSharp.Algo.MarketRuleHelper.WhenRegisterFailed)

   \- the rule for the event of the failed order registration on exchange. 
- [MarketRuleHelper.WhenCancelFailed](xref:StockSharp.Algo.MarketRuleHelper.WhenCancelFailed)

   \- the rule for the event of the failed order cancel on exchange. 
- [MarketRuleHelper.WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled)

   \- the rule for the event of the order cancel on exchange. 
- [MarketRuleHelper.WhenMatched](xref:StockSharp.Algo.MarketRuleHelper.WhenMatched)

   \- the rule for the event of the fully matched order on exchange. 
- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged)

   \- the rule for the event of the order change. 
- [MarketRuleHelper.WhenNewTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewTrade)

   \- the rule for the event of the trade occurrence by the order. 

For the [Portfolio](xref:StockSharp.BusinessEntities.Portfolio)

- [MarketRuleHelper.WhenMoneyLess](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyLess)

   \- the rule for the event of the money decrease in the portfolio below a specified level. 
- [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore)

   \- the rule for the event of the money increase in the portfolio above a specified level. 

For [Position](xref:StockSharp.BusinessEntities.Position)

- [MarketRuleHelper.WhenLess](xref:StockSharp.Algo.MarketRuleHelper.WhenLess)

   \- the rule for the event of the position decrease below a specified level. 
- [MarketRuleHelper.WhenMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMore)

   \- the rule for the event of the position increase above a specified level. 
- [MarketRuleHelper.Changed](xref:StockSharp.Algo.MarketRuleHelper.Changed)

   \- the rule for the event of the position change. 

For the [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager)

- [StrategyHelper.WhenPnLLess](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPnLLess)

   \- the rule for the event of the profit decrease below a specified level. 
- [StrategyHelper.WhenPnLMore](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPnLMore)

   \- the rule for the event of the profit increase above a specified level. 
- [StrategyHelper.WhenPositionChanged](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged)

   \- the rule for the event of the profit change. 

For the [Candle](xref:StockSharp.Algo.Candles.Candle)

- [MarketRuleHelper.WhenClosePriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenClosePriceMore)

   \- the rule for the event of the candle closing price increase above a specified level. 
- [MarketRuleHelper.WhenClosePriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenClosePriceLess)

   \- the rule for the event of the candle closing price decrease below a specified level. 
- [MarketRuleHelper.WhenTotalVolumeMore](xref:StockSharp.Algo.MarketRuleHelper.WhenTotalVolumeMore)

   \- the rule for the event of the candle total volume increase above a specified level. 
- [MarketRuleHelper.WhenCurrentCandleTotalVolumeMore](xref:StockSharp.Algo.MarketRuleHelper.WhenCurrentCandleTotalVolumeMore)

   \- the rule for the event of the current candle total volume increase above a specified level. 
- [MarketRuleHelper.WhenCandlesStarted](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesStarted)

   \- the rule for the event of the new candles occurrence. 
- [MarketRuleHelper.WhenCandlesChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesChanged)

   \- the rule for the event of the candles change. 
- [MarketRuleHelper.WhenCandlesFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesFinished)

   \- the rule for the event of the candles end. 
- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged)

   \- the rule for the event of the candle change. 
- [MarketRuleHelper.WhenFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenFinished)

   \- the rule for the event of the candle finish. 
- [MarketRuleHelper.WhenPartiallyFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinished)

   \- the rule for the event of the candle partial finish. 
- [MarketRuleHelper.WhenPartiallyFinishedCandles](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinishedCandles)

   \- the rule for the event of the candles partial finish. 

For the [Strategy](xref:StockSharp.Algo.Strategies.Strategy)

- [StrategyHelper.WhenNewMyTrade](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenNewMyTrade)

   \- the rule for the event of the strategy new trades occurrence. 
- [StrategyHelper.WhenPositionChanged](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged)

   \- the rule for the event of the strategy position change. 
- [StrategyHelper.WhenStarted](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStarted)

   \- the rule for the event of the strategy work start. 
- [StrategyHelper.WhenStopping](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStopping)

   \- the rule for the event of the strategy work stopping. 
- [StrategyHelper.WhenStopped](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStopped)

   \- the rule for the event of the strategy work stopped. 
- [StrategyHelper.WhenError](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenError)

   \- the rule for the event of the strategy error. 
- [StrategyHelper.WhenWarning](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenWarning)

   \- the rule for the event of the strategy warning. 

For the [IConnector](xref:StockSharp.BusinessEntities.IConnector)

- [MarketRuleHelper.WhenIntervalElapsed](xref:StockSharp.Algo.MarketRuleHelper.WhenIntervalElapsed)

   \- the rule for the event of the 

  [IConnector.MarketTimeChanged](xref:StockSharp.BusinessEntities.IConnector.MarketTimeChanged)

   change on value, which greater than or equal to the parameter. 
- [MarketRuleHelper.WhenTimeCome](xref:StockSharp.Algo.MarketRuleHelper.WhenTimeCome)

   \- a rule that is activated upon the exact time occurrence. 
- [MarketRuleHelper.WhenNewMyTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewMyTrade)

   \- the rule for the event of the new trade occurrence. 
- [MarketRuleHelper.WhenNewOrder](xref:StockSharp.Algo.MarketRuleHelper.WhenNewOrder)

   \- the rule for the event of the new order occurrence. 

Similarly to the conditions there are predefined actions: 

- [StrategyHelper.Register](xref:StockSharp.Algo.Strategies.StrategyHelper.Register)

   \- the action which registers the order. 
- [StrategyHelper.ReRegister](xref:StockSharp.Algo.Strategies.StrategyHelper.ReRegister)

   \- the action which reregisters the order. 
- [StrategyHelper.Cancel](xref:StockSharp.Algo.Strategies.StrategyHelper.Cancel)

   \- the action which cancels the order. 
- [Protect](xref:Overload:StockSharp.Algo.Strategies.Extensions.Protect)

   \- the action which protects trades simultaneously with 

  [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy)

   and 

  [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy)

   strategies. 

If you need to create your own unique rule (on any event, which is not provided as standard), you must create the derived [MarketRule\`2](xref:StockSharp.Algo.MarketRule`2) class, which will work with a prerequisite. Below is the [WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore) method implementation: 

```cs
		
private sealed class PortfolioRule : MarketRule<Portfolio, Portfolio>
{
	private readonly Func<Portfolio, bool> _changed;
	private readonly Portfolio _portfolio;
	private readonly IConnector _connector;
	public PortfolioRule(Portfolio portfolio, IConnector connector, Func<Portfolio, bool> changed) : base(portfolio)
	{
		if (portfolio == null)
			throw new ArgumentNullException("portfolio");
		if (changed == null)
			throw new ArgumentNullException("changed");
		_changed = changed;
		_portfolio = portfolio;
		_connector = connector;
		_connector.PortfolioChanged += OnPortfolioChanged;
	}
	private void OnPortfolioChanged(Portfolio portfolio)
	{
		if ((portfolio==_portfolio) && _changed(_portfolio))
			Activate(_portfolio);
	}
	protected override void DisposeManaged()
	{
		_connector.PortfolioChanged -= OnPortfolioChanged;
		base.DisposeManaged();
	}
}
		
public static MarketRule<Portfolio, Portfolio> WhenMoneyMore(this Portfolio portfolio, Unit money)
{
	if (portfolio == null)
		throw new ArgumentNullException("portfolio");
	if (money == null)
		throw new ArgumentNullException("money");
	var finishMoney = money.Type == UnitTypes.Limit ? money : portfolio.CurrentValue + money;
	return new PortfolioRule(portfolio, pf => pf.CurrentValue > finishMoney)
	{
		Name = "Money increase of portfolio {0} above {1}".Put(portfolio, finishMoney)
	};
}
		
```

The *PortfolioRule* rule subscribes to the [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) event, and as soon as it is called, then the condition is checked to exceed the current level of money in the portfolio above a specified limit. If the condition returns **true**, then the rule is activated through the [MarketRule\`2.Activate](xref:StockSharp.Algo.MarketRule`2.Activate) method. 
