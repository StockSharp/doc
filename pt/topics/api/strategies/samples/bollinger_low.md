# Estratégia Bollinger com Foco na Banda Inferior

## Visão Geral

`BollingerStrategyLowBandStrategy` é uma estratégia baseada no indicador [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands). Abre uma posição curta quando o preço atinge o limite inferior das Bandas de Bollinger e fecha-a quando o preço atinge a linha média.

## Componentes Principais

A estratégia herda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) e utiliza parâmetros para configuração:

```cs
public class BollingerStrategyLowBandStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **Período de Bollinger** - período do indicador Bandas de Bollinger (predefinição 20)
- **Desvio de Bollinger** - multiplicador do desvio padrão (predefinição 2.0)
- **Tipo de vela** - tipo de vela com que trabalhar (predefinição 5 minutos)

Todos os parâmetros estão disponíveis para otimização com intervalos de valores especificados.

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), o indicador Bandas de Bollinger é criado, a subscrição de velas é configurada e a visualização é preparada:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Criar indicador
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// Criar subscrição e associar indicador
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// Configurar visualização no gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## Processamento de Velas

O método `ProcessCandle` é chamado para cada vela concluída e implementa a lógica de negociação:

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// Ignorar velas incompletas
	if (candle.State != CandleStates.Finished)
		return;

	// Verificar se a estratégia está pronta para negociar
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// Lógica de negociação:
	// Vender quando o preço toca na banda inferior (apenas quando não existe posição)
	if (candle.ClosePrice <= typed.LowBand && Position == 0)
	{
		SellMarket(Volume);
	}
	// Comprar para fechar a posição quando o preço atinge a linha média (apenas com uma posição curta)
	else if (candle.ClosePrice >= typed.MiddleBand && Position < 0)
	{
		BuyMarket(Math.Abs(Position));
	}
}
```

## Lógica de Negociação

- **Sinal de venda**: o preço de fecho da vela atinge ou fica abaixo da Banda de Bollinger inferior quando não existe posição aberta
- **Sinal de compra** (fecho de posição curta): o preço de fecho da vela atinge ou excede a linha média da Banda de Bollinger quando existe uma posição curta
- O volume da posição é fixo na abertura e igual a toda a posição atual no fecho

## Funcionalidades

- A estratégia determina automaticamente os instrumentos com que trabalhar através do método `GetWorkingSecurities()`
- A estratégia trabalha apenas com velas concluídas
- A estratégia utiliza apenas a banda inferior e a linha média do indicador Bandas de Bollinger
- Apenas são abertas posições curtas
- O indicador e as transações são visualizados num gráfico quando existe uma área gráfica disponível
- A otimização de parâmetros é suportada para encontrar definições ideais da estratégia
