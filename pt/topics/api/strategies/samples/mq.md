# Estratégia de Cotação

## Visão Geral

`MqStrategy` é uma estratégia que utiliza um mecanismo de cotação para gerir posições de mercado. Cria um processador de cotação com base na posição atual, permitindo responder de forma adaptativa a alterações nas condições de mercado.

## Componentes Principais

```cs
public class MqStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _quotingProcessor;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **PriceType** - tipo de preço de mercado para cotação (predefinição Following)
- **PriceOffset** - desvio do preço em relação ao preço de mercado
- **BestPriceOffset** - desvio mínimo para atualização da cotação (predefinição 0,1%)

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), a estratégia subscreve alterações do tempo de mercado:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Subscrever alterações do tempo de mercado para atualizações de cotação
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(default);
}
```

## Gestão do Processador de Cotação

O método `Connector_CurrentTimeChanged` é chamado quando o tempo de mercado muda e gere a criação e atualização do processador de cotação:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Criar um novo processador apenas se o atual estiver parado ou não existir
	if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
		return;

	// Libertar recursos do processador antigo, se existir
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	// Determinar o lado da cotação com base na posição atual
	var side = Position <= 0 ? Sides.Buy : Sides.Sell;

	// Criar novo comportamento de cotação
	var behavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Calcular volume de cotação
	var quotingVolume = Volume + Math.Abs(Position);

	// Criar e inicializar o processador
	_quotingProcessor = new QuotingProcessor(
		behavior,
		Security,
		Portfolio,
		side,
		quotingVolume,
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

	// Subscrever eventos do processador para logging
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

	// Iniciar o processador
	_quotingProcessor.Start();
}
```

## Libertação de Recursos

No método [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped), a estratégia liberta recursos:

```cs
protected override void OnStopped()
{
	// Anular subscrição para evitar fugas de memória
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Libertar recursos do processador atual, se existir
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## Lógica de Negociação

- A estratégia responde a alterações do tempo de mercado
- A direção da cotação é determinada com base na posição atual:
  - Se posição <= 0, é criada uma cotação Buy
  - Se posição > 0, é criada uma cotação Sell
- O volume de cotação é calculado como o volume base mais o valor absoluto da posição atual
- [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) com [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) é utilizado para cotação

## Funcionalidades

- Utiliza o processador de cotação moderno em vez das estratégias de cotação legadas
- Responde de forma adaptativa a alterações de posição ao mudar a direção da cotação
- Suporta a configuração de vários parâmetros de cotação (tipo de preço, desvio, desvio mínimo)
- Inclui logging detalhado dos eventos do processador de cotação
- Gere corretamente recursos ao parar a estratégia e ao criar novos processadores
- Suporta trabalho com diferentes tipos de preços de mercado (Following, Best, Opposite, etc.)
