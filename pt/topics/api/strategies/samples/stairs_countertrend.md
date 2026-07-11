# Estratégia Stairs Contra a Tendência

## Visão Geral

`StairsCountertrendStrategy` é uma estratégia de negociação contra a tendência que abre posições contra uma tendência estabelecida de um comprimento específico.

## Componentes Principais

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<int> _length;
	private readonly StrategyParam<DataType> _candleType;
	
	private int _bullLength;
	private int _bearLength;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **Length** - número de velas consecutivas numa direção para identificar uma tendência (predefinição 3)
- **CandleType** - tipo de vela com que trabalhar (predefinição 5 minutos)

O parâmetro Length está disponível para otimização no intervalo de 2 a 10 com passo de 1.

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), os contadores são reiniciados, a subscrição de velas é criada e a visualização é preparada:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Reiniciar contadores
	_bullLength = 0;
	_bearLength = 0;

	// Criar subscrição
	var subscription = SubscribeCandles(CandleType);
	
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
}
```

## Processamento de Velas

O método `ProcessCandle` é chamado para cada vela concluída e implementa a lógica de negociação:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Verificar se a vela está concluída
	if (candle.State != CandleStates.Finished)
		return;

	// Verificar se a estratégia está pronta para negociar
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Atualizar contadores com base na direção da vela
	if (candle.OpenPrice < candle.ClosePrice)
	{
		// Vela de alta
		_bullLength++;
		_bearLength = 0;
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		// Vela de baixa
		_bullLength = 0;
		_bearLength++;
	}

	// Estratégia contra a tendência: 
	// Vender após Length velas de alta consecutivas
	if (_bullLength >= Length && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Comprar após Length velas de baixa consecutivas
	else if (_bearLength >= Length && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de Negociação

- **Sinal de venda**: `Length` velas de alta consecutivas (preço de fecho acima do preço de abertura) quando não existe posição curta
- **Sinal de compra**: `Length` velas de baixa consecutivas (preço de fecho abaixo do preço de abertura) quando não existe posição longa
- O volume da posição aumenta pelo valor da posição atual a cada nova transação

## Funcionalidades

- A estratégia determina automaticamente os instrumentos com que trabalhar através do método `GetWorkingSecurities()`
- A estratégia trabalha apenas com velas concluídas
- A estratégia utiliza ordens de mercado para entrada em posição
- A estratégia aplica uma abordagem contra a tendência, abrindo posições contra a tendência estabelecida
- Os contadores de velas são reiniciados quando aparece uma vela na direção oposta
- Velas e transações são visualizadas no gráfico quando existe uma área gráfica disponível
- A otimização do comprimento da sequência é suportada para encontrar definições ideais da estratégia
