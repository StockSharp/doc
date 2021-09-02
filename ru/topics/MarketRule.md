# Правило

В [S\#](StockSharpAbout.md) для [IMarketRule](xref:StockSharp.Algo.IMarketRule) уже есть ряд предопределенных условий и действий для наиболее распространенных сценариев. Ниже представлены сгруппированные по торговым объектам списки условий класса [MarketRuleHelper](xref:StockSharp.Algo.MarketRuleHelper): 

Для [Security](xref:StockSharp.BusinessEntities.Security)

- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие изменения инструмента. 
- [MarketRuleHelper.WhenNewTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewTrade(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие появления у инструмента новой сделки. 
- [MarketRuleHelper.WhenMarketDepthChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenMarketDepthChanged(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие изменения стакана по инструменту. 
- [MarketRuleHelper.WhenBestBidPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Messages.Unit)) \- правило на событие повышения лучшего бида выше определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Messages.Unit)) \- правило на событие понижения лучшего бида ниже определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Messages.Unit)) \- правило на событие повышения лучшего оффера выше определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Messages.Unit)) \- правило на событие понижения лучшего оффера ниже определенного уровня. 
- [MarketRuleHelper.WhenLastTradePriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceMore(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Messages.Unit)) \- правило на событие повышения цены последней сделки выше определенного уровня. 
- [MarketRuleHelper.WhenLastTradePriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceLess(StockSharp.BusinessEntities.Security,StockSharp.BusinessEntities.IMarketDataProvider,StockSharp.Messages.Unit)) \- правило на событие понижения цены последней сделки ниже определенного уровня. 

Для [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth)

- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged(StockSharp.BusinessEntities.MarketDepth,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие изменения стакана. 
- [MarketRuleHelper.WhenSpreadMore](xref:StockSharp.Algo.MarketRuleHelper.WhenSpreadMore(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие повышения размера спреда стакана выше определенного уровня. 
- [MarketRuleHelper.WhenSpreadLess](xref:StockSharp.Algo.MarketRuleHelper.WhenSpreadLess(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие понижения размера спреда стакана ниже определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие повышения лучшего бида выше определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие понижения лучшего бида ниже определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие повышения лучшего оффера выше определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit,StockSharp.BusinessEntities.IMarketDataProvider)) \- правило на событие понижения лучшего оффера ниже определенного уровня. 

Для [Order](xref:StockSharp.BusinessEntities.Order)

- [MarketRuleHelper.WhenRegistered](xref:StockSharp.Algo.MarketRuleHelper.WhenRegistered(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие успешной регистрации заявки на бирже. 
- [MarketRuleHelper.WhenPartiallyMatched](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyMatched(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие частичного исполнения заявки. 
- [MarketRuleHelper.WhenRegisterFailed](xref:StockSharp.Algo.MarketRuleHelper.WhenRegisterFailed(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие неудачной регистрации заявки на бирже. 
- [MarketRuleHelper.WhenCancelFailed](xref:StockSharp.Algo.MarketRuleHelper.WhenCancelFailed(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие неудачного снятия заявки на бирже. 
- [MarketRuleHelper.WhenCanceled](xref:StockSharp.Algo.MarketRuleHelper.WhenCanceled(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие отмены заявки. 
- [MarketRuleHelper.WhenMatched](xref:StockSharp.Algo.MarketRuleHelper.WhenMatched(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие полного исполнения заявки. 
- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие изменения заявки. 
- [MarketRuleHelper.WhenNewTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewTrade(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие появления сделки по заявке. 

Для [Portfolio](xref:StockSharp.BusinessEntities.Portfolio)

- [MarketRuleHelper.WhenMoneyLess](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyLess(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit)) \- правило на событие уменьшения денег в портфеле ниже определенного уровня. 
- [MarketRuleHelper.WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit)) \- правило на событие увеличения денег в портфеле выше определенного уровня. 

Для [Position](xref:StockSharp.BusinessEntities.Position)

- [MarketRuleHelper.WhenLess](xref:StockSharp.Algo.MarketRuleHelper.WhenLess(StockSharp.BusinessEntities.Position,StockSharp.BusinessEntities.IPositionProvider,StockSharp.Messages.Unit)) \- правило на событие уменьшения позиции ниже определенного уровня. 
- [MarketRuleHelper.WhenMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMore(StockSharp.BusinessEntities.Position,StockSharp.BusinessEntities.IPositionProvider,StockSharp.Messages.Unit)) \- правило на событие увеличения позиции выше определенного уровня. 
- [MarketRuleHelper.Changed](xref:StockSharp.Algo.MarketRuleHelper.Changed(StockSharp.BusinessEntities.Position,StockSharp.BusinessEntities.IPositionProvider)) \- правило на событие изменения позиции. 

Для [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager)

- [StrategyHelper.WhenPnLLess](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPnLLess(StockSharp.Algo.Strategies.Strategy,StockSharp.Messages.Unit)) \- правило на событие уменьшения прибыли ниже определенного уровня. 
- [StrategyHelper.WhenPnLMore](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPnLMore(StockSharp.Algo.Strategies.Strategy,StockSharp.Messages.Unit)) \- правило на событие увеличения прибыли выше определенного уровня. 
- [StrategyHelper.WhenPositionChanged](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged(StockSharp.Algo.Strategies.Strategy)) \- правило на событие изменения прибыли. 

Для [Candle](xref:StockSharp.Algo.Candles.Candle)

- [MarketRuleHelper.WhenClosePriceMore](xref:StockSharp.Algo.MarketRuleHelper.WhenClosePriceMore(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.Candle,StockSharp.Messages.Unit)) \- правило на событие повышения цены закрытия свечи выше определенного уровня. 
- [MarketRuleHelper.WhenClosePriceLess](xref:StockSharp.Algo.MarketRuleHelper.WhenClosePriceLess(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.Candle,StockSharp.Messages.Unit)) \- правило на событие понижения цены закрытия свечи ниже определенного уровня. 
- [MarketRuleHelper.WhenTotalVolumeMore](xref:StockSharp.Algo.MarketRuleHelper.WhenTotalVolumeMore(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.Candle,StockSharp.Messages.Unit)) \- правило на событие превышения общего объема свечи выше определенного уровня. 
- [MarketRuleHelper.WhenCurrentCandleTotalVolumeMore](xref:StockSharp.Algo.MarketRuleHelper.WhenCurrentCandleTotalVolumeMore(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.CandleSeries,StockSharp.Messages.Unit)) \- правило на событие превышения общего объема текущей свечи выше определенного уровня. 
- [MarketRuleHelper.WhenCandlesStarted](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesStarted(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.CandleSeries)) \- правило на событие появления новых свечей. 
- [MarketRuleHelper.WhenCandlesChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesChanged(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.CandleSeries)) \- правило на событие изменения свечей. 
- [MarketRuleHelper.WhenCandlesFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenCandlesFinished(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.CandleSeries)) \- правило на событие окончания свечей. 
- [MarketRuleHelper.WhenChanged](xref:StockSharp.Algo.MarketRuleHelper.WhenChanged(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.Candle)) \- правило на событие изменения свечи. 
- [MarketRuleHelper.WhenFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenFinished(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.Candle)) \- правило на событие окончания свечи. 
- [MarketRuleHelper.WhenPartiallyFinished](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinished(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.Candle,StockSharp.BusinessEntities.IConnector,System.Decimal)) \- правило на событие частичного окончания свечи. 
- [MarketRuleHelper.WhenPartiallyFinishedCandles](xref:StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinishedCandles(StockSharp.Algo.Candles.ICandleManager,StockSharp.Algo.Candles.CandleSeries,StockSharp.BusinessEntities.IConnector,System.Decimal)) \- правило на событие частичного окончания свечей. 

Для [Strategy](xref:StockSharp.Algo.Strategies.Strategy)

- [StrategyHelper.WhenNewMyTrade](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenNewMyTrade(StockSharp.Algo.Strategies.Strategy)) \- правило на событие появление новых сделок стратегии. 
- [StrategyHelper.WhenPositionChanged](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged(StockSharp.Algo.Strategies.Strategy)) \- правило на событие изменения позиции у стратегии. 
- [StrategyHelper.WhenStarted](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStarted(StockSharp.Algo.Strategies.Strategy)) \- правило на событие начала работы стратегии. 
- [StrategyHelper.WhenStopping](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStopping(StockSharp.Algo.Strategies.Strategy)) \- правило на событие начала остановки работы стратегии. 
- [StrategyHelper.WhenStopped](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenStopped(StockSharp.Algo.Strategies.Strategy)) \- правило на событие полной остановки работы стратегии. 
- [StrategyHelper.WhenError](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenError(StockSharp.Algo.Strategies.Strategy,System.Boolean)) \- правило на событие ошибки стратегии. 
- [StrategyHelper.WhenWarning](xref:StockSharp.Algo.Strategies.StrategyHelper.WhenWarning(StockSharp.Algo.Strategies.Strategy)) \- правило на событие предупреждения стратегии. 

Для [IConnector](xref:StockSharp.BusinessEntities.IConnector)

- [MarketRuleHelper.WhenIntervalElapsed](xref:StockSharp.Algo.MarketRuleHelper.WhenIntervalElapsed(StockSharp.BusinessEntities.IConnector,System.TimeSpan)) \- правило на событие изменения [IConnector.MarketTimeChanged](xref:StockSharp.BusinessEntities.IConnector.MarketTimeChanged) на значение, большее или равно параметру. 
- [MarketRuleHelper.WhenTimeCome](xref:StockSharp.Algo.MarketRuleHelper.WhenTimeCome(StockSharp.BusinessEntities.IConnector,System.DateTimeOffset[])) \- правило, которое активизируется при наступлении точного времени. 
- [MarketRuleHelper.WhenNewMyTrade](xref:StockSharp.Algo.MarketRuleHelper.WhenNewMyTrade(StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие появление новой сделки. 
- [MarketRuleHelper.WhenNewOrder](xref:StockSharp.Algo.MarketRuleHelper.WhenNewOrder(StockSharp.BusinessEntities.ITransactionProvider)) \- правило на событие появление новой заявки. 

Аналогично условиям есть и предопределенные действия: 

- [StrategyHelper.Register](xref:StockSharp.Algo.Strategies.StrategyHelper.Register(StockSharp.Algo.IMarketRule,StockSharp.BusinessEntities.Order)) \- действие, регистрирующее заявку. 
- [StrategyHelper.ReRegister](xref:StockSharp.Algo.Strategies.StrategyHelper.ReRegister(StockSharp.Algo.IMarketRule,StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)) \- действие, перерегистрирующее заявку. 
- [StrategyHelper.Cancel](xref:StockSharp.Algo.Strategies.StrategyHelper.Cancel(StockSharp.Algo.IMarketRule,StockSharp.BusinessEntities.Order)) \- действие, отменяющее заявку. 
- [Protect](xref:StockSharp.Algo.Strategies.Extensions.Protect(StockSharp.Algo.MarketRule{StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.MyTrade},StockSharp.Messages.Unit,StockSharp.Messages.Unit)) \- действие, защищающее сделки одновременно стратегиями [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) и [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy). 

Если требуется создать свое уникальное правило (на какое\-то событие, которое не предусмотрено стандартно), необходимо создать свой класс\-наследник [MarketRule\`2](xref:StockSharp.Algo.MarketRule`2), который будет работать с необходимым условием. Ниже приведена реализация метода [WhenMoneyMore](xref:StockSharp.Algo.MarketRuleHelper.WhenMoneyMore(StockSharp.BusinessEntities.Portfolio,StockSharp.BusinessEntities.IPortfolioProvider,StockSharp.Messages.Unit)): 

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
