# Собственное правило

Если требуется создать свое уникальное правило (на какое\-то событие, которое не предусмотрено стандартно), необходимо создать свой класс\-наследник [MarketRule\<TToken,TArg\>](xref:StockSharp.Algo.MarketRule`2), который будет работать с необходимым условием. Ниже приведена реализация метода [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [StockSharp.BusinessEntities.IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) provider, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) money **)**: 

```cs
private sealed class PortfolioRule : MarketRule<Portfolio, Portfolio>
{
	private readonly Func<Portfolio, bool> _changed;
	private readonly Portfolio _portfolio;
	private readonly IPortfolioProvider _provider;

	public PortfolioRule(Portfolio portfolio, IPortfolioProvider provider, Func<Portfolio, bool> changed)
		: base(portfolio)
	{
		_changed = changed ?? throw new ArgumentNullException(nameof(changed));
		_portfolio = portfolio ?? throw new ArgumentNullException(nameof(portfolio));
		_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		_provider.PortfolioChanged += OnPortfolioChanged;
	}

	private void OnPortfolioChanged(Portfolio portfolio)
	{
		if (portfolio == _portfolio && _changed(_portfolio))
			Activate(_portfolio);
	}

	protected override void DisposeManaged()
	{
		_provider.PortfolioChanged -= OnPortfolioChanged;
		base.DisposeManaged();
	}
}

public static MarketRule<Portfolio, Portfolio> WhenMoneyMore(this Portfolio portfolio, IPortfolioProvider provider, Unit money)
{
	if (portfolio == null)
		throw new ArgumentNullException(nameof(portfolio));
	if (money == null)
		throw new ArgumentNullException(nameof(money));

	var finishMoney = money.Type == UnitTypes.Percent ? portfolio.CurrentValue + money : money;

	return new PortfolioRule(portfolio, provider, pf => pf.CurrentValue > finishMoney)
	{
		Name = $"PF {portfolio.Name} > {finishMoney}"
	};
}
```

Правило *PortfolioRule* подписывается на событие [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) через интерфейс [IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) и, как только оно вызывается, то проверяется условие на превышение текущего уровня денежных средств в портфеле выше определенного лимита. Если условие возвращает **true**, то активируется правило через метод [MarketRule\<TToken,TArg\>.Activate](xref:StockSharp.Algo.MarketRule`2.Activate).