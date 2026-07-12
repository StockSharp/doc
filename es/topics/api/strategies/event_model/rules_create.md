# Crear una regla propia

Si necesita crear su propia regla única (sobre cualquier evento que no se proporcione de forma estándar), debe crear una clase derivada de [MarketRule\<TToken,TArg\>](xref:StockSharp.Algo.MarketRule`2), que trabajará con una condición previa. A continuación se muestra la implementación del método [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [StockSharp.BusinessEntities.IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) provider, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) money **)**: 

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
		Name = "Aumento de fondos de la cartera {0} por encima de {1}".Put(portfolio, finishMoney)
	};
}		
```

La regla *PortfolioRule* se suscribe al evento [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) y, en cuanto se llama, se comprueba la condición de que el nivel actual de dinero en la cartera supere un límite especificado. Si la condición devuelve **true**, la regla se activa mediante el método [MarketRule\<TToken,TArg\>.Activate](xref:StockSharp.Algo.MarketRule`2.Activate). 
