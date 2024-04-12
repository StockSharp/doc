# На истории

Тестирование на исторических данных позволяет проводить как анализ рынка для поиска закономерностей, так и [оптимизацию параметров стратегии](optimization.md). Вся работа при этом заключена в классе [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) (подробнее про [настройки тестирования](extended_settings.md)), который получает сохраненные в локальном хранилище данные через специальный [API](../market_data_storage/api.md). Тестирование идет по тиковым сделкам ([Trade](xref:StockSharp.BusinessEntities.Trade)) и стаканам ([MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth)). Если на период в истории нет сохраненных стаканов, то они генерируются на основе сделок с помощью [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator). 

Данные для тестирования на истории должны быть заранее скачаны и сохранены в специальном [S\#](../../api.md) формате. Это можно сделать самостоятельно, используя [Коннекторы](../connectors.md) и [Storage API](../market_data_storage/api.md), или настроить и запустить специальную программу [Hydra](../../hydra.md). 

В дистрибутиве [S\#](../../api.md) находится пример SampleHistoryTesting (а также архив HistoryData.zip, где лежат исторические данные по тикам, стаканам и свечам, для примера), который тестирует стратегию [Cкользящая Cредняя](https://ru.wikipedia.org/wiki/Скользящая_средняя) на истории. Для сравнения скорости и качества, тестирование идет с различным набором маркет\-данных: 

![samplehistorytest](../../../images/sample_history_test.png)

## Тестирование на истории стратегии скользящих средних из примеров SampleSMA и SampleSmartSMA

1. В начале необходимо создать настройки для тестирования: 

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
   		new EmulationInfo
   		{
   			UseCandleTimeFrame = timeFrame,
   			CustomHistoryAdapter = g => new FinamMessageAdapter(g),
   			CurveColor = Colors.DarkBlue,
   			StrategyName = LocalizedStrings.FinamCandles
   		},
   		FinamCandlesChart,
   		FinamCandlesEquity,
   		FinamCandlesPosition),
   	Tuple.Create(
   		YahooCandlesCheckBox,
   		YahooCandlesProgress,
   		YahooCandlesParameterGrid,
   		// candles
   		new EmulationInfo
   		{
   			UseCandleTimeFrame = timeFrame,
   			CustomHistoryAdapter = g => new YahooMessageAdapter(g),
   			CurveColor = Colors.DarkBlue,
   			StrategyName = LocalizedStrings.YahooCandles
   		},
   		YahooCandlesChart,
   		YahooCandlesEquity,
   		YahooCandlesPosition),
   };
   		
   ```
2. Далее, создать объект [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), через который [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) будет получать исторические данные: 

   ```cs
   // хранилище, через которое будет производиться доступ к тиковой и котировочной базе
   var storageRegistry = new StorageRegistry
   {
   	// set historical path
   	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
   };
   ```

   > [!CAUTION]
   > В конструктор [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) передается путь к директории, где лежит история для **всех инструментов**, а не к директории с конкретным инструментом. Например, если архив HistoryData.zip был распакован в директорию *C:\\R\\RIZ2@FORTS\\*, то в [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) необходимо передать путь *C:\\*. Подробнее, в разделе [API](../market_data_storage/api.md). 
3. Далее в цикле создаются инструмент, портфель, стратегия, шлюз для тестирования и т.д. с соответствующими настройками, в зависимости от значений флагов, установленных в главном окне (Тики, Тики и стаканы, Свечи и пр.). Если значение флага False, то программа переходит к следующему набору настроек. 

   ```cs
   foreach (var set in settings)
      {
        if (set.Item1.IsChecked == false)
            continue;
      .................
   }	
   		
   ```
4. Создаем инструменты и портфели, по которым будет производиться тестирование: 

   ```cs
   // создаем тестовый инструмент, на котором будет производится тестирование
   var security = new Security
   {
   	Id = SecId.Text, // sec id has the same name as folder with historical data
   	Code = secCode,
   	Board = board,
   };
   // тестовый портфель
   var portfolio = new Portfolio
   {
   	Name = "test account",
   	BeginValue = 1000000,
   };
   				
   ```
5. Создание самого [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector), куда передаются инструменты, портфели, интерфейс хранилища [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), а также настройки тестирования: 

   ```cs
   // создаем шлюз для эмуляции
   // инициализируем настройки (инструмент в истории обновляется раз в секунду)
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
   	UseExternalCandleSource = emulationInfo.UseCandleTimeFrame != null,
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
6. В событии получения нового инструмента задаем начальные значения Level1, регистрируем стакан или создаем и настраиваем генератор стакана. Также в зависимости от настроек регистрируем получение ордерлога и сделок. Запускаем стратегию и генерацию свечей. А также запускаем сам эмулятор. 

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
7. Подключение: 

   ```cs
   						_connector.Connect();
   					
   ```

   Для переданных в конструктор [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) инструментов и портфелей вызываются [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity) и [IPortfolioProvider.NewPortfolio](xref:StockSharp.BusinessEntities.IPortfolioProvider.NewPortfolio). 
8. Создание самой стратегии [Cкользящая Cредняя](https://ru.wikipedia.org/wiki/Скользящая_средняя): 

   ```cs
   // создаем торговую стратегию, скользящие средние на 80 5-минуток и 10 5-минуток
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
9. Подписка на событие [Strategy.PnLChanged](xref:StockSharp.Algo.Strategies.Strategy.PnLChanged), для расчета кривой эквити (подробнее, в разделе [Кривая эквити](../trading_algorithms/equity_curve.md)), а также визуальное наблюдение за прогрессом тестирования (в примере используются элементы в виде полос прогресса): 

   ```cs
   // копируем параметры на визуальную панель
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
10. Запуск начала тестирования: 

    ```cs
    // запускаем эмуляцию
    foreach (var connector in _connectors)
    {
    		connector.Connect();
    		// устанавливаем комиссию
    		connector.SendInMessage(new CommissionRuleMessage
    		{
    			Rule = new CommissionPerTradeRule { Value = 0.01m }
    		});
    }
    					 
    ```
