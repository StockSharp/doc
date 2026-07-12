# Estrategia de quoting de spread

## Descripción general

`MqSpreadStrategy` es una estrategia que crea un spread en el mercado colocando simultáneamente quotes de compra y venta. Usa dos procesadores de quoting para gestionar órdenes en ambos lados del mercado.

## Componentes principales

```cs
public class MqSpreadStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _buyProcessor;
	private QuotingProcessor _sellProcessor;
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
	Connector_CurrentTimeChanged(new TimeSpan());
}
```

## Gestión de procesadores de quoting

El método `Connector_CurrentTimeChanged` se llama cuando cambia el tiempo de mercado y gestiona la creación y actualización de procesadores de quoting:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Crear nuevos procesadores solo con posición cero y si los actuales están detenidos
	if (Position != 0)
		return;

	if (_buyProcessor != null && _buyProcessor.LeftVolume > 0)
		return;

	if (_sellProcessor != null && _sellProcessor.LeftVolume > 0)
		return;

	// Liberar recursos de procesadores existentes
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	// Crear comportamientos para quoting de mercado
	var buyBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	var sellBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Crear procesador para compra
	_buyProcessor = new QuotingProcessor(
		buyBehavior,
		Security,
		Portfolio,
		Sides.Buy,
		Volume,
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

	// Crear procesador para venta
	_sellProcessor = new QuotingProcessor(
		sellBehavior,
		Security,
		Portfolio,
		Sides.Sell,
		Volume,
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

	// Registrar creación de nuevos procesadores de quoting
	this.AddInfoLog($"Spread de compra/venta creado en {CurrentTime}");

	// Suscribirse a eventos del procesador de compra para registro
	_buyProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Orden de compra {order.TransactionId} registrada al precio {order.Price}");

	_buyProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Error de orden de compra: {fail.Error.Message}");

	_buyProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Operación de compra ejecutada: {trade.Trade.Volume} a {trade.Trade.Price}");

	_buyProcessor.Finished += isOk => {
		this.AddInfoLog($"Quoting de compra finalizado correctamente: {isOk}");
		_buyProcessor?.Dispose();
		_buyProcessor = null;
	};

	// Suscribirse a eventos del procesador de venta para registro
	_sellProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Orden de venta {order.TransactionId} registrada al precio {order.Price}");

	_sellProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Error de orden de venta: {fail.Error.Message}");

	_sellProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Operación de venta ejecutada: {trade.Trade.Volume} a {trade.Trade.Price}");

	_sellProcessor.Finished += isOk => {
		this.AddInfoLog($"Quoting de venta finalizado correctamente: {isOk}");
		_sellProcessor?.Dispose();
		_sellProcessor = null;
	};

	// Iniciar ambos procesadores
	_buyProcessor.Start();
	_sellProcessor.Start();
}
```

## Liberación de recursos

En el método [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped), la estrategia libera recursos:

```cs
protected override void OnStopped()
{
	// Desuscribirse para evitar fugas de memoria
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Liberar recursos de procesadores
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	base.OnStopped();
}
```

## Lógica de trading

- La estrategia responde a cambios del tiempo de mercado
- Con posición cero y procesadores detenidos, se crean dos procesadores nuevos:
  - Procesador de compra (Buy)
  - Procesador de venta (Sell)
- Ambos procesadores se configuran con el mismo volumen y usan los mismos ajustes de quoting
- Los procesadores crean un spread en el mercado colocando simultáneamente órdenes de compra y venta

## Características

- Usa el procesador de quoting moderno [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) con [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)
- Crea un spread en el mercado colocando simultáneamente órdenes de compra y venta
- Trabaja solo con posición cero, evitando la acumulación de riesgo no deseado
- Admite configuración de varios parámetros de quoting (tipo de precio, desplazamiento, desviación mínima)
- Incluye registro detallado de eventos del procesador de quoting
- Gestiona correctamente los recursos al detener la estrategia y crear nuevos procesadores
- Admite trabajar con distintos tipos de precios de mercado (Following, Best, Opposite, etc.)
