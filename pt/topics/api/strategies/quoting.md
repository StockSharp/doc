# Algoritmo de Cotação

## Visão Geral

Um algoritmo de cotação é um mecanismo que permite colocar e atualizar automaticamente ordens no mercado com o objetivo de obter o melhor preço de execução. Em vez de colocar ordens de mercado agressivas, a cotação utiliza ordens limitadas, o que ajuda a minimizar o slippage e a reduzir os custos de negociação.

## Componentes Principais

StockSharp fornece dois componentes essenciais para cotação:

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** — o processador principal que analisa dados de mercado e o estado das ordens para recomendar ações de cotação.

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** — uma interface que define o comportamento de cotação, incluindo o cálculo do preço ideal e a determinação da necessidade de atualizar ordens.

## Exemplos de Estratégias com Cotação

A documentação apresenta exemplos de estratégias que demonstram a utilização do mecanismo de cotação:

- **MqStrategy** — um exemplo de estratégia que utiliza o mecanismo de cotação para gerir uma posição no mercado.
- **MqSpreadStrategy** — um exemplo de estratégia que cria um spread no mercado ao colocar simultaneamente cotações de compra e venda.
- **StairsCountertrendStrategy** — um exemplo de estratégia contra tendência que utiliza cotação para uma entrada mais precisa no mercado.

Estes exemplos ajudam a compreender como integrar o mecanismo de cotação nos seus próprios algoritmos de negociação.

## Comportamentos de Cotação

StockSharp suporta vários comportamentos de cotação:

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** — cotação baseada no preço de mercado com desvio e tipo configuráveis.
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** — cotação baseada no melhor preço com desvio configurável.
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** — cotação a um preço fixo.
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** — cotação baseada no melhor preço por volume.
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** — cotação baseada num nível especificado no livro de ordens.
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** — cotação baseada no preço da última transação.
- **[TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** — cotação de opções baseada no preço teórico.
- **[VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** — cotação de opções baseada na volatilidade.

## Utilização na Sua Própria Estratégia

### Passo 1: Criar um Comportamento de Cotação

```csharp
// Criar um comportamento para cotação de mercado
var behavior = new MarketQuotingBehavior(
	new Unit(0.01m), // Desvio do preço em relação à melhor cotação
	new Unit(0.1m, UnitTypes.Percent), // Desvio mínimo para atualização da cotação
	MarketPriceTypes.Following // Tipo de preço de mercado para cotação
);
```

### Passo 2: Criar e Inicializar o Processador

```csharp
// Criar um processador de cotação
_quotingProcessor = new QuotingProcessor(
	behavior,
	Security, // Instrumento
	Portfolio, // Portefólio
	Sides.Buy, // Direção da cotação
	Volume, // Volume da cotação
	Volume, // Volume máximo da ordem
	TimeSpan.Zero, // Sem timeout
	this, // A estratégia implementa ISubscriptionProvider
	this, // A estratégia implementa IMarketRuleContainer
	this, // A estratégia implementa ITransactionProvider
	this, // A estratégia implementa ITimeProvider
	this, // A estratégia implementa IMarketDataProvider
	IsFormedAndOnlineAndAllowTrading, // Verificar permissão de negociação
	true, // Utilizar preços do livro de ordens
	true  // Utilizar o preço da última transação se o livro de ordens estiver vazio
)
{
	Parent = this
};
```

### Passo 3: Subscrever Eventos do Processador

```csharp
// Subscrever eventos do processador para logging e tratamento
_quotingProcessor.OrderRegistered += order =>
	this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

_quotingProcessor.OrderFailed += fail =>
	this.AddInfoLog($"Order failed: {fail.Error.Message}");

_quotingProcessor.OwnTrade += trade =>
	this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

_quotingProcessor.Finished += isOk => {
	this.AddInfoLog($"Quoting finished with success: {isOk}");
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
};
```

### Passo 4: Iniciar o Processador

```csharp
// Iniciar o processador
_quotingProcessor.Start();
```

### Passo 5: Libertar Recursos

Não se esqueça de limpar os recursos do processador ao parar a estratégia:

```csharp
protected override void OnStopped()
{
	// Libertar recursos do processador atual
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
	
	base.OnStopped();
}
```

## Vantagens da Utilização

1. **Slippage Reduzido** — a cotação ajuda a obter um melhor preço de execução em comparação com ordens de mercado.

2. **Flexibilidade de Configuração** — vários comportamentos de cotação para diferentes situações de mercado.

3. **Atualizações Automáticas** — o processador acompanha automaticamente as alterações do mercado e atualiza as ordens quando necessário.

4. **Controlo Total** — possibilidade de configurar parâmetros de cotação, incluindo desvio de preço, desvio mínimo e tipo de preço de mercado.

5. **Gestão de Risco** — possibilidade de definir um timeout para limitar o tempo de execução.

## Casos de Utilização Aplicados

- **Market Making** — criação de liquidez no mercado através da colocação de cotações nos dois lados.
- **Negociação Algorítmica** — melhoria do preço de execução em estratégias automatizadas.
- **Negociação Contra Tendência** — entrada mais precisa no mercado contra a tendência.
- **Arbitragem** — cotação simultânea em vários mercados para tirar partido de discrepâncias de preço.

Implementar o mecanismo de cotação na sua estratégia pode melhorar significativamente a execução das ordens e aumentar a sua eficiência.
