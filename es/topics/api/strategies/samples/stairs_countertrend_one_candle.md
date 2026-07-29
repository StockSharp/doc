# Estrategia de contratendencia de una vela

## Descripción general

`OneCandleCountertrendStrategy` es una estrategia simple de contratendencia que toma decisiones basándose en el análisis de una sola vela.

## Componentes principales

```cs
public class OneCandleCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- `CandleType` - tipo de vela con el que trabajar (predeterminado 5 minutos)

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), se crea la suscripción a velas y se prepara la visualización:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

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

	// Estrategia de contratendencia: comprar en vela bajista, vender en vela alcista
	if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
	{
		// Vela alcista - vender
		SellMarket(Volume + Math.Abs(Position));
	}
	else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
	{
		// Vela bajista - comprar
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de negociación

- **Señal de venta**: vela alcista (precio de cierre por encima del precio de apertura) cuando no hay posición corta
- **Señal de compra**: vela bajista (precio de cierre por debajo del precio de apertura) cuando no hay posición larga
- El volumen de posición aumenta por el importe de la posición actual con cada nueva operación

## Características

- La estrategia determina automáticamente los instrumentos con los que trabajar mediante el método `GetWorkingSecurities()`
- La estrategia solo trabaja con velas completadas
- La estrategia usa órdenes de mercado para entrada en posición
- La estrategia aplica una lógica simple de detección de contratendencia basada en una sola vela
- Las velas y operaciones se visualizan en el gráfico cuando hay un área gráfica disponible
