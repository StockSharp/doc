# Subscrições de Alto Nível

## Visão Geral

A classe `Strategy` fornece um conjunto de métodos de alto nível para subscrição de dados de mercado: `SubscribeCandles`, `SubscribeTicks`, `SubscribeLevel1` e `SubscribeOrderBook`. Estes métodos devolvem um objeto `ISubscriptionHandler<T>` que permite associar manipuladores de dados e indicadores num estilo fluente conveniente.

Ao contrário da criação manual de um objeto `Subscription` e da chamada a `Subscribe()`, os métodos de alto nível:

- Criam automaticamente uma subscrição com os parâmetros corretos
- Fornecem um `ISubscriptionHandler<T>` tipado para associar manipuladores
- Integram-se com o sistema de indicadores através dos métodos `Bind`
- Suportam renderização automática no gráfico
- Gerem corretamente o ciclo de vida da subscrição

## Métodos de Subscrição

### SubscribeCandles

Subscreve candles. Aceita um período ou `DataType`:

```csharp
// Subscrever por período
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// Subscrever por DataType (suporta todos os tipos de velas)
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// Assinar usando um objeto Subscription pronto
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

O parâmetro `isFinishedOnly` é `true` por defeito -- o manipulador recebe apenas velas concluídas.

### SubscribeTicks

Subscreve negócios tick:

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### SubscribeLevel1

Subscreve dados Level1 (melhores preços bid/ask, último negócio e outros campos):

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### SubscribeOrderBook

Subscreve o livro de ordens:

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

Se o parâmetro `security` não for especificado, é usado o `Security` da estratégia.

## Interface ISubscriptionHandler

O objeto `ISubscriptionHandler<T>` fornece os seguintes métodos:

### Iniciar / parar

Iniciar e parar a subscrição:

```csharp
handler.Start();   // chama Subscribe
handler.Stop();    // chama UnSubscribe
```

### Bind (sem indicadores)

Associar um manipulador de dados simples:

```csharp
handler.Bind(Action<T> callback);
```

### Bind (com indicadores)

Associar um manipulador com um ou mais indicadores. O indicador processa automaticamente os dados recebidos e o manipulador recebe o valor já calculado:

```csharp
// Um indicador -- valor decimal
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// Dois indicadores
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// Até oito indicadores
handler.Bind(ind1, ind2, ind3, ..., callback);

// Array de indicadores
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

O manipulador com `Bind` é chamado apenas quando todos os indicadores devolveram um valor não vazio.

### BindWithEmpty

Semelhante a `Bind`, mas o manipulador é chamado mesmo se o indicador devolver um valor vazio. Os valores são representados como `decimal?`:

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### BindEx

Fornece acesso ao objeto `IIndicatorValue` completo em vez do `decimal` extraído:

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## Exemplo: Estratégia com Indicadores

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // Vinculação de dois indicadores -- o manipulador é chamado
        // quando ambos os indicadores estiverem formados
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // Configuração do gráfico
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## Exemplo: Subscrição de Ticks

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("Tick: preço={0}, volume={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## Exemplo: Subscrição do Livro de Ordens

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Spread: {0}", spread);
            }
        })
        .Start();
}
```

## Diferenças em Relação à Criação Manual de Subscrições

| Aspeto | Subscrição manual | Método de alto nível |
|--------|-------------------|----------------------|
| Criação | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| Tratamento de dados | Subscrição de eventos do conector | `Bind(callback)` |
| Indicadores | Chamada manual a `indicator.Process()` | Automaticamente através de `Bind(indicator, callback)` |
| Registo de indicadores | Adição manual a `Indicators` | Automaticamente ao usar `Bind` |
| Renderização no gráfico | Integração manual com `IChart` | `DrawCandles`, `DrawIndicator` |

Os métodos de alto nível são recomendados para utilização na maioria das estratégias, pois reduzem significativamente a quantidade de código e diminuem a probabilidade de erros.
