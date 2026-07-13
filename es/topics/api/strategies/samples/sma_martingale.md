# Medias móviles con Martingale

## Descripción general

`SmaStrategyMartingaleStrategy` es una estrategia de negociación basada en el cruce de dos medias móviles simples ([SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)) con elementos de martingale. La estrategia usa SMAs larga y corta para determinar señales de entrada y salida, aumentando el tamaño de posición con cada nueva operación.

## Componentes principales

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// Variables para almacenar valores anteriores de indicadores
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **LongSmaLength** - periodo de la media móvil larga (predeterminado 80)
- **ShortSmaLength** - periodo de la media móvil corta (predeterminado 30)
- **CandleType** - tipo de vela con el que trabajar (predeterminado 5 minutos)

Todos los parámetros están disponibles para optimización con rangos de valores especificados.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se crean indicadores SMA, se configura la suscripción a velas y se prepara la visualización:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Crear indicadores
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// Agregar indicadores a la colección de la estrategia para seguimiento automático de IsFormed
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// Crear suscripción y vincular indicadores
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// Configurar visualización en el gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Procesamiento de velas

El método `ProcessCandle` se llama para cada vela completada e implementa la lógica de negociación:

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Omitir velas incompletas
	if (candle.State != CandleStates.Finished)
		return;

	// Comprobar si la estrategia está lista para operar
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Para el primer valor, solo guardar datos sin generar señales
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// Obtener comparación actual y anterior de valores de indicadores
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// Guardar valores actuales como anteriores para la próxima vela
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// Comprobar cruce (señal)
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// Cancelar órdenes activas antes de colocar nuevas
	CancelActiveOrders();

	// Determinar dirección de operación
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// Calcular tamaño de posición (aumentar posición con cada operación: enfoque martingale)
	var volume = Volume + Math.Abs(Position);

	// Crear y registrar una orden con el precio correspondiente
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## Lógica de negociación

- **Señal de compra**: la SMA corta cruza la SMA larga desde abajo
- **Señal de venta**: la SMA corta cruza la SMA larga desde arriba
- El tamaño de posición aumenta por el importe de la posición actual con cada nueva operación (elemento martingale)
- El precio de la orden se establece en el valor actual de la SMA corta, redondeado al tamaño de tick del instrumento

## Características

- La estrategia determina automáticamente los instrumentos con los que trabajar mediante el método `GetWorkingSecurities()`
- La estrategia solo trabaja con velas completadas
- La estrategia sigue cruces de indicadores comparando la relación actual y anterior entre SMAs
- Todas las órdenes activas se cancelan antes de colocar nuevas
- Se implementa el principio martingale: aumento del tamaño de posición con cada nueva operación
- Los indicadores y operaciones se visualizan en el gráfico cuando hay un área gráfica disponible
- Se admite optimización de parámetros para encontrar ajustes óptimos de estrategia
