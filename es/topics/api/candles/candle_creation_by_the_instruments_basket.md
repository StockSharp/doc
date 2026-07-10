# Creación de velas mediante una cesta de instrumentos

Para crear velas de [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity), [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) o [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity), se usa el mismo mecanismo de suscripción que para los instrumentos [Security](xref:StockSharp.BusinessEntities.Security) normales.

A continuación se muestra un ejemplo de creación de velas de 1 minuto para el spread AAPL - MSFT:

```cs
private Connector _connector;
private Security _instr1;
private Security _instr2;
private WeightedIndexSecurity _indexInstr;
private Subscription _indexSubscription;
private const string _secCode1 = "AAPL";
private const string _secCode2 = "MSFT";
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candleElement;

// Configuración de conexión y del conector
private void ConfigureConnector()
{
	if (_connector.Configure(this))
	{
		_connector.Save().Serialize(_connectorFile);
	}
}

// Configuración del gráfico
private void SetupChart()
{
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	_candleElement = new ChartCandleElement();
	_area.Elements.Add(_candleElement);

	// Suscribirse al evento de recepción de velas
	_connector.CandleReceived += OnCandleReceived;
}

// Registro de servicios
private void RegisterServices()
{
	ConfigManager.RegisterService<ISecurityProvider>(_connector);
	ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// Crear instrumento de índice y suscribirse a velas
private void CreateIndexAndSubscribe()
{
	// Crear instrumento de índice (spread)
	_indexInstr = new WeightedIndexSecurity()
	{
		Board = ExchangeBoard.Nyse,
		Id = "IndexInstr"
	};

	// Añadir instrumentos con pesos (1 y -1 para el spread)
	_indexInstr.Weights.Add(_instr1, 1);
	_indexInstr.Weights.Add(_instr2, -1);

	// Crear suscripción a velas del instrumento de índice
	_indexSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),  // velas de 1 minuto
		_indexInstr)  // Nuestro instrumento índice
	{
		MarketData =
		{
			// Configurar la suscripción para construir velas desde ticks
			BuildMode = MarketDataBuildModes.Build,
			BuildFrom = DataType.Ticks,

			// Solicitar datos históricos de 30 días
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};

	// Añadir elemento al gráfico y vincularlo a la suscripción
	_chart.AddElement(_area, _candleElement, _indexSubscription);

	// Iniciar suscripción
	_connector.Subscribe(_indexSubscription);
}

// Controlador del evento de recepción de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Comprobar si la vela pertenece a nuestra suscripción
	if (subscription != _indexSubscription)
		return;

	// Si es necesario, limitar el procesamiento a velas completadas
	if (candle.State != CandleStates.Finished)
		return;

	// Dibujar la vela en el gráfico
	var chartData = new ChartDrawData();
	chartData.Group(candle.OpenTime).Add(_candleElement, candle);

	this.GuiAsync(() => _chart.Draw(chartData));
}

// Cancelar la suscripción al cerrar la aplicación
private void Unsubscribe()
{
	if (_indexSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_indexSubscription);
		_indexSubscription = null;
	}
}
```

## Otros casos de uso para suscripciones a índices

### Creación de una suscripción a velas de índice a partir de velas de componentes

```cs
// Crear suscripción para construir velas de índice desde velas de componentes
var indexFromCandlesSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	_indexInstr)
{
	MarketData =
	{
		// Configurar la suscripción para construir desde velas de componentes
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Iniciar suscripción
_connector.Subscribe(indexFromCandlesSubscription);
```

### Creación de una suscripción a velas de índice a partir de libros de órdenes

```cs
// Crear suscripción para construir velas de índice desde libros de órdenes
var indexFromDepthSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	_indexInstr)
{
	MarketData =
	{
		// Configurar la suscripción para construir desde libros de órdenes
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle,  // Usar el punto medio del spread
		From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

// Iniciar suscripción
_connector.Subscribe(indexFromDepthSubscription);
```

### Trabajando con el índice de volatilidad

```cs
// Crear índice de volatilidad basado en una expresión
var volatilityIndex = new ExpressionIndexSecurity
{
	Board = ExchangeBoard.Nyse,
	Id = "VOLX",
	Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // Fórmula para calcular la volatilidad
};

// Añadir el instrumento principal al índice
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// Crear suscripción a velas del índice de volatilidad
var volatilitySubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	volatilityIndex)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Iniciar suscripción
_connector.Subscribe(volatilitySubscription);
```

## Véase también

[Futuros continuos](../instruments/continuous_futures.md)

[Índice](../instruments/index.md)