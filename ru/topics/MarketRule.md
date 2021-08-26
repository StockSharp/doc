# Правило

В [S\#](StockSharpAbout.md) для [IMarketRule](../api/StockSharp.Algo.IMarketRule.html) уже есть ряд предопределенных условий и действий для наиболее распространенных сценариев. Ниже представлены сгруппированные по торговым объектам списки условий класса [MarketRuleHelper](../api/StockSharp.Algo.MarketRuleHelper.html): 

Для [Security](../api/StockSharp.BusinessEntities.Security.html)

- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- правило на событие изменения инструмента. 
- [MarketRuleHelper.WhenNewTrade](../api/StockSharp.Algo.MarketRuleHelper.WhenNewTrade.html)

   \- правило на событие появления у инструмента новой сделки. 
- [MarketRuleHelper.WhenMarketDepthChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenMarketDepthChanged.html)

   \- правило на событие изменения стакана по инструменту. 
- [MarketRuleHelper.WhenBestBidPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore.html)

   \- правило на событие повышения лучшего бида выше определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess.html)

   \- правило на событие понижения лучшего бида ниже определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore.html)

   \- правило на событие повышения лучшего оффера выше определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess.html)

   \- правило на событие понижения лучшего оффера ниже определенного уровня. 
- [MarketRuleHelper.WhenLastTradePriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceMore.html)

   \- правило на событие повышения цены последней сделки выше определенного уровня. 
- [MarketRuleHelper.WhenLastTradePriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenLastTradePriceLess.html)

   \- правило на событие понижения цены последней сделки ниже определенного уровня. 

Для [MarketDepth](../api/StockSharp.BusinessEntities.MarketDepth.html)

- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- правило на событие изменения стакана. 
- [MarketRuleHelper.WhenSpreadMore](../api/StockSharp.Algo.MarketRuleHelper.WhenSpreadMore.html)

   \- правило на событие повышения размера спреда стакана выше определенного уровня. 
- [MarketRuleHelper.WhenSpreadLess](../api/StockSharp.Algo.MarketRuleHelper.WhenSpreadLess.html)

   \- правило на событие понижения размера спреда стакана ниже определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceMore.html)

   \- правило на событие повышения лучшего бида выше определенного уровня. 
- [MarketRuleHelper.WhenBestBidPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestBidPriceLess.html)

   \- правило на событие понижения лучшего бида ниже определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceMore.html)

   \- правило на событие повышения лучшего оффера выше определенного уровня. 
- [MarketRuleHelper.WhenBestAskPriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenBestAskPriceLess.html)

   \- правило на событие понижения лучшего оффера ниже определенного уровня. 

Для [Order](../api/StockSharp.BusinessEntities.Order.html)

- [MarketRuleHelper.WhenRegistered](../api/StockSharp.Algo.MarketRuleHelper.WhenRegistered.html)

   \- правило на событие успешной регистрации заявки на бирже. 
- [MarketRuleHelper.WhenPartiallyMatched](../api/StockSharp.Algo.MarketRuleHelper.WhenPartiallyMatched.html)

   \- правило на событие частичного исполнения заявки. 
- [MarketRuleHelper.WhenRegisterFailed](../api/StockSharp.Algo.MarketRuleHelper.WhenRegisterFailed.html)

   \- правило на событие неудачной регистрации заявки на бирже. 
- [MarketRuleHelper.WhenCancelFailed](../api/StockSharp.Algo.MarketRuleHelper.WhenCancelFailed.html)

   \- правило на событие неудачного снятия заявки на бирже. 
- [MarketRuleHelper.WhenCanceled](../api/StockSharp.Algo.MarketRuleHelper.WhenCanceled.html)

   \- правило на событие отмены заявки. 
- [MarketRuleHelper.WhenMatched](../api/StockSharp.Algo.MarketRuleHelper.WhenMatched.html)

   \- правило на событие полного исполнения заявки. 
- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- правило на событие изменения заявки. 
- [MarketRuleHelper.WhenNewTrade](../api/StockSharp.Algo.MarketRuleHelper.WhenNewTrade.html)

   \- правило на событие появления сделки по заявке. 

Для [Portfolio](../api/StockSharp.BusinessEntities.Portfolio.html)

- [MarketRuleHelper.WhenMoneyLess](../api/StockSharp.Algo.MarketRuleHelper.WhenMoneyLess.html)

   \- правило на событие уменьшения денег в портфеле ниже определенного уровня. 
- [MarketRuleHelper.WhenMoneyMore](../api/StockSharp.Algo.MarketRuleHelper.WhenMoneyMore.html)

   \- правило на событие увеличения денег в портфеле выше определенного уровня. 

Для [Position](../api/StockSharp.BusinessEntities.Position.html)

- [MarketRuleHelper.WhenLess](../api/StockSharp.Algo.MarketRuleHelper.WhenLess.html)

   \- правило на событие уменьшения позиции ниже определенного уровня. 
- [MarketRuleHelper.WhenMore](../api/StockSharp.Algo.MarketRuleHelper.WhenMore.html)

   \- правило на событие увеличения позиции выше определенного уровня. 
- [MarketRuleHelper.Changed](../api/StockSharp.Algo.MarketRuleHelper.Changed.html)

   \- правило на событие изменения позиции. 

Для [IPnLManager](../api/StockSharp.Algo.PnL.IPnLManager.html)

- [StrategyHelper.WhenPnLLess](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPnLLess.html)

   \- правило на событие уменьшения прибыли ниже определенного уровня. 
- [StrategyHelper.WhenPnLMore](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPnLMore.html)

   \- правило на событие увеличения прибыли выше определенного уровня. 
- [StrategyHelper.WhenPositionChanged](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged.html)

   \- правило на событие изменения прибыли. 

Для [Candle](../api/StockSharp.Algo.Candles.Candle.html)

- [MarketRuleHelper.WhenClosePriceMore](../api/StockSharp.Algo.MarketRuleHelper.WhenClosePriceMore.html)

   \- правило на событие повышения цены закрытия свечи выше определенного уровня. 
- [MarketRuleHelper.WhenClosePriceLess](../api/StockSharp.Algo.MarketRuleHelper.WhenClosePriceLess.html)

   \- правило на событие понижения цены закрытия свечи ниже определенного уровня. 
- [MarketRuleHelper.WhenTotalVolumeMore](../api/StockSharp.Algo.MarketRuleHelper.WhenTotalVolumeMore.html)

   \- правило на событие превышения общего объема свечи выше определенного уровня. 
- [MarketRuleHelper.WhenCurrentCandleTotalVolumeMore](../api/StockSharp.Algo.MarketRuleHelper.WhenCurrentCandleTotalVolumeMore.html)

   \- правило на событие превышения общего объема текущей свечи выше определенного уровня. 
- [MarketRuleHelper.WhenCandlesStarted](../api/StockSharp.Algo.MarketRuleHelper.WhenCandlesStarted.html)

   \- правило на событие появления новых свечей. 
- [MarketRuleHelper.WhenCandlesChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenCandlesChanged.html)

   \- правило на событие изменения свечей. 
- [MarketRuleHelper.WhenCandlesFinished](../api/StockSharp.Algo.MarketRuleHelper.WhenCandlesFinished.html)

   \- правило на событие окончания свечей. 
- [MarketRuleHelper.WhenChanged](../api/StockSharp.Algo.MarketRuleHelper.WhenChanged.html)

   \- правило на событие изменения свечи. 
- [MarketRuleHelper.WhenFinished](../api/StockSharp.Algo.MarketRuleHelper.WhenFinished.html)

   \- правило на событие окончания свечи. 
- [MarketRuleHelper.WhenPartiallyFinished](../api/StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinished.html)

   \- правило на событие частичного окончания свечи. 
- [MarketRuleHelper.WhenPartiallyFinishedCandles](../api/StockSharp.Algo.MarketRuleHelper.WhenPartiallyFinishedCandles.html)

   \- правило на событие частичного окончания свечей. 

Для [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html)

- [StrategyHelper.WhenNewMyTrade](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenNewMyTrade.html)

   \- правило на событие появление новых сделок стратегии. 
- [StrategyHelper.WhenPositionChanged](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenPositionChanged.html)

   \- правило на событие изменения позиции у стратегии. 
- [StrategyHelper.WhenStarted](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenStarted.html)

   \- правило на событие начала работы стратегии. 
- [StrategyHelper.WhenStopping](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenStopping.html)

   \- правило на событие начала остановки работы стратегии. 
- [StrategyHelper.WhenStopped](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenStopped.html)

   \- правило на событие полной остановки работы стратегии. 
- [StrategyHelper.WhenError](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenError.html)

   \- правило на событие ошибки стратегии. 
- [StrategyHelper.WhenWarning](../api/StockSharp.Algo.Strategies.StrategyHelper.WhenWarning.html)

   \- правило на событие предупреждения стратегии. 

Для [IConnector](../api/StockSharp.BusinessEntities.IConnector.html)

- [MarketRuleHelper.WhenIntervalElapsed](../api/StockSharp.Algo.MarketRuleHelper.WhenIntervalElapsed.html)

   \- правило на событие изменения 

  [IConnector.MarketTimeChanged](../api/StockSharp.BusinessEntities.IConnector.MarketTimeChanged.html)

   на значение, большее или равно параметру. 
- [MarketRuleHelper.WhenTimeCome](../api/StockSharp.Algo.MarketRuleHelper.WhenTimeCome.html)

   \- правило, которое активизируется при наступлении точного времени. 
- [MarketRuleHelper.WhenNewMyTrade](../api/StockSharp.Algo.MarketRuleHelper.WhenNewMyTrade.html)

   \- правило на событие появление новой сделки. 
- [MarketRuleHelper.WhenNewOrder](../api/StockSharp.Algo.MarketRuleHelper.WhenNewOrder.html)

   \- правило на событие появление новой заявки. 

Аналогично условиям есть и предопределенные действия: 

- [StrategyHelper.Register](../api/StockSharp.Algo.Strategies.StrategyHelper.Register.html)

   \- действие, регистрирующее заявку. 
- [StrategyHelper.ReRegister](../api/StockSharp.Algo.Strategies.StrategyHelper.ReRegister.html)

   \- действие, перерегистрирующее заявку. 
- [StrategyHelper.Cancel](../api/StockSharp.Algo.Strategies.StrategyHelper.Cancel.html)

   \- действие, отменяющее заявку. 
- [Protect](../api/Overload:StockSharp.Algo.Strategies.Extensions.Protect.html)

   \- действие, защищающее сделки одновременно стратегиями 

  [TakeProfitStrategy](../api/StockSharp.Algo.Strategies.Protective.TakeProfitStrategy.html)

   и 

  [StopLossStrategy](../api/StockSharp.Algo.Strategies.Protective.StopLossStrategy.html)

  . 

Если требуется создать свое уникальное правило (на какое\-то событие, которое не предусмотрено стандартно), необходимо создать свой класс\-наследник [MarketRule\`2](../api/StockSharp.Algo.MarketRule`2.html), который будет работать с необходимым условием. Ниже приведена реализация метода [WhenMoneyMore](../api/StockSharp.Algo.MarketRuleHelper.WhenMoneyMore.html): 

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

Правило *PortfolioRule* подписывается на событие [IPortfolioProvider.PortfolioChanged](../api/StockSharp.BusinessEntities.IPortfolioProvider.PortfolioChanged.html) и, как только оно вызывается, то проверяется условие на превышение текущего уровня денежных средств в портфеле выше определенного лимита. Если условие возвращает **true**, то активируется правило через метод [MarketRule\`2.Activate](../api/StockSharp.Algo.MarketRule`2.Activate.html). 
