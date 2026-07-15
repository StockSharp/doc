# Estratégia Contra a Tendência com Cotação

## Visão Geral

`StairsCountertrendStrategy` é uma estratégia de negociação contra a tendência que abre posições contra uma tendência estabelecida de um comprimento específico, utilizando um mecanismo de cotação para uma entrada no mercado mais precisa.

## Componentes Principais

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleDataType;
	private readonly StrategyParam<int> _length;
	private QuotingProcessor _quotingProcessor;

	private int _bullLength;
	private int _bearLength;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **Tipo de vela** - tipo de vela com que trabalhar (predefinição 1 minuto)
- **Período** - número de velas consecutivas numa direção para identificar uma tendência (predefinição 5)

O parâmetro Length está disponível para otimização no intervalo de 2 a 10 com passo de 1.

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), os contadores são reiniciados, a subscrição de velas é criada e a visualização é preparada:

```cs
protected override void OnStarted2(DateTime time)
{
	// Reiniciar contadores no início
	_bullLength = 0;
	_bearLength = 0;

	// Criar subscrição de velas
	var subscription = SubscribeCandles(CandleDataType);

	subscription
		.Bind(ProcessCandle)
		.Start();

	// Configurar visualização no gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}

	base.OnStarted2(time);
}
```

## Processamento de Velas

O método `ProcessCandle` é chamado para cada vela concluída e implementa a lógica de deteção da tendência e gestão do processador de cotação:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Identificar vela de alta ou de baixa
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"Vela de alta detetada. Sequência: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"Vela de baixa detetada. Sequência: {_bearLength}");
	}

	// Parar processador existente quando for necessária mudança de direção
	if (_quotingProcessor != null)
	{
		// Verificar se o processador deve ser limpo (mudança de tendência ou posição)
		var shouldClearProcessor = false;

		// Necessário vender se houver tendência de alta e nenhuma posição curta
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// Necessário comprar se houver tendência de baixa e nenhuma posição longa
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// Criar novo processador de cotação quando necessário
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// Tendência de alta - abrir posição curta
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"A iniciar cotação de venda após {_bullLength} velas de alta");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// Tendência de baixa - abrir posição longa
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"A iniciar cotação de compra após {_bearLength} velas de baixa");
		}
	}
}
```

## Criar Processador de Cotação

O método `CreateQuotingProcessor` cria um processador de cotação com a direção especificada:

```cs
private void CreateQuotingProcessor(Sides side)
{
	// Criar comportamento para cotação de mercado
	var behavior = new MarketQuotingBehavior(
		0, // Sem desvio de preço
		new Unit(0.1m, UnitTypes.Percent), // Usar 0,1% como desvio mínimo
		MarketPriceTypes.Following // Seguir preço de mercado
	);

	// Criar processador de cotação
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // Volume de cotação
		Volume, // Volume máximo da ordem
		TimeSpan.Zero, // Sem tempo limite
		this, // Strategy implementa ISubscriptionProvider
		this, // Strategy implementa IMarketRuleContainer
		this, // Strategy implementa ITransactionProvider
		this, // Strategy implementa ITimeProvider
		this, // Strategy implementa IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Verificar permissão de negociação
		true, // Usar preços do livro de ordens
		true // Usar preço da última transação se o livro de ordens estiver vazio
	)
	{
		Parent = this
	};

	// Subscrever eventos do processador
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Ordem {order.TransactionId} registada ao preço {order.Price}");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Falha na ordem: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Negócio executado: {trade.Trade.Volume} ao preço {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Inicializar processador
	_quotingProcessor.Start();
}
```

## Lógica de Negociação

- **Sinal de venda**: `Length` velas de alta consecutivas (preço de fecho acima do preço de abertura) quando não existe posição curta
- **Sinal de compra**: `Length` velas de baixa consecutivas (preço de fecho abaixo do preço de abertura) quando não existe posição longa
- O processador de cotação é usado para entrada no mercado, seguindo o preço de mercado

## Funcionalidades

- A estratégia determina automaticamente os instrumentos com que trabalhar através do método `GetWorkingSecurities()`
- A estratégia trabalha apenas com velas concluídas
- A cotação é usada em vez de ordens de mercado para uma entrada no mercado mais eficiente
- A estratégia aplica uma abordagem contra a tendência, abrindo posições contra a tendência estabelecida
- É implementado registo detalhado dos principais eventos para depuração
- O processador de cotação é automaticamente limpo quando a direção da tendência muda ou quando os objetivos são atingidos
- A visualização de velas e transações no gráfico é suportada
- A otimização do parâmetro de comprimento da sequência é implementada para configuração da estratégia
