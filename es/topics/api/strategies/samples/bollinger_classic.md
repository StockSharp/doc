# Estrategia Bollinger clásica

## Descripción general

`BollingerStrategyClassicStrategy` es una estrategia basada en el indicador [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Abre posiciones cuando el precio alcanza el límite superior o inferior de las bandas de Bollinger.

## Componentes principales

La estrategia hereda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) y usa parámetros para configuración:

```cs
public class BollingerStrategyClassicStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **BollingerLength** - periodo del indicador bandas de Bollinger (predeterminado 20)
- **BollingerDeviation** - multiplicador de desviación estándar (predeterminado 2.0)
- **CandleType** - tipo de vela con el que trabajar (predeterminado 5 minutos)

Todos los parámetros están disponibles para optimización con rangos de valores especificados.

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se crea el indicador bandas de Bollinger, se configura la suscripción a velas y se prepara la visualización:

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

El método `ProcessCandle` se llama para cada vela completada e implementa la lógica de trading:

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

	// Lógica de trading:
	// Vender cuando el precio alcanza o supera la banda superior
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Comprar cuando el precio alcanza o cae por debajo de la banda inferior
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de trading

- **Señal de venta**: el precio de cierre de la vela alcanza o supera la banda superior de Bollinger cuando no hay posición corta
- **Señal de compra**: el precio de cierre de la vela alcanza o cae por debajo de la banda inferior de Bollinger cuando no hay posición larga
- El volumen de posición aumenta por el importe de la posición actual con cada nueva operación

## Características

- La estrategia determina automáticamente los instrumentos con los que trabajar mediante el método `GetWorkingSecurities()`
- La estrategia solo trabaja con velas completadas
- El indicador y las operaciones se visualizan en un gráfico cuando hay un área gráfica disponible
- Se admite optimización de parámetros para encontrar ajustes óptimos de estrategia
