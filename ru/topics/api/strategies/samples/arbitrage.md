# Стратегия арбитража

## Обзор

`ArbitrageStrategy` - это стратегия арбитража между фьючерсом и базовым активом. Она отслеживает спреды между инструментами и открывает позиции при возникновении арбитражных возможностей.

## Основные компоненты

Стратегия наследуется от [Strategy](xref:StockSharp.Algo.Strategies.Strategy) и использует параметры для настройки:

```cs
public class ArbitrageStrategy : Strategy
{
	private enum ArbitrageState
	{
		Contango,        // Фьючерс дороже базового актива
		Backvordation,   // Базовый актив дороже фьючерса
		None,            // Нет позиции
		OrderRegistration // В процессе регистрации заявок
	}

	// Параметры стратегии
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

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **FutureSecurity** - инструмент фьючерса
- **StockSecurity** - инструмент базового актива
- **FuturePortfolio** - портфель для торговли фьючерсом
- **StockPortfolio** - портфель для торговли базовым активом
- **StockMultiplicator** - мультипликатор для базового актива (например, размер лота)
- **FutureVolume** - объем для торговли фьючерсом
- **StockVolume** - объем для торговли базовым активом
- **ProfitToExit** - порог прибыли для выхода из позиции
- **SpreadToGenerateSignal** - порог спреда для генерации сигнала входа

## Инициализация стратегии

В методе [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) проверяются параметры, создаются подписки на стаканы и собственные сделки:

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

	// Подписка на обновления стаканов для обоих инструментов
	var futureDepthSubscription = new Subscription(DataType.MarketDepth, FutureSecurity);
	var stockDepthSubscription = new Subscription(DataType.MarketDepth, StockSecurity);

	futureDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
	stockDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);

	// Подписка на собственные сделки для отслеживания цен исполнения
	this
		.WhenOwnTradeReceived()
		.Do(OnNewMyTrade)
		.Apply(this);

	// Отправка запросов на подписку на рыночные данные
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## Обработка рыночных данных

Метод `ProcessMarketDepth` вызывается при обновлении стакана и реализует основную логику:

```cs
private void ProcessMarketDepth(IOrderBookMessage depth)
{
	// Обновление последнего стакана для каждого инструмента
	if (depth.SecurityId == _futId)
		_lastFut = depth;
	else if (depth.SecurityId == _stockId)
		_lastSt = depth;

	// Ожидание данных для обоих инструментов
	if (_lastFut is null || _lastSt is null)
		return;

	// Расчёт средневзвешенных цен для определённых объёмов
	_futBid = GetAveragePrice(_lastFut, Sides.Sell, FutureVolume);
	_futAck = GetAveragePrice(_lastFut, Sides.Buy, FutureVolume);
	_stBid = GetAveragePrice(_lastSt, Sides.Sell, StockVolume) * StockMultiplicator;
	_stAsk = GetAveragePrice(_lastSt, Sides.Buy, StockVolume) * StockMultiplicator;

	// Проверка валидности цен
	if (_futBid == 0 || _futAck == 0 || _stBid == 0 || _stAsk == 0)
		return;

	// Расчёт спредов
	var contangoSpread = _futBid - _stAsk;        // Цена фьючерса > цены базового актива
	var backvordationSpread = _stBid - _futAck;   // Цена базового актива > цены фьючерса

	decimal spread;
	ArbitrageState arbitrageSignal;

	// Определение лучшей арбитражной возможности
	if (backvordationSpread > contangoSpread)
	{
		arbitrageSignal = ArbitrageState.Backvordation;
		spread = backvordationSpread;
	}
	else
	{
		arbitrageSignal = ArbitrageState.Contango;
		spread = contangoSpread;
	}

	// Логирование текущего состояния и спредов
	LogInfo($"Current state {_currentState}, enter spread = {_enterSpread}");
	LogInfo($"{ArbitrageState.Backvordation} spread = {backvordationSpread}");
	LogInfo($"{ArbitrageState.Contango}        spread = {contangoSpread}");
	LogInfo($"Entry from spread:{SpreadToGenerateSignal}. Exit from profit:{ProfitToExit}");

	// Пересчёт прибыли на основе текущих рыночных условий
	if (_currentState != ArbitrageState.None && _currentState != ArbitrageState.OrderRegistration)
	{
		CalculateProfit();
		LogInfo($"Profit: {_profit}");
	}

	// Обработка сигналов на основе текущего состояния и рыночных условий
	ProcessSignals(arbitrageSignal, spread);
}
```

## Логика торговли

Обработка сигналов и принятие решений о входе/выходе реализованы в методе `ProcessSignals`:

```cs
private void ProcessSignals(ArbitrageState arbitrageSignal, decimal spread)
{
	// Вход в новую позицию, когда нет открытой позиции и спред превышает порог
	if (_currentState == ArbitrageState.None && spread > SpreadToGenerateSignal)
	{
		_currentState = ArbitrageState.OrderRegistration;

		if (arbitrageSignal == ArbitrageState.Backvordation)
		{
			ExecuteBackvardation();
		}
		else
		{
			ExecuteContango();
		}
	}
	// Выход из позиции Backvordation, когда достигнут порог прибыли
	else if (_currentState == ArbitrageState.Backvordation && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseBackvardationPosition();
	}
	// Выход из позиции Contango, когда достигнут порог прибыли
	else if (_currentState == ArbitrageState.Contango && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseContangoPosition();
	}
}
```

## Расчёт прибыли

Метод `CalculateProfit` рассчитывает текущую прибыль на основе цен входа и текущих цен:

```cs
private void CalculateProfit()
{
	switch (_currentState)
	{
		case ArbitrageState.Backvordation:
			// Купить фьючерс, продать базовый актив - прибыль, когда цена фьючерса растёт и цена базового актива падает
			_profit = (_stockExitPrice * StockMultiplicator - _stAsk) + (_futBid - _futureBuyPrice);
			break;

		case ArbitrageState.Contango:
			// Продать фьючерс, купить базовый актив - прибыль, когда цена фьючерса падает и цена базового актива растёт
			_profit = (_futureExitPrice - _futAck) + (_stBid - _stockBuyPrice * StockMultiplicator);
			break;

		default:
			_profit = 0;
			break;
	}
}
```

## Генерация заявок

Для выполнения арбитражных стратегий используются методы для генерации заявок:

```cs
private (Order buy, Order sell) GenerateOrdersBackvardation()
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

## Особенности

- Стратегия поддерживает работу с двумя разными инструментами и двумя портфелями
- Используются рыночные заявки для быстрого исполнения
- Для отслеживания исполнения заявок используются правила (IMarketRule)
- Рассчитывается средневзвешенная цена на основе объёма для получения более точных цен
- Логика арбитража учитывает как прямой (контанго), так и обратный (бэквордация) спреды
- Поддерживается автоматический расчет прибыли и выход при достижении целевого порога