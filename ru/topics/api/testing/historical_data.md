# На истории

Тестирование на исторических данных позволяет проводить как анализ рынка для поиска закономерностей, так и [оптимизацию параметров стратегии](optimization.md). Основную работу при этом выполняет класс [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector), который получает сохраненные в локальном хранилище данные через специальный [API](../market_data_storage/api.md). Дополнительные параметры описаны в разделе [настройки тестирования](extended_settings.md).

Тестирование может выполняться по различным типам маркет-данных:
- Тиковые сделки ([ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage))
- Стаканы ([IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage))
- Свечи разных таймфреймов
- [OrderLog](xref:StockSharp.Messages.IOrderLogMessage) (лог заявок)
- [Level1](xref:StockSharp.Messages.Level1ChangeMessage) (лучшие цены спроса и предложения)
- Комбинации различных типов данных

Если на период тестирования отсутствуют сохраненные стаканы, они могут быть сгенерированы на основе сделок с помощью [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) или восстановлены из ордерлога с помощью [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder).

Данные для тестирования на истории должны быть заранее скачаны и сохранены в специальном [S#](../../api.md) формате. Это можно сделать самостоятельно, используя [Коннекторы](../connectors.md) и [Storage API](../market_data_storage/api.md), или настроить и запустить специальную программу [Hydra](../../hydra.md).

## Основные этапы тестирования на истории

### 1. Настройка хранилища данных

Первым шагом необходимо создать объект [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), через который [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) будет получать исторические данные:

```csharp
// хранилище, через которое будет производиться доступ к историческим данным
var storageRegistry = new StorageRegistry
{
	// устанавливаем путь к директории с историческими данными
	DefaultDrive = new LocalMarketDataDrive(Paths.FileSystem, HistoryPath.Folder)
};
```

> [!CAUTION]
> В конструктор [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) передается путь к корневой директории, где хранится история для **всех инструментов**, а не к директории с конкретным инструментом. Например, если архив HistoryData.zip был распакован в директорию *C:\\R\\RIZ2@FORTS\\*, то в [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) необходимо передать путь *C:\\*. Подробнее, в разделе [API](../market_data_storage/api.md).

### 2. Создание инструментов и портфелей

```csharp
// создаем тестовый инструмент, на котором будет производиться тестирование
var security = new Security
{
	Id = SecId.Text, // ID инструмента соответствует имени папки с историческими данными
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

### 3. Создание эмуляционного коннектора

```csharp
// создаем коннектор для эмуляции
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
				// исполнение заявки, если историческая цена коснулась цены заявки
				// По умолчанию включено (оптимистичный сценарий)
				// Для более строгого тестирования можно выключить
				MatchOnTouch = false,

				// комиссия для сделок
				CommissionRules = new ICommissionRule[]
				{
					new CommissionTradeRule { Value = 0.01m },
				}
			}
		}
	},
	HistoryMessageAdapter =
	{
		StorageRegistry = storageRegistry,
		// установка диапазона тестирования
		StartDate = startTime,
		StopDate = stopTime,
		OrderLogMarketDepthBuilders =
		{
			{
				secId,
				new OrderLogMarketDepthBuilder(secId)
			}
		}
	},
	// установка интервала обновления времени рынка
	MarketTimeChangedInterval = timeFrame,
};
```

### 4. Подписка на события и настройка генерации данных

При подключении настраиваем получение нужных данных в зависимости от параметров тестирования:

```csharp
connector.SecurityReceived += (subscr, s) =>
{
	if (s != security)
		return;
		
	// заполняем значения Level1
	await connector.EmulationAdapter.SendInMessageAsync(level1Info);
	
	// подписываемся на нужные данные в зависимости от настроек тестирования
	if (emulationInfo.UseMarketDepth)
	{
		connector.Subscribe(new(DataType.MarketDepth, security));
		
		// если нужно генерировать стаканы
		if (generateDepths || emulationInfo.UseCandle != null)
		{
			// если нет исторических данных стаканов, но они требуются стратегии,
			// используем генератор на основе последних цен
			connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
			{
				Interval = TimeSpan.FromSeconds(1), // частота обновления стакана - 1 сек
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
	
	// запускаем стратегию перед началом эмуляции
	strategy.Start();
	
	// запускаем загрузку исторических данных
	await connector.StartAsync();
};
```

### 5. Создание и настройка стратегии

```csharp
// создаем торговую стратегию на основе скользящих средних с периодами 80 и 10
var strategy = new SmaStrategy
{
	LongSma = 80,
	ShortSma = 10,
	Volume = 1,
	Portfolio = portfolio,
	Security = security,
	Connector = connector,
	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
	// по умолчанию интервал 1 мин, что избыточно для диапазона в несколько месяцев
	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// настраиваем тип используемых данных для построения свечей
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

### 6. Визуализация результатов

Для наглядного отображения результатов тестирования подписываемся на изменения P&L и позиции:

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

// подписываемся на обновление прогресса
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. Запуск тестирования

```csharp
// запускаем эмуляцию
connector.Connect();
```

## Современная реализация тестирования на истории

В последних версиях [S#](../../api.md) пример тестирования на истории был существенно модернизирован и теперь позволяет тестировать стратегию с использованием различных типов маркет-данных:

- Тики (сделки)
- Стаканы (книга заявок)
- Свечи различных таймфреймов
- Ордерлог (лог заявок)
- Level1 данные (лучшие цены)
- Комбинации разных типов данных

Для каждого типа данных создается отдельная вкладка с графиками и статистикой:

```csharp
// создаем режимы тестирования
_settings = new[]
{
	(
		TicksCheckBox,
		TicksProgress,
		TicksParameterGrid,
		// тики
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
		// тики + стаканы
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
	
	// другие комбинации типов данных
};
```

Такой подход позволяет наглядно сравнивать результаты работы стратегии при использовании разных источников данных.

## Улучшенная стратегия SMA

Стратегия скользящего среднего (SMA) была переработана и теперь использует более современный подход к подписке на данные и обработке свечей:

```csharp
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// создаем подписку на свечи нужного типа
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

	// создаем индикаторы
	var longSma = new SMA { Length = LongSma };
	var shortSma = new SMA { Length = ShortSma };

	// подписываемся на свечи и связываем их с индикаторами
	SubscribeCandles(subscription)
		.Bind(longSma, shortSma, OnProcess)
		.Start();

	// настраиваем отображение на графике
	var area = CreateChartArea();

	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
		DrawIndicator(area, longSma);
		DrawOwnTrades(area);
	}

	// настраиваем защиту позиций
	StartProtection(TakeValue, StopValue);
}
```

Обработка свечей и принятие торговых решений теперь выделено в отдельный метод:

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// проверяем, что свеча завершена
	if (candle.State != CandleStates.Finished)
		return;

	// анализируем пересечение индикаторов
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong) // произошло пересечение
	{
		// если короткая меньше длинной - продаем, иначе покупаем
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// рассчитываем объем для открытия позиции или разворота
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		// используем цену закрытия свечи
		var price = candle.ClosePrice;

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		_isShortLessThenLong = isShortLessThenLong;
	}
}
```

## Дополнительные настройки тестирования

В [S#](../../api.md) доступны расширенные настройки для тестирования, включая:

- Генерация стаканов с заданными параметрами
- Настройка комиссий
- Настройка проскальзывания цен
- Эмуляция задержек исполнения

Более подробно эти настройки описаны в разделе [Настройки тестирования](extended_settings.md).