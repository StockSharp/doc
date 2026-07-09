# Estratégia de Cotação de Spread

## Visão Geral

`MqSpreadStrategy` é uma estratégia que cria um spread no mercado ao colocar simultaneamente cotações de compra e venda. Utiliza dois processadores de cotação para gerir ordens em ambos os lados do mercado.

## Componentes Principais

```cs
public class MqSpreadStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _buyProcessor;
	private QuotingProcessor _sellProcessor;
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
	Connector_CurrentTimeChanged(new TimeSpan());
}
```

## Gestão dos Processadores de Cotação

O método `Connector_CurrentTimeChanged` é chamado quando o tempo de mercado muda e gere a criação e atualização dos processadores de cotação:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Criar novos processadores apenas com posição zero e se os atuais estiverem parados
	if (Position != 0)
		return;

	if (_buyProcessor != null && _buyProcessor.LeftVolume > 0)
		return;

	if (_sellProcessor != null && _sellProcessor.LeftVolume > 0)
		return;

	// Libertar recursos dos processadores existentes
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	// Criar comportamentos para cotação de mercado
	var buyBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	var sellBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Criar processador para compra
	_buyProcessor = new QuotingProcessor(
		buyBehavior,
		Security,
		Portfolio,
		Sides.Buy,
		Volume,
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

	// Criar processador para venda
	_sellProcessor = new QuotingProcessor(
		sellBehavior,
		Security,
		Portfolio,
		Sides.Sell,
		Volume,
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

	// Registar criação de novos processadores de cotação
	this.AddInfoLog($"Spread de compra/venda criado em {CurrentTime}");

	// Subscrever eventos do processador de compra para logging
	_buyProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Ordem de compra {order.TransactionId} registada ao preço {order.Price}");

	_buyProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Falha na ordem de compra: {fail.Error.Message}");

	_buyProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Negócio de compra executado: {trade.Trade.Volume} ao preço {trade.Trade.Price}");

	_buyProcessor.Finished += isOk => {
		this.AddInfoLog($"Cotação de compra concluída com sucesso: {isOk}");
		_buyProcessor?.Dispose();
		_buyProcessor = null;
	};

	// Subscrever eventos do processador de venda para logging
	_sellProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Ordem de venda {order.TransactionId} registada ao preço {order.Price}");

	_sellProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Falha na ordem de venda: {fail.Error.Message}");

	_sellProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Negócio de venda executado: {trade.Trade.Volume} ao preço {trade.Trade.Price}");

	_sellProcessor.Finished += isOk => {
		this.AddInfoLog($"Cotação de venda concluída com sucesso: {isOk}");
		_sellProcessor?.Dispose();
		_sellProcessor = null;
	};

	// Iniciar ambos os processadores
	_buyProcessor.Start();
	_sellProcessor.Start();
}
```

## Libertação de Recursos

No método [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped), a estratégia liberta recursos:

```cs
protected override void OnStopped()
{
	// Anular subscrição para evitar fugas de memória
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Libertar recursos dos processadores
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	base.OnStopped();
}
```

## Lógica de Negociação

- A estratégia responde a alterações do tempo de mercado
- Com posição zero e processadores parados, são criados dois novos processadores:
  - Processador de compra (Buy)
  - Processador de venda (Sell)
- Ambos os processadores são configurados com o mesmo volume e utilizam as mesmas definições de cotação
- Os processadores criam um spread no mercado ao colocar simultaneamente ordens de compra e venda

## Funcionalidades

- Utiliza o processador de cotação moderno [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) com [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)
- Cria um spread no mercado ao colocar simultaneamente ordens de compra e venda
- Trabalha apenas com posição zero, evitando a acumulação de risco indesejado
- Suporta a configuração de vários parâmetros de cotação (tipo de preço, desvio, desvio mínimo)
- Inclui logging detalhado dos eventos do processador de cotação
- Gere corretamente recursos ao parar a estratégia e ao criar novos processadores
- Suporta trabalho com diferentes tipos de preços de mercado (Following, Best, Opposite, etc.)
