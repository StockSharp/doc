# Estratégia de Arbitragem

## Visão Geral

`ArbitrageStrategy` é uma estratégia de arbitragem entre um contrato de futuros e o seu ativo subjacente. Acompanha spreads entre instrumentos e abre posições quando surgem oportunidades de arbitragem.

## Componentes Principais

A estratégia herda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) e utiliza parâmetros para configuração:

```cs
public class ArbitrageStrategy : Strategy
{
	private enum ArbitrageState
	{
		Contango,        // O preço dos futuros é superior ao ativo subjacente
		Backwardation,   // O preço do ativo subjacente é superior aos futuros
		None,            // Sem posição
		OrderRegistration // Em processo de registo de ordens
	}

	// Parâmetros da estratégia
	private readonly StrategyParam<Security> _futureSecurity;
	private readonly StrategyParam<Security> _stockSecurity;
	private readonly StrategyParam<Portfolio> _futurePortfolio;
	private readonly StrategyParam<Portfolio> _stockPortfolio;
	private readonly StrategyParam<decimal> _stockMultiplicator;
	private readonly StrategyParam<decimal> _futureVolume;
	private readonly StrategyParam<decimal> _stockVolume;
	private readonly StrategyParam<decimal> _profitToExit;
	private readonly StrategyParam<decimal> _spreadToGenerateSignal;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **FutureSecurity** - instrumento de futuros
- **StockSecurity** - instrumento do ativo subjacente
- **FuturePortfolio** - portefólio para negociação de futuros
- **StockPortfolio** - portefólio para negociação do ativo subjacente
- **StockMultiplicator** - multiplicador do ativo subjacente (por exemplo, tamanho do lote)
- **FutureVolume** - volume para negociação de futuros
- **StockVolume** - volume para negociação do ativo subjacente
- **ProfitToExit** - limiar de lucro para saída da posição
- **SpreadToGenerateSignal** - limiar de spread para geração do sinal de entrada

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), os parâmetros são validados e são criadas subscrições aos livros de ordens e às transações próprias:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	if (FutureSecurity == null)
		throw new InvalidOperationException("Future security is not specified.");

	if (StockSecurity == null)
		throw new InvalidOperationException("Stock security is not specified.");

	if (FuturePortfolio == null)
		throw new InvalidOperationException("Future portfolio is not specified.");

	if (StockPortfolio == null)
		throw new InvalidOperationException("Stock portfolio is not specified.");

	_futId = FutureSecurity.ToSecurityId();
	_stockId = StockSecurity.ToSecurityId();

	// Subscrição de atualizações do livro de ordens para ambos os instrumentos
	var futureDepthSubscription = new Subscription(DataType.MarketDepth, FutureSecurity);
	var stockDepthSubscription = new Subscription(DataType.MarketDepth, StockSecurity);

	futureDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);
	stockDepthSubscription.WhenOrderBookReceived(this).Do(ProcessMarketDepth).Apply(this);

	// Subscrição de transações próprias para acompanhar preços de execução
	this
		.WhenOwnTradeReceived()
		.Do(OnOwnTradeReceived)
		.Apply(this);

	// Enviar pedidos de subscrição de dados de mercado
	Subscribe(futureDepthSubscription);
	Subscribe(stockDepthSubscription);
}
```

## Processamento de Dados de Mercado

O método `ProcessMarketDepth` é chamado quando um livro de ordens é atualizado e implementa a lógica principal:

```cs
private void ProcessMarketDepth(IOrderBookMessage depth)
{
	// Atualizar o último livro de ordens de cada instrumento
	if (depth.SecurityId == _futId)
		_lastFut = depth;
	else if (depth.SecurityId == _stockId)
		_lastSt = depth;

	// Aguardar dados para ambos os instrumentos
	if (_lastFut is null || _lastSt is null)
		return;

	// Calcular preços médios ponderados por volume para volumes específicos
	_futBid = GetAveragePrice(_lastFut, Sides.Sell, FutureVolume);
	_futAck = GetAveragePrice(_lastFut, Sides.Buy, FutureVolume);
	_stBid = GetAveragePrice(_lastSt, Sides.Sell, StockVolume) * StockMultiplicator;
	_stAsk = GetAveragePrice(_lastSt, Sides.Buy, StockVolume) * StockMultiplicator;

	// Validar preços
	if (_futBid == 0 || _futAck == 0 || _stBid == 0 || _stAsk == 0)
		return;

	// Calcular spreads
	var contangoSpread = _futBid - _stAsk;        // Preço dos futuros > preço do ativo subjacente
	var backwardationSpread = _stBid - _futAck;   // Preço do ativo subjacente > preço dos futuros

	decimal spread;
	ArbitrageState arbitrageSignal;

	// Determinar a melhor oportunidade de arbitragem
	if (backwardationSpread > contangoSpread)
	{
		arbitrageSignal = ArbitrageState.Backwardation;
		spread = backwardationSpread;
	}
	else
	{
		arbitrageSignal = ArbitrageState.Contango;
		spread = contangoSpread;
	}

	// Registar estado atual e spreads
	LogInfo($"Current state {_currentState}, enter spread = {_enterSpread}");
	LogInfo($"{ArbitrageState.Backwardation} spread = {backwardationSpread}");
	LogInfo($"{ArbitrageState.Contango}        spread = {contangoSpread}");
	LogInfo($"Entry from spread:{SpreadToGenerateSignal}. Exit from profit:{ProfitToExit}");

	// Recalcular lucro com base nas condições atuais de mercado
	if (_currentState != ArbitrageState.None && _currentState != ArbitrageState.OrderRegistration)
	{
		CalculateProfit();
		LogInfo($"Profit: {_profit}");
	}

	// Processar sinais com base no estado atual e nas condições de mercado
	ProcessSignals(arbitrageSignal, spread);
}
```

## Lógica de Negociação

O processamento de sinais e a tomada de decisões de entrada/saída são implementados no método `ProcessSignals`:

```cs
private void ProcessSignals(ArbitrageState arbitrageSignal, decimal spread)
{
	// Entrar numa nova posição quando não existe posição aberta e o spread excede o limiar
	if (_currentState == ArbitrageState.None && spread > SpreadToGenerateSignal)
	{
		_currentState = ArbitrageState.OrderRegistration;

		if (arbitrageSignal == ArbitrageState.Backwardation)
		{
			ExecuteBackwardation();
		}
		else
		{
			ExecuteContango();
		}
	}
	// Sair da posição de Backwardation quando o limiar de lucro é atingido
	else if (_currentState == ArbitrageState.Backwardation && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseBackwardationPosition();
	}
	// Sair da posição de Contango quando o limiar de lucro é atingido
	else if (_currentState == ArbitrageState.Contango && _profit >= ProfitToExit)
	{
		_currentState = ArbitrageState.OrderRegistration;
		CloseContangoPosition();
	}
}
```

## Cálculo de Lucro

O método `CalculateProfit` calcula o lucro atual com base nos preços de entrada e nos preços atuais:

```cs
private void CalculateProfit()
{
	switch (_currentState)
	{
		case ArbitrageState.Backwardation:
			// Comprar futuros, vender ativo subjacente - lucro quando o preço dos futuros sobe e o preço do ativo subjacente desce
			_profit = (_stockExitPrice * StockMultiplicator - _stAsk) + (_futBid - _futureBuyPrice);
			break;

		case ArbitrageState.Contango:
			// Vender futuros, comprar ativo subjacente - lucro quando o preço dos futuros desce e o preço do ativo subjacente sobe
			_profit = (_futureExitPrice - _futAck) + (_stBid - _stockBuyPrice * StockMultiplicator);
			break;

		default:
			_profit = 0;
			break;
	}
}
```

## Geração de Ordens

Para executar estratégias de arbitragem, são utilizados métodos para gerar ordens:

```cs
private (Order buy, Order sell) GenerateOrdersBackwardation()
{
	var futureBuy = CreateOrder(Sides.Buy, FutureVolume);
	futureBuy.Portfolio = FuturePortfolio;
	futureBuy.Security = FutureSecurity;
	futureBuy.Type = OrderTypes.Market;

	var stockSell = CreateOrder(Sides.Sell, StockVolume);
	stockSell.Portfolio = StockPortfolio;
	stockSell.Security = StockSecurity;
	stockSell.Type = OrderTypes.Market;

	return (futureBuy, stockSell);
}

private (Order sell, Order buy) GenerateOrdersContango()
{
	var futureSell = CreateOrder(Sides.Sell, FutureVolume);
	futureSell.Portfolio = FuturePortfolio;
	futureSell.Security = FutureSecurity;
	futureSell.Type = OrderTypes.Market;

	var stockBuy = CreateOrder(Sides.Buy, StockVolume);
	stockBuy.Portfolio = StockPortfolio;
	stockBuy.Security = StockSecurity;
	stockBuy.Type = OrderTypes.Market;

	return (futureSell, stockBuy);
}
```

## Funcionalidades

- A estratégia suporta trabalhar com dois instrumentos diferentes e dois portefólios
- São utilizadas ordens de mercado para execução rápida
- Regras (IMarketRule) são utilizadas para acompanhar a execução das ordens
- O preço médio ponderado por volume é calculado com base no volume para preços mais precisos
- A lógica de arbitragem considera spreads diretos (contango) e inversos (backwardation)
- Suporta cálculo automático de lucro e saída quando o limiar alvo é atingido
