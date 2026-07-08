# Tipo de vela personalizado

[S#](../../api.md) le permite ampliar las capacidades de construcción de velas al proporcionar la posibilidad de trabajar con tipos de velas personalizados. Esto es útil en los casos en que necesita trabajar con velas que actualmente no son compatibles con [S#](../../api.md). A continuación se muestra el proceso de creación de su propio tipo de vela utilizando el ejemplo de las velas Delta (velas formadas en función de la diferencia entre los volúmenes de compra y venta).

## Implementación de las velas Delta

1. Primero, debe crear su propio tipo de mensaje de vela. El tipo debe heredar de la clase [CandleMessage](xref:StockSharp.Messages.CandleMessage):

   ```cs
   /// <summary>
   /// Vela formada a partir de la delta de volúmenes de compra y venta.
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
       // Obtenemos el identificador del tipo de mensaje desde el helper
       // para usar el mismo valor en RegisterCandleType

       /// <summary>
       /// Inicializar una nueva instancia de <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }

       /// <summary>
       /// Valor umbral de delta para formar la vela.
       /// </summary>
       public decimal DeltaThreshold { get; set; }

       /// <summary>
       /// Valor actual de delta.
       /// </summary>
       public decimal CurrentDelta { get; set; }

       /// <summary>
       /// Crear una copia de <see cref="DeltaCandleMessage"/>.
       /// </summary>
       /// <returns>Copia.</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }

       /// <summary>
       /// Parámetro de la vela.
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }

       /// <summary>
       /// Tipo del argumento de la vela.
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. Luego debe crear su propio tipo de datos en la clase [DataType](xref:StockSharp.Messages.DataType):

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// Definir un MessageType único para velas delta.
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;

       /// <summary>
       /// Tipo de datos <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public static readonly DataType CandleDelta =
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();

       /// <summary>
       /// Crear un tipo de datos para velas delta.
       /// </summary>
       /// <param name="threshold">Valor umbral de delta.</param>
       /// <returns>Tipo de datos.</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }

       /// <summary>
       /// Registrar el tipo de vela delta en el sistema.
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // Registrar nuevo tipo de vela en StockSharp
           Extensions.RegisterCandleType<decimal>(
               typeof(DeltaCandleMessage),      // Candle message type
               DeltaCandleType,                // Message type
               "delta",                        // File name for storage
               str => str.To<decimal>(),       // Converter from string to parameter
               arg => arg.ToString(),          // Converter from parameter to string
               a => a > 0,                     // Parameter validator
               false                           // Whether such candles can be obtained from the source (not only built)
           );
       }
   }
   ```

3. A continuación, debe crear un constructor de velas para el nuevo tipo. Para ello, cree una implementación de [CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1):

   ```cs
   /// <summary>
   /// Constructor de velas para el tipo <see cref="DeltaCandleMessage"/>.
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// Inicializa una nueva instancia de <see cref="DeltaCandleBuilder"/>.
       /// </summary>
       /// <param name="exchangeInfoProvider">Proveedor de información del exchange.</param>
       public DeltaCandleBuilder(IExchangeInfoProvider exchangeInfoProvider)
           : base(exchangeInfoProvider)
       {
       }

       /// <inheritdoc />
       protected override DeltaCandleMessage CreateCandle(ICandleBuilderSubscription subscription, ICandleBuilderValueTransform transform)
       {
           var time = transform.Time;

           return FirstInitCandle(subscription, new DeltaCandleMessage
           {
               DeltaThreshold = subscription.Message.GetArg<decimal>(),
               OpenTime = time,
               CloseTime = time,
               HighTime = time,
               LowTime = time,
               CurrentDelta = 0
           }, transform);
       }

       /// <inheritdoc />
       protected override bool IsCandleFinishedBeforeChange(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           // La vela se cierra cuando el valor absoluto de delta supera el umbral
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }

       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);

           // Actualizar delta según el lado de la operación
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. Luego debe registrar el constructor de velas en [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider):

   ```cs
   private Connector _connector;
   ...
   // Registrar tipo de vela delta en el sistema
   DeltaCandleHelper.RegisterDeltaCandleType();

   // Registrar constructor de velas delta
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. Cree una suscripción para las velas de tipo `DeltaCandleMessage` y solicite datos a partir de ella:

   ```cs
   // Valor umbral de delta
   decimal deltaThreshold = 1000m;

   // Crear suscripción a velas delta
   var subscription = new Subscription(
       // Usar nuestro método de extensión para crear un tipo de datos
       deltaThreshold.Delta(),
       security)
   {
       MarketData =
       {
           // Indicar que las velas se construirán desde ticks
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };

   // Suscribirse al evento de vela recibida
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;

       var deltaCandle = (DeltaCandleMessage)candle;

       // Procesar vela delta
       Console.WriteLine($"Delta-candle {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };

   // Suscribirse a la transición al modo online
   _connector.SubscriptionOnline += sub =>
   {
       if (sub == subscription)
           Console.WriteLine("Delta-candle subscription has transitioned to online mode");
   };

   // Iniciar la suscripción
   _connector.Subscribe(subscription);
   ```

## Uso de las velas Delta en estrategias de trading

Ejemplo de una estrategia simple que utiliza velas delta:

```cs
public class DeltaCandleStrategy : Strategy
{
	private readonly StrategyParam<decimal> _deltaThreshold;
	private readonly StrategyParam<decimal> _volume;
	private readonly StrategyParam<decimal> _signalDelta;

	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _deltaIndicatorElement;

	public decimal DeltaThreshold
	{
		get => _deltaThreshold.Value;
		set => _deltaThreshold.Value = value;
	}

	public decimal Volume
	{
		get => _volume.Value;
		set => _volume.Value = value;
	}

	public decimal SignalDelta
	{
		get => _signalDelta.Value;
		set => _signalDelta.Value = value;
	}

	public DeltaCandleStrategy()
	{
		// Parámetros de la estrategia
		_deltaThreshold = Param(nameof(DeltaThreshold), 1000m)
			.SetDisplay("Delta Threshold Value", "Volume delta value for candle formation", "Main Settings")
			.SetGreaterThanZero()
			.SetCanOptimize(true)
			.SetOptimize(500m, 2000m, 100m);

		_volume = Param(nameof(Volume), 1m)
			.SetDisplay("Order Volume", "Volume for trading operations", "Main Settings")
			.SetGreaterThanZero();

		_signalDelta = Param(nameof(SignalDelta), 500m)
			.SetDisplay("Minimum Delta for Signal", "Minimum delta value for signal generation", "Main Settings")
			.SetGreaterThanZero()
			.SetCanOptimize(true);

		Name = "DeltaCandleStrategy";
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Inicialización del gráfico si está disponible
		_chart = GetChart();
		if (_chart != null)
		{
			var area = _chart.AddArea();
			_chartCandleElement = area.AddCandles();
			_deltaIndicatorElement = area.AddIndicator();
			_deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
			_deltaIndicatorElement.Color = System.Drawing.Color.Purple;
		}

		// Crear una suscripción a velas delta
		var subscription = new Subscription(DeltaThreshold.Delta(), Security)
		{
			MarketData =
			{
				BuildMode = MarketDataBuildModes.Build,
				BuildFrom = DataType.Ticks
			}
		};

		// Crear una regla para procesar velas delta
		this
			.WhenCandleReceived(subscription)
			.Do(ProcessDeltaCandle)
			.Apply(this);

		// Iniciar la suscripción
		Subscribe(subscription);
	}

	private void ProcessDeltaCandle(ICandleMessage candle)
	{
		// Dibujar en el gráfico si está disponible
		if (_chart != null)
		{
			var deltaCandle = (DeltaCandleMessage)candle;

			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);

			_chart.Draw(data);
		}

		// Procesar solo velas finalizadas
		if (candle.State != CandleStates.Finished)
			return;

		var deltaCandle = (DeltaCandleMessage)candle;

		// Comprobar si delta es suficiente para una señal
		if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
		{
			this.AddInfoLog($"Delta {deltaCandle.CurrentDelta} is less than the threshold value {SignalDelta}. No signal is generated.");
			return;
		}

		// La dirección de la operación depende del signo de delta
		var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;

		this.AddInfoLog($"Delta-candle completed. Delta: {deltaCandle.CurrentDelta}. Direction: {direction}");

		// Usar el precio de cierre de la vela para determinar el precio
		var price = deltaCandle.ClosePrice;
		var volume = Volume;

		// Si ya tenemos una posición en la dirección opuesta,
		// aumentar el volumen para cerrar la posición existente
		if ((Position < 0 && direction == Sides.Buy) ||
			(Position > 0 && direction == Sides.Sell))
		{
			volume = Math.Max(volume, Math.Abs(Position) + volume);
		}

		// Registrar una orden
		RegisterOrder(this.CreateOrder(direction, price, volume));
	}
}
```

## Puntos importantes al crear tipos de velas personalizados

1. **Unicidad de MessageTypes**: asegúrese de que el identificador `MessageTypes` que elija no entre en conflicto con los tipos existentes en StockSharp. Se recomienda usar valores superiores a 10000 para tipos personalizados.

2. **Registro del tipo de vela**: el registro a través de `Extensions.RegisterCandleType` es necesario para una correcta integración con los controles gráficos y el almacenamiento de datos de StockSharp. Sin el registro, el tipo de vela solo funcionará en el código, pero no estará disponible en la interfaz de usuario.

3. **Parámetro de la vela**: implemente la propiedad `ArgType` que devuelve el tipo del argumento de la vela. Esto se utiliza para la correcta visualización de los parámetros en la interfaz gráfica.

4. **Sistema de archivos**: el parámetro `fileName` en el método `RegisterCandleType` se utiliza para guardar las velas en el sistema de archivos cuando se usa el almacenamiento de datos de StockSharp.

5. **Validación de parámetros**: el método de validación de parámetros se utiliza en StockSharp para comprobar la corrección de los valores antes de crear una suscripción.

Así, hemos creado un tipo de vela completamente personalizado que se integra correctamente con todo el ecosistema de StockSharp (incluida la interfaz de usuario y el almacenamiento de datos) y que se puede utilizar para construir estrategias de trading basadas en el análisis del delta de volumen.
