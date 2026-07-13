# API de alto nivel en estrategias

StockSharp proporciona un conjunto de APIs de alto nivel para simplificar el trabajo con tareas comunes en estrategias de negociación. Estas interfaces permiten escribir código más limpio, centrado en la lógica de negociación en lugar de detalles técnicos.

## Gestión simplificada de suscripciones

Los métodos de alto nivel para trabajar con suscripciones ocultan la complejidad de gestionar el ciclo de vida de suscripciones y el procesamiento de datos.

### Método SubscribeCandles

En lugar de crear manualmente una suscripción y configurar controladores de eventos, puede usar el método [SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)):

```cs
// Crear y configurar una suscripción a velas en una sola línea
var subscription = SubscribeCandles(CandleType);
```

Este método devuelve un objeto de tipo [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1), que proporciona una interfaz cómoda para configurar posteriormente la suscripción.

### Vinculación automática de indicadores con la suscripción

La API de alto nivel facilita vincular indicadores a una suscripción de datos:

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
	// Vincular indicadores a la suscripción de velas
	.Bind(longSma, shortSma, OnProcess)
	// Iniciar procesamiento
	.Start();
```

#### Adición automática de indicadores a la colección Strategy.Indicators

Es importante señalar que al usar el método [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) para enlazar indicadores con una suscripción, **no necesita** agregar adicionalmente estos indicadores a la colección [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators), como suele hacerse en el enfoque tradicional (descrito en la [documentación de indicadores](indicators.md)). El sistema automáticamente:

1. Agrega indicadores a la colección [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators)
2. Realiza seguimiento del estado de formación de los indicadores
3. Actualiza el estado [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) de la estrategia

Esto simplifica significativamente el código y reduce la probabilidad de errores.

Si necesita recibir valores de indicadores incluso cuando algunos de ellos todavía no tienen datos (`IIndicatorValue.IsEmpty` es `true`), use el método `BindWithEmpty`. En este caso, los argumentos del controlador deben ser de tipo `decimal?`. También puede usar `BindEx` para inspeccionar directamente los objetos `IIndicatorValue` sin procesar.

#### Uso de BindEx para trabajar con valores de indicador sin procesar

Si un indicador devuelve valores no estándar (no solo números), puede usar el método [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)), que proporciona acceso al objeto [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) original:

```cs
subscription
	.BindEx(indicator, OnProcessWithRawValue)
	.Start();

// El controlador recibe el IIndicatorValue original
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
	// Acceso a propiedades de IIndicatorValue
	if (value.IsFinal)
	{
		// Para indicadores que devuelven valores booleanos
		var boolValue = value.GetValue<bool>();

		// U otros tipos de datos específicos de un indicador concreto
		// ...
	}
}
```

El método [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) es especialmente útil en los siguientes casos:

- Trabajo con indicadores que devuelven valores booleanos (por ejemplo, [Fractals](xref:StockSharp.Algo.Indicators.Fractals))
- Acceso a propiedades adicionales del tipo de valor del indicador (por ejemplo, el flag [IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal))
- Trabajo con indicadores que devuelven datos estructurados

#### Trabajo con indicadores complejos (IComplexIndicator)

Para indicadores complejos que contienen varios indicadores internos (por ejemplo, [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), [MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence)), la API proporciona sobrecargas especiales de los métodos `Bind` y `BindEx`:

```cs
// Crear un indicador complejo
var bollinger = new BollingerBands
{
	Length = 20,
	Deviation = 2
};

// Vincular el indicador complejo a una suscripción
subscription
	.BindEx(bollinger, OnProcessBollinger)
	.Start();

// El controlador recibe la instancia BollingerBandsValue
private void OnProcessBollinger(ICandleMessage candle, IIndicatorValue value)
{
	var typed = (BollingerBandsValue)value;

	// Usar valores de las bandas de Bollinger
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
}
```

Para un trabajo más flexible, puede usar [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) con acceso directo al valor del indicador complejo:

```cs
subscription.BindEx(bollinger, (candle, indicatorValue) =>
{
	var typed = (BollingerBandsValue)indicatorValue;

	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
});
```

El método [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) para indicadores complejos automáticamente:

1. Procesa los datos de entrada mediante el indicador complejo
2. Pasa el `IIndicatorValue` resultante al controlador especificado

Convierta el valor al **tipo de valor** dedicado del indicador para trabajar con sus campos individuales.

### El método `Bind` establece una conexión entre los datos de suscripción y los indicadores. Cuando se recibe una nueva vela:

1. La vela se envía automáticamente para procesamiento a los indicadores
2. Los resultados de procesamiento se pasan al controlador especificado (en el ejemplo, el método `OnProcess`)
3. Todo el código de sincronización y gestión de estado queda oculto al desarrollador

El controlador recibe valores listos para usar como tipos `decimal` simples. El método se llama solo cuando todos los indicadores vinculados devuelven datos:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Trabajar directamente con valores de indicador listos
	var isShortLessThenLong = shortValue < longValue;

	// La lógica de negociación usa valores numéricos limpios
	// sin necesidad de extraerlos de IIndicatorValue
	// ...
}
```

Esto simplifica significativamente el código y lo hace más legible, ya que el desarrollador no necesita:
- Manejar manualmente el evento de recepción de una vela
- Pasar datos manualmente a indicadores
- Extraer valores de los resultados de indicadores

## Gestión simplificada de gráficos

### Visualización automática

La API de alto nivel proporciona métodos simples para vincular suscripciones e indicadores a elementos de gráfico:

```cs
var area = CreateChartArea();

// area puede ser null cuando se ejecuta sin GUI
if (area != null)
{
	// Vinculación automática de velas al área del gráfico
	DrawCandles(area, subscription);

	// Dibujo de indicadores con personalización de color
	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);

	// Dibujo de operaciones propias
	DrawOwnTrades(area);

	// Dibujo de órdenes
	DrawOrders(area);
}
```

#### Método DrawCandles

El método [DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) enlaza automáticamente una suscripción a velas con un elemento de visualización de velas en el gráfico:

```cs
// Crear un elemento de gráfico para mostrar velas
IChartCandleElement candles = DrawCandles(area, subscription);

// Se pueden configurar parámetros adicionales del elemento
candles.DrawOpenClose = true;  // Mostrar líneas de apertura/cierre
candles.DrawHigh = true;       // Mostrar máximos
candles.DrawLow = true;        // Mostrar mínimos
```

El método devuelve un elemento de gráfico [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) que se puede personalizar posteriormente.

#### Método DrawIndicator

El método [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) crea y configura un elemento de gráfico para mostrar valores de indicadores:

```cs
// Adición simple de un indicador al gráfico con color predeterminado
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// Adición de un indicador con un color primario especificado
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// Adición de un indicador con colores primario y secundario especificados
IChartIndicatorElement bollingerElem = DrawIndicator(
	area,
	bollinger,
	System.Drawing.Color.Blue,    // Color primario
	System.Drawing.Color.Gray     // Color secundario (para la segunda línea)
);

// Configuración adicional del elemento
smaElem.DrawStyle = DrawStyles.Line;           // Estilo de dibujo: línea
rsiFast.DrawStyle = DrawStyles.Dot;            // Estilo de dibujo: puntos
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // Estilo de dibujo: guion-punto
```

El método devuelve un elemento de gráfico [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) que se puede personalizar. Para indicadores con múltiples valores (por ejemplo, [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)), el color primario se aplica al primer valor y el color secundario al segundo.

#### Método DrawOwnTrades

El método [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) crea un elemento para mostrar las operaciones propias de la estrategia en el gráfico:

```cs
// Crear un elemento para mostrar operaciones
IChartTradeElement trades = DrawOwnTrades(area);

// Configuración del elemento
trades.BuyColor = System.Drawing.Color.Green;   // Color para operaciones de compra
trades.SellColor = System.Drawing.Color.Red;    // Color para operaciones de venta
trades.FullTitle = "Operaciones de mi estrategia"; // Título del elemento
```

Este método configura automáticamente la visualización de todas las operaciones ejecutadas por la estrategia. Las operaciones se muestran en el gráfico como marcadores en los puntos donde fueron ejecutadas, teniendo en cuenta el lado de la operación (compra/venta).

#### Método DrawOrders

El método [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) crea un elemento para mostrar órdenes en el gráfico:

```cs
// Crear un elemento para mostrar órdenes
IChartOrderElement orders = DrawOrders(area);

// Configuración del elemento
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // Color para órdenes de compra activas
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // Color para órdenes de venta activas
orders.BuyColor = System.Drawing.Color.Green;              // Color para órdenes de compra ejecutadas
orders.SellColor = System.Drawing.Color.Red;               // Color para órdenes de venta ejecutadas
orders.CancelColor = System.Drawing.Color.Gray;            // Color para órdenes canceladas
```

Este método configura automáticamente la visualización de todas las órdenes colocadas por la estrategia. Las órdenes se muestran como marcadores en sus niveles de precio con codificación de colores distinta para diferentes estados de orden.

#### Método CreateChartArea

El método [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) crea una nueva área en el gráfico de la estrategia:

```cs
// Crear la primera área para velas e indicadores
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// Crear una segunda área para indicadores separados (por ejemplo, RSI)
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

Dividir el gráfico en áreas permite mostrar de forma más visual distintos tipos de datos. Por ejemplo, los indicadores con un rango de valores diferente al precio (RSI, stochastic, etc.) se muestran mejor en áreas separadas.

Ventajas de los métodos de visualización de alto nivel:
- No es necesario crear manualmente objetos `ChartDrawData`
- No es necesario gestionar agrupación de datos por tiempo
- No es necesario llamar a `chart.Draw()` para actualizar el gráfico
- Sincronización automática de datos entre suscripciones y elementos de gráfico
- Gestión simplificada de la apariencia de elementos gráficos

El sistema actualiza automáticamente el gráfico cuando se reciben nuevos datos, lo que permite al desarrollador evitar centrarse en detalles técnicos de visualización.

## Protección de posiciones

### Método StartProtection

Para proteger posiciones abiertas, StockSharp proporciona el método de alto nivel [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)):

```cs
// Iniciar protección de posición con niveles Take Profit y Stop Loss
StartProtection(TakeValue, StopValue);
```

Este método configura automáticamente la protección de todas las posiciones abiertas:
- Sigue cambios de precio
- Crea automáticamente órdenes para cerrar posiciones cuando se alcanzan niveles Take Profit o Stop Loss
- Admite varios tipos de unidades de medida (valores absolutos, porcentajes, puntos)
- Puede usar un stop dinámico para protección adaptativa de posiciones

Ejemplo con parámetros adicionales:

```cs
// Iniciar protección con stop dinámico y órdenes de mercado
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute), // Take Profit
	stopLoss: new Unit(2, UnitTypes.Percent),     // Stop Loss en porcentaje
	isStopTrailing: true,                         // Activar stop dinámico
	useMarketOrders: true                         // Usar órdenes de mercado
);
```

## Ventajas de la API de alto nivel

La API de alto nivel en estrategias StockSharp proporciona las siguientes ventajas:

1. **Reducción de volumen de código** - realizar tareas comunes requiere menos líneas de código

2. **Separación de responsabilidades** - la lógica de negociación se separa de los detalles técnicos de procesamiento de datos y visualización

3. **Mejor legibilidad** - el código se vuelve más comprensible y expresivo, centrado en la lógica de negocio

4. **Menor probabilidad de errores** - muchos errores típicos se eliminan mediante la automatización de tareas rutinarias

5. **Trabajo con tipos de datos limpios** - en lugar de trabajar con objetos complejos, puede operar con tipos de datos simples (por ejemplo, `decimal`)

## Ejemplo de estrategia que usa la API de alto nivel

A continuación se muestra un ejemplo completo de una estrategia que demuestra el uso de la API de alto nivel:

```cs
public class SmaStrategy : Strategy
{
	private bool? _isShortLessThenLong;

	public SmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
		_long = Param(nameof(Long), 80);
		_short = Param(nameof(Short), 30);
		_takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
		_stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
	}

	private readonly StrategyParam<DataType> _candleType;
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}

	private readonly StrategyParam<int> _long;
	public int Long
	{
		get => _long.Value;
		set => _long.Value = value;
	}

	private readonly StrategyParam<int> _short;
	public int Short
	{
		get => _short.Value;
		set => _short.Value = value;
	}

	private readonly StrategyParam<Unit> _takeValue;
	public Unit TakeValue
	{
		get => _takeValue.Value;
		set => _takeValue.Value = value;
	}

	private readonly StrategyParam<Unit> _stopValue;
	public Unit StopValue
	{
		get => _stopValue.Value;
		set => _stopValue.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Crear indicadores
		var longSma = new SMA { Length = Long };
		var shortSma = new SMA { Length = Short };

		// Crear una suscripción a velas y vincularla a indicadores
		var subscription = SubscribeCandles(CandleType);
		subscription
			.Bind(longSma, shortSma, OnProcess)
			.Start();

		// Configurar visualización
		var area = CreateChartArea();
		if (area != null)
		{
			DrawCandles(area, subscription);
			DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
			DrawIndicator(area, longSma);
			DrawOwnTrades(area);
		}

		// Iniciar protección de posición
		StartProtection(TakeValue, StopValue);
	}

	private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
	{
		// Procesar solo velas finalizadas
		if (candle.State != CandleStates.Finished)
			return;

		// Lógica de negociación basada en cruce de indicadores
		var isShortLessThenLong = shortValue < longValue;

		if (_isShortLessThenLong == null)
		{
			_isShortLessThenLong = isShortLessThenLong;
		}
		else if (_isShortLessThenLong != isShortLessThenLong)
		{
			// Se produjo un cruce
			var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
			var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
			var priceStep = GetSecurity().PriceStep ?? 1;
			var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

			// Colocar una orden
			if (direction == Sides.Buy)
				BuyLimit(price, volume);
			else
				SellLimit(price, volume);

			// Guardar posición actual del indicador
			_isShortLessThenLong = isShortLessThenLong;
		}
	}
}
```

## Conclusión

La API de alto nivel en StockSharp simplifica significativamente el desarrollo de estrategias de negociación, permitiendo a los desarrolladores centrarse en la lógica de negociación en lugar de detalles técnicos. Es especialmente útil para casos de uso típicos donde no se requiere ajuste fino del procesamiento de datos o la visualización.

Combinada con el sistema de parámetros de estrategia, el modelo de eventos y los mecanismos de protección de posiciones, la API de alto nivel convierte a StockSharp en una herramienta potente y cómoda para la negociación algorítmica, adecuada tanto para principiantes como para desarrolladores experimentados.
