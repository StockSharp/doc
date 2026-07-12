# Referência de Cotação

## Visão Geral

Esta secção contém uma referência completa da arquitetura e dos componentes do sistema de cotação no StockSharp. São descritos todos os comportamentos de cotação disponíveis, tipos de ação e interfaces. Para uma introdução básica, consulte [Algoritmo de Cotação](quoting.md).

## Arquitetura

### QuotingStrategy (Obsoleto)

A classe [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) está obsoleta. Recomenda-se utilizar [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) em alternativa.

Parâmetros principais da classe obsoleta:

- `QuotingSide` -- direção da cotação (compra/venda)
- `QuotingVolume` -- volume da cotação
- `TimeOut` -- timeout de execução
- `UseBidAsk` -- utilizar preços do livro de ordens
- `UseLastTradePrice` -- utilizar o preço da última transação

### QuotingEngine

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) -- o núcleo funcional do sistema de cotação. Calcula o volume restante e timeouts, devolvendo recomendações de ação sem efeitos secundários.

### QuotingBehaviorAlgo

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- implementação da interface `IPositionModifyAlgo` para gestão algorítmica de posições.

Métodos principais:

| Método | Descrição |
|--------|-----------|
| `UpdateMarketData(time, price, volume)` | Atualizar dados de mercado |
| `UpdateOrderBook(depth)` | Atualizar livro de ordens |
| `GetNextAction()` | Obter a próxima ação de cotação |

Suporta modos VWAP e TWAP.

### QuotingProcessor

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) -- o processador principal para executar cotação dentro de uma estratégia. Gere o ciclo de vida das ordens: colocação, modificação, cancelamento.

## Tipos QuotingAction

O processador devolve ações do tipo [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction):

| Tipo | Descrição |
|------|-----------|
| `None(reason)` | Nenhuma ação necessária |
| `PlaceOrder(price, volume)` | Colocar uma nova ordem |
| `ModifyOrder(price, volume)` | Modificar uma ordem existente |
| `CancelOrder()` | Cancelar a ordem |
| `Finish(success, reason)` | Terminar a cotação |

## Comportamentos de Cotação

StockSharp fornece 10 tipos de comportamentos de cotação, cada um implementando a interface [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior):

| # | Classe | Descrição | Parâmetros Principais |
|---|--------|-----------|-----------------------|
| 1 | [BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | Melhor preço do livro de ordens | BestPriceOffset |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | Preço da última transação | BestPriceOffset |
| 3 | [LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | Preço limite fixo | LimitPrice |
| 4 | [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | Preço de mercado com desvio | PriceOffset, PriceType (Following/Opposite/Middle) |
| 5 | [VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | Volatilidade de opção (Black-Scholes) | IVRange, Model |
| 6 | [TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | Preço teórico de opção | TheorPriceOffset |
| 7 | [BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | Volume cumulativo no livro de ordens | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | Nível de profundidade do livro de ordens | Level (Range\<int\>), OwnLevel |
| 9 | [VWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- média ponderada por volume | BestPriceOffset |
| 10 | [TWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- média ponderada por tempo | TimeInterval, PriceBufferSize (predefinição 10) |

## Interface IQuotingBehavior

A interface [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) define dois métodos essenciais:

### CalculateBestPrice

Calcula o melhor preço para colocar uma ordem:

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### NeedQuoting

Determina se a ordem atual precisa de ser atualizada:

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

Devolve `null` se não for necessária atualização, ou o novo preço para colocar a ordem.

## Exemplos de Utilização

### Cotação de Mercado com Desvio

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### Cotação VWAP

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### Cotação de Volatilidade de Opções

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### Exemplo Completo com Processador

```cs
// Escolher o comportamento de cotação
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// Criar o processador
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"Cotação concluída: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## Ver Também

- [Algoritmo de Cotação](quoting.md)
- [Estratégia de Cotação](samples/mq.md)
- [Cotação por Volatilidade](../options/volatility_trading.md)
