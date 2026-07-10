# Estrategia de pairs trading

## Descripción general

`PairsTradingStrategy` es una estrategia de pairs trading basada en arbitraje estadístico entre dos instrumentos relacionados. Sigue el spread entre los precios de dos activos y abre posiciones cuando el spread se desvía significativamente de la media, esperando una reversión a la media.

## Componentes principales

La estrategia hereda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) y usa parámetros para configuración:

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// Últimos precios de cada instrumento
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **SpreadLength** - periodo para calcular la media y desviación estándar del spread (predeterminado 20)
- **EntryThreshold** - umbral Z-Score para entrar en posición (predeterminado 2.0)
- **ExitThreshold** - umbral Z-Score para salir de posición (predeterminado 0.5)
- **CandleType** - tipo de vela con el que trabajar (predeterminado 5 minutos)

Todos los parámetros están disponibles para optimización con rangos de valores especificados.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se crean indicadores y se configuran suscripciones a velas para dos instrumentos:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Obtener dos instrumentos para pairs trading
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("Deben especificarse dos instrumentos.");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// Indicadores para calcular la media y desviación estándar del spread
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// Suscribirse a velas del primer instrumento
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// Suscribirse a velas del segundo instrumento con procesamiento de spread
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## Procesamiento de spread

El método `ProcessSpread` calcula el Z-Score del spread y genera señales de trading:

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// Calcular spread como diferencia de precios
	var spread = price1 - price2;

	// Procesar indicadores
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// Calcular Z-Score: desviación del spread respecto a la media en unidades de desviación estándar
	var zScore = (spread - mean) / dev;

	// Spread demasiado alto: vender el primer instrumento, comprar el segundo
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Spread demasiado bajo: comprar el primer instrumento, vender el segundo
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Reversión a la media: cerrar posición
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## Lógica de trading

- **Señal de venta**: el Z-Score del spread supera el umbral de entrada (predeterminado 2.0) cuando no hay posición corta
- **Señal de compra**: el Z-Score del spread cae por debajo del umbral de entrada negativo (predeterminado -2.0) cuando no hay posición larga
- **Cerrar posición**: el valor absoluto del Z-Score cae por debajo del umbral de salida (predeterminado 0.5), indicando que el spread revierte a la media

## Características

- La estrategia trabaja con dos instrumentos obtenidos mediante el método `GetWorkingSecurities()`
- El spread se calcula como la diferencia de precios de cierre de velas de dos instrumentos
- Z-Score se usa para normalizar la desviación del spread respecto a la media
- La estrategia implementa el concepto clásico de reversión a la media
- La estrategia solo trabaja con velas completadas
- Se admite optimización de parámetros para encontrar ajustes óptimos de estrategia
