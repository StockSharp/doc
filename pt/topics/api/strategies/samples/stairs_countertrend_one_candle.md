# Estratégia Contra a Tendência de Uma Vela

## Visão Geral

`OneCandleCountertrendStrategy` é uma estratégia simples contra a tendência que toma decisões com base na análise de uma única vela.

## Componentes Principais

```cs
public class OneCandleCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **Tipo de vela** - tipo de vela com que trabalhar (predefinição 5 minutos)

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), a subscrição de velas é criada e a visualização é preparada:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

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

	// Estratégia contra a tendência: comprar em vela de baixa, vender em vela de alta
	if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
	{
		// Vela de alta - vender
		SellMarket(Volume + Math.Abs(Position));
	}
	else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
	{
		// Vela de baixa - comprar
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de Negociação

- **Sinal de venda**: vela de alta (preço de fecho acima do preço de abertura) quando não existe posição curta
- **Sinal de compra**: vela de baixa (preço de fecho abaixo do preço de abertura) quando não existe posição longa
- O volume da posição aumenta pelo valor da posição atual a cada nova transação

## Funcionalidades

- A estratégia determina automaticamente os instrumentos com que trabalhar através do método `GetWorkingSecurities()`
- A estratégia trabalha apenas com velas concluídas
- A estratégia utiliza ordens de mercado para entrada em posição
- A estratégia aplica uma lógica simples de deteção contra a tendência baseada numa única vela
- Velas e transações são visualizadas no gráfico quando existe uma área gráfica disponível
