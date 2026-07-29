# Estrategia Bollinger con foco en la banda inferior

## Descripción general

`BollingerStrategyLowBandStrategy` es una estrategia basada en el indicador [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Abre una posición corta cuando el precio alcanza el límite inferior de las bandas de Bollinger y la cierra cuando el precio alcanza la línea media.

## Componentes principales

La estrategia hereda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) y usa parámetros para configuración:

```cs
public class BollingerStrategyLowBandStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **BollingerLength** - periodo del indicador de bandas de Bollinger (predeterminado 20)
- **BollingerDeviation** - multiplicador de desviación estándar (predeterminado 2.0)
- `CandleType` - tipo de vela con el que trabajar (predeterminado 5 minutos)

Todos los parámetros están disponibles para optimización con rangos de valores especificados.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se crea el indicador de bandas de Bollinger, se configura la suscripción a velas y se prepara la visualización:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Crear indicador
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// Crear suscripción y vincular indicador
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// Configurar visualización en el gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## Procesamiento de velas

El método `ProcessCandle` se llama para cada vela completada e implementa la lógica de negociación:

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// Omitir velas incompletas
	if (candle.State != CandleStates.Finished)
		return;

	// Comprobar si la estrategia está lista para operar
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// Lógica de negociación:
	// Vender cuando el precio toca la banda inferior (solo cuando no existe posición)
	if (candle.ClosePrice <= typed.LowBand && Position == 0)
	{
		SellMarket(Volume);
	}
	// Comprar para cerrar la posición cuando el precio alcanza la línea media (solo con una posición corta)
	else if (candle.ClosePrice >= typed.MiddleBand && Position < 0)
	{
		BuyMarket(Math.Abs(Position));
	}
}
```

## Lógica de negociación

- **Señal de venta**: el precio de cierre de la vela alcanza o cae por debajo de la banda inferior de Bollinger cuando no hay posición abierta
- **Señal de compra** (cierre de posición corta): el precio de cierre de la vela alcanza o supera la línea media de Bollinger cuando hay una posición corta
- El volumen de posición es fijo al abrir y equivale a toda la posición actual al cerrar

## Características

- La estrategia determina automáticamente los instrumentos con los que trabajar mediante el método `GetWorkingSecurities()`
- La estrategia solo trabaja con velas completadas
- La estrategia usa solo la banda inferior y la línea media del indicador de bandas de Bollinger
- Solo se abren posiciones cortas
- El indicador y las operaciones se visualizan en un gráfico cuando hay un área gráfica disponible
- Se admite optimización de parámetros para encontrar ajustes óptimos de estrategia
