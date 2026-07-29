# Estrategia de múltiples marcos temporales

## Descripción general

`MultiTimeframeStrategy` es una estrategia que usa dos marcos temporales para tomar decisiones de negociación. Las velas horarias determinan la dirección de la tendencia mediante cruces de medias móviles, mientras que las velas de 5 minutos con el indicador [RelativeStrengthIndex](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) se usan para una entrada precisa en la dirección de la tendencia.

## Componentes principales

La estrategia hereda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) y usa parámetros para configuración:

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// Dirección de tendencia en el marco temporal superior
	private Sides? _hourlyTrend;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **FastSmaLength** - periodo de la media móvil rápida para el gráfico horario (predeterminado 10)
- **SlowSmaLength** - periodo de la media móvil lenta para el gráfico horario (predeterminado 30)
- **RsiLength** - periodo RSI para el gráfico de 5 minutos (predeterminado 14)
- `TakeProfit` - tamaño de take-profit en porcentaje (predeterminado 2)
- `StopLoss` - tamaño de stop-loss en porcentaje (predeterminado 1)

Todos los parámetros están disponibles para optimización con rangos de valores especificados.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se crean indicadores y se configuran suscripciones a velas para dos marcos temporales:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// Velas horarias para detectar tendencia (cruce SMA)
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

	// Velas de 5 minutos para entrada precisa (RSI)
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// Configurar protección de posición (take-profit y stop-loss)
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// Configurar visualización en el gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Procesamiento de velas horarias

El método `ProcessHourlyCandle` determina la dirección de la tendencia en el marco temporal superior:

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Determinar tendencia por cruce de medias móviles
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## Procesamiento de velas de 5 minutos

El método `ProcessEntryCandle` implementa la entrada en posición según la señal RSI en la dirección de la tendencia:

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// Comprar: tendencia alcista y RSI en zona de sobreventa
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Vender: tendencia bajista y RSI en zona de sobrecompra
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de negociación

- **Detección de tendencia**: SMA rápida por encima de SMA lenta en el gráfico horario indica tendencia alcista; por debajo indica tendencia bajista
- **Señal de compra**: tendencia alcista en el gráfico horario y RSI < 30 en el gráfico de 5 minutos cuando no hay posición larga
- **Señal de venta**: tendencia bajista en el gráfico horario y RSI > 70 en el gráfico de 5 minutos cuando no hay posición corta
- **Protección de posición**: take-profit y stop-loss automáticos mediante `StartProtection`

## Características

- La estrategia usa dos marcos temporales: horario para tendencia y 5 minutos para entrada
- La entrada en posición solo se realiza en la dirección de la tendencia del marco temporal superior
- RSI se usa como filtro para encontrar puntos de entrada óptimos (sobreventa/sobrecompra)
- Las posiciones se protegen automáticamente con stop-loss y take-profit
- La estrategia solo trabaja con velas completadas
- Los indicadores y operaciones se visualizan en el gráfico cuando hay un área gráfica disponible
- Se admite optimización de parámetros para encontrar ajustes óptimos de estrategia
