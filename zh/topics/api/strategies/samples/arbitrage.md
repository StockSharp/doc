# 套利策略

## 概览

`ArbitrageStrategy` 是一种期货合约与其基础资产之间的套利策略。它跟踪工具之间的价差，并在出现套利机会时开仓。

## 主要组件

该策略继承自 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 并使用参数进行配置：

```cs
public class ArbitrageStrategy : Strategy
{
	private enum ArbitrageState
	{
		Contango,        // Futures price is higher than the underlying asset
		Backwardation,   // Underlying asset price is higher than the futures
		None,            // No position
		OrderRegistration // In the process of registering orders
	}

	// Strategy parameters
	private readonly StrategyParam<Security> _futureSecurity;
	private readonly StrategyParam<Security> _stockSecurity;
	private readonly StrategyParam<Portfolio> _futurePortfolio;
	private readonly StrategyParam<Portfolio> _stockPortfolio;
	private readonly StrategyParam<decimal> _stockMultiplicator;
	private readonly StrategyParam<decimal> _futureVolume;
	private readonly StrategyParam<decimal> _stockVolume;
	private readonly StrategyParam<decimal> _profitToExit;
	private readonly StrategyParam<decimal> _spreadToGenerateSignal;
}
```

## 策略参数

该策略允许自定义以下参数：

- **FutureSecurity** - 期货工具
- **股票证券** - 基础资产工具
- **FuturePortfolio** - 期货交易组合
- **股票投资组合** - 用于基础资产交易的投资组合
- **股票乘数** - 标的资产的乘数（e.g.，合约单位）
- **期货交易量** - 期货交易的交易量
- **股票交易量** - 基础资产交易的交易量
- **ProfitToExit** - 平仓的利润阈值
- **SpreadToGenerateSignal** - 产生入场信号的点差阈值

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，对参数进行了验证，并创建了对订单簿和自身交易的订阅：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	if (FutureSecurity == null)
		throw new InvalidOperationException("Future security is not specified.");

	if (StockSecurity == null)
		throw new InvalidOperationException("Stock security is not specified.");

	if (FuturePortfolio == null)
		throw new InvalidOperationException("Future portfolio is not specified.");

	if (StockPortfolio == null)
		throw new InvalidOperationException("Stock portfolio is not specified.");

	_futId = FutureSecurity.ToSecurityId();
	_stockId = StockSecurity.ToSecurityId();

	// Subscription to order book updates for both instruments
	var futureDepthSubscription = new Subscription(DataType.MarketDepth, FutureSecurity);
	var stockDepthSubscription = new Subscription(DataType.MarketDepth, StockSecurity);

	futureDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
	stockDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);

	// Subscription to own trades to track execution prices
	this
		.WhenOwnTradeReceived()
		.Do(OnOwnTradeReceived)
		.Apply(this);

	// Sending requests for market data subscription
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## 处理市场数据

当订单簿更新时，会调用 `ProcessMarketDepth` 方法，并实现主要逻辑：

```cs
private void ProcessMarketDepth(IOrderBookMessage depth)
{
	// Update the last order book for each instrument
	if (depth.SecurityId == _futId)
		_lastFut = depth;
	else if (depth.SecurityId == _stockId)
		_lastSt = depth;

	// Wait for data for both instruments
	if (_lastFut is null || _lastSt is null)
		return;

	// Calculate volume-weighted average prices for specific volumes
	_futBid = GetAveragePrice(_lastFut, Sides.Sell, FutureVolume);
	_futAck = GetAveragePrice(_lastFut, Sides.Buy, FutureVolume);
	_stBid = GetAveragePrice(_lastSt, Sides.Sell, StockVolume) * StockMultiplicator;
	_stAsk = GetAveragePrice(_lastSt, Sides.Buy, StockVolume) * StockMultiplicator;

	// Validate prices
	if (_futBid == 0 || _futAck == 0 || _stBid == 0 || _stAsk == 0)
		return;

	// Calculate spreads
	var contangoSpread = _futBid - _stAsk;        // Futures price > underlying asset price
	var backwardationSpread = _stBid - _futAck;   // Underlying asset price > futures price

	decimal spread;
	ArbitrageState arbitrageSignal;

	// Determine the best arbitrage opportunity
	if (backwardationSpread > contangoSpread)
	{
		arbitrageSignal = ArbitrageState.Backwardation;
		spread = backwardationSpread;
	}
	else
	{
		arbitrageSignal = ArbitrageState.Contango;
		spread = contangoSpread;
	}

	// Log current state and spreads
	LogInfo($"Current state {_currentState}, enter spread = {_enterSpread}");
	LogInfo($"{ArbitrageState.Backwardation} spread = {backwardationSpread}");
	LogInfo($"{ArbitrageState.Contango}        spread = {contangoSpread}");
	LogInfo($"Entry from spread:{SpreadToGenerateSignal}. Exit from profit:{ProfitToExit}");

	// Recalculate profit based on current market conditions
	if (_currentState != ArbitrageState.None && _currentState != ArbitrageState.OrderRegistration)
	{
		CalculateProfit();
		LogInfo($"Profit: {_profit}");
	}

	// Process signals based on current state and market conditions
	ProcessSignals(arbitrageSignal, spread);
}
```

## 交易逻辑

信号处理和进出决策在 `ProcessSignals` 方法中实现：

```cs
private void ProcessSignals(ArbitrageState arbitrageSignal, decimal spread)
{
	// Enter a new position when there's no open position and spread exceeds threshold
	if (_currentState == ArbitrageState.None && spread > SpreadToGenerateSignal)
	{
		_currentState = ArbitrageState.OrderRegistration;

		if (arbitrageSignal == ArbitrageState.Backwardation)
		{
			ExecuteBackwardation();
		}
		else
		{
			ExecuteContango();
		}
	}
	// Exit from Backwardation position when profit threshold is reached
	else if (_currentState == ArbitrageState.Backwardation && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseBackwardationPosition();
	}
	// Exit from Contango position when profit threshold is reached
	else if (_currentState == ArbitrageState.Contango && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseContangoPosition();
	}
}
```

## 利润计算

`CalculateProfit` 方法根据入场价格和当前价格计算当前利润：

```cs
private void CalculateProfit()
{
	switch (_currentState)
	{
		case ArbitrageState.Backwardation:
			// Buy futures, sell underlying asset - profit when futures price rises and underlying asset price falls
			_profit = (_stockExitPrice * StockMultiplicator - _stAsk) + (_futBid - _futureBuyPrice);
			break;

		case ArbitrageState.Contango:
			// Sell futures, buy underlying asset - profit when futures price falls and underlying asset price rises
			_profit = (_futureExitPrice - _futAck) + (_stBid - _stockBuyPrice * StockMultiplicator);
			break;

		default:
			_profit = 0;
			break;
	}
}
```

## 订单生成

为了执行套利策略，使用生成订单的方法：

```cs
private (Order buy, Order sell) GenerateOrdersBackwardation()
{
	var futureBuy = CreateOrder(Sides.Buy, FutureVolume);
	futureBuy.Portfolio = FuturePortfolio;
	futureBuy.Security = FutureSecurity;
	futureBuy.Type = OrderTypes.Market;

	var stockSell = CreateOrder(Sides.Sell, StockVolume);
	stockSell.Portfolio = StockPortfolio;
	stockSell.Security = StockSecurity;
	stockSell.Type = OrderTypes.Market;

	return (futureBuy, stockSell);
}

private (Order sell, Order buy) GenerateOrdersContango()
{
	var futureSell = CreateOrder(Sides.Sell, FutureVolume);
	futureSell.Portfolio = FuturePortfolio;
	futureSell.Security = FutureSecurity;
	futureSell.Type = OrderTypes.Market;

	var stockBuy = CreateOrder(Sides.Buy, StockVolume);
	stockBuy.Portfolio = StockPortfolio;
	stockBuy.Security = StockSecurity;
	stockBuy.Type = OrderTypes.Market;

	return (futureSell, stockBuy);
}
```

## 特征

- 该策略支持同时使用两种不同的工具和两个投资组合
- 市价单用于快速执行
- 规则 (IMarketRule) 用于跟踪订单执行
- 成交量加权平均价格是根据成交量计算以获得更准确的价格
- 套利逻辑同时考虑正向（现货溢价）和反向（期货贴水）价差
- 支持在达到目标阈值时自动计算利润并退出