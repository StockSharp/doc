# Create own rule

If you need to create your own unique rule (on any event, which is not provided as standard), you must create the derived [MarketRule\<TToken,TArg\>](xref:StockSharp.Algo.MarketRule`2) class, which will work with a prerequisite. Below is the [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [StockSharp.BusinessEntities.IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) provider, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) money **)** method implementation: 

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

The *PortfolioRule* rule subscribes to the [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) event, and as soon as it is called, then the condition is checked to exceed the current level of money in the portfolio above a specified limit. If the condition returns **true**, then the rule is activated through the [MarketRule\<TToken,TArg\>.Activate](xref:StockSharp.Algo.MarketRule`2.Activate) method. 
