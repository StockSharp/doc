# Datos históricos

Las pruebas con datos históricos permiten tanto analizar el mercado para encontrar patrones como realizar la [optimización de parámetros de estrategias](optimization.md). El trabajo principal lo realiza la clase [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector), que obtiene los datos almacenados en un repositorio local mediante una [API](../market_data_storage/api.md) especial. Los parámetros adicionales se describen en la sección [Configuración de pruebas](extended_settings.md).

Las pruebas pueden realizarse con distintos tipos de datos de mercado:
- Operaciones tick ([ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage))
- Libros de órdenes ([IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage))
- Velas de distintos marcos temporales
- [OrderLog](xref:StockSharp.Messages.IOrderLogMessage)
- [Level1](xref:StockSharp.Messages.Level1ChangeMessage) (mejores precios bid y ask)
- Combinaciones de distintos tipos de datos

Si no hay libros de órdenes guardados para el período de prueba, pueden generarse a partir de operaciones usando [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) o reconstruirse desde el registro de órdenes con [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder).

Los datos para pruebas históricas deben descargarse y guardarse previamente en un formato especial de [S#](../../api.md). Esto puede hacerse manualmente con [Conectores](../connectors.md) y la [Storage API](../market_data_storage/api.md), o configurando y ejecutando la aplicación especial [Hydra](../../hydra.md).

## Etapas principales de las pruebas históricas

### 1. Configurar el almacenamiento de datos

El primer paso es crear un objeto [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) mediante el cual [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) accederá a los datos históricos:

```csharp
// almacenamiento para acceder a datos históricos
var storageRegistry = new StorageRegistry
{
	// establecer la ruta al directorio con datos históricos
	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
};
```

> [!CAUTION]
> El constructor [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) recibe la ruta al directorio raíz donde se almacena el historial de **todos los instrumentos**, no a un directorio con un instrumento concreto. Por ejemplo, si el archivo HistoryData.zip se descomprimió en el directorio *C:\\MarketData\\AAPL@NASDAQ\\*, debe pasar la ruta *C:\\MarketData\\* a [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive). Más detalles en la sección [API](../market_data_storage/api.md).

### 2. Crear instrumentos y carteras

```csharp
// crear instrumento de prueba para las pruebas
var security = new Security
{
	Id = SecId.Text, // el ID del instrumento corresponde al nombre de la carpeta con datos históricos
	Code = secCode,
	Board = board,
};

// cartera de prueba
var portfolio = new Portfolio
{
	Name = "cuenta de prueba",
	BeginValue = 1000000,
};
```

### 3. Crear el conector de emulación

```csharp
// crear conector para la emulación
var connector = new HistoryEmulationConnector(
	new[] { security },
	new[] { portfolio })
{
	EmulationAdapter =
	{
		Emulator =
		{
			Settings =
			{
				// ejecutar la orden si el precio histórico tocó el precio de nuestra orden limitada
				// De forma predeterminada está desactivado; el precio debe atravesar el precio de la orden limitada
				// (modo de prueba más estricto)
				MatchOnTouch = false,

				// comisión para operaciones
				CommissionRules = new ICommissionRule[]
				{
					new CommissionPerTradeRule { Value = 0.01m },
				}
			}
		}
	},
	UseExternalCandleSource = emulationInfo.UseCandle != null,
	CreateDepthFromOrdersLog = emulationInfo.UseOrderLog,
	CreateTradesFromOrdersLog = emulationInfo.UseOrderLog,
	HistoryMessageAdapter =
	{
		StorageRegistry = storageRegistry,
		// establecer el rango de pruebas
		StartDate = startTime,
		StopDate = stopTime,
		OrderLogMarketDepthBuilders =
		{
			{ secId, new ItchOrderLogMarketDepthBuilder(secId) }
		}
	},
	// establecer el intervalo de actualización del tiempo de mercado
	MarketTimeChangedInterval = timeFrame,
};
```

### 4. Suscribirse a eventos y configurar la generación de datos

Al conectarnos, configuramos la recepción de los datos necesarios según los parámetros de prueba:

```csharp
connector.SecurityReceived += (subscr, s) =>
{
	if (s != security)
		return;

	// rellenar valores Level1
	connector.EmulationAdapter.SendInMessage(level1Info);

	// suscribirse a los datos necesarios según la configuración de pruebas
	if (emulationInfo.UseMarketDepth)
	{
		connector.Subscribe(new(DataType.MarketDepth, security));

		// si necesitamos generar libros de órdenes
		if (generateDepths || emulationInfo.UseCandle != null)
		{
			// si no hay datos históricos de libro de órdenes disponibles, pero la estrategia los requiere,
			// usar un generador basado en los últimos precios
			connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
			{
				Interval = TimeSpan.FromSeconds(1), // frecuencia de actualización del libro de órdenes: 1 s
				MaxAsksDepth = maxDepth,
				MaxBidsDepth = maxDepth,
				UseTradeVolume = true,
				MaxVolume = maxVolume,
				MinSpreadStepCount = 2,
				MaxSpreadStepCount = 5,
				MaxPriceStepCount = 3
			});
		}
	}

	if (emulationInfo.UseOrderLog)
	{
		connector.Subscribe(new(DataType.OrderLog, security));
	}

	if (emulationInfo.UseTicks)
	{
		connector.Subscribe(new(DataType.Ticks, security));
	}

	if (emulationInfo.UseLevel1)
	{
		connector.Subscribe(new(DataType.Level1, security));
	}

	// iniciar la estrategia antes de que comience la emulación
	strategy.Start();

	// iniciar la carga de datos históricos
	connector.Start();
};
```

### 5. Crear y configurar la estrategia

```csharp
// crear una estrategia de negociación basada en medias móviles con períodos 80 y 10
var strategy = new SmaStrategy
{
	LongSma = 80,
	ShortSma = 10,
	Volume = 1,
	Portfolio = portfolio,
	Security = security,
	Connector = connector,
	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
	// el intervalo predeterminado es 1 min, lo que es excesivo para un rango de varios meses
	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// configurar el tipo de datos usado para construir velas
if (emulationInfo.UseCandle != null)
{
	strategy.CandleType = emulationInfo.UseCandle;

	if (strategy.CandleType != TimeSpan.FromMinutes(1).TimeFrame())
	{
		strategy.BuildFrom = TimeSpan.FromMinutes(1).TimeFrame();
	}
}
else if (emulationInfo.UseTicks)
	strategy.BuildFrom = DataType.Ticks;
else if (emulationInfo.UseLevel1)
{
	strategy.BuildFrom = DataType.Level1;
	strategy.BuildField = emulationInfo.BuildField;
}
else if (emulationInfo.UseOrderLog)
	strategy.BuildFrom = DataType.OrderLog;
else if (emulationInfo.UseMarketDepth)
	strategy.BuildFrom = DataType.MarketDepth;
```

### 6. Visualizar resultados

Para mostrar visualmente los resultados de las pruebas, nos suscribimos a los cambios de P&L y posición:

```csharp
var pnlCurve = equity.CreateCurve(LocalizedStrings.PnL + " " + emulationInfo.StrategyName, Colors.Green, Colors.Red, DrawStyles.Area);
var realizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLRealized + " " + emulationInfo.StrategyName, Colors.Black, DrawStyles.Line);
var unrealizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLUnreal + " " + emulationInfo.StrategyName, Colors.DarkGray, DrawStyles.Line);
var commissionCurve = equity.CreateCurve(LocalizedStrings.Commission + " " + emulationInfo.StrategyName, Colors.Red, DrawStyles.DashedLine);

strategy.PnLReceived2 += (s, pf, t, r, u, c) =>
{
	var data = equity.CreateData();

	data
		.Group(t)
		.Add(pnlCurve, r - (c ?? 0))
		.Add(realizedPnLCurve, r)
		.Add(unrealizedPnLCurve, u ?? 0)
		.Add(commissionCurve, c ?? 0);

	equity.Draw(data);
};

var posItems = pos.CreateCurve(emulationInfo.StrategyName, emulationInfo.CurveColor, DrawStyles.Line);

strategy.PositionReceived += (s, p) =>
{
	var data = pos.CreateData();

	data
		.Group(p.LocalTime)
		.Add(posItems, p.CurrentValue);

	pos.Draw(data);
};

// suscribirse a las actualizaciones de progreso
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. Iniciar la prueba

```csharp
// iniciar la emulación
connector.Connect();
```

## Implementación moderna de pruebas históricas

En las versiones recientes de [S#](../../api.md), el ejemplo de pruebas históricas se ha modernizado considerablemente y ahora permite probar estrategias usando distintos tipos de datos de mercado:

- Ticks (operaciones)
- Libros de órdenes
- Velas de distintos marcos temporales
- Registro de órdenes
- Datos Level1 (mejores precios)
- Combinaciones de distintos tipos de datos

Se crea una pestaña independiente con gráficos y estadísticas para cada tipo de datos:

```csharp
// crear modos de prueba
_settings = new[]
{
	(
		TicksCheckBox,
		TicksProgress,
		TicksParameterGrid,
		// ticks
		new EmulationInfo
		{
			UseTicks = true,
			CurveColor = Colors.DarkGreen,
			StrategyName = LocalizedStrings.Ticks
		},
		TicksChart,
		TicksEquity,
		TicksPosition
	),

	(
		TicksAndDepthsCheckBox,
		TicksAndDepthsProgress,
		TicksAndDepthsParameterGrid,
		// ticks + libros de órdenes
		new EmulationInfo
		{
			UseTicks = true,
			UseMarketDepth = true,
			CurveColor = Colors.Red,
			StrategyName = LocalizedStrings.TicksAndDepths
		},
		TicksAndDepthsChart,
		TicksAndDepthsEquity,
		TicksAndDepthsPosition
	),

	// otras combinaciones de tipos de datos
};
```

Este enfoque permite comparar visualmente el rendimiento de la estrategia al usar distintas fuentes de datos.

## Estrategia SMA mejorada

La estrategia de media móvil (SMA) se ha rediseñado y ahora usa un enfoque más moderno para la suscripción a datos y el procesamiento de velas:

```csharp
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// crear una suscripción a velas del tipo requerido
	var dt = CandleTimeFrame is null
		? CandleType
		: DataType.Create(CandleType.MessageType, CandleTimeFrame);

	var subscription = new Subscription(dt, Security)
	{
		MarketData =
		{
			IsFinishedOnly = true,
			BuildFrom = BuildFrom,
			BuildMode = BuildFrom is null ? MarketDataBuildModes.LoadAndBuild : MarketDataBuildModes.Build,
			BuildField = BuildField,
		}
	};

	// crear indicadores
	var longSma = new SMA { Length = LongSma };
	var shortSma = new SMA { Length = ShortSma };

	// suscribirse a velas y enlazarlas con indicadores
	SubscribeCandles(subscription)
		.Bind(longSma, shortSma, OnProcess)
		.Start();

	// configurar la visualización en el gráfico
	var area = CreateChartArea();

	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
		DrawIndicator(area, longSma);
		DrawOwnTrades(area);
	}

	// configurar la protección de posición
	StartProtection(TakeValue, StopValue);
}
```

El procesamiento de velas y las decisiones de negociación ahora están separados en un método dedicado:

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// comprobar si la vela está completada
	if (candle.State != CandleStates.Finished)
		return;

	// analizar el cruce de indicadores
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong) // se produjo un cruce
	{
		// si la corta es menor que la larga, vender; en caso contrario, comprar
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// calcular el volumen para abrir una posición o revertirla
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		// usar el precio de cierre de la vela
		var price = candle.ClosePrice;

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		_isShortLessThenLong = isShortLessThenLong;
	}
}
```

## Configuración adicional de pruebas

En [S#](../../api.md) hay configuraciones ampliadas para pruebas, entre ellas:

- Generación de libros de órdenes con parámetros especificados
- Configuración de comisiones
- Configuración de deslizamiento de precio
- Emulación de retraso de ejecución

Estas configuraciones se describen con más detalle en la sección [Configuración de pruebas](extended_settings.md).
