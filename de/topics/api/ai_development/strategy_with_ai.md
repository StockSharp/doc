# Eine Strategie mit KI schreiben

Eine Schritt-für-Schritt-Anleitung zur Erstellung einer StockSharp-Handelsstrategie mithilfe von KI-Tools.

## Vorbereitung

### 1. Ein KI-Tool installieren

Wählen Sie eines der verfügbaren Tools:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (erfordert Node.js)
- **Cursor** — Download von [cursor.com](https://cursor.com)
- **GitHub Copilot** — installieren Sie das Plugin für Ihre IDE

### 2. Ein Projekt erstellen

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Kontext vorbereiten

Erstellen Sie eine Datei `CLAUDE.md` (oder `.cursorrules`) im Projekt-Root:

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

## Schritt-für-Schritt-Beispiel: SMA-Strategie

### Schritt 1: Die Aufgabe der KI beschreiben

Beispiel-Prompt:

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

### Schritt 2: Den generierten Code überprüfen

Die KI generiert etwa Folgendes:

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

### Schritt 3: Was zu prüfen ist

Gehen Sie diese Checkliste durch:

- **Vererbung**: Klasse erbt von `Strategy` ✓
- **Parameter**: verwendet `StrategyParam<T>` für die Optimierung ✓
- **Candle-Abonnement**: über `Subscribe(new Subscription(...))` ✓
- **Candle-Verarbeitung**: über die Regel `WhenCandlesFinished` ✓
- **IsFormed-Prüfung**: Indikatoren werden auf Bereitschaft geprüft ✓
- **Orders**: über `RegisterOrder()` mit `BuyAtMarket` / `SellAtMarket` ✓
- **Position**: `Position` wird vor dem Platzieren von Orders geprüft ✓

### Schritt 4: Die KI bitten, Backtesting hinzuzufügen

```
Add backtesting code for this strategy using historical data.
Use HistoryEmulationConnector, load data from local storage,
and output summary statistics (PnL, trade count, max drawdown).
```

## Beispiel-Prompts

### Bollinger-Bands-Strategie

```
Create a StockSharp strategy that trades using Bollinger Bands:
- Buy when price touches the lower band
- Sell when price touches the upper band
- Period 20, multiplier 2.0
- Stop-loss: 1% from entry price
- Take-profit: 2% from entry price
- Use StrategyParam for all parameters
```

### Arbitrage-Strategie

```
Create a pairs arbitrage strategy on StockSharp:
- Two instruments (specified via parameters)
- Calculate the spread between prices
- Enter when spread deviates by 2 standard deviations
- Exit when spread returns to the mean
- Volume neutralization (equal positions in monetary terms)
```

### Orderbuch-Scalping

```
Create a scalping strategy on StockSharp:
- Subscribe to order book (MarketDepth) via Subscribe
- Analyze bid/ask imbalance
- Enter on strong imbalance (> 3:1)
- Quick exit on take-profit (5 ticks)
- Stop-loss: 3 ticks
- Maximum 1 position at a time
```

## Häufige Fehler der KI

### 1. Veraltete Events

**Falsch** (alte API):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Korrekt** (aktuelle API):
```csharp
// Use subscriptions
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Erstellung von Orders ohne Hilfsmethoden

**Falsch**:
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

**Korrekt** (unter Verwendung von Strategie-Hilfsmethoden):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Fehlende IsFormed-Prüfung

**Falsch**:
```csharp
var value = _sma.Process(candle);
// Using value immediately — may not be ready
```

**Korrekt**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Tipps

1. **Der KI Dokumentation geben** — verweisen Sie sie auf [doc.stocksharp.com](https://doc.stocksharp.com) oder kopieren Sie Codebeispiele aus `Samples/`
2. **CLAUDE.md verwenden** — eine Projektregel-Datei reduziert die Anzahl der Fehler erheblich
3. **Einfach beginnen** — erstellen Sie zuerst eine einfache Strategie und fügen Sie dann Filter und Risikomanagement hinzu
4. **Auf historischen Daten testen** — führen Sie vor dem Live-Handel immer einen Backtest durch
5. **Das Repository klonen** — wenn die KI Zugriff auf die StockSharp-Quellen hat, wird sie die API genauer verwenden
