# Compatibilidad de estrategias con plataformas StockSharp

Al desarrollar estrategias de trading en StockSharp, es importante considerar su compatibilidad con varias plataformas: [Designer](../../designer.md), [Shell](../../shell.md), [Runner](../../runner.md) y [backtesting en la nube](../../designer/backtesting/cloud_backtesting.md). Siguiendo las recomendaciones siguientes, creará una estrategia que funciona correctamente en todos los entornos.

## Parámetros del constructor de la estrategia

### Evite parámetros en el constructor

Para garantizar la compatibilidad con plataformas StockSharp, especialmente con backtesting en la nube, **no debe agregar parámetros al constructor de la estrategia**:

```cs
// Correcto: constructor sin parámetros
public class SmaStrategy : Strategy
{
	public SmaStrategy()
	{
		// Inicialización de parámetros
	}
}

// Incorrecto: constructor con parámetros
public class SmaStrategy : Strategy
{
	public SmaStrategy(int longLength, int shortLength) // No use este enfoque
	{
		// ...
	}
}
```

Las plataformas StockSharp crean instancias de estrategia mediante un constructor sin parámetros. Si su estrategia requiere un constructor con parámetros, no se inicializará correctamente.

## Uso de StrategyParam en lugar de propiedades habituales

### Ventajas de StrategyParam

En lugar de crear propiedades C# habituales y luego sobrescribir los métodos `Save` y `Load`, use [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) para todos los parámetros personalizables:

```cs
// Correcto: uso de StrategyParam
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
	get => _longSmaLength.Value;
	set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
	_longSmaLength = Param(nameof(LongSmaLength), 80)
						.SetDisplay("Long SMA length", string.Empty, "Base settings");
}

// Incorrecto: uso de propiedades habituales
private int _longSmaLength = 80; // No use este enfoque

public int LongSmaLength
{
	get => _longSmaLength;
	set => _longSmaLength = value;
}
```

Los parámetros creados mediante [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) automáticamente:
- Se muestran en las interfaces de usuario de la plataforma
- Se guardan y cargan sin sobrescribir los métodos `Save` y `Load`
- Se usan en optimización
- Se serializan correctamente al enviarse a backtesting en la nube

## Trabajo con la interfaz de usuario

### Use abstracciones en lugar de acceso directo a la UI

En lugar de acceder directamente a elementos de la interfaz de usuario, use las abstracciones proporcionadas por StockSharp:

```cs
// Enfoque correcto: uso de IChart
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Obtener el gráfico proporcionado por el entorno de ejecución
	_chart = GetChart();
	
	if (_chart != null)
	{
		// El gráfico está disponible (por ejemplo, en Designer o Shell)
		InitChart();
	}
	else
	{
		// El gráfico no está disponible (por ejemplo, en Runner o backtesting en la nube)
		// La estrategia continúa funcionando sin visualización
	}
}

private void InitChart()
{
	// Configurar el gráfico mediante la interfaz abstracta
	_chart.ClearAreas();
	var area = _chart.AddArea();
	_chartCandleElement = area.AddCandles();
	// ...
}
```

El método [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) devuelve una interfaz [IChart](xref:StockSharp.Charting.IChart) si hay un gráfico disponible en el entorno de ejecución actual. Si la estrategia se ejecuta en el [Runner](../../runner.md) de consola o en backtesting en la nube, donde no hay interfaz gráfica, el método devolverá `null`.

La interfaz [IChart](xref:StockSharp.Charting.IChart) proporciona métodos para trabajar con gráficos:
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - para agregar un área al gráfico
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - para eliminar un área
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - para agregar un elemento al gráfico
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - para eliminar un elemento
- [Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - para restablecer valores de elementos

### Compruebe la disponibilidad del gráfico

Compruebe siempre la disponibilidad del gráfico antes de usarlo:

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
	if (_chart == null) return; // Comprobación importante
	
	var data = _chart.CreateData();
	data.Group(candle.OpenTime)
		.Add(_chartCandleElement, candle)
		.Add(_longSmaIndicatorElement, longSma)
		.Add(_shortSmaIndicatorElement, shortSma);
	_chart.Draw(data);
}
```

## Hilos y sincronización

### Evite crear hilos adicionales

En StockSharp, **no necesita crear hilos adicionales** para procesar datos. Todos los eventos (datos de mercado, transacciones) llegan en un único hilo:

```cs
// Correcto: uso de controladores de eventos estándar
private void ProcessCandle(ICandleMessage candle)
{
	// Procesar la vela en el hilo principal
	var longSmaIsFormedPrev = _longSma.IsFormed;
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	
	// ...
}

// Incorrecto: creación de hilos adicionales
private void ProcessCandle(ICandleMessage candle)
{
	// NO haga esto
	Task.Run(() => {
		var longSmaIsFormedPrev = _longSma.IsFormed;
		// ...
	});
}
```

### Evite objetos de sincronización

Como todos los eventos se procesan en un único hilo, **no hay necesidad de usar objetos de sincronización**:

```cs
// Correcto: procesamiento habitual sin sincronización
private void ProcessCandle(ICandleMessage candle)
{
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	// ...
}

// Incorrecto: sincronización innecesaria
private readonly object _syncLock = new object(); // No es necesario

private void ProcessCandle(ICandleMessage candle)
{
	lock (_syncLock) // No es necesario
	{
		var ls = _longSma.Process(candle);
		// ...
	}
}
```

## Recursos externos

### Use la infraestructura de StockSharp

En lugar de acceder directamente a recursos externos (archivos, bases de datos, red), use las capacidades proporcionadas por las plataformas StockSharp:

```cs
// Correcto: uso de mecanismos integrados para guardar datos
protected override void OnStopped()
{
	// Los datos se guardan automáticamente mediante parámetros de estrategia
	base.OnStopped();
}

// Incorrecto: acceso directo a recursos externos
protected override void OnStopped()
{
	// NO haga esto
	File.WriteAllText("results.txt", $"PnL: {PnL}");
	
	// ni esto
	using (var connection = new SqlConnection("..."))
	{
		// ...
	}
	
	base.OnStopped();
}
```

### Almacenamiento de datos

Para guardar resultados de estrategia, use:

- [Parámetros de estrategia](parameters.md) para ajustes y configuración
- Mecanismos de almacenamiento integrados en [Designer](../../designer.md) y [Shell](../../shell.md)
- [Estadísticas](xref:StockSharp.Algo.Statistics.StatisticManager) para recopilar métricas de trading

### Métodos Save y Load

Los métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) y [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) están diseñados específicamente para guardar datos adicionales de estrategia que no son ajustes ni parámetros. Este es el lugar ideal para guardar datos necesarios para restaurar el estado de la estrategia:

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings); // Primero guardar parámetros de estrategia
	
	// Luego guardar datos personalizados
	settings.SetValue("CustomState", _customState);
	settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings); // Primero cargar parámetros de estrategia
	
	// Luego cargar datos personalizados
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
	
	if (settings.Contains("LastSignalTime"))
		_lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

Sin embargo, los principales parámetros configurables deben seguir implementándose mediante [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1), como se describió anteriormente, ya que esto garantiza su visualización automática en la interfaz de usuario.

## Suscripción a datos de mercado

### Use reglas en lugar de suscripción directa

Para procesar datos de mercado, se recomienda usar el [Modelo de eventos](event_model.md) y reglas:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	var subscription = new Subscription(Series, Security);

	// Correcto: uso de reglas para procesar datos
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Subscribe(subscription);
}
```

Las reglas tienen varias ventajas importantes frente a los controladores de eventos habituales:

1. **Desuscripción automática** - las reglas se desuscriben automáticamente de eventos cuando la estrategia se detiene o cuando ya no son necesarias. No necesita gestionar suscripciones manualmente.

2. **API de alto nivel** - las reglas proporcionan una interfaz más comprensible y cómoda que los controladores de eventos estándar. Por ejemplo, `WhenCandlesFinished` es mucho más claro que suscribirse al evento `CandleReceived` y luego comprobar el estado de la vela.

3. **Combinación de condiciones** - las reglas se pueden combinar mediante operadores como `And`, `Or` y otros, creando condiciones de activación complejas:

```cs
// Ejemplo de combinación de reglas
var tickSub = new Subscription(DataType.Ticks, Security);

tickSub
	.WhenTickTradeReceived(this)
	.And(Portfolio.WhenChanged(Connector))
	.Do(() => {
		// Código que se ejecuta solo cuando hay una nueva operación
		// y cambia el saldo de la cartera
	})
	.Apply(this);

Subscribe(tickSub);
```

4. **Gestión del ciclo de vida** - las reglas pueden hacerse de una sola ejecución (`Once()`), tener condiciones de cancelación (`Until()`), agregar acciones retrasadas, etc.

## Ejemplo de estrategia compatible

A continuación se muestra un ejemplo de una estrategia que sigue todas las recomendaciones y funcionará correctamente en todas las plataformas StockSharp:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Long SMA length", string.Empty, "Base settings")
							.SetCanOptimize(true);
							
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetDisplay("Short SMA length", string.Empty, "Base settings")
							.SetCanOptimize(true);
							
		_series = Param(nameof(Series), TimeSpan.FromMinutes(15).TimeFrame())
					.SetDisplay("Series", string.Empty, "Base settings");
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);
		
		// Inicializar gráfico si está disponible
		_chart = GetChart();
		if (_chart != null)
			InitChart();
		
		var subscription = new Subscription(Series, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Subscribe(subscription);
	}

	private void InitChart()
	{
		_chart.ClearAreas();
		var area = _chart.AddArea();
		
		_chartCandleElement = area.AddCandles();
		
		_longSmaIndicatorElement = area.AddIndicator(_longSma);
		_longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
		_longSmaIndicatorElement.DrawStyle = DrawStyles.Line;
		
		_shortSmaIndicatorElement = area.AddIndicator(_shortSma);
		_shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
		_shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		var ls = _longSma.Process(candle);
		var ss = _shortSma.Process(candle);
		
		// Dibujar en el gráfico si está disponible
		if (_chart != null)
		{
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_longSmaIndicatorElement, ls)
				.Add(_shortSmaIndicatorElement, ss);
			_chart.Draw(data);
		}
		
		if (!_longSma.IsFormed)
			return;
			
		var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		if (isShortLessCurrent == isShortLessPrev)
			return;
			
		// Lógica de trading
		var volume = Volume + Math.Abs(Position);

		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}
}
```

## Ver también

- [Parámetros de estrategia](parameters.md)
- [Modelo de eventos](event_model.md)
- [Logging de estrategia](logging.md)
