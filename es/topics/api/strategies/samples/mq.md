# Estrategia de quoting

## Descripción general

`MqStrategy` es una estrategia que usa un mecanismo de quoting para gestionar posiciones de mercado. Crea un procesador de quoting basado en la posición actual, lo que le permite responder de forma adaptativa a cambios en las condiciones de mercado.

## Componentes principales

```cs
public class MqStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _quotingProcessor;
}
```

## Parámetros de estrategia

La estrategia permite personalizar los siguientes parámetros:

- **PriceType** - tipo de precio de mercado para quoting (predeterminado Following)
- **PriceOffset** - desplazamiento del precio respecto al precio de mercado
- **BestPriceOffset** - desviación mínima para actualizar la quote (predeterminado 0.1%)

## Inicialización de la estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), la estrategia se suscribe a cambios de tiempo de mercado:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Suscribirse a cambios de tiempo de mercado para actualizar quotes
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(default);
}
```

## Gestión del procesador de quoting

El método `Connector_CurrentTimeChanged` se llama cuando cambia el tiempo de mercado y gestiona la creación y actualización del procesador de quoting:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Crear un nuevo procesador solo si el actual está detenido o no existe
	if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
		return;

	// Liberar recursos del procesador anterior si existe
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	// Determinar el lado de quoting según la posición actual
	var side = Position <= 0 ? Sides.Buy : Sides.Sell;

	// Crear nuevo comportamiento de quoting
	var behavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Calcular volumen de quoting
	var quotingVolume = Volume + Math.Abs(Position);

	// Crear e inicializar el procesador
	_quotingProcessor = new QuotingProcessor(
		behavior,
		Security,
		Portfolio,
		side,
		quotingVolume,
		Volume, // Volumen máximo de orden
		TimeSpan.Zero, // Sin timeout
		this, // Strategy implementa ISubscriptionProvider
		this, // Strategy implementa IMarketRuleContainer
		this, // Strategy implementa ITransactionProvider
		this, // Strategy implementa ITimeProvider
		this, // Strategy implementa IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Comprobar permiso de trading
		true, // Usar precios del libro de órdenes
		true  // Usar precio de la última operación si el libro de órdenes está vacío
	)
	{
		Parent = this
	};

	// Suscribirse a eventos del procesador para logging
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Orden {order.TransactionId} registrada al precio {order.Price}");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Error de orden: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Operación ejecutada: {trade.Trade.Volume} a {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk => {
		this.AddInfoLog($"Quoting finalizado correctamente: {isOk}");
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Iniciar el procesador
	_quotingProcessor.Start();
}
```

## Liberación de recursos

En el método [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped), la estrategia libera recursos:

```cs
protected override void OnStopped()
{
	// Desuscribirse para evitar fugas de memoria
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Liberar recursos del procesador actual si existe
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## Lógica de trading

- La estrategia responde a cambios del tiempo de mercado
- La dirección de quoting se determina según la posición actual:
  - Si position <= 0, se crea una cotización de compra
  - Si position > 0, se crea una cotización de venta
- El volumen de quoting se calcula como el volumen base más el valor absoluto de la posición actual
- Se usa [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) con [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) para quoting

## Características

- Usa el procesador de quoting moderno en lugar de estrategias de quoting heredadas
- Responde de forma adaptativa a cambios de posición cambiando la dirección de quoting
- Admite configuración de varios parámetros de quoting (tipo de precio, desplazamiento, desviación mínima)
- Incluye logging detallado de eventos del procesador de quoting
- Gestiona correctamente los recursos al detener la estrategia y crear nuevos procesadores
- Admite trabajar con distintos tipos de precios de mercado (Following, Best, Opposite, etc.)
