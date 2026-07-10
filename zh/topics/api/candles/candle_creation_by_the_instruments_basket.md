# 为交易品种篮子创建K线

为 [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity)、[WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) 或 [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity) 创建K线时，使用的订阅机制与普通 [Security](xref:StockSharp.BusinessEntities.Security) 交易品种相同。

以下示例为 AAPL - MSFT 价差创建 1 分钟K线：

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

// 连接设置和连接器配置
private void ConfigureConnector()
{
	if (_connector.Configure(this))
	{
		_connector.Save().Serialize(_connectorFile);
	}
}

// 图表设置
private void SetupChart()
{
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	_candleElement = new ChartCandleElement();
	_area.Elements.Add(_candleElement);
	
	// 订阅 K线接收事件
	_connector.CandleReceived += OnCandleReceived;
}

// 服务注册
private void RegisterServices()
{
	ConfigManager.RegisterService<ISecurityProvider>(_connector);
	ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// 创建指数交易品种并订阅 K线
private void CreateIndexAndSubscribe()
{
	// 创建指数交易品种（价差）
	_indexInstr = new WeightedIndexSecurity() 
	{ 
		Board = ExchangeBoard.Nyse, 
		Id = "IndexInstr" 
	};
	
	// 添加带权重的交易品种（价差使用 1 和 -1）
	_indexInstr.Weights.Add(_instr1, 1);
	_indexInstr.Weights.Add(_instr2, -1);
	
	// 创建指数交易品种 K线订阅
	_indexSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),  // 1分钟K线
		_indexInstr)  // 我们的指数工具
	{
		MarketData = 
		{
			// 配置订阅以从 tick 构建 K线
			BuildMode = MarketDataBuildModes.Build,
			BuildFrom = DataType.Ticks,
			
			// 请求 30 天的历史数据
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};
	
	// 向图表添加元素并将其绑定到订阅
	_chart.AddElement(_area, _candleElement, _indexSubscription);
	
	// 启动订阅
	_connector.Subscribe(_indexSubscription);
}

// K线接收事件处理器
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否属于我们的订阅
	if (subscription != _indexSubscription)
		return;
	
	// 如有需要，仅处理已完成的 K线
	if (candle.State != CandleStates.Finished)
		return;
	
	// 在图表上绘制 K线
	var chartData = new ChartDrawData();
	chartData.Group(candle.OpenTime).Add(_candleElement, candle);
	
	this.GuiAsync(() => _chart.Draw(chartData));
}

// 关闭应用程序时取消订阅
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

### 使用成分交易品种K线创建指数K线订阅

```cs
// 创建订阅以从组件 K线构建指数 K线
var indexFromCandlesSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	_indexInstr)
{
	MarketData = 
	{
		// 配置订阅以从组件 K线构建
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// 启动订阅
_connector.Subscribe(indexFromCandlesSubscription);
```

### 使用市场深度创建指数K线订阅

```cs
// 创建订阅以从订单簿构建指数 K线
var indexFromDepthSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	_indexInstr)
{
	MarketData = 
	{
		// 配置订阅以从订单簿构建
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle,  // 使用价差中点
		From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

// 启动订阅
_connector.Subscribe(indexFromDepthSubscription);
```

### 使用波动率指数

```cs
// 基于表达式创建波动率指数
var volatilityIndex = new ExpressionIndexSecurity
{
	Board = ExchangeBoard.Nyse,
	Id = "VOLX",
	Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // 计算波动率的公式
};

// 将主交易品种添加到指数
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// 创建波动率指数 K线订阅
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

// 启动订阅
_connector.Subscribe(volatilitySubscription);
```

## 另请参阅

[连续期货](../instruments/continuous_futures.md)

[指数](../instruments/index.md)
