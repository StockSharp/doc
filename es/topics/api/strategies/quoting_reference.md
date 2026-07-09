# Referencia de quoting

## Descripción general

Esta sección contiene una referencia completa de la arquitectura y los componentes del sistema de quoting en StockSharp. Se describen todos los comportamientos de quoting, tipos de acción e interfaces disponibles. Para una introducción básica, consulte [Algoritmo de quoting](quoting.md).

## Arquitectura

### QuotingStrategy (obsoleto)

La clase [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) está obsoleta. Se recomienda usar [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) en su lugar.

Parámetros principales de la clase obsoleta:

- `QuotingSide` -- dirección de quoting (compra/venta)
- `QuotingVolume` -- volumen de quoting
- `TimeOut` -- timeout de ejecución
- `UseBidAsk` -- usar precios del libro de órdenes
- `UseLastTradePrice` -- usar precio de la última operación

### QuotingEngine

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) -- núcleo funcional del sistema de quoting. Calcula volumen restante y timeouts, y devuelve recomendaciones de acción sin efectos secundarios.

### QuotingBehaviorAlgo

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- implementación de la interfaz `IPositionModifyAlgo` para gestión algorítmica de posiciones.

Métodos principales:

| Método | Descripción |
|--------|-------------|
| `UpdateMarketData(time, price, volume)` | Actualizar datos de mercado |
| `UpdateOrderBook(depth)` | Actualizar libro de órdenes |
| `GetNextAction()` | Obtener la siguiente acción de quoting |

Admite modos VWAP y TWAP.

### QuotingProcessor

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) -- procesador principal para ejecutar quoting dentro de una estrategia. Gestiona el ciclo de vida de órdenes: colocación, modificación, cancelación.

## Tipos QuotingAction

El procesador devuelve acciones de tipo [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction):

| Tipo | Descripción |
|------|-------------|
| `None(reason)` | No se requiere ninguna acción |
| `PlaceOrder(price, volume)` | Colocar una nueva orden |
| `ModifyOrder(price, volume)` | Modificar una orden existente |
| `CancelOrder()` | Cancelar la orden |
| `Finish(success, reason)` | Finalizar quoting |

## Comportamientos de quoting

StockSharp proporciona 10 tipos de comportamientos de quoting, cada uno implementando la interfaz [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior):

| # | Clase | Descripción | Parámetros clave |
|---|-------|-------------|------------------|
| 1 | [BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | Mejor precio del libro de órdenes | BestPriceOffset |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | Precio de la última operación | BestPriceOffset |
| 3 | [LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | Precio límite fijo | LimitPrice |
| 4 | [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | Precio de mercado con desplazamiento | PriceOffset, PriceType (Following/Opposite/Middle) |
| 5 | [VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | Volatilidad de opción (Black-Scholes) | IVRange, Model |
| 6 | [TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | Precio teórico de opción | TheorPriceOffset |
| 7 | [BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | Volumen acumulado en el libro de órdenes | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | Nivel de profundidad del libro de órdenes | Level (Range\<int\>), OwnLevel |
| 9 | [VWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- promedio ponderado por volumen | BestPriceOffset |
| 10 | [TWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- promedio ponderado por tiempo | TimeInterval, PriceBufferSize (predeterminado 10) |

## Interfaz IQuotingBehavior

La interfaz [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) define dos métodos clave:

### CalculateBestPrice

Calcula el mejor precio para colocar una orden:

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### NeedQuoting

Determina si la orden actual necesita actualizarse:

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

Devuelve `null` si no se necesita actualización, o el nuevo precio para colocar la orden.

## Ejemplos de uso

### Quoting de mercado con desplazamiento

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### Quoting VWAP

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### Quoting de volatilidad de opciones

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### Ejemplo completo con procesador

```cs
// Elegir el comportamiento de quoting
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// Crear el procesador
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"Quoting finalizado: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## Ver también

- [Algoritmo de quoting](quoting.md)
- [Estrategia de quoting](samples/mq.md)
- [Quoting de volatilidad](../options/volatility_trading.md)
