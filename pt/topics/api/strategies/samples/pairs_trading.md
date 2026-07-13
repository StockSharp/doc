# Estratégia de Negociação de Pares

## Visão Geral

`PairsTradingStrategy` é uma estratégia de negociação de pares baseada em arbitragem estatística entre dois instrumentos relacionados. Acompanha o spread entre os preços de dois ativos e abre posições quando o spread se desvia significativamente da média, esperando uma reversão para a média.

## Componentes Principais

A estratégia herda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) e utiliza parâmetros para configuração:

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// Últimos preços para cada instrumento
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **SpreadLength** - período para calcular a média e o desvio padrão do spread (predefinição 20)
- **EntryThreshold** - limiar de Z-Score para entrar numa posição (predefinição 2.0)
- **ExitThreshold** - limiar de Z-Score para sair de uma posição (predefinição 0.5)
- **CandleType** - tipo de vela com que trabalhar (predefinição 5 minutos)

Todos os parâmetros estão disponíveis para otimização com intervalos de valores especificados.

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), são criados indicadores e configuradas subscrições de velas para dois instrumentos:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Obter dois instrumentos para negociação de pares
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("Dois instrumentos devem ser especificados.");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// Indicadores para calcular a média e o desvio padrão do spread
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// Subscrever velas do primeiro instrumento
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// Subscrever velas do segundo instrumento com processamento do spread
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## Processamento do Spread

O método `ProcessSpread` calcula o Z-Score do spread e gera sinais de negociação:

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// Calcular o spread como diferença de preços
	var spread = price1 - price2;

	// Processar indicadores
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// Calcular Z-Score: desvio do spread face à média em unidades de desvio padrão
	var zScore = (spread - mean) / dev;

	// Spread demasiado alto: vender o primeiro instrumento, comprar o segundo
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Spread demasiado baixo: comprar o primeiro instrumento, vender o segundo
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Reversão para a média: fechar posição
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## Lógica de Negociação

- **Sinal de venda**: o Z-Score do spread excede o limiar de entrada (predefinição 2.0) quando não existe posição curta
- **Sinal de compra**: o Z-Score do spread fica abaixo do limiar de entrada negativo (predefinição -2.0) quando não existe posição longa
- **Fechar posição**: o valor absoluto do Z-Score desce abaixo do limiar de saída (predefinição 0.5), sinalizando que o spread está a reverter para a média

## Funcionalidades

- A estratégia trabalha com dois instrumentos obtidos através do método `GetWorkingSecurities()`
- O spread é calculado como a diferença dos preços de fecho das velas de dois instrumentos
- O Z-Score é usado para normalizar o desvio do spread face à média
- A estratégia implementa o conceito clássico de reversão à média
- A estratégia trabalha apenas com velas concluídas
- A otimização de parâmetros é suportada para encontrar definições ideais da estratégia
