# Trabajo con gráficos en estrategias

En StockSharp, la clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) proporciona una interfaz cómoda para visualizar actividad de negociación en un gráfico. En este artículo veremos cómo acceder a un gráfico desde una estrategia, crear áreas (ChartArea), agregar varios elementos (velas, indicadores, operaciones) y renderizar datos.

## Acceso al gráfico

### Método GetChart

Para acceder al gráfico desde una estrategia, use el método [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart):

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Obtener el gráfico
	_chart = GetChart();

	// Comprobar disponibilidad del gráfico
	if (_chart != null)
	{
		// Inicializar el gráfico
		InitializeChart();
	}
	else
	{
		// El gráfico no está disponible, por ejemplo, al ejecutarse en modo consola
		LogInfo("El gráfico no está disponible. Visualización desactivada.");
	}
}
```

El método [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) devuelve una interfaz [IChart](xref:StockSharp.Charting.IChart) que proporciona acceso a las funciones del gráfico. Es importante comprobar el resultado contra `null`, ya que el gráfico puede no estar disponible, por ejemplo, al ejecutar una estrategia en modo consola o en pruebas en la nube.

### Método SetChart

En algunos casos, el gráfico puede establecerse desde fuera. Para ello, use el método [Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)):

```cs
// Establecer el gráfico desde una fuente externa
public void ConfigureVisualization(IChart chart)
{
	SetChart(chart);

	if (chart != null)
	{
		InitializeChart();
	}
}
```

## Creación de áreas de gráfico

Después de obtener acceso al gráfico, puede crear una o varias áreas para mostrar distintos datos. Use el método [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea):

```cs
private void InitializeChart()
{
	// Crear el área principal para velas e indicadores
	_mainArea = CreateChartArea();

	// Crear un área adicional para volumen
	_volumeArea = CreateChartArea();

	// Configurar áreas y agregar elementos
	ConfigureChartElements();
}
```

También puede usar directamente el método [IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart)):

```cs
private void InitializeChart()
{
	// Limpiar áreas existentes si es necesario
	foreach (var area in _chart.Areas.ToArray())
		_chart.RemoveArea(area);

	// Crear el área principal para velas e indicadores
	_mainArea = _chart.AddArea();

	// Crear un área adicional para volumen
	_volumeArea = _chart.AddArea();

	// Configurar áreas y agregar elementos
	ConfigureChartElements();
}
```

## Agregar elementos al gráfico

Después de crear áreas de gráfico, puede agregar distintos elementos para mostrar datos. StockSharp admite distintos tipos de elementos, como velas, indicadores, operaciones y órdenes.

### Agregar velas

Para mostrar velas, use el método [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) del área de gráfico:

```cs
private void ConfigureChartElements()
{
	// Agregar un elemento de velas al área principal
	_candleElement = _mainArea.AddCandles();

	// Configurar visualización de velas
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick; // Velas japonesas
	_candleElement.AntiAliasing = true; // Suavizado
	_candleElement.UpFillColor = Color.Green; // Color del cuerpo de vela alcista
	_candleElement.DownFillColor = Color.Red; // Color del cuerpo de vela bajista
	_candleElement.UpBorderColor = Color.DarkGreen; // Color del borde de vela alcista
	_candleElement.DownBorderColor = Color.DarkRed; // Color del borde de vela bajista
	_candleElement.StrokeThickness = 1; // Grosor de línea
	_candleElement.ShowAxisMarker = true; // Mostrar marcador del eje Y
}
```

La interfaz [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) proporciona muchas propiedades para configurar la visualización de velas:

- **DrawStyle** - estilo de visualización de velas:
  - **CandleStick** - velas japonesas
  - **Ohlc** - barras
  - **LineOpen/LineHigh/LineLow/LineClose** - líneas para los precios correspondientes
  - **BoxVolume** - cajas de volumen
  - **ClusterProfile** - perfil de clúster
  - **Area** - área
  - **PnF** - gráfico punto y figura

- **Ajustes de color**:
  - **UpFillColor/DownFillColor** - color del cuerpo de vela alcista/bajista
  - **UpBorderColor/DownBorderColor** - color del borde de vela alcista/bajista
  - **LineColor** - color de línea para gráficos de tipo línea
  - **AreaColor** - color de área para tipo Area

- **Otros ajustes**:
  - **StrokeThickness** - grosor de línea
  - **AntiAliasing** - suavizado
  - **ShowAxisMarker** - mostrar marcador del eje Y

### Agregar indicadores

Para mostrar indicadores, use el método [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})):

```cs
// Crear indicadores
_sma = new SimpleMovingAverage { Length = SmaLength };
_bollinger = new BollingerBands
{
	Length = BollingerLength,
	Deviation = BollingerDeviation
};

// Agregar indicadores a la colección de la estrategia
Indicators.Add(_sma);
Indicators.Add(_bollinger);

// Visualizar indicadores
_smaElement = DrawIndicator(_mainArea, _sma, Color.Blue);
_bollingerUpperElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerLowerElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerMiddleElement = DrawIndicator(_mainArea, _bollinger, Color.Gray);
```

El método [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) crea automáticamente un elemento de indicador y lo agrega al área de gráfico especificada. Puede especificar un color y un color adicional para la visualización.

También puede agregar un elemento de indicador directamente mediante el método [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) del área de gráfico:

```cs
// Agregar SMA directamente mediante el área de gráfico
var smaElement = _mainArea.AddIndicator(_sma);
smaElement.Color = Color.Blue;
smaElement.StrokeThickness = 2;
smaElement.DrawStyle = DrawStyles.Line;
smaElement.AntiAliasing = true;
smaElement.ShowAxisMarker = true;
smaElement.AutoAssignYAxis = true; // Asignar eje Y automáticamente
```

La interfaz [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) proporciona las siguientes propiedades para configuración:

- **Color** - color principal del indicador
- **AdditionalColor** - color adicional (para indicadores con dos líneas)
- **StrokeThickness** - grosor de línea
- **AntiAliasing** - suavizado
- **DrawStyle** - estilo de dibujo (línea, puntos, histograma, etc.)
- **ShowAxisMarker** - mostrar marcador del eje Y
- **AutoAssignYAxis** - asignar eje Y automáticamente

### Agregar operaciones

Para mostrar operaciones, use el método [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)):

```cs
// Agregar un elemento para mostrar operaciones
_tradesElement = DrawOwnTrades(_mainArea);

// Configurar visualización de operaciones
_tradesElement.BuyBrush = Color.Green;  // Color de compra
_tradesElement.SellBrush = Color.Red;   // Color de venta
_tradesElement.PointSize = 10;          // Tamaño del punto
```

### Agregar órdenes

Para mostrar órdenes, use el método [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)):

```cs
// Agregar un elemento para mostrar órdenes
_ordersElement = DrawOrders(_mainArea);

// Configurar visualización de órdenes
_ordersElement.ActiveBrush = Color.Blue;     // Color de órdenes activas
_ordersElement.CanceledBrush = Color.Gray;   // Color de órdenes canceladas
_ordersElement.DoneBrush = Color.Green;      // Color de órdenes completadas
_ordersElement.ErrorColor = Color.Red;       // Color de error
_ordersElement.PointSize = 8;                // Tamaño del punto
```

La interfaz [IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) proporciona las siguientes propiedades para configuración:

- **ActiveBrush** - color de órdenes activas
- **CanceledBrush** - color de órdenes canceladas
- **DoneBrush** - color de órdenes completadas
- **ErrorColor** - color de error
- **ErrorStrokeColor** - color de borde de error
- **Filter** - filtro de visualización de órdenes

## Dibujo de datos en el gráfico

Después de configurar todos los elementos de gráfico, puede pasar a dibujar datos. Se usan distintos métodos según el tipo de datos.

### Dibujar velas e indicadores

La forma más eficiente de dibujar datos es usar el método [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) con un objeto [IChartDrawData](xref:StockSharp.Charting.IChartDrawData):

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Procesar vela en indicadores
	var smaValue = _sma.Process(candle);
	var bollingerValue = _bollinger.Process(candle);

	// Si el gráfico no está disponible, omitir dibujo
	if (_chart == null)
		return;

	// Crear datos para dibujar
	var drawData = _chart.CreateData();

	// Agrupar datos por hora de vela
	var group = drawData.Group(candle.OpenTime);

	// Agregar vela
	group.Add(_candleElement,
		candle.DataType,
		candle.SecurityId,
		candle.OpenPrice,
		candle.HighPrice,
		candle.LowPrice,
		candle.ClosePrice,
		candle.PriceLevels,
		candle.State);

	// Agregar valores de indicadores
	group.Add(_smaElement, smaValue);

	if (bollingerValue != null)
	{
		group.Add(_bollingerUpperElement, bollingerValue);
		group.Add(_bollingerMiddleElement, bollingerValue);
		group.Add(_bollingerLowerElement, bollingerValue);
	}

	// Dibujar datos en el gráfico
	_chart.Draw(drawData);
}
```

El método [IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) crea un objeto [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) usado para agrupar y agregar datos para distintos elementos de gráfico. La agrupación de datos se realiza por timestamp mediante el método [Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset)).

Para agregar datos de distintos tipos, se usan varias sobrecargas del método [Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates)) del objeto [IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem).

### Dibujar operaciones y órdenes

Para dibujar operaciones y órdenes, normalmente se usa un mecanismo automático que se activa cuando se reciben nuevas operaciones o cambian órdenes. Sin embargo, si se requiere dibujo manual, puede usar el siguiente código:

```cs
// Dibujar una operación
var tradeDrawData = _chart.CreateData();
var tradeGroup = tradeDrawData.Group(trade.Time);
tradeGroup.Add(_tradesElement, trade.Id, trade.StringId, trade.Side, trade.Price, trade.Volume);
_chart.Draw(tradeDrawData);

// Dibujar una orden
var orderDrawData = _chart.CreateData();
var orderGroup = orderDrawData.Group(order.Time);
orderGroup.Add(_ordersElement, order.Id, order.StringId, order.Side, order.Price, order.Volume);
_chart.Draw(orderDrawData);
```

## Ejemplo completo de renderizado de gráfico en una estrategia

A continuación se muestra un ejemplo completo de una estrategia con configuración y renderizado de gráfico:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _smaLength;
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;

	private SimpleMovingAverage _sma;
	private BollingerBands _bollinger;

	private IChart _chart;
	private IChartArea _mainArea;
	private IChartArea _volumeArea;

	private IChartCandleElement _candleElement;
	private IChartIndicatorElement _smaElement;
	private IChartIndicatorElement _bollingerUpperElement;
	private IChartIndicatorElement _bollingerMiddleElement;
	private IChartIndicatorElement _bollingerLowerElement;
	private IChartOrderElement _ordersElement;
	private IChartTradeElement _tradesElement;

	public SmaStrategy()
	{
		_smaLength = Param(nameof(SmaLength), 20);
		_bollingerLength = Param(nameof(BollingerLength), 20);
		_bollingerDeviation = Param(nameof(BollingerDeviation), 2m);
	}

	public int SmaLength
	{
		get => _smaLength.Value;
		set => _smaLength.Value = value;
	}

	public int BollingerLength
	{
		get => _bollingerLength.Value;
		set => _bollingerLength.Value = value;
	}

	public decimal BollingerDeviation
	{
		get => _bollingerDeviation.Value;
		set => _bollingerDeviation.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Crear indicadores
		_sma = new SimpleMovingAverage { Length = SmaLength };
		_bollinger = new BollingerBands
		{
			Length = BollingerLength,
			Deviation = BollingerDeviation
		};

		// Agregar indicadores a la colección de la estrategia
		Indicators.Add(_sma);
		Indicators.Add(_bollinger);

		// Obtener el gráfico
		_chart = GetChart();

		// Inicializar el gráfico si está disponible
		if (_chart != null)
		{
			InitializeChart();
		}

		// Suscribirse a velas
		var subscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			Security);

		subscription
			.WhenCandlesFinished(this)
			.Do(ProcessCandle)
			.Apply(this);

		Subscribe(subscription);
	}

	private void InitializeChart()
	{
		// Limpiar áreas existentes
		foreach (var area in _chart.Areas.ToArray())
			_chart.RemoveArea(area);

		// Crear el área principal para velas e indicadores
		_mainArea = _chart.AddArea();

		// Crear un área adicional para volumen
		_volumeArea = _chart.AddArea();

		// Configurar elementos del gráfico
		ConfigureChartElements();
	}

	private void ConfigureChartElements()
	{
		// Agregar un elemento para mostrar velas
		_candleElement = _mainArea.AddCandles();
		_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
		_candleElement.AntiAliasing = true;
		_candleElement.UpFillColor = Color.Green;
		_candleElement.DownFillColor = Color.Red;
		_candleElement.UpBorderColor = Color.DarkGreen;
		_candleElement.DownBorderColor = Color.DarkRed;
		_candleElement.StrokeThickness = 1;
		_candleElement.ShowAxisMarker = true;

		// Agregar elementos para indicadores
		_smaElement = _mainArea.AddIndicator(_sma);
		_smaElement.Color = Color.Blue;
		_smaElement.StrokeThickness = 2;

		_bollingerUpperElement = _mainArea.AddIndicator(_bollinger);
		_bollingerUpperElement.Color = Color.Purple;
		_bollingerUpperElement.StrokeThickness = 1;

		_bollingerMiddleElement = _mainArea.AddIndicator(_bollinger);
		_bollingerMiddleElement.Color = Color.Gray;
		_bollingerMiddleElement.StrokeThickness = 1;

		_bollingerLowerElement = _mainArea.AddIndicator(_bollinger);
		_bollingerLowerElement.Color = Color.Purple;
		_bollingerLowerElement.StrokeThickness = 1;

		// Agregar elementos para órdenes y operaciones
		_ordersElement = DrawOrders(_mainArea);
		_tradesElement = DrawOwnTrades(_mainArea);
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		// Procesar vela con indicadores
		var smaValue = _sma.Process(candle);
		var bollingerValue = _bollinger.Process(candle);

		// Si el gráfico no está disponible, omitir dibujo
		if (_chart == null)
			return;

		// Dibujar datos en el gráfico
		var drawData = _chart.CreateData();
		var group = drawData.Group(candle.OpenTime);

		// Agregar vela
		group.Add(_candleElement,
			candle.DataType,
			candle.SecurityId,
			candle.OpenPrice,
			candle.HighPrice,
			candle.LowPrice,
			candle.ClosePrice,
			candle.PriceLevels,
			candle.State);

		// Agregar valores de indicadores
		group.Add(_smaElement, smaValue);

		if (bollingerValue != null)
		{
			group.Add(_bollingerUpperElement, bollingerValue);
			group.Add(_bollingerMiddleElement, bollingerValue);
			group.Add(_bollingerLowerElement, bollingerValue);
		}

		// Dibujar datos en el gráfico
		_chart.Draw(drawData);

		// Lógica de negociación
		if (!IsFormed)
			return;

		// ... implementación de lógica de negociación ...
	}
}
```

## Conclusión

Usar gráficos en estrategias StockSharp permite visualizar la actividad de negociación, lo que simplifica significativamente el desarrollo, la depuración y la supervisión de estrategias de negociación. La clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) proporciona muchos métodos para trabajar con gráficos, lo que permite agregar fácilmente distintos elementos y renderizar datos.

Al desarrollar una estrategia con interfaz gráfica, considere siempre que el gráfico puede no estar disponible, por ejemplo, al ejecutarse en modo consola o en pruebas en la nube. Por lo tanto, es importante comprobar el resultado del método [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) contra `null` y proporcionar un escenario alternativo para que la estrategia funcione sin visualización.

## Ver también

- [Indicadores en estrategias](indicators.md)
- [Operaciones de negociación en estrategias](trading_operations.md)
