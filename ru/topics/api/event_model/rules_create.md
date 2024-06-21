# Собственное правило

Если требуется создать свое уникальное правило (на какое\-то событие, которое не предусмотрено стандартно), необходимо создать свой класс\-наследник [MarketRule\<TToken,TArg\>](xref:StockSharp.Algo.MarketRule`2), который будет работать с необходимым условием. Ниже приведена реализация метода [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [StockSharp.BusinessEntities.IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) provider, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) money **)**: 

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
		Name = "Увеличение денег портфеля {0} выше {1}".Put(portfolio, finishMoney)
	};
}		
```

Правило *PortfolioRule* подписывается на событие [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) и, как только оно вызывается, то проверяется условие на превышение текущего уровня денежных средств в портфеле выше определенного лимита. Если условие возвращает **true**, то активируется правило через метод [MarketRule\<TToken,TArg\>.Activate](xref:StockSharp.Algo.MarketRule`2.Activate).