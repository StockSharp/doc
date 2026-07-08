# Eigene Regel erstellen

Wenn Sie eine eigene eindeutige Regel erstellen müssen (für ein Ereignis, das nicht standardmäßig bereitgestellt wird), müssen Sie eine abgeleitete Klasse [MarketRule\<TToken,TArg\>](xref:StockSharp.Algo.MarketRule`2) erstellen, die mit einer Vorbedingung arbeitet. Unten sehen Sie die Implementierung der Methode [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.Portfolio](xref:StockSharp.BusinessEntities.Portfolio) portfolio, [StockSharp.BusinessEntities.IPortfolioProvider](xref:StockSharp.BusinessEntities.IPortfolioProvider) provider, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) money **)**:

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

Die Regel *PortfolioRule* abonniert das Ereignis [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged). Sobald es ausgelöst wird, wird geprüft, ob der aktuelle Geldbetrag im Portfolio den angegebenen Grenzwert überschreitet. Wenn die Bedingung **true** zurückgibt, wird die Regel über die Methode [MarketRule\<TToken,TArg\>.Activate](xref:StockSharp.Algo.MarketRule`2.Activate) aktiviert.
