# Algoritmo de cotización

## Descripción general

Un algoritmo de cotización es un mecanismo que permite colocar y actualizar automáticamente órdenes en el mercado con el objetivo de lograr el mejor precio de ejecución. En lugar de colocar órdenes de mercado agresivas, la cotización usa órdenes limitadas, lo que ayuda a minimizar el deslizamiento y reducir costes de negociación.

## Componentes principales

StockSharp proporciona dos componentes clave para cotización:

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** -- el procesador principal que analiza datos de mercado y estado de órdenes para recomendar acciones de cotización.

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** -- una interfaz que define el comportamiento de cotización, incluido el cálculo del precio óptimo y la determinación de la necesidad de actualizar órdenes.

## Ejemplos de estrategias con cotización

La documentación presenta ejemplos de estrategias que demuestran el uso del mecanismo de cotización:

- **MqStrategy** -- ejemplo de una estrategia que usa el mecanismo de cotización para gestionar una posición en el mercado.
- **MqSpreadStrategy** -- ejemplo de una estrategia que crea un spread en el mercado colocando simultáneamente cotizaciones de compra y venta.
- **StairsCountertrendStrategy** -- ejemplo de una estrategia de contratendencia que usa cotización para una entrada más precisa en el mercado.

Estos ejemplos ayudan a comprender cómo integrar el mecanismo de cotización en sus propios algoritmos de negociación.

## Comportamientos de cotización

StockSharp admite varios comportamientos de cotización:

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** -- cotización basada en precio de mercado con desplazamiento y tipo personalizables.
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** -- cotización basada en el mejor precio con desplazamiento personalizable.
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** -- cotización a un precio fijo.
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** -- cotización basada en el mejor precio por volumen.
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** -- cotización basada en un nivel especificado del libro de órdenes.
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** -- cotización basada en el precio de la última operación.
- **[TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** -- cotización de opciones basada en precio teórico.
- **[VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** -- cotización de opciones basada en volatilidad.

## Uso en su propia estrategia

### Paso 1: Crear un comportamiento de cotización

```csharp
// Crear un comportamiento para cotización de mercado
var behavior = new MarketQuotingBehavior(
	new Unit(0.01m), // Desplazamiento de precio respecto a la mejor cotización
	new Unit(0.1m, UnitTypes.Percent), // Desviación mínima para actualizar la cotización
	MarketPriceTypes.Following // Tipo de precio de mercado para cotización
);
```

### Paso 2: Crear e inicializar el procesador

```csharp
// Crear un procesador de cotización
_quotingProcessor = new QuotingProcessor(
	behavior,
	Security, // Instrumento
	Portfolio, // Cartera
	Sides.Buy, // Dirección de cotización
	Volume, // Volumen de cotización
	Volume, // Volumen máximo de orden
	TimeSpan.Zero, // Sin timeout
	this, // Strategy implementa ISubscriptionProvider
	this, // Strategy implementa IMarketRuleContainer
	this, // Strategy implementa ITransactionProvider
	this, // Strategy implementa ITimeProvider
	this, // Strategy implementa IMarketDataProvider
	IsFormedAndOnlineAndAllowTrading, // Comprobar permiso de negociación
	true, // Usar precios del libro de órdenes
	true  // Usar precio de la última operación si el libro de órdenes está vacío
)
{
	Parent = this
};
```

### Paso 3: Suscribirse a eventos del procesador

```csharp
// Suscribirse a eventos del procesador para registro y manejo
_quotingProcessor.OrderRegistered += order =>
	this.AddInfoLog($"Orden {order.TransactionId} registrada al precio {order.Price}");

_quotingProcessor.OrderFailed += fail =>
	this.AddInfoLog($"Error de orden: {fail.Error.Message}");

_quotingProcessor.OwnTrade += trade =>
	this.AddInfoLog($"Operación ejecutada: {trade.Trade.Volume} a {trade.Trade.Price}");

_quotingProcessor.Finished += isOk => {
	this.AddInfoLog($"Cotización finalizada correctamente: {isOk}");
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
};
```

### Paso 4: Iniciar el procesador

```csharp
// Iniciar el procesador
_quotingProcessor.Start();
```

### Paso 5: Liberar recursos

No olvide limpiar los recursos del procesador al detener la estrategia:

```csharp
protected override void OnStopped()
{
	// Liberar recursos del procesador actual
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## Ventajas del uso

1. **Deslizamiento reducido** -- la cotización ayuda a obtener un mejor precio de ejecución frente a las órdenes de mercado.

2. **Flexibilidad de configuración** -- varios comportamientos de cotización para distintas situaciones de mercado.

3. **Actualizaciones automáticas** -- el procesador sigue automáticamente los cambios de mercado y actualiza órdenes cuando es necesario.

4. **Control completo** -- posibilidad de configurar parámetros de cotización, incluidos desplazamiento de precio, desviación mínima y tipo de precio de mercado.

5. **Gestión de riesgos** -- posibilidad de establecer un timeout para limitar el tiempo de ejecución.

## Casos de uso aplicados

- **Creación de mercado** -- creación de liquidez en el mercado mediante cotizaciones en ambos lados.
- **Negociación algorítmica** -- mejora del precio de ejecución en estrategias automatizadas.
- **Negociación de contratendencia** -- entrada más precisa en el mercado contra la tendencia.
- **Arbitraje** -- cotización simultánea en múltiples mercados para aprovechar discrepancias de precio.

Implementar el mecanismo de cotización en su estrategia puede mejorar significativamente la ejecución de órdenes y aumentar su eficiencia.
