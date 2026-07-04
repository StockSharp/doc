# Arbitrage-Strategie

## Überblick

`ArbitrageStrategy` ist eine Arbitrage-Strategie zwischen einem Futures-Kontrakt und seinem Basiswert. Sie verfolgt Spreads zwischen Instrumenten und eröffnet Positionen, wenn Arbitragegelegenheiten entstehen.

## Hauptkomponenten

Die Strategie erbt von [Strategy](xref:StockSharp.Algo.Strategies.Strategy) und verwendet Parameter zur Konfiguration:

```cs
public class ArbitrageStrategy : Strategy
{
	private enum ArbitrageState
	{
		Contango,        // Futures-Preis ist höher als der Basiswert
		Backwardation,   // Preis des Basiswerts ist höher als der Futures-Preis
		None,            // Keine Position
		OrderRegistration // Im Prozess der Orderregistrierung
	}

	// Strategieparameter
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

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **FutureSecurity** - Futures-Instrument
- **StockSecurity** - Instrument des Basiswerts
- **FuturePortfolio** - Portfolio für den Futures-Handel
- **StockPortfolio** - Portfolio für den Handel mit dem Basiswert
- **StockMultiplicator** - Multiplikator für den Basiswert (z. B. Lotgröße)
- **FutureVolume** - Volumen für den Futures-Handel
- **StockVolume** - Volumen für den Handel mit dem Basiswert
- **ProfitToExit** - Gewinnschwelle für den Positionsausstieg
- **SpreadToGenerateSignal** - Spread-Schwellenwert zur Erzeugung eines Einstiegssignals

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) werden Parameter validiert sowie Abonnements für Orderbücher und eigene Trades erstellt:

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

	// Orderbuchaktualisierungen für beide Instrumente abonnieren
	var futureDepthSubscription = new Subscription(DataType.MarketDepth, FutureSecurity);
	var stockDepthSubscription = new Subscription(DataType.MarketDepth, StockSecurity);

	futureDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
	stockDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);

	// Eigene Trades abonnieren, um Ausführungspreise zu verfolgen
	this
		.WhenOwnTradeReceived()
		.Do(OnOwnTradeReceived)
		.Apply(this);

	// Anforderungen für Marktdatenabonnements senden
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## Verarbeitung von Marktdaten

Die Methode `ProcessMarketDepth` wird bei einer Orderbuchaktualisierung aufgerufen und implementiert die Hauptlogik:

```cs
private void ProcessMarketDepth(IOrderBookMessage depth)
{
	// Letztes Orderbuch je Instrument aktualisieren
	if (depth.SecurityId == _futId)
		_lastFut = depth;
	else if (depth.SecurityId == _stockId)
		_lastSt = depth;

	// Auf Daten für beide Instrumente warten
	if (_lastFut is null || _lastSt is null)
		return;

	// Volumengewichtete Durchschnittspreise für bestimmte Volumina berechnen
	_futBid = GetAveragePrice(_lastFut, Sides.Sell, FutureVolume);
	_futAck = GetAveragePrice(_lastFut, Sides.Buy, FutureVolume);
	_stBid = GetAveragePrice(_lastSt, Sides.Sell, StockVolume) * StockMultiplicator;
	_stAsk = GetAveragePrice(_lastSt, Sides.Buy, StockVolume) * StockMultiplicator;

	// Preise validieren
	if (_futBid == 0 || _futAck == 0 || _stBid == 0 || _stAsk == 0)
		return;

	// Spreads berechnen
	var contangoSpread = _futBid - _stAsk;        // Futures-Preis > Preis des Basiswerts
	var backwardationSpread = _stBid - _futAck;   // Preis des Basiswerts > Futures-Preis

	decimal spread;
	ArbitrageState arbitrageSignal;

	// Beste Arbitragegelegenheit bestimmen
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

	// Aktuellen Zustand und Spreads protokollieren
	LogInfo($"Current state {_currentState}, enter spread = {_enterSpread}");
	LogInfo($"{ArbitrageState.Backwardation} spread = {backwardationSpread}");
	LogInfo($"{ArbitrageState.Contango}        spread = {contangoSpread}");
	LogInfo($"Entry from spread:{SpreadToGenerateSignal}. Exit from profit:{ProfitToExit}");

	// Gewinn anhand der aktuellen Marktbedingungen neu berechnen
	if (_currentState != ArbitrageState.None && _currentState != ArbitrageState.OrderRegistration)
	{
		CalculateProfit();
		LogInfo($"Profit: {_profit}");
	}

	// Signale anhand des aktuellen Zustands und der Marktbedingungen verarbeiten
	ProcessSignals(arbitrageSignal, spread);
}
```

## Handelslogik

Signalverarbeitung sowie Ein- und Ausstiegsentscheidungen sind in der Methode `ProcessSignals` implementiert:

```cs
private void ProcessSignals(ArbitrageState arbitrageSignal, decimal spread)
{
	// Neue Position eröffnen, wenn keine Position offen ist und der Spread den Schwellenwert überschreitet
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
	// Aus Backwardation-Position aussteigen, wenn die Gewinnschwelle erreicht ist
	else if (_currentState == ArbitrageState.Backwardation && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseBackwardationPosition();
	}
	// Aus Contango-Position aussteigen, wenn die Gewinnschwelle erreicht ist
	else if (_currentState == ArbitrageState.Contango && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseContangoPosition();
	}
}
```

## Gewinnberechnung

Die Methode `CalculateProfit` berechnet den aktuellen Gewinn anhand der Einstiegspreise und der aktuellen Preise:

```cs
private void CalculateProfit()
{
	switch (_currentState)
	{
		case ArbitrageState.Backwardation:
			// Futures kaufen, Basiswert verkaufen - Gewinn, wenn Futures-Preis steigt und Basiswertpreis fällt
			_profit = (_stockExitPrice * StockMultiplicator - _stAsk) + (_futBid - _futureBuyPrice);
			break;

		case ArbitrageState.Contango:
			// Futures verkaufen, Basiswert kaufen - Gewinn, wenn Futures-Preis fällt und Basiswertpreis steigt
			_profit = (_futureExitPrice - _futAck) + (_stBid - _stockBuyPrice * StockMultiplicator);
			break;

		default:
			_profit = 0;
			break;
	}
}
```

## Ordererzeugung

Zur Ausführung von Arbitrage-Strategien werden Methoden zur Ordererzeugung verwendet:

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

## Funktionen

- Die Strategie unterstützt die Arbeit mit zwei verschiedenen Instrumenten und zwei Portfolios.
- Für schnelle Ausführung werden Market-Orders verwendet.
- Regeln (IMarketRule) werden genutzt, um die Orderausführung zu verfolgen.
- Für genauere Preise wird ein volumengewichteter Durchschnittspreis anhand des Volumens berechnet.
- Die Arbitrage-Logik berücksichtigt sowohl direkte (Contango) als auch inverse (Backwardation) Spreads.
- Automatische Gewinnberechnung und Ausstieg beim Erreichen des Zielschwellenwerts werden unterstützt.
