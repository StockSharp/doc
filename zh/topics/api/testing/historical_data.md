# 历史数据

在历史数据上进行测试可以让市场分析找到模式，并进行[策略参数优化](optimization.md)。主要工作由[HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector)类执行，该类通过特殊的[API](../market_data_storage/api.md)从本地存储库中检索数据。附加参数在[测试设置](extended_settings.md)部分中描述。

可以使用各种类型的市场数据进行测试：
- 逐笔交易 ([ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage))
- 订单簿 ([IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage))
- 不同时间框架的K线
- [订单日志](xref:StockSharp.Messages.IOrderLogMessage)
- [Level1](xref:StockSharp.Messages.Level1ChangeMessage)（最佳买价和卖价）
- 不同数据类型的组合

如果在测试期间没有保存的订单簿，可以使用 [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) 基于交易生成，或者使用 [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder) 从订单日志重建。

历史测试的数据必须事先以特殊的 [S#](../../api.md) 格式下载并保存。可以手动使用 [连接器](../connectors.md) 和 [Storage API](../market_data_storage/api.md) 完成，也可以通过配置并运行特殊的 [Hydra](../../hydra.md) 应用程序完成。

## 历史测试的主要阶段

### 1. 设置数据存储

第一步是创建一个 [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) 对象，[HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) 将通过它访问历史数据：

```csharp
// 用于访问历史数据的存储
var storageRegistry = new StorageRegistry
{
	// 设置历史数据目录路径
	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
};
```

> [!CAUTION]
> [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) 构造函数接收存储**所有品种**历史数据的根目录路径，而不是某个特定品种的目录。例如，如果 HistoryData.zip 压缩包解压到了 *C:\MarketData\AAPL@NASDAQ\* 目录下，那么你需要将路径 *C:\MarketData\* 传递给 [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive)。更多详情请参见 [API](../market_data_storage/api.md) 部分。

### 2. 创建工具和投资组合

```csharp
// 创建用于测试的测试工具
var security = new Security
{
	Id = SecId.Text, // ID of the instrument corresponds to the name of the folder with historical data
	Code = secCode,
	Board = board,
};

// 测试投资组合
var portfolio = new Portfolio
{
	Name = "test account",
	BeginValue = 1000000,
};
```

### 3. 创建仿真连接器

```csharp
// 创建仿真用连接器
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
				// 当历史价格触及限价单价格时撮合订单
				// 默认关闭；价格必须穿过限价单价格
// （更严格的测试模式）
				MatchOnTouch = false,
				
				// 成交手续费
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
		// 设置测试范围
		StartDate = startTime,
		StopDate = stopTime,
		OrderLogMarketDepthBuilders =
		{
			{ secId, new ItchOrderLogMarketDepthBuilder(secId) }
		}
	},
	// 设置市场时间更新间隔
	MarketTimeChangedInterval = timeFrame,
};
```

### 4. 订阅事件和配置数据生成

在连接时，我们根据测试参数设置接收必要的数据：

```csharp
connector.SecurityReceived += (subscr, s) =>
{
	if (s != security)
		return;
		
	// 填充 Level1 值
	connector.EmulationAdapter.SendInMessage(level1Info);
	
	// 根据测试设置订阅必要数据
	if (emulationInfo.UseMarketDepth)
	{
		connector.Subscribe(new(DataType.MarketDepth, security));
		
		// 如果需要生成订单簿
		if (generateDepths || emulationInfo.UseCandle != null)
		{
			// 如果策略需要但没有历史订单簿数据，
			// 使用基于最新价格的生成器
			connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
			{
				Interval = TimeSpan.FromSeconds(1), // 订单簿刷新频率 - 1 秒
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
	
	// 仿真开始前启动策略
	strategy.Start();
	
	// 开始加载历史数据
	connector.Start();
};
```

### 5. 创建和配置策略

```csharp
// 创建基于周期 80 和 10 移动平均线的交易策略
var strategy = new SmaStrategy
{
	LongSma = 80,
	ShortSma = 10,
	Volume = 1,
	Portfolio = portfolio,
	Security = security,
	Connector = connector,
	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
	// 默认间隔为 1 分钟，对于数月范围来说过于频繁
	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// 配置用于构建K线的数据类型
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

### 6. 可视化结果

为了以可视化方式显示测试结果，我们订阅了损益和持仓变动：

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

// 订阅进度更新
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. 开始测试

```csharp
// 启动仿真
connector.Connect();
```

## 历史测试的现代实现

在最新版本的[S#](../../api.md)中，历史测试示例已经得到显著现代化，现在允许使用各种类型的市场数据进行测试策略：

- 逐笔成交
- 订单簿
- 不同时间框架的K线
- 订单日志
- 一级数据（最佳价格）
- 不同数据类型的组合

为每种数据类型创建一个包含图表和统计信息的独立标签页：

```csharp
// 创建测试模式
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
		// 逐笔成交 + 订单簿
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
	
	// 其他数据类型组合
};
```

这种方法允许在使用不同数据源时对策略性能进行视觉比较。

## 改进的SMA策略

移动平均线（SMA）策略已经重新设计，现在使用更现代的数据订阅和K线处理方法：

```csharp
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 创建所需类型K线的订阅
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

	// 创建指标
	var longSma = new SMA { Length = LongSma };
	var shortSma = new SMA { Length = ShortSma };

	// 订阅K线并将其绑定到指标
	SubscribeCandles(subscription)
		.Bind(longSma, shortSma, OnProcess)
		.Start();

	// 配置图表显示
	var area = CreateChartArea();

	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
		DrawIndicator(area, longSma);
		DrawOwnTrades(area);
	}

	// 配置持仓保护
	StartProtection(TakeValue, StopValue);
}
```

K线的处理和交易决策现在被分离到一个专用的方法中：

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// 检查K线是否完成
	if (candle.State != CandleStates.Finished)
		return;

	// 分析指标交叉
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong) // 发生交叉
	{
		// short 小于 long 时卖出，否则买入
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// 计算开仓或反转持仓的数量
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		// 使用K线收盘价
		var price = candle.ClosePrice;

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		_isShortLessThenLong = isShortLessThenLong;
	}
}
```

## 附加测试设置

测试的扩展设置可在 [S#](../../api.md) 中使用，包括：

- 生成具有指定参数的订单簿
- 佣金设置
- 价格滑点设置
- 执行延迟模拟

这些设置在 [测试设置](extended_settings.md) 部分有更详细的描述。
