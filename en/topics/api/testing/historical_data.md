# Historical Data

Testing on historical data allows both market analysis to find patterns and [strategy parameter optimization](optimization.md). The main work is performed by the [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) class, which retrieves data stored in a local repository through a special [API](../market_data_storage/api.md). Additional parameters are described in the [testing settings](extended_settings.md) section.

Testing can be performed using various types of market data:
- Tick trades ([Trade](xref:StockSharp.BusinessEntities.Trade))
- Order books ([MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth))
- Candles of different timeframes
- Order log
- Level1 (best bid and ask prices)
- Combinations of different data types

If there are no saved order books for the testing period, they can be generated based on trades using [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) or reconstructed from the order log using [OrderLogMarketDepthBuilder](xref:StockSharp.Algo.Testing.OrderLogMarketDepthBuilder).

Data for historical testing must be downloaded and saved in a special [S#](../../api.md) format in advance. This can be done manually using [Connectors](../connectors.md) and [Storage API](../market_data_storage/api.md), or by configuring and running the special [Hydra](../../hydra.md) application.

## Main stages of historical testing

### 1. Setting up the data storage

The first step is to create an [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) object through which [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) will access historical data:

```csharp
// storage for accessing historical data
var storageRegistry = new StorageRegistry
{
    // set path to directory with historical data
    DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
};
```

> [!CAUTION]
> The [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) constructor takes the path to the root directory where history for **all instruments** is stored, not to a directory with a specific instrument. For example, if the HistoryData.zip archive was unpacked into the *C:\\R\\RIZ2@FORTS\\* directory, then you need to pass the path *C:\\* to [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive). More details in the [API](../market_data_storage/api.md) section.

### 2. Creating instruments and portfolios

```csharp
// create test instrument for testing
var security = new Security
{
    Id = SecId.Text, // ID of the instrument corresponds to the name of the folder with historical data
    Code = secCode,
    Board = board,
};

// test portfolio
var portfolio = new Portfolio
{
    Name = "test account",
    BeginValue = 1000000,
};
```

### 3. Creating the emulation connector

```csharp
// create connector for emulation
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
                // match order if historical price touched our limit order price
                // By default it's turned off, price should go through the limit order price
                // (more strict testing mode)
                MatchOnTouch = false,
                
                // commission for trades
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
        // set testing range
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
    // set market time update interval
    MarketTimeChangedInterval = timeFrame,
};
```

### 4. Subscribing to events and configuring data generation

When connecting, we set up receiving the necessary data depending on the testing parameters:

```csharp
connector.SecurityReceived += (subscr, s) =>
{
    if (s != security)
        return;
        
    // fill Level1 values
    connector.EmulationAdapter.SendInMessage(level1Info);
    
    // subscribe to necessary data depending on testing settings
    if (emulationInfo.UseMarketDepth)
    {
        connector.Subscribe(new(DataType.MarketDepth, security));
        
        // if we need to generate order books
        if (generateDepths || emulationInfo.UseCandle != null)
        {
            // if no historical order book data is available but required by the strategy,
            // use generator based on last prices
            connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
            {
                Interval = TimeSpan.FromSeconds(1), // order book refresh frequency - 1 sec
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
    
    // start strategy before emulation begins
    strategy.Start();
    
    // start loading historical data
    connector.Start();
};
```

### 5. Creating and configuring the strategy

```csharp
// create trading strategy based on moving averages with periods 80 and 10
var strategy = new SmaStrategy
{
    LongSma = 80,
    ShortSma = 10,
    Volume = 1,
    Portfolio = portfolio,
    Security = security,
    Connector = connector,
    LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
    // default interval is 1 min, which is excessive for a range of several months
    UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// configure the type of data used to build candles
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

### 6. Visualizing results

To display testing results visually, we subscribe to P&L and position changes:

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

// subscribe to progress updates
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. Starting the test

```csharp
// start emulation
connector.Connect();
```

## Modern implementation of historical testing

In the latest versions of [S#](../../api.md), the historical testing example has been significantly modernized and now allows testing strategies using various types of market data:

- Ticks (trades)
- Order books
- Candles of different timeframes
- Order log
- Level1 data (best prices)
- Combinations of different data types

A separate tab with charts and statistics is created for each data type:

```csharp
// create testing modes
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
        // ticks + order books
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
    
    // other combinations of data types
};
```

This approach allows for visual comparison of strategy performance when using different data sources.

## Improved SMA Strategy

The Moving Average (SMA) strategy has been redesigned and now uses a more modern approach to data subscription and candle processing:

```csharp
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);

    // create subscription to candles of the required type
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

    // create indicators
    var longSma = new SMA { Length = LongSma };
    var shortSma = new SMA { Length = ShortSma };

    // subscribe to candles and bind them to indicators
    SubscribeCandles(subscription)
        .Bind(longSma, shortSma, OnProcess)
        .Start();

    // configure display on the chart
    var area = CreateChartArea();

    if (area != null)
    {
        DrawCandles(area, subscription);
        DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
        DrawIndicator(area, longSma);
        DrawOwnTrades(area);
    }

    // configure position protection
    StartProtection(TakeValue, StopValue);
}
```

Candle processing and trading decisions are now separated into a dedicated method:

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
    LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

    // check if the candle is completed
    if (candle.State != CandleStates.Finished)
        return;

    // analyze indicator crossover
    var isShortLessThenLong = shortValue < longValue;

    if (_isShortLessThenLong == null)
    {
        _isShortLessThenLong = isShortLessThenLong;
    }
    else if (_isShortLessThenLong != isShortLessThenLong) // crossover occurred
    {
        // if short is less than long - sell, otherwise buy
        var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

        // calculate volume for opening position or reversal
        var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

        // use the candle's close price
        var price = candle.ClosePrice;

        if (direction == Sides.Buy)
            BuyLimit(price, volume);
        else
            SellLimit(price, volume);

        _isShortLessThenLong = isShortLessThenLong;
    }
}
```

## Additional testing settings

Extended settings for testing are available in [S#](../../api.md), including:

- Generation of order books with specified parameters
- Commission settings
- Price slippage settings
- Execution delay emulation

These settings are described in more detail in the [Testing Settings](extended_settings.md) section.