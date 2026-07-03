# 策略日志记录

在 StockSharp 中，[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类继承自 [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver)，这允许您使用内置工具记录交易策略运行期间发生的所有操作和事件。

## 日志级别

StockSharp 支持以下日志级别（按重要性递增顺序列出）：

1. 详细 - 用于追踪的最详细的日志记录级别
2. 调试 - 用于调试的消息
3. 信息 - 常规信息消息
4. 警告 - 关于潜在问题的警告
5. 错误 - 错误消息

## 策略中的日志记录方法

该策略提供了以下将消息写入日志的方法：

### 详细日志

[LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) 方法旨在记录用于跟踪的详细消息：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	LogVerbose("Strategy started with parameters: Long SMA={0}, Short SMA={1}", LongSmaLength, ShortSmaLength);
	
	// ...
}
```

### LogDebug

[LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) 方法用于调试消息：

```cs
private void ProcessCandle(ICandleMessage candle)
{
	LogDebug("Processing candle: {0}, Open={1}, Close={2}, High={3}, Low={4}, Volume={5}", 
		candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);
	
	// ...
}
```

### LogInfo

[LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) 方法用于常规信息消息：

```cs
private void CalculateSignal(decimal shortSma, decimal longSma)
{
	bool isShortGreaterThanLong = shortSma > longSma;
	
	LogInfo("Signal: {0}, Short SMA={1}, Long SMA={2}", 
		isShortGreaterThanLong ? "Buy" : "Sell", shortSma, longSma);
	
	// ...
}
```

### LogWarning

[LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) 方法用于记录警告：

```cs
public void RegisterOrder(Order order)
{
	if (order.Volume <= 0)
	{
		LogWarning("Attempt to register an order with invalid volume: {0}", order.Volume);
		return;
	}
	
	// ...
}
```

### LogError

[LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) 方法用于记录错误消息：

```cs
try
{
	// Some actions
}
catch (Exception ex)
{
	LogError("Error while performing operation: {0}", ex.Message);
	Stop();
}
```

还有一个重载 [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception))，它可以直接接受一个异常：

```cs
try
{
	// Some actions
}
catch (Exception ex)
{
	LogError(ex);
	Stop();
}
```

## 配置日志级别

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类包含一个 [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel) 属性，该属性决定哪些消息将被写入日志：

```cs
// Set the logging level for the strategy
strategy.LogLevel = LogLevels.Info;
```

使用所选的日志记录级别，只有该级别及更高级别的消息会被记录。例如，如果设置为 `LogLevels.Info`，则 Verbose 和 Debug 消息将被忽略。

## 日志级别参数

为了在策略构造函数中方便地配置日志级别，你可以添加一个参数：

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<LogLevels> _logLevel;
	
	public SmaStrategy()
	{
		_logLevel = Param(nameof(LogLevel), LogLevels.Info)
					.SetDisplay("Logging Level", "Level of log message detail", "Logging Settings");
	}
	
	public override LogLevels LogLevel
	{
		get => _logLevel.Value;
		set => _logLevel.Value = value;
	}
	
	// ...
}
```

## 在实际策略中的使用示例

### 日志策略启动和停止

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	LogInfo("Strategy {0} started at {1}. Instrument: {2}, Portfolio: {3}", 
		Name, time, Security?.Code, Portfolio?.Name);
	
	// ...
}

protected override void OnStopped()
{
	LogInfo("Strategy {0} stopped. Position: {1}, P&L: {2}", 
		Name, Position, PnL);
	
	base.OnStopped();
}
```

### 记录交易

```cs
protected override void OnOwnTradeReceived(MyTrade trade)
{
	LogInfo("{0} {1} {2} at price {3}. Volume: {4}",
		trade.Order.Direction == Sides.Buy ? "Bought" : "Sold",
		trade.Order.Security.Code,
		trade.Order.Type,
		trade.Trade.Price,
		trade.Trade.Volume);

	base.OnOwnTradeReceived(trade);
}
```

### 记录订单注册错误

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
	LogError("Order registration error {0}: {1}", 
		fail.Order.TransactionId, fail.Error.Message);
	
	base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## 连接日志监听器

要接收策略发出的消息，需要通过 [LogManager](xref:Ecng.Logging.LogManager) 连接监听器：

```cs
// Create log manager
var logManager = new LogManager();

// Add log listener to console
logManager.Listeners.Add(new ConsoleLogListener());

// Add strategy to log sources
logManager.Sources.Add(strategy);
```

## 查看日志

写入策略日志的消息可以查看：

1. 在“日志”面板的[Designer](../../designer.md)程序中
2. 在日志文件中，如果配置了 [FileLogListener](xref:Ecng.Logging.FileLogListener)
3. 在用户界面通过 [LogControl](xref:StockSharp.Xaml.LogControl)，如果使用 [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener)

## 另请参阅

[日志记录](../logging.md)
[日志控制组件](../graphical_user_interface/logging/log_panel.md)
