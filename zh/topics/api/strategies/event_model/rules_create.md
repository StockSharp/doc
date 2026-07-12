# 创建自己的规则

如果您需要创建自己的独特规则（针对任何标准未提供的事件），您必须创建派生的 [MarketRule\<TToken,TArg\>](xref:StockSharp.Algo.MarketRule`2) 类，该类将与前提条件一起工作。下面是 [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [StockSharp.BusinessEntities.IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) provider, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) money **)** 方法的实现：

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
		Name = "投资组合 {0} 的资金增加超过 {1}".Put(portfolio, finishMoney)
	};
}		
```

*PortfolioRule* 规则订阅了 [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) 事件，一旦事件被触发，就会检查投资组合中的当前资金水平是否超过指定的限额。如果条件返回 **true**，则通过 [MarketRule\<TToken,TArg\>.Activate](xref:StockSharp.Algo.MarketRule`2.Activate) 方法激活该规则。
