# Estrategia de arbitraje

## Descripción general

`ArbitrageStrategy` es una estrategia de arbitraje entre un contrato de futuros y su activo subyacente. Sigue los spreads entre instrumentos y abre posiciones cuando aparecen oportunidades de arbitraje.

## Componentes principales

La estrategia hereda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) y usa parámetros para configuración:

```cs
public class ArbitrageStrategy : Strategy
{
	private enum ArbitrageState
	{
		Contango,        // El precio del futuro es mayor que el del activo subyacente
		Backwardation,   // El precio del activo subyacente es mayor que el del futuro
		None,            // Sin posición
		OrderRegistration // En proceso de registro de órdenes
	}

	// Parámetros de estrategia
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

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **FutureSecurity** - instrumento de futuros
- **StockSecurity** - instrumento del activo subyacente
- **FuturePortfolio** - cartera para la negociación de futuros
- **StockPortfolio** - cartera para la negociación del activo subyacente
- **StockMultiplicator** - multiplicador del activo subyacente (por ejemplo, tamaño de lote)
- **FutureVolume** - volumen para la negociación de futuros
- **StockVolume** - volumen para la negociación del activo subyacente
- **ProfitToExit** - umbral de beneficio para salir de la posición
- **SpreadToGenerateSignal** - umbral de spread para generar señal de entrada

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se validan los parámetros y se crean suscripciones a libros de órdenes y operaciones propias:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	if (FutureSecurity == null)
		throw new InvalidOperationException("No se especificó el instrumento de futuros.");

	if (StockSecurity == null)
		throw new InvalidOperationException("No se especificó el instrumento de acciones.");

	if (FuturePortfolio == null)
		throw new InvalidOperationException("No se especificó la cartera de futuros.");

	if (StockPortfolio == null)
		throw new InvalidOperationException("No se especificó la cartera de acciones.");

	_futId = FutureSecurity.ToSecurityId();
	_stockId = StockSecurity.ToSecurityId();

	// Suscripción a actualizaciones del libro de órdenes para ambos instrumentos
	var futureDepthSubscription = new Subscription(DataType.MarketDepth, FutureSecurity);
	var stockDepthSubscription = new Subscription(DataType.MarketDepth, StockSecurity);

	futureDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
	stockDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);

	// Suscripción a operaciones propias para seguir precios de ejecución
	this
		.WhenOwnTradeReceived()
		.Do(OnOwnTradeReceived)
		.Apply(this);

	// Envío de solicitudes de suscripción a datos de mercado
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## Procesamiento de datos de mercado

El método `ProcessMarketDepth` se llama cuando se actualiza un libro de órdenes e implementa la lógica principal:

```cs
private void ProcessMarketDepth(IOrderBookMessage depth)
{
	// Actualizar el último libro de órdenes de cada instrumento
	if (depth.SecurityId == _futId)
		_lastFut = depth;
	else if (depth.SecurityId == _stockId)
		_lastSt = depth;

	// Esperar datos de ambos instrumentos
	if (_lastFut is null || _lastSt is null)
		return;

	// Calcular precios medios ponderados por volumen para volúmenes específicos
	_futBid = GetAveragePrice(_lastFut, Sides.Sell, FutureVolume);
	_futAck = GetAveragePrice(_lastFut, Sides.Buy, FutureVolume);
	_stBid = GetAveragePrice(_lastSt, Sides.Sell, StockVolume) * StockMultiplicator;
	_stAsk = GetAveragePrice(_lastSt, Sides.Buy, StockVolume) * StockMultiplicator;

	// Validar precios
	if (_futBid == 0 || _futAck == 0 || _stBid == 0 || _stAsk == 0)
		return;

	// Calcular spreads
	var contangoSpread = _futBid - _stAsk;        // Precio del futuro > precio del activo subyacente
	var backwardationSpread = _stBid - _futAck;   // Precio del activo subyacente > precio del futuro

	decimal spread;
	ArbitrageState arbitrageSignal;

	// Determinar la mejor oportunidad de arbitraje
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

	// Registrar estado actual y spreads
	LogInfo($"Estado actual {_currentState}, spread de entrada = {_enterSpread}");
	LogInfo($"{ArbitrageState.Backwardation} spread = {backwardationSpread}");
	LogInfo($"{ArbitrageState.Contango}        spread = {contangoSpread}");
	LogInfo($"Entrada por spread:{SpreadToGenerateSignal}. Salida por beneficio:{ProfitToExit}");

	// Recalcular beneficio según las condiciones actuales de mercado
	if (_currentState != ArbitrageState.None && _currentState != ArbitrageState.OrderRegistration)
	{
		CalculateProfit();
		LogInfo($"Beneficio: {_profit}");
	}

	// Procesar señales según el estado actual y las condiciones de mercado
	ProcessSignals(arbitrageSignal, spread);
}
```

## Lógica de negociación

El procesamiento de señales y la toma de decisiones de entrada/salida se implementan en el método `ProcessSignals`:

```cs
private void ProcessSignals(ArbitrageState arbitrageSignal, decimal spread)
{
	// Entrar en una nueva posición cuando no hay posición abierta y el spread supera el umbral
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
	// Salir de la posición Backwardation cuando se alcanza el umbral de beneficio
	else if (_currentState == ArbitrageState.Backwardation && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseBackwardationPosition();
	}
	// Salir de la posición Contango cuando se alcanza el umbral de beneficio
	else if (_currentState == ArbitrageState.Contango && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseContangoPosition();
	}
}
```

## Cálculo de beneficio

El método `CalculateProfit` calcula el beneficio actual según los precios de entrada y los precios actuales:

```cs
private void CalculateProfit()
{
	switch (_currentState)
	{
		case ArbitrageState.Backwardation:
			// Comprar futuros, vender activo subyacente: beneficio cuando sube el precio del futuro y baja el del activo subyacente
			_profit = (_stockExitPrice * StockMultiplicator - _stAsk) + (_futBid - _futureBuyPrice);
			break;

		case ArbitrageState.Contango:
			// Vender futuros, comprar activo subyacente: beneficio cuando baja el precio del futuro y sube el del activo subyacente
			_profit = (_futureExitPrice - _futAck) + (_stBid - _stockBuyPrice * StockMultiplicator);
			break;

		default:
			_profit = 0;
			break;
	}
}
```

## Generación de órdenes

Para ejecutar estrategias de arbitraje, se usan métodos de generación de órdenes:

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

## Características

- La estrategia admite trabajar con dos instrumentos distintos y dos carteras
- Se usan órdenes de mercado para una ejecución rápida
- Se usan reglas (IMarketRule) para seguir la ejecución de órdenes
- El precio medio ponderado por volumen se calcula según el volumen para obtener precios más precisos
- La lógica de arbitraje considera spreads directos (contango) e inversos (backwardation)
- Admite cálculo automático de beneficio y salida cuando se alcanza el umbral objetivo
