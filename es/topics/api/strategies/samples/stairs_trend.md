# Estrategia Stairs de tendencia

## Descripción general

`StairsTrendStrategy` es una estrategia de negociación basada en el análisis de velas consecutivas para determinar una tendencia. La estrategia abre posiciones cuando se forma una tendencia sostenida de una longitud específica.

## Componentes principales

```cs
public class StairsTrendStrategy : Strategy
{
	private readonly StrategyParam<int> _lengthParam;
	private readonly StrategyParam<DataType> _candleType;

	private int _bullLength;
	private int _bearLength;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **Length** - número de velas consecutivas en una dirección para identificar una tendencia (predeterminado 3)
- **CandleType** - tipo de vela con el que trabajar (predeterminado 5 minutos)

El parámetro Length está disponible para optimización en el rango de 2 a 10 con un paso de 1.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se restablecen los contadores, se crea la suscripción a velas y se prepara la visualización:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Restablecer contadores
	_bullLength = 0;
	_bearLength = 0;

	// Crear suscripción
	var subscription = SubscribeCandles(CandleType);

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
}
```

## Procesamiento de velas

El método `ProcessCandle` se llama para cada vela completada e implementa la lógica de negociación:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Comprobar si la vela está finalizada
	if (candle.State != CandleStates.Finished)
		return;

	// Comprobar si la estrategia está lista para operar
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Actualizar contadores según la dirección de la vela
	if (candle.OpenPrice < candle.ClosePrice)
	{
		// Vela alcista
		_bullLength++;
		_bearLength = 0;
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		// Vela bajista
		_bullLength = 0;
		_bearLength++;
	}

	// Estrategia de tendencia:
	// Comprar después de Length velas alcistas consecutivas
	if (_bullLength >= Length && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Vender después de Length velas bajistas consecutivas
	else if (_bearLength >= Length && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de negociación

- **Señal de compra**: `Length` velas alcistas consecutivas (precio de cierre por encima del precio de apertura) cuando no hay posición larga
- **Señal de venta**: `Length` velas bajistas consecutivas (precio de cierre por debajo del precio de apertura) cuando no hay posición corta
- El volumen de posición aumenta por el importe de la posición actual con cada nueva operación

## Características

- La estrategia determina automáticamente los instrumentos con los que trabajar mediante el método `GetWorkingSecurities()`
- La estrategia solo trabaja con velas completadas
- La estrategia usa órdenes de mercado para entrada en posición
- La estrategia aplica una lógica simple de detección de tendencia basada en una secuencia de velas
- Los contadores de velas se restablecen cuando aparece una vela en la dirección opuesta
- Las velas y operaciones se visualizan en el gráfico cuando hay un área gráfica disponible
- Se admite optimización de longitud de secuencia para encontrar ajustes óptimos de estrategia
