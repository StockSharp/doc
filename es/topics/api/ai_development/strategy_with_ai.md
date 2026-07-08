# Escribir una estrategia con IA

Una guía paso a paso para crear una estrategia de trading de StockSharp usando herramientas de IA.

## Preparación

### 1. Instale una herramienta de IA

Elija una de las herramientas disponibles:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (requiere Node.js)
- **Cursor** — descargue desde [cursor.com](https://cursor.com)
- **GitHub Copilot** — instale el plugin para su IDE

### 2. Cree un proyecto

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Prepare el contexto

Cree un archivo `CLAUDE.md` (o `.cursorrules`) en la raíz del proyecto:

```markdown
# Reglas del proyecto

- Framework: StockSharp 5.x, .NET 10
- Strategies inherit from Strategy class
- Subscribe to candles via Connector.Subscribe(subscription)
- Register orders via RegisterOrder(order)
- Logging: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Indicators: create via new and call indicator.Process(candle)
- Always handle connector.Error and strategy errors
```

## Ejemplo paso a paso: estrategia SMA

### Paso 1: Describa la tarea a la IA

Ejemplo de prompt:

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

### Paso 2: Revise el código generado

La IA generará algo como esto:

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
                // La SMA rápida cruza por encima de la lenta — comprar
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // La SMA rápida cruza por debajo de la lenta — vender
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### Paso 3: Qué comprobar

Repase esta lista de verificación:

- **Herencia**: la clase hereda de `Strategy` ✓
- **Parámetros**: usa `StrategyParam<T>` para la optimización ✓
- **Suscripción a velas**: mediante `Subscribe(new Subscription(...))` ✓
- **Procesamiento de velas**: mediante la regla `WhenCandlesFinished` ✓
- **Comprobación de IsFormed**: se verifica que los indicadores estén listos ✓
- **Órdenes**: mediante `RegisterOrder()` con `BuyAtMarket` / `SellAtMarket` ✓
- **Posición**: `Position` se comprueba antes de colocar órdenes ✓

### Paso 4: Pida a la IA que añada backtesting

```
Add backtesting code for this strategy using historical data.
Use HistoryEmulationConnector, load data from local storage,
and output summary statistics (PnL, trade count, max drawdown).
```

## Ejemplos de prompts

### Estrategia de bandas de Bollinger

```
Create a StockSharp strategy that trades using Bollinger Bands:
- Buy when price touches the lower band
- Sell when price touches the upper band
- Period 20, multiplier 2.0
- Stop-loss: 1% from entry price
- Take-profit: 2% from entry price
- Use StrategyParam for all parameters
```

### Estrategia de arbitraje

```
Create a pairs arbitrage strategy on StockSharp:
- Two instruments (specified via parameters)
- Calculate the spread between prices
- Enter when spread deviates by 2 standard deviations
- Exit when spread returns to the mean
- Volume neutralization (equal positions in monetary terms)
```

### Scalping con libro de órdenes

```
Create a scalping strategy on StockSharp:
- Subscribe to order book (MarketDepth) via Subscribe
- Analyze bid/ask imbalance
- Enter on strong imbalance (> 3:1)
- Quick exit on take-profit (5 ticks)
- Stop-loss: 3 ticks
- Maximum 1 position at a time
```

## Errores comunes de la IA

### 1. Eventos obsoletos

**Incorrecto** (API antigua):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Correcto** (API actual):
```csharp
// Usar suscripciones
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Creación de órdenes sin helpers

**Incorrecto**:
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

**Correcto** (usando los helpers de la estrategia):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Falta la comprobación de IsFormed

**Incorrecto**:
```csharp
var value = _sma.Process(candle);
// Using value immediately — may not be ready
```

**Correcto**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Consejos

1. **Proporcione documentación a la IA** — indíquele [doc.stocksharp.com](https://doc.stocksharp.com) o copie ejemplos de código de `Samples/`
2. **Use CLAUDE.md** — un archivo de reglas del proyecto reduce considerablemente el número de errores
3. **Empiece de forma simple** — cree primero una estrategia básica y luego añada filtros y gestión de riesgo
4. **Pruebe con el histórico** — ejecute siempre un backtest antes de operar en real
5. **Clone el repositorio** — si la IA tiene acceso a los fuentes de StockSharp, usará la API con mayor precisión
