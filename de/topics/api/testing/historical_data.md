# Historische Daten

Tests mit historischen Daten ermöglichen sowohl Marktanalysen zum Finden von Mustern als auch die [Optimierung von Strategieparametern](optimization.md). Die Hauptarbeit übernimmt die Klasse [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector), die Daten aus einem lokalen Repository über eine spezielle [API](../market_data_storage/api.md) abruft. Zusätzliche Parameter werden im Abschnitt [Testeinstellungen](extended_settings.md) beschrieben.

Tests können mit verschiedenen Arten von Marktdaten durchgeführt werden:
- Tick-Trades ([ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage))
- Orderbücher ([IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage))
- Kerzen verschiedener Zeitrahmen
- [OrderLog](xref:StockSharp.Messages.IOrderLogMessage)
- [Level1](xref:StockSharp.Messages.Level1ChangeMessage) (beste Geld- und Briefkurse)
- Kombinationen verschiedener Datentypen

Wenn für den Testzeitraum keine gespeicherten Orderbücher vorhanden sind, können sie auf Basis von Trades mit [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) generiert oder aus dem Orderprotokoll mit [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder) rekonstruiert werden.

Daten für historische Tests müssen im Voraus in einem speziellen [S#](../../api.md)-Format heruntergeladen und gespeichert werden. Dies kann manuell über [Konnektoren](../connectors.md) und die [Storage API](../market_data_storage/api.md) erfolgen oder durch Konfiguration und Start der speziellen Anwendung [Hydra](../../hydra.md).

## Hauptphasen des historischen Testens

### 1. Datenspeicher einrichten

Der erste Schritt besteht darin, ein [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry)-Objekt zu erstellen, über das [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) auf historische Daten zugreift:

```csharp
// Speicher für den Zugriff auf historische Daten
var storageRegistry = new StorageRegistry
{
	// Pfad zum Verzeichnis mit historischen Daten setzen
	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
};
```

> [!CAUTION]
> Der Konstruktor [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) erwartet den Pfad zum Stammverzeichnis, in dem die Historie für **alle Instrumente** gespeichert ist, nicht den Pfad zu einem Verzeichnis mit einem bestimmten Instrument. Wenn das Archiv HistoryData.zip beispielsweise in das Verzeichnis *C:\\MarketData\\AAPL@NASDAQ\\* entpackt wurde, müssen Sie den Pfad *C:\\MarketData\\* an [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) übergeben. Weitere Details finden Sie im Abschnitt [API](../market_data_storage/api.md).

### 2. Instrumente und Portfolios erstellen

```csharp
// Testinstrument für das Testen erstellen
var security = new Security
{
	Id = SecId.Text, // ID des Instruments entspricht dem Namen des Ordners mit historischen Daten
	Code = secCode,
	Board = board,
};

// Testportfolio
var portfolio = new Portfolio
{
	Name = "Testkonto",
	BeginValue = 1000000,
};
```

### 3. Emulationsconnector erstellen

```csharp
// Connector für die Emulation erstellen
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
				// Order matchen, wenn der historische Preis unseren Limit-Order-Preis berührt hat
				// Standardmäßig ist dies deaktiviert; der Preis muss den Limit-Order-Preis durchlaufen
				// (strengerer Testmodus)
				MatchOnTouch = false,

				// Kommission für Trades
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
		// Testbereich setzen
		StartDate = startTime,
		StopDate = stopTime,
		OrderLogMarketDepthBuilders =
		{
			{ secId, new ItchOrderLogMarketDepthBuilder(secId) }
		}
	},
	// Intervall für Marktzeit-Aktualisierungen setzen
	MarketTimeChangedInterval = timeFrame,
};
```

### 4. Ereignisse abonnieren und Datengenerierung konfigurieren

Beim Verbinden richten wir den Empfang der erforderlichen Daten abhängig von den Testparametern ein:

```csharp
connector.SecurityReceived += (subscr, s) =>
{
	if (s != security)
		return;

	// Level1-Werte füllen
	connector.EmulationAdapter.SendInMessage(level1Info);

	// Erforderliche Daten abhängig von den Testeinstellungen abonnieren
	if (emulationInfo.UseMarketDepth)
	{
		connector.Subscribe(new(DataType.MarketDepth, security));

		// Wenn Orderbücher generiert werden müssen
		if (generateDepths || emulationInfo.UseCandle != null)
		{
			// Wenn keine historischen Orderbuchdaten verfügbar sind, die Strategie sie aber benötigt,
			// Generator auf Basis der letzten Preise verwenden
			connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
			{
				Interval = TimeSpan.FromSeconds(1), // Aktualisierungsfrequenz des Orderbuchs - 1 Sek.
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

	// Strategie starten, bevor die Emulation beginnt
	strategy.Start();

	// Laden historischer Daten starten
	connector.Start();
};
```

### 5. Strategie erstellen und konfigurieren

```csharp
// Handelsstrategie auf Basis gleitender Durchschnitte mit Perioden 80 und 10 erstellen
var strategy = new SmaStrategy
{
	LongSma = 80,
	ShortSma = 10,
	Volume = 1,
	Portfolio = portfolio,
	Security = security,
	Connector = connector,
	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
	// Standardintervall ist 1 Min., was für einen Bereich von mehreren Monaten übermäßig ist
	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// Datentyp konfigurieren, der zum Erstellen von Kerzen verwendet wird
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

### 6. Ergebnisse visualisieren

Um Testergebnisse visuell anzuzeigen, abonnieren wir P&L- und Positionsänderungen:

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

// Fortschrittsaktualisierungen abonnieren
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. Test starten

```csharp
// Emulation starten
connector.Connect();
```

## Moderne Implementierung historischer Tests

In den aktuellen Versionen von [S#](../../api.md) wurde das Beispiel für historische Tests erheblich modernisiert. Es ermöglicht jetzt das Testen von Strategien mit verschiedenen Arten von Marktdaten:

- Ticks (Trades)
- Orderbücher
- Kerzen verschiedener Zeitrahmen
- Orderprotokoll
- Level1-Daten (beste Preise)
- Kombinationen verschiedener Datentypen

Für jeden Datentyp wird eine separate Registerkarte mit Charts und Statistiken erstellt:

```csharp
// Testmodi erstellen
_settings = new[]
{
	(
		TicksCheckBox,
		TicksProgress,
		TicksParameterGrid,
		// Ticks
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
		// Ticks + Orderbücher
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

	// andere Kombinationen von Datentypen
};
```

Dieser Ansatz ermöglicht einen visuellen Vergleich der Strategieleistung bei Verwendung unterschiedlicher Datenquellen.

## Verbesserte SMA-Strategie

Die Strategie mit gleitendem Durchschnitt (SMA) wurde überarbeitet und verwendet nun einen moderneren Ansatz für Datenabonnements und Kerzenverarbeitung:

```csharp
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Abonnement für Kerzen des erforderlichen Typs erstellen
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

	// Indikatoren erstellen
	var longSma = new SMA { Length = LongSma };
	var shortSma = new SMA { Length = ShortSma };

	// Kerzen abonnieren und an Indikatoren binden
	SubscribeCandles(subscription)
		.Bind(longSma, shortSma, OnProcess)
		.Start();

	// Anzeige im Chart konfigurieren
	var area = CreateChartArea();

	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
		DrawIndicator(area, longSma);
		DrawOwnTrades(area);
	}

	// Positionenschutz konfigurieren
	StartProtection(TakeValue, StopValue);
}
```

Die Kerzenverarbeitung und Handelsentscheidungen sind nun in eine eigene Methode ausgelagert:

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// Prüfen, ob die Kerze abgeschlossen ist
	if (candle.State != CandleStates.Finished)
		return;

	// Indikatorkreuzung analysieren
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong) // Kreuzung ist aufgetreten
	{
		// Wenn der kurze Wert kleiner als der lange ist - verkaufen, sonst kaufen
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// Volumen zum Öffnen der Position oder für die Umkehr berechnen
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		// Schlusskurs der Kerze verwenden
		var price = candle.ClosePrice;

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		_isShortLessThenLong = isShortLessThenLong;
	}
}
```

## Zusätzliche Testeinstellungen

Erweiterte Einstellungen für Tests sind in [S#](../../api.md) verfügbar, darunter:

- Generierung von Orderbüchern mit angegebenen Parametern
- Kommissionseinstellungen
- Preis-Slippage-Einstellungen
- Emulation von Ausführungsverzögerungen

Diese Einstellungen werden im Abschnitt [Testeinstellungen](extended_settings.md) ausführlicher beschrieben.
