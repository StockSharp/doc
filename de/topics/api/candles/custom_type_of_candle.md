# Benutzerdefinierter Candle-Typ

[S#](../../api.md) ermöglicht die Erweiterung der Candle-Erstellungsfunktionen durch die Möglichkeit, mit benutzerdefinierten Candle-Typen zu arbeiten. Dies ist nützlich in Fällen, in denen Sie mit Candles arbeiten müssen, die derzeit nicht von [S#](../../api.md) unterstützt werden. Nachfolgend wird der Prozess der Erstellung eines eigenen Candle-Typs am Beispiel von Delta-Candles beschrieben (Candles, die auf der Grundlage der Differenz zwischen Kauf- und Verkaufsvolumen gebildet werden).

## Implementierung von Delta-Candles

1. Zunächst müssen Sie Ihren eigenen Candle-Message-Typ erstellen. Der Typ muss von der Klasse [CandleMessage](xref:StockSharp.Messages.CandleMessage) erben:

   ```cs
   /// <summary>
   /// Kerze, die auf Basis des Deltas von Kauf- und Verkaufsvolumen gebildet wird.
   /// </summary>
   public class DeltaCandleMessage : CandleMessage
   {
       // Nachrichtentypkennung aus dem Helper abrufen
       // um denselben Wert in RegisterCandleType zu verwenden

       /// <summary>
       /// Neue Instanz von <see cref="DeltaCandleMessage"/> initialisieren.
       /// </summary>
       public DeltaCandleMessage()
           : base(DeltaCandleHelper.DeltaCandleType)
       {
       }

       /// <summary>
       /// Delta-Schwellenwert für die Kerzenbildung.
       /// </summary>
       public decimal DeltaThreshold { get; set; }

       /// <summary>
       /// Aktueller Delta-Wert.
       /// </summary>
       public decimal CurrentDelta { get; set; }

       /// <summary>
       /// Kopie von <see cref="DeltaCandleMessage"/> erstellen.
       /// </summary>
       /// <returns>Kopie.</returns>
       public override Message Clone()
       {
           return CopyTo(new DeltaCandleMessage
           {
               DeltaThreshold = DeltaThreshold,
               CurrentDelta = CurrentDelta
           });
       }

       /// <summary>
       /// Kerzenparameter.
       /// </summary>
       public override object Arg
       {
           get => DeltaThreshold;
           set => DeltaThreshold = (decimal)value;
       }

       /// <summary>
       /// Typ des Kerzenarguments.
       /// </summary>
       public override Type ArgType => typeof(decimal);
   }
   ```

2. Dann müssen Sie Ihren eigenen Datentyp in der Klasse [DataType](xref:StockSharp.Messages.DataType) erstellen:

   ```cs
   public static class DeltaCandleHelper
   {
       /// <summary>
       /// Eindeutigen MessageType für Delta-Kerzen definieren.
       /// </summary>
       public const MessageTypes DeltaCandleType = (MessageTypes)10001;

       /// <summary>
       /// Datentyp <see cref="DeltaCandleMessage"/>.
       /// </summary>
       public static readonly DataType CandleDelta =
           DataType.Create(typeof(DeltaCandleMessage)).Immutable();

       /// <summary>
       /// Datentyp für Delta-Kerzen erstellen.
       /// </summary>
       /// <param name="threshold">Delta-Schwellenwert.</param>
       /// <returns>Datentyp.</returns>
       public static DataType Delta(this decimal threshold)
       {
           return DataType.Create(typeof(DeltaCandleMessage), threshold);
       }

       /// <summary>
       /// Delta-Kerzentyp im System registrieren.
       /// </summary>
       public static void RegisterDeltaCandleType()
       {
           // Neuen Kerzentyp in StockSharp registrieren
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

3. Als Nächstes müssen Sie einen Candle-Builder für den neuen Typ erstellen. Erstellen Sie dazu eine Implementierung von [CandleBuilder\<TCandleMessage\>](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1):

   ```cs
   /// <summary>
   /// Kerzen-Builder für den Typ <see cref="DeltaCandleMessage"/>.
   /// </summary>
   public class DeltaCandleBuilder : CandleBuilder<DeltaCandleMessage>
   {
       /// <summary>
       /// Initialisiert eine neue Instanz von <see cref="DeltaCandleBuilder"/>.
       /// </summary>
       /// <param name="exchangeInfoProvider">Börseninformationsprovider.</param>
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
           // Die Kerze wird geschlossen, wenn der absolute Delta-Wert den Schwellenwert überschreitet
           return Math.Abs(candle.CurrentDelta) >= candle.DeltaThreshold;
       }

       /// <inheritdoc />
       protected override void UpdateCandle(ICandleBuilderSubscription subscription, DeltaCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           base.UpdateCandle(subscription, candle, transform);

           // Delta anhand der Trade-Seite aktualisieren
           if (transform.Side == Sides.Buy)
               candle.CurrentDelta += transform.Volume ?? 0;
           else if (transform.Side == Sides.Sell)
               candle.CurrentDelta -= transform.Volume ?? 0;
       }
   }
   ```

4. Dann müssen Sie den Candle-Builder in [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider) registrieren:

   ```cs
   private Connector _connector;
   ...
   // Delta-Kerzentyp im System registrieren
   DeltaCandleHelper.RegisterDeltaCandleType();

   // Delta-Kerzen-Builder registrieren
   _connector.Adapter.CandleBuilderProvider.Register(new DeltaCandleBuilder(_connector.ExchangeInfoProvider));
   ```

5. Erstellen Sie ein Abonnement für Candles vom Typ `DeltaCandleMessage` und fordern Sie Daten dafür an:

   ```cs
   // Delta-Schwellenwert
   decimal deltaThreshold = 1000m;

   // Abonnement für Delta-Kerzen erstellen
   var subscription = new Subscription(
       // Unsere Erweiterungsmethode zum Erstellen eines Datentyps verwenden
       deltaThreshold.Delta(),
       security)
   {
       MarketData =
       {
           // Angeben, dass Kerzen aus Ticks erstellt werden
           BuildMode = MarketDataBuildModes.Build,
           BuildFrom = DataType.Ticks
       }
   };

   // Ereignis zum Empfang einer Kerze abonnieren
   _connector.CandleReceived += (sub, candle) =>
   {
       if (sub != subscription)
           return;

       var deltaCandle = (DeltaCandleMessage)candle;

       // Delta-Kerze verarbeiten
       Console.WriteLine($"Delta-candle {candle.OpenTime}: O:{candle.OpenPrice} H:{candle.HighPrice} " +
                        $"L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume} Delta:{deltaCandle.CurrentDelta}");
   };

   // Übergang in den Online-Modus abonnieren
   _connector.SubscriptionOnline += sub =>
   {
       if (sub == subscription)
           Console.WriteLine("Delta-Kerzen-Abonnement ist in den Online-Modus gewechselt");
   };

   // Abonnement starten
   _connector.Subscribe(subscription);
   ```

## Verwendung von Delta-Candles in Handelsstrategien

Beispiel einer einfachen Strategie mit Delta-Candles:

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
		// Strategieparameter
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

		// Diagramm initialisieren, falls verfügbar
		_chart = GetChart();
		if (_chart != null)
		{
			var area = _chart.AddArea();
			_chartCandleElement = area.AddCandles();
			_deltaIndicatorElement = area.AddIndicator();
			_deltaIndicatorElement.DrawStyle = DrawStyles.Histogram;
			_deltaIndicatorElement.Color = System.Drawing.Color.Purple;
		}

		// Abonnement für Delta-Kerzen erstellen
		var subscription = new Subscription(DeltaThreshold.Delta(), Security)
		{
			MarketData =
			{
				BuildMode = MarketDataBuildModes.Build,
				BuildFrom = DataType.Ticks
			}
		};

		// Regel zur Verarbeitung von Delta-Kerzen erstellen
		this
			.WhenCandleReceived(subscription)
			.Do(ProcessDeltaCandle)
			.Apply(this);

		// Abonnement starten
		Subscribe(subscription);
	}

	private void ProcessDeltaCandle(ICandleMessage candle)
	{
		// Im Diagramm zeichnen, falls verfügbar
		if (_chart != null)
		{
			var deltaCandle = (DeltaCandleMessage)candle;

			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_deltaIndicatorElement, deltaCandle.CurrentDelta);

			_chart.Draw(data);
		}

		// Nur abgeschlossene Kerzen verarbeiten
		if (candle.State != CandleStates.Finished)
			return;

		var deltaCandle = (DeltaCandleMessage)candle;

		// Prüfen, ob das Delta für ein Signal ausreicht
		if (Math.Abs(deltaCandle.CurrentDelta) < SignalDelta)
		{
			this.AddInfoLog($"Delta {deltaCandle.CurrentDelta} liegt unter dem Schwellenwert {SignalDelta}. Es wird kein Signal erzeugt.");
			return;
		}

		// Operationsrichtung hängt vom Vorzeichen des Deltas ab
		var direction = deltaCandle.CurrentDelta > 0 ? Sides.Buy : Sides.Sell;

		this.AddInfoLog($"Delta-Kerze abgeschlossen. Delta: {deltaCandle.CurrentDelta}. Richtung: {direction}");

		// Schlusskurs der Kerze zur Preisbestimmung verwenden
		var price = deltaCandle.ClosePrice;
		var volume = Volume;

		// Wenn bereits eine Position in Gegenrichtung vorhanden ist,
		// Volumen erhöhen, um die bestehende Position zu schließen
		if ((Position < 0 && direction == Sides.Buy) ||
			(Position > 0 && direction == Sides.Sell))
		{
			volume = Math.Max(volume, Math.Abs(Position) + volume);
		}

		// Order registrieren
		RegisterOrder(this.CreateOrder(direction, price, volume));
	}
}
```

## Wichtige Punkte bei der Erstellung benutzerdefinierter Candle-Typen

1. **Eindeutigkeit von MessageTypes** — stellen Sie sicher, dass der von Ihnen gewählte `MessageTypes`-Bezeichner nicht mit vorhandenen Typen in StockSharp in Konflikt steht. Es wird empfohlen, für benutzerdefinierte Typen Werte größer als 10000 zu verwenden.

2. **Registrierung des Candle-Typs** — die Registrierung über `Extensions.RegisterCandleType` ist für die korrekte Integration mit den grafischen Steuerelementen und dem Datenspeicher von StockSharp erforderlich. Ohne Registrierung funktioniert der Candle-Typ nur im Code, ist aber nicht in der Benutzeroberfläche verfügbar.

3. **Candle-Parameter** — implementieren Sie die Eigenschaft `ArgType`, die den Typ des Candle-Arguments zurückgibt. Dies wird für die korrekte Anzeige von Parametern in der grafischen Benutzeroberfläche verwendet.

4. **Dateisystem** — der Parameter `fileName` in der Methode `RegisterCandleType` wird verwendet, um Candles im Dateisystem zu speichern, wenn Sie den StockSharp-Datenspeicher verwenden.

5. **Parametervalidierung** — die Methode zur Parametervalidierung wird in StockSharp verwendet, um die Korrektheit der Werte vor der Erstellung eines Abonnements zu überprüfen.

Somit haben wir einen vollständig benutzerdefinierten Candle-Typ erstellt, der ordnungsgemäß in das gesamte StockSharp-Ökosystem (einschließlich der Benutzeroberfläche und des Datenspeichers) integriert ist und zur Erstellung von Handelsstrategien basierend auf der Analyse des Volumendeltas verwendet werden kann.
