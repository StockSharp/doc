# Estrategia de contratendencia con quoting

## Descripción general

`StairsCountertrendStrategy` es una estrategia de trading de contratendencia que abre posiciones contra una tendencia establecida de una longitud específica, usando un mecanismo de quoting para una entrada más precisa en el mercado.

## Componentes principales

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleDataType;
	private readonly StrategyParam<int> _length;
	private QuotingProcessor _quotingProcessor;

	private int _bullLength;
	private int _bearLength;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **CandleDataType** - tipo de vela con el que trabajar (predeterminado 1 minuto)
- **Length** - número de velas consecutivas en una dirección para identificar una tendencia (predeterminado 5)

El parámetro Length está disponible para optimización en el rango de 2 a 10 con un paso de 1.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se restablecen los contadores, se crea la suscripción a velas y se prepara la visualización:

```cs
protected override void OnStarted2(DateTime time)
{
	// Restablecer contadores al inicio
	_bullLength = 0;
	_bearLength = 0;

	// Crear suscripción a velas
	var subscription = SubscribeCandles(CandleDataType);

	subscription
		.Bind(ProcessCandle)
		.Start();

	// Configurar visualización en el gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}

	base.OnStarted2(time);
}
```

## Procesamiento de velas

El método `ProcessCandle` se llama para cada vela completada e implementa la lógica de detección de tendencia y gestión del procesador de quoting:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Identificar vela alcista o bajista
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"Bullish candle detected. Streak: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"Bearish candle detected. Streak: {_bearLength}");
	}

	// Detener el procesador existente cuando se requiere cambio de dirección
	if (_quotingProcessor != null)
	{
		// Comprobar si el procesador debe limpiarse (cambio de tendencia o posición)
		var shouldClearProcessor = false;

		// Hay que vender si hay tendencia alcista y no hay posición corta
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// Hay que comprar si hay tendencia bajista y no hay posición larga
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// Crear nuevo procesador de quoting cuando sea necesario
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// Tendencia alcista - abrir posición corta
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"Starting SELL quoting after {_bullLength} bullish candles");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// Tendencia bajista - abrir posición larga
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"Starting BUY quoting after {_bearLength} bearish candles");
		}
	}
}
```

## Crear procesador de quoting

El método `CreateQuotingProcessor` crea un procesador de quoting con la dirección especificada:

```cs
private void CreateQuotingProcessor(Sides side)
{
	// Crear comportamiento para quoting de mercado
	var behavior = new MarketQuotingBehavior(
		0, // Sin desplazamiento de precio
		new Unit(0.1m, UnitTypes.Percent), // Usar 0.1% como desviación mínima
		MarketPriceTypes.Following // Seguir el precio de mercado
	);

	// Crear procesador de quoting
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // Volumen de quoting
		Volume, // Volumen máximo de orden
		TimeSpan.Zero, // Sin timeout
		this, // Strategy implementa ISubscriptionProvider
		this, // Strategy implementa IMarketRuleContainer
		this, // Strategy implementa ITransactionProvider
		this, // Strategy implementa ITimeProvider
		this, // Strategy implementa IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Comprobar permiso de trading
		true, // Usar precios del libro de órdenes
		true // Usar precio de la última operación si el libro de órdenes está vacío
	)
	{
		Parent = this
	};

	// Suscribirse a eventos del procesador
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Order failed: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Inicializar procesador
	_quotingProcessor.Start();
}
```

## Lógica de trading

- **Señal de venta**: `Length` velas alcistas consecutivas (precio de cierre por encima del precio de apertura) cuando no hay posición corta
- **Señal de compra**: `Length` velas bajistas consecutivas (precio de cierre por debajo del precio de apertura) cuando no hay posición larga
- Se usa un procesador de quoting para la entrada al mercado, siguiendo el precio de mercado

## Características

- La estrategia determina automáticamente los instrumentos con los que trabajar mediante el método `GetWorkingSecurities()`
- La estrategia solo trabaja con velas completadas
- Se usa quoting en lugar de órdenes de mercado para una entrada al mercado más eficiente
- La estrategia aplica un enfoque de contratendencia, abriendo posiciones contra la tendencia establecida
- Se implementa logging detallado de los eventos principales para depuración
- El procesador de quoting se limpia automáticamente cuando cambia la dirección de tendencia o se alcanzan objetivos
- Se admite visualización de velas y operaciones en el gráfico
- Se implementa optimización del parámetro de longitud de secuencia para la configuración de la estrategia
