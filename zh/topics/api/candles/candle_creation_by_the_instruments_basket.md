# 为证券篮子创建蜡烛

为 [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)、[WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) 或 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) 创建蜡烛时，使用的订阅机制与普通 [Security](xref:StockSharp.BusinessEntities.Security) 证券相同。

以下示例为 AAPL - MSFT 价差创建 1 分钟蜡烛：

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

// Connection setup and connector configuration
private void ConfigureConnector()
{
	if (_connector.Configure(this))
	{
		_connector.Save().Serialize(_connectorFile);
	}
}

// Chart setup
private void SetupChart()
{
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	_candleElement = new ChartCandleElement();
	_area.Elements.Add(_candleElement);
	
	// Subscribe to candle reception event
	_connector.CandleReceived += OnCandleReceived;
}

// Service registration
private void RegisterServices()
{
	ConfigManager.RegisterService<ISecurityProvider>(_connector);
	ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// Creating index instrument and subscribing to candles
private void CreateIndexAndSubscribe()
{
	// Create index instrument (spread)
	_indexInstr = new WeightedIndexSecurity() 
	{ 
		Board = ExchangeBoard.Nyse, 
		Id = "IndexInstr" 
	};
	
	// Add instruments with weights (1 and -1 for spread)
	_indexInstr.Weights.Add(_instr1, 1);
	_indexInstr.Weights.Add(_instr2, -1);
	
	// Create subscription to index instrument candles
	_indexSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),  // 1-minute candles
		_indexInstr)  // Our index instrument
	{
		MarketData = 
		{
			// Configure subscription to build candles from ticks
			BuildMode = MarketDataBuildModes.Build,
			BuildFrom = DataType.Ticks,
			
			// Request historical data for 30 days
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};
	
	// Add element to chart and bind it to subscription
	_chart.AddElement(_area, _candleElement, _indexSubscription);
	
	// Start subscription
	_connector.Subscribe(_indexSubscription);
}

// Handler for candle reception event
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check if the candle belongs to our subscription
	if (subscription != _indexSubscription)
		return;
	
	// If needed, limit processing to only completed candles
	if (candle.State != CandleStates.Finished)
		return;
	
	// Draw the candle on the chart
	var chartData = new ChartDrawData();
	chartData.Group(candle.OpenTime).Add(_candleElement, candle);
	
	this.GuiAsync(() => _chart.Draw(chartData));
}

// Unsubscribe when closing the application
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

## 指数订阅的其他使用场景

### 使用成分证券蜡烛创建指数蜡烛订阅

```cs
// Create subscription to build index candles from component candles
var indexFromCandlesSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	_indexInstr)
{
	MarketData = 
	{
		// Configure subscription to build from component candles
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Start subscription
_connector.Subscribe(indexFromCandlesSubscription);
```

### 使用市场深度创建指数蜡烛订阅

```cs
// Create subscription to build index candles from order books
var indexFromDepthSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	_indexInstr)
{
	MarketData = 
	{
		// Configure subscription to build from order books
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle,  // Use middle of spread
		From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

// Start subscription
_connector.Subscribe(indexFromDepthSubscription);
```

### 使用波动率指数

```cs
// Create volatility index based on expression
var volatilityIndex = new ExpressionIndexSecurity
{
	Board = ExchangeBoard.Nyse,
	Id = "VOLX",
	Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // Formula for calculating volatility
};

// Add main instrument to index
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// Create subscription to volatility index candles
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

// Start subscription
_connector.Subscribe(volatilitySubscription);
```

## 另请参阅

[连续期货](../instruments/continuous_futures.md)

[指数](../instruments/index.md)
