# Arbitrage Strategy

## Overview

`ArbitrageStrategy` is an arbitrage strategy between a futures contract and its underlying asset. It tracks spreads between instruments and opens positions when arbitrage opportunities arise.

## Main Components

The strategy inherits from [Strategy](xref:StockSharp.Algo.Strategies.Strategy) and uses parameters for configuration:

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

## Strategy Parameters

The strategy allows customizing the following parameters:

- **FutureSecurity** - futures instrument
- **StockSecurity** - underlying asset instrument
- **FuturePortfolio** - portfolio for futures trading
- **StockPortfolio** - portfolio for underlying asset trading
- **StockMultiplicator** - multiplier for the underlying asset (e.g., lot size)
- **FutureVolume** - volume for futures trading
- **StockVolume** - volume for underlying asset trading
- **ProfitToExit** - profit threshold for position exit
- **SpreadToGenerateSignal** - spread threshold for entry signal generation

## Strategy Initialization

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method, parameters are validated, subscriptions to order books and own trades are created:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);

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
		.Do(OnNewMyTrade)
		.Apply(this);

	// Sending requests for market data subscription
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## Processing Market Data

The `ProcessMarketDepth` method is called when an order book is updated and implements the main logic:

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

## Trading Logic

Signal processing and entry/exit decision making are implemented in the `ProcessSignals` method:

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

## Profit Calculation

The `CalculateProfit` method calculates current profit based on entry prices and current prices:

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

## Order Generation

For executing arbitrage strategies, methods for generating orders are used:

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

## Features

- The strategy supports working with two different instruments and two portfolios
- Market orders are used for fast execution
- Rules (IMarketRule) are used to track order execution
- Volume-weighted average price is calculated based on volume for more accurate prices
- Arbitrage logic considers both direct (contango) and reverse (backwardation) spreads
- Supports automatic profit calculation and exit when target threshold is reached