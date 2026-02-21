# Writing a Strategy with AI

A step-by-step guide to creating a StockSharp trading strategy using AI tools.

## Preparation

### 1. Install an AI Tool

Choose one of the available tools:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (requires Node.js)
- **Cursor** — download from [cursor.com](https://cursor.com)
- **GitHub Copilot** — install the plugin for your IDE

### 2. Create a Project

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Prepare Context

Create a `CLAUDE.md` (or `.cursorrules`) file in the project root:

```markdown
# Project Rules

- Framework: StockSharp 5.x, .NET 10
- Strategies inherit from Strategy class
- Subscribe to candles via Connector.Subscribe(subscription)
- Register orders via RegisterOrder(order)
- Logging: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Indicators: create via new and call indicator.Process(candle)
- Always handle connector.Error and strategy errors
```

## Step-by-Step Example: SMA Strategy

### Step 1: Describe the Task to the AI

Example prompt:

```
Create a trading strategy using StockSharp that:
- Inherits from Strategy
- Uses two simple moving averages (SMA): fast (period 10) and slow (period 30)
- When the fast SMA crosses above the slow SMA — buy
- When the fast SMA crosses below the slow SMA — sell
- Position size: 1 lot
- Uses 5-minute candles
- Subscribes to candles in OnStarted()
- Processes candles via subscription rules
```

### Step 2: Review the Generated Code

The AI will generate something like:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _fastPeriod;
    private readonly StrategyParam<int> _slowPeriod;
    private readonly StrategyParam<DataType> _candleType;

    private SimpleMovingAverage _fastSma;
    private SimpleMovingAverage _slowSma;

    private bool _prevFastAbove;
    private bool _isFirstCandle = true;

    public SmaCrossStrategy()
    {
        _fastPeriod = Param(nameof(FastPeriod), 10);
        _slowPeriod = Param(nameof(SlowPeriod), 30);
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(5)));
    }

    public int FastPeriod
    {
        get => _fastPeriod.Value;
        set => _fastPeriod.Value = value;
    }

    public int SlowPeriod
    {
        get => _slowPeriod.Value;
        set => _slowPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _fastSma = new SimpleMovingAverage { Length = FastPeriod };
        _slowSma = new SimpleMovingAverage { Length = SlowPeriod };
        _isFirstCandle = true;

        var subscription = new Subscription(CandleType, Security);

        this
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var fastValue = _fastSma.Process(candle);
        var slowValue = _slowSma.Process(candle);

        if (!_fastSma.IsFormed || !_slowSma.IsFormed)
            return;

        var fastAbove = fastValue.GetValue<decimal>() > slowValue.GetValue<decimal>();

        if (!_isFirstCandle)
        {
            if (fastAbove && !_prevFastAbove)
            {
                // Fast SMA crossed above slow — buy
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // Fast SMA crossed below slow — sell
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### Step 3: What to Check

Go through this checklist:

- **Inheritance**: class inherits from `Strategy` ✓
- **Parameters**: uses `StrategyParam<T>` for optimization ✓
- **Candle subscription**: via `Subscribe(new Subscription(...))` ✓
- **Candle processing**: via `WhenCandlesFinished` rule ✓
- **IsFormed check**: indicators are checked for readiness ✓
- **Orders**: via `RegisterOrder()` with `BuyAtMarket` / `SellAtMarket` ✓
- **Position**: `Position` is checked before placing orders ✓

### Step 4: Ask the AI to Add Backtesting

```
Add backtesting code for this strategy using historical data.
Use HistoryEmulationConnector, load data from local storage,
and output summary statistics (PnL, trade count, max drawdown).
```

## Example Prompts

### Bollinger Bands Strategy

```
Create a StockSharp strategy that trades using Bollinger Bands:
- Buy when price touches the lower band
- Sell when price touches the upper band
- Period 20, multiplier 2.0
- Stop-loss: 1% from entry price
- Take-profit: 2% from entry price
- Use StrategyParam for all parameters
```

### Arbitrage Strategy

```
Create a pairs arbitrage strategy on StockSharp:
- Two instruments (specified via parameters)
- Calculate the spread between prices
- Enter when spread deviates by 2 standard deviations
- Exit when spread returns to the mean
- Volume neutralization (equal positions in monetary terms)
```

### Order Book Scalping

```
Create a scalping strategy on StockSharp:
- Subscribe to order book (MarketDepth) via Subscribe
- Analyze bid/ask imbalance
- Enter on strong imbalance (> 3:1)
- Quick exit on take-profit (5 ticks)
- Stop-loss: 3 ticks
- Maximum 1 position at a time
```

## Common AI Mistakes

### 1. Outdated Events

**Wrong** (old API):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Correct** (current API):
```csharp
// Use subscriptions
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Creating Orders Without Helpers

**Wrong**:
```csharp
var order = new Order
{
    Security = Security,
    Portfolio = Portfolio,
    Side = Sides.Buy,
    Type = OrderTypes.Market,
    Volume = 1,
};
```

**Correct** (using strategy helpers):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Missing IsFormed Check

**Wrong**:
```csharp
var value = _sma.Process(candle);
// Using value immediately — may not be ready
```

**Correct**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Tips

1. **Give the AI documentation** — point it to [doc.stocksharp.com](https://doc.stocksharp.com) or copy code examples from `Samples/`
2. **Use CLAUDE.md** — a project rules file greatly reduces the number of mistakes
3. **Start simple** — create a basic strategy first, then add filters and risk management
4. **Test on history** — always run a backtest before live trading
5. **Clone the repository** — if the AI has access to StockSharp sources, it will use the API more accurately
