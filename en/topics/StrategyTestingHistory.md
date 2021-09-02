# Historical data

Testing on historical data allows to carry out both a market analysis to find patterns and the [strategy parameters optimization](StrategyTestingOptimization.md). At that all work is performed within the [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) class (for more details, see [Extended settings](TestingSettings.md)), which receives the data stored in the local storage through the special [API](StoragesApi.md). Testing is carried out on the [candles](Candles.md), the tick trades ([Trade](xref:StockSharp.BusinessEntities.Trade)) and the order books ([MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth)). If there are no saved order books within a period of history, they are generated based on trades by using [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator). 

The data for backtesting must be pre\-downloaded and stored in a special [S\#](StockSharpAbout.md) format. This can be done on one's own using [Connectors](API_Connectors.md) and [Storage API](StoragesApi.md), or to set up and run the special [S\#.Data](Hydra.md) app. 

The [S\#](StockSharpAbout.md) installation package contains an example of SampleHistoryTesting (as well as the HistoryData.zip archive, where are the historical data on ticks, order books and candles, for example) which tests the [Moving Average](https://en.wikipedia.org/wiki/Moving_average#Simple_moving_average) strategy on the history. Testing is carried out with a different sets of market data for a comparison of the speed and quality: 

![samplehistorytest](../images/sample_history_test.png)

## Backtesting of moving averages strategy

1. At the beginning it is necessary to create the settings for the testing: 

   ```cs
   var settings = new[]
   {
   	Tuple.Create(
   		TicksCheckBox,
   		TicksProgress,
   		TicksParameterGrid,
   		// ticks
   		new EmulationInfo {UseTicks = true, CurveColor = Colors.DarkGreen, StrategyName = LocalizedStrings.Ticks},
   		TicksChart,
   		TicksEquity,
   		TicksPosition),
   	Tuple.Create(
   		TicksAndDepthsCheckBox,
   		TicksAndDepthsProgress,
   		TicksAndDepthsParameterGrid,
   		// ticks + order book
   		new EmulationInfo {UseTicks = true, UseMarketDepth = true, CurveColor = Colors.Red, StrategyName = LocalizedStrings.XamlStr757},
   		TicksAndDepthsChart,
   		TicksAndDepthsEquity,
   		TicksAndDepthsPosition),
   	Tuple.Create(
   		DepthsCheckBox,
   		DepthsProgress,
   		DepthsParameterGrid,
   		// order book
   		new EmulationInfo {UseMarketDepth = true, CurveColor = Colors.OrangeRed, StrategyName = LocalizedStrings.MarketDepths},
   		DepthsChart,
   		DepthsEquity,
   		DepthsPosition),
   	Tuple.Create(
   		CandlesCheckBox,
   		CandlesProgress,
   		CandlesParameterGrid,
   		// candles
   		new EmulationInfo {UseCandleTimeFrame = timeFrame, CurveColor = Colors.DarkBlue, StrategyName = LocalizedStrings.Candles},
   		CandlesChart,
   		CandlesEquity,
   		CandlesPosition),
   	
   	Tuple.Create(
   		CandlesAndDepthsCheckBox,
   		CandlesAndDepthsProgress,
   		CandlesAndDepthsParameterGrid,
   		// candles + orderbook
   		new EmulationInfo {UseMarketDepth = true, UseCandleTimeFrame = timeFrame, CurveColor = Colors.Cyan, StrategyName = LocalizedStrings.XamlStr635},
   		CandlesAndDepthsChart,
   		CandlesAndDepthsEquity,
   		CandlesAndDepthsPosition),
   	Tuple.Create(
   		OrderLogCheckBox,
   		OrderLogProgress,
   		OrderLogParameterGrid,
   		// order log
   		new EmulationInfo {UseOrderLog = true, CurveColor = Colors.CornflowerBlue, StrategyName = LocalizedStrings.OrderLog},
   		OrderLogChart,
   		OrderLogEquity,
   		OrderLogPosition),
   	Tuple.Create(
   		Level1CheckBox,
   		Level1Progress,
   		Level1ParameterGrid,
   		// order log
   		new EmulationInfo {UseLevel1 = true, CurveColor = Colors.Aquamarine, StrategyName = LocalizedStrings.Level1},
   		Level1Chart,
   		Level1Equity,
   		Level1Position),
   	Tuple.Create(
   		FinamCandlesCheckBox,
   		FinamCandlesProgress,
   		FinamCandlesParameterGrid,
   		// candles
   		new EmulationInfo {UseCandleTimeFrame = timeFrame, HistorySource = d => _finamHistorySource.GetCandles(security, timeFrame, d.Date, d.Date), CurveColor = Colors.DarkBlue, StrategyName = LocalizedStrings.FinamCandles},
   		FinamCandlesChart,
   		FinamCandlesEquity,
   		FinamCandlesPosition),
   	Tuple.Create(
   		YahooCandlesCheckBox,
   		YahooCandlesProgress,
   		YahooCandlesParameterGrid,
   		// candles
   		new EmulationInfo {UseCandleTimeFrame = timeFrame, HistorySource = d => new YahooHistorySource(_exchangeInfoProvider).GetCandles(security, timeFrame, d.Date, d.Date), CurveColor = Colors.DarkBlue, StrategyName = LocalizedStrings.YahooCandles},
   		YahooCandlesChart,
   		YahooCandlesEquity,
   		YahooCandlesPosition),
   };
   		
   ```
2. Next, to create the [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) object, through which [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) will get historical data: 

   ```cs
   // storage to historical data
   var storageRegistry = new StorageRegistry
   {
   	// set historical path
   	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
   };
   ```

   > [!CAUTION]
   > The path to the directory with the history is passed to the [StockSharp.Algo.Storages.LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) constructor. This path is to the directory with the history for **all instruments**, and not to the directory with the specific instrument. For example, if the HistoryData.zip archive was unpacked to the *C:\\E\\ESZ2@NYSE\\* directory, then the path *C:\\* should be passed to [StockSharp.Algo.Storages.LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive). For more details, see [API](StoragesApi.md). 
3. Next, the instrument, the portfolio, the strategy, the gateway for testing, etc. are created in the cycle with the appropriate settings, depending on the flags values specified in the main window (Ticks, Ticks and Order Books, Candles, etc.). If the flag is set to False, the program proceeds to the next set of settings 

   ```cs
   foreach (var set in settings)
   {
   	if (set.Item1.IsChecked == false)
   		continue;
      .................
   }	
   		
   ```
4. Creating instruments and portfolios, by which the testing will be carried out: 

   ```cs
   var security = new Security
   {
   	Id = SecId.Text, // sec id has the same name as folder with historical data
   	Code = secCode,
   	Board = board,
   };
   var portfolio = new Portfolio
   {
   	Name = "test account",
   	BeginValue = 1000000,
   };
   				
   ```
5. Creation of [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) itself, to which instruments, portfolios. [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) storage interface, and testing settings are passed: 

   ```cs
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
   				// match order if historical price touched our limit order price. 
   				// It is terned off, and price should go through limit order price level
   				// (more "severe" test mode)
   				MatchOnTouch = false,
   			}
   		}
   	},
   	//UseExternalCandleSource = emulationInfo.UseCandleTimeFrame != null,
   	CreateDepthFromOrdersLog = emulationInfo.UseOrderLog,
   	CreateTradesFromOrdersLog = emulationInfo.UseOrderLog,
   	HistoryMessageAdapter =
   	{
   		StorageRegistry = storageRegistry,
   		// set history range
   		StartDate = startTime,
   		StopDate = stopTime,
   		OrderLogMarketDepthBuilders =
   		{
   			{
   				secId,
   				LocalizedStrings.ActiveLanguage == Languages.Russian
   					? (IOrderLogMarketDepthBuilder)new PlazaOrderLogMarketDepthBuilder(secId)
   					: new ItchOrderLogMarketDepthBuilder(secId)
   			}
   		}
   	},
   	// set market time freq as time frame
   	MarketTimeChangedInterval = timeFrame,
   };
   ```
6. In the new instrument getting event we specify the Level1 initial values, register the order book or create and set up the order book generator. Also, depending on the settings, we register the order log and trades receiving. Starting the strategy and the candles generating. As well as starting the emulator itself. 

   ```cs
   connector.NewSecurity += s =>
   {
   	if (s != security)
   		return;
   	// fill level1 values
   	connector.HistoryMessageAdapter.SendOutMessage(level1Info);
   	if (emulationInfo.HistorySource != null)
   	{
   		if (emulationInfo.UseCandleTimeFrame != null)
   		{
   			connector.RegisterHistorySource(security, MarketDataTypes.CandleTimeFrame, emulationInfo.UseCandleTimeFrame.Value, emulationInfo.HistorySource);
   		}
   		if (emulationInfo.UseTicks)
   		{
   			connector.RegisterHistorySource(security, MarketDataTypes.Trades, null, emulationInfo.HistorySource);
   		}
   		if (emulationInfo.UseLevel1)
   		{
   			connector.RegisterHistorySource(security, MarketDataTypes.Level1, null, emulationInfo.HistorySource);
   		}
   		if (emulationInfo.UseMarketDepth)
   		{
   			connector.RegisterHistorySource(security, MarketDataTypes.MarketDepth, null, emulationInfo.HistorySource);
   		}
   	}
   	else
   	{
   		if (emulationInfo.UseMarketDepth)
   		{
   			connector.SubscribeMarketDepth(security);
   			if (
   				// if order book will be generated
   					generateDepths ||
   				// of backtesting will be on candles
   					emulationInfo.UseCandleTimeFrame != TimeSpan.Zero
   				)
   			{
   				// if no have order book historical data, but strategy is required,
   				// use generator based on last prices
   				connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
   				{
   					IsSubscribe = true,
   					Generator = new RandomWalkTradeGenerator(new SecurityId { SecurityCode = security.Code })
   					{
   						Interval = TimeSpan.FromSeconds(1),
   						MaxVolume = maxVolume,
   						MaxPriceStepCount = 3,	
   						GenerateOriginSide = true,
   						MinVolume = minVolume,
   						RandomArrayLength = 99,
   					}
   				});
   			}
   		}
   		if (emulationInfo.UseOrderLog)
   		{
   			connector.SubscribeOrderLog(security);
   		}
   		if (emulationInfo.UseTicks)
   		{
   			connector.SubscribeTrades(security);
   		}
   		if (emulationInfo.UseLevel1)
   		{
   			connector.SubscribeLevel1(security);
   		}
   	}
   	// start strategy before emulation started
   	strategy.Start();
   	_series = new CandleSeries(typeof(TimeFrameCandle), security, timeFrame);
   	connector.SubscribeCandles(series);
   	// start historical data loading when connection established successfully and all data subscribed
   	connector.Start();
   };
   ```
7. Connection: 

   ```cs
   						_connector.Connect();
   					
   ```

   [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity) and [IPortfolioProvider.NewPortfolio](xref:StockSharp.BusinessEntities.IPortfolioProvider.NewPortfolio) are called for instruments and portfolios passed to the [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) constructor. 
8. Creation of the [Moving Average](https://en.wikipedia.org/wiki/Moving_average#Simple_moving_average) strategy itself: 

   ```cs
   // create strategy based on 80 5-min and 10 5-min
   var strategy = new SmaStrategy(chart, _candlesElem, _tradesElem, _shortMa, _shortElem, _longMa, _longElem, _series)
   {
   	Volume = 1,
   	Portfolio = portfolio,
   	Security = security,
   	Connector = connector,
   	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
   	// by default interval is 1 min,
   	// it is excessively for time range with several months
   	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
   };
   ```
9. Subscription to the [PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged) event, to calculate the equity curve (for more details, see [Equity curve](Equity.md)), as well as the visual observation over the testing progress (the elements in the form of progress bar are used in this example): 

   ```cs
   // fill parameters panel
   statistic.Parameters.Clear();
   statistic.Parameters.AddRange(strategy.StatisticManager.Parameters);
   var equity = set.Item6;
   var pnlCurve = equity.CreateCurve(LocalizedStrings.PnL + " " + emulationInfo.StrategyName, emulationInfo.CurveColor, ChartIndicatorDrawStyles.Area);
   var unrealizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLUnreal + emulationInfo.StrategyName, Colors.Black, ChartIndicatorDrawStyles.Line);
   var commissionCurve = equity.CreateCurve(LocalizedStrings.Str159 + " " + emulationInfo.StrategyName, Colors.Red, ChartIndicatorDrawStyles.DashedLine);
   var posItems = set.Item7.CreateCurve(emulationInfo.StrategyName, emulationInfo.CurveColor, ChartIndicatorDrawStyles.Line);
   strategy.PnLChanged += () =>
   {
   	var pnl = new EquityData
   	{
   		Time = strategy.CurrentTime,
   		Value = strategy.PnL - strategy.Commission ?? 0
   	};
   	var unrealizedPnL = new EquityData
   	{
   		Time = strategy.CurrentTime,
   		Value = strategy.PnLManager.UnrealizedPnL ?? 0
   	};
   	var commission = new EquityData
   	{
   		Time = strategy.CurrentTime,
   		Value = strategy.Commission ?? 0
   	};
   	pnlCurve.Add(pnl);
   	unrealizedPnLCurve.Add(unrealizedPnL);
   	commissionCurve.Add(commission);
   };
   strategy.PositionChanged += () => posItems.Add(new EquityData { Time = strategy.CurrentTime, Value = strategy.Position });
   var nextTime = startTime + progressStep;
   // handle historical time for update ProgressBar
   connector.MarketTimeChanged += d =>
   {
   	if (connector.CurrentTime < nextTime && connector.CurrentTime < stopTime)
   		return;
   	var steps = (connector.CurrentTime - startTime).Ticks / progressStep.Ticks + 1;
   	nextTime = startTime + (steps * progressStep.Ticks).To<TimeSpan>();
   	this.GuiAsync(() => progressBar.Value = steps);
   };
   					
   ```
10. Starting the beginning of the testing: 

    ```cs
    // start emulation
    foreach (var connector in _connectors)
    {
    	// raise NewSecurity and NewPortfolio for full fill strategy properties
    	connector.Connect();
    	// 1 cent commission for trade
    		connector.SendInMessage(new CommissionRuleMessage
    		{
    			Rule = new CommissionPerTradeRule { Value = 0.01m }
    		});
    }
    					 
    ```
