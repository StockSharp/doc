# Logging in Strategy

In StockSharp, the [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class inherits from [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), which allows you to use built-in tools to log all actions and events occurring during the operation of a trading strategy.

## Logging Levels

StockSharp supports the following logging levels (listed in order of increasing importance):

1. Verbose - the most detailed logging level for tracing
2. Debug - messages for debugging
3. Info - regular informational messages
4. Warning - warnings about potential problems
5. Error - error messages

## Logging Methods in Strategy

The strategy provides the following methods for writing messages to the log:

### LogVerbose

The [LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) method is designed to record detailed messages for tracing:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    LogVerbose("Strategy started with parameters: Long SMA={0}, Short SMA={1}", LongSmaLength, ShortSmaLength);
    
    // ...
}
```

### LogDebug

The [LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) method is used for debug messages:

```cs
private void ProcessCandle(ICandleMessage candle)
{
    LogDebug("Processing candle: {0}, Open={1}, Close={2}, High={3}, Low={4}, Volume={5}", 
        candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);
    
    // ...
}
```

### LogInfo

The [LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) method is used for regular informational messages:

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

The [LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) method is used to record warnings:

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

The [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) method is used to record error messages:

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

There is also an overload [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception)) that directly accepts an exception:

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

## Configuring the Logging Level

The [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class contains a [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel) property that determines which messages will be written to the log:

```cs
// Set the logging level for the strategy
strategy.LogLevel = LogLevels.Info;
```

With the selected logging level, only messages of that level and higher levels will be recorded. For example, if `LogLevels.Info` is set, Verbose and Debug messages will be ignored.

## LogLevel Parameter

For convenient logging level configuration in the strategy constructor, you can add a parameter:

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

## Examples of Use in a Real Strategy

### Logging Strategy Start and Stop

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
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

### Logging Trades

```cs
protected override void OnNewMyTrade(MyTrade trade)
{
    LogInfo("{0} {1} {2} at price {3}. Volume: {4}", 
        trade.Order.Direction == Sides.Buy ? "Bought" : "Sold",
        trade.Order.Security.Code,
        trade.Order.Type,
        trade.Trade.Price,
        trade.Trade.Volume);
    
    base.OnNewMyTrade(trade);
}
```

### Logging Order Registration Errors

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
    LogError("Order registration error {0}: {1}", 
        fail.Order.TransactionId, fail.Error.Message);
    
    base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## Viewing Logs

Messages written to the strategy log can be viewed:

1. In the [Designer](../../designer.md) program on the "Logs" panel
2. In log files, if [FileLogListener](xref:Ecng.Logging.FileLogListener) is configured
3. In the user interface through [LogControl](xref:StockSharp.Xaml.LogControl), if [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) is used

## See also

[Logging](../logging.md)
[LogControl Component](../graphical_user_interface/logging/log_panel.md)
