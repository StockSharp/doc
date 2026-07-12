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
# Regras do projeto

- Plataforma: StockSharp 5.x, .NET 10
- As estratégias herdam da classe Strategy
- Subscrever candles via Connector.Subscribe(subscription)
- Registar ordens via RegisterOrder(order)
- Registo: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Indicadores: criar com new e chamar indicator.Process(candle)
- Tratar sempre connector.Error e os erros da estratégia
```

## Exemplo Passo a Passo: Estratégia SMA

### Passo 1: Descreva a Tarefa para a IA

Exemplo de prompt:

```
Crie uma estratégia de negociação com StockSharp que:
- herde de Strategy
- use duas médias móveis simples (SMA): rápida (período 10) e lenta (período 30)
- compre quando a SMA rápida cruza acima da SMA lenta
- venda quando a SMA rápida cruza abaixo da SMA lenta
- tamanho da posição: 1 lote
- use candles de 5 minutos
- subscreva candles em OnStarted()
- processe candles via regras de subscrição
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
                // A SMA rápida cruzou acima da lenta — comprar
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // A SMA rápida cruzou abaixo da lenta — vender
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

### Passo 4: Peça à IA para adicionar testes históricos

```
Adicione código de testes históricos para esta estratégia usando dados históricos.
Use HistoryEmulationConnector, carregue dados do armazenamento local
e apresente estatísticas resumidas (PnL, número de transações, drawdown máximo).
```

## Exemplos de Prompts

### Estratégia de Bandas de Bollinger

```
Crie uma estratégia StockSharp que negoceie com Bandas de Bollinger:
- comprar quando o preço toca na banda inferior
- vender quando o preço toca na banda superior
- período 20, multiplicador 2,0
- stop-loss: 1% a partir do preço de entrada
- take-profit: 2% a partir do preço de entrada
- usar StrategyParam para todos os parâmetros
```

### Estratégia de Arbitragem

```
Crie uma estratégia de arbitragem de pares em StockSharp:
- dois instrumentos (especificados via parâmetros)
- calcular o spread entre os preços
- entrar quando o spread se desvia 2 desvios-padrão
- sair quando o spread regressa à média
- neutralização de volume (posições equivalentes em termos monetários)
```

### Scalping no Livro de Ofertas

```
Crie uma estratégia de scalping em StockSharp:
- subscrever o livro de ofertas (MarketDepth) via Subscribe
- analisar o desequilíbrio bid/ask
- entrar com desequilíbrio forte (> 3:1)
- saída rápida por take-profit (5 ticks)
- stop-loss: 3 ticks
- no máximo 1 posição de cada vez
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
// Usar assinaturas
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
// Usar o valor imediatamente — pode ainda não estar pronto
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
4. **Teste no histórico** — sempre execute um teste histórico antes da negociação ao vivo
5. **Clone o repositório** — se a IA tiver acesso aos códigos-fonte do StockSharp, ela usará a API com mais precisão
