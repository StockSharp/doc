# Escrevendo uma Estratégia com IA

Um guia passo a passo para criar uma estratégia de negociação StockSharp usando ferramentas de IA.

## Preparação

### 1. Instale uma Ferramenta de IA

Escolha uma das ferramentas disponíveis:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (requer Node.js)
- **Cursor** — baixe em [cursor.com](https://cursor.com)
- **GitHub Copilot** — instale o plugin para a sua IDE

### 2. Crie um Projeto

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Prepare o Contexto

Crie um arquivo `CLAUDE.md` (ou `.cursorrules`) na raiz do projeto:

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

## Exemplo Passo a Passo: Estratégia SMA

### Passo 1: Descreva a Tarefa para a IA

Exemplo de prompt:

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

### Passo 2: Revise o Código Gerado

A IA gerará algo como:

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

### Passo 3: O Que Verificar

Percorra esta lista de verificação:

- **Herança**: a classe herda de `Strategy` ✓
- **Parâmetros**: usa `StrategyParam<T>` para otimização ✓
- **Assinatura de candles**: via `Subscribe(new Subscription(...))` ✓
- **Processamento de candles**: via a regra `WhenCandlesFinished` ✓
- **Verificação de IsFormed**: os indicadores são verificados quanto à prontidão ✓
- **Ordens**: via `RegisterOrder()` com `BuyAtMarket` / `SellAtMarket` ✓
- **Posição**: `Position` é verificada antes de enviar ordens ✓

### Passo 4: Peça à IA para Adicionar Backtesting

```
Add backtesting code for this strategy using historical data.
Use HistoryEmulationConnector, load data from local storage,
and output summary statistics (PnL, trade count, max drawdown).
```

## Exemplos de Prompts

### Estratégia de Bandas de Bollinger

```
Create a StockSharp strategy that trades using Bollinger Bands:
- Buy when price touches the lower band
- Sell when price touches the upper band
- Period 20, multiplier 2.0
- Stop-loss: 1% from entry price
- Take-profit: 2% from entry price
- Use StrategyParam for all parameters
```

### Estratégia de Arbitragem

```
Create a pairs arbitrage strategy on StockSharp:
- Two instruments (specified via parameters)
- Calculate the spread between prices
- Enter when spread deviates by 2 standard deviations
- Exit when spread returns to the mean
- Volume neutralization (equal positions in monetary terms)
```

### Scalping no Livro de Ofertas

```
Create a scalping strategy on StockSharp:
- Subscribe to order book (MarketDepth) via Subscribe
- Analyze bid/ask imbalance
- Enter on strong imbalance (> 3:1)
- Quick exit on take-profit (5 ticks)
- Stop-loss: 3 ticks
- Maximum 1 position at a time
```

## Erros Comuns da IA

### 1. Eventos Desatualizados

**Errado** (API antiga):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Correto** (API atual):
```csharp
// Use subscriptions
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Criação de Ordens Sem Helpers

**Errado**:
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

**Correto** (usando os helpers da estratégia):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// or
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Verificação de IsFormed Ausente

**Errado**:
```csharp
var value = _sma.Process(candle);
// Using value immediately — may not be ready
```

**Correto**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Dicas

1. **Forneça a documentação à IA** — aponte-a para [doc.stocksharp.com](https://doc.stocksharp.com) ou copie exemplos de código de `Samples/`
2. **Use o CLAUDE.md** — um arquivo de regras do projeto reduz muito o número de erros
3. **Comece simples** — crie primeiro uma estratégia básica, depois adicione filtros e gestão de risco
4. **Teste no histórico** — sempre execute um backtest antes da negociação ao vivo
5. **Clone o repositório** — se a IA tiver acesso aos códigos-fonte do StockSharp, ela usará a API com mais precisão
