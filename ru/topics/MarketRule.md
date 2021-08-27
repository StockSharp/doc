# Правило

В [S\#](StockSharpAbout.md) для [IMarketRule](xref:StockSharp.Algo.IMarketRule) уже есть ряд предопределенных условий и действий для наиболее распространенных сценариев. Ниже представлены сгруппированные по торговым объектам списки условий класса [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper): 

Для [Security](xref:StockSharp.BusinessEntities.Security)

- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged) \- правило на событие изменения инструмента. 
- [MarketRuleHelper.WhenNewTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewTrade) \- правило на событие появления у инструмента новой сделки. 
- [MarketRuleHelper.WhenMarketDepthChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenMarketDepthChanged) \- правило на событие изменения стакана по инструменту. 
- [MarketRuleHelper.WhenBestBidPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore) \- правило на событие повышения лучшего бида выше определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess) \- правило на событие понижения лучшего бида ниже определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore) \- правило на событие повышения лучшего оффера выше определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess) \- правило на событие понижения лучшего оффера ниже определенного уровня. 
- [MarketRuleHelper.WhenLastTradePriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceMore) \- правило на событие повышения цены последней сделки выше определенного уровня. 
- [MarketRuleHelper.WhenLastTradePriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceLess) \- правило на событие понижения цены последней сделки ниже определенного уровня. 

Для [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth)

- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged) \- правило на событие изменения стакана. 
- [MarketRuleHelper.WhenSpreadMore](xref:StockSharp.Algo.MarketRuleHelper.WhenSpreadMore) \- правило на событие повышения размера спреда стакана выше определенного уровня. 
- [MarketRuleHelper.WhenSpreadLess](xref:StockSharp.Algo.MarketRuleHelper.WhenSpreadLess) \- правило на событие понижения размера спреда стакана ниже определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore) \- правило на событие повышения лучшего бида выше определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess) \- правило на событие понижения лучшего бида ниже определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore) \- правило на событие повышения лучшего оффера выше определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess) \- правило на событие понижения лучшего оффера ниже определенного уровня. 

Для [Order](xref:StockSharp.BusinessEntities.Order)

- [MarketRuleHelper.WhenRegistered](xref:StockSharp.Algo.MarketRuleHelper.WhenRegistered) \- правило на событие успешной регистрации заявки на бирже. 
- [MarketRuleHelper.WhenPartiallyMatched](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyMatched) \- правило на событие частичного исполнения заявки. 
- [MarketRuleHelper.WhenRegisterFailed](xref:StockSharp.Algo.MarketRuleHelper.WhenRegisterFailed) \- правило на событие неудачной регистрации заявки на бирже. 
- [MarketRuleHelper.WhenCancelFailed](xref:StockSharp.Algo.MarketRuleHelper.WhenCancelFailed) \- правило на событие неудачного снятия заявки на бирже. 
- [MarketRuleHelper.WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled) \- правило на событие отмены заявки. 
- [MarketRuleHelper.WhenMatched](xref:StockSharp.Algo.MarketRuleHelper.WhenMatched) \- правило на событие полного исполнения заявки. 
- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged) \- правило на событие изменения заявки. 
- [MarketRuleHelper.WhenNewTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewTrade) \- правило на событие появления сделки по заявке. 

Для [Portfolio](xref:StockSharp.BusinessEntities.Portfolio)

- [MarketRuleHelper.WhenMoneyLess](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyLess) \- правило на событие уменьшения денег в портфеле ниже определенного уровня. 
- [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore) \- правило на событие увеличения денег в портфеле выше определенного уровня. 

Для [Position](xref:StockSharp.BusinessEntities.Position)

- [MarketRuleHelper.WhenLess](xref:StockSharp.Algo.MarketRuleHelper.WhenLess) \- правило на событие уменьшения позиции ниже определенного уровня. 
- [MarketRuleHelper.WhenMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMore) \- правило на событие увеличения позиции выше определенного уровня. 
- [MarketRuleHelper.Changed](xref:StockSharp.Algo.MarketRuleHelper.Changed) \- правило на событие изменения позиции. 

Для [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager)

- [StrategyHelper.WhenPnLLess](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPnLLess) \- правило на событие уменьшения прибыли ниже определенного уровня. 
- [StrategyHelper.WhenPnLMore](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPnLMore) \- правило на событие увеличения прибыли выше определенного уровня. 
- [StrategyHelper.WhenPositionChanged](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged) \- правило на событие изменения прибыли. 

Для [Candle](xref:StockSharp.Algo.Candles.Candle)

- [MarketRuleHelper.WhenClosePriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenClosePriceMore) \- правило на событие повышения цены закрытия свечи выше определенного уровня. 
- [MarketRuleHelper.WhenClosePriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenClosePriceLess) \- правило на событие понижения цены закрытия свечи ниже определенного уровня. 
- [MarketRuleHelper.WhenTotalVolumeMore](xref:StockSharp.Algo.MarketRuleHelper.WhenTotalVolumeMore) \- правило на событие превышения общего объема свечи выше определенного уровня. 
- [MarketRuleHelper.WhenCurrentCandleTotalVolumeMore](xref:StockSharp.Algo.MarketRuleHelper.WhenCurrentCandleTotalVolumeMore) \- правило на событие превышения общего объема текущей свечи выше определенного уровня. 
- [MarketRuleHelper.WhenCandlesStarted](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesStarted) \- правило на событие появления новых свечей. 
- [MarketRuleHelper.WhenCandlesChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesChanged) \- правило на событие изменения свечей. 
- [MarketRuleHelper.WhenCandlesFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesFinished) \- правило на событие окончания свечей. 
- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged) \- правило на событие изменения свечи. 
- [MarketRuleHelper.WhenFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenFinished) \- правило на событие окончания свечи. 
- [MarketRuleHelper.WhenPartiallyFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinished) \- правило на событие частичного окончания свечи. 
- [MarketRuleHelper.WhenPartiallyFinishedCandles](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinishedCandles) \- правило на событие частичного окончания свечей. 

Для [Strategy](xref:StockSharp.Algo.Strategies.Strategy)

- [StrategyHelper.WhenNewMyTrade](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenNewMyTrade) \- правило на событие появление новых сделок стратегии. 
- [StrategyHelper.WhenPositionChanged](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged) \- правило на событие изменения позиции у стратегии. 
- [StrategyHelper.WhenStarted](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStarted) \- правило на событие начала работы стратегии. 
- [StrategyHelper.WhenStopping](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStopping) \- правило на событие начала остановки работы стратегии. 
- [StrategyHelper.WhenStopped](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStopped) \- правило на событие полной остановки работы стратегии. 
- [StrategyHelper.WhenError](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenError) \- правило на событие ошибки стратегии. 
- [StrategyHelper.WhenWarning](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenWarning) \- правило на событие предупреждения стратегии. 

Для [IConnector](xref:StockSharp.BusinessEntities.IConnector)

- [MarketRuleHelper.WhenIntervalElapsed](xref:StockSharp.Algo.MarketRuleHelper.WhenIntervalElapsed) \- правило на событие изменения [IConnector.MarketTimeChanged](xref:StockSharp.BusinessEntities.IConnector.MarketTimeChanged) на значение, большее или равно параметру. 
- [MarketRuleHelper.WhenTimeCome](xref:StockSharp.Algo.MarketRuleHelper.WhenTimeCome) \- правило, которое активизируется при наступлении точного времени. 
- [MarketRuleHelper.WhenNewMyTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewMyTrade) \- правило на событие появление новой сделки. 
- [MarketRuleHelper.WhenNewOrder](xref:StockSharp.Algo.MarketRuleHelper.WhenNewOrder) \- правило на событие появление новой заявки. 

Аналогично условиям есть и предопределенные действия: 

- [StrategyHelper.Register](xref:StockSharp.Algo.Strategies.StrategyHelper.Register) \- действие, регистрирующее заявку. 
- [StrategyHelper.ReRegister](xref:StockSharp.Algo.Strategies.StrategyHelper.ReRegister) \- действие, перерегистрирующее заявку. 
- [StrategyHelper.Cancel](xref:StockSharp.Algo.Strategies.StrategyHelper.Cancel) \- действие, отменяющее заявку. 
- [Protect](xref:Overload:StockSharp.Algo.Strategies.Extensions.Protect) \- действие, защищающее сделки одновременно стратегиями [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) и [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy). 

Если требуется создать свое уникальное правило (на какое\-то событие, которое не предусмотрено стандартно), необходимо создать свой класс\-наследник [MarketRule\`2](xref:StockSharp.Algo.MarketRule`2), который будет работать с необходимым условием. Ниже приведена реализация метода [WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore): 

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

Правило *PortfolioRule* подписывается на событие [IPortfolioProvider.PortfolioChanged](xref:StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged) и, как только оно вызывается, то проверяется условие на превышение текущего уровня денежных средств в портфеле выше определенного лимита. Если условие возвращает **true**, то активируется правило через метод [MarketRule\`2.Activate](xref:StockSharp.Algo.MarketRule`2.Activate). 
