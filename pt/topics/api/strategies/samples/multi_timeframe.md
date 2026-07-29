# Estratégia de múltiplos períodos

## Visão Geral

`MultiTimeframeStrategy` é uma estratégia que utiliza dois períodos para tomar decisões de negociação. Velas horárias determinam a direção da tendência através de cruzamentos de médias móveis, enquanto velas de 5 minutos com o indicador [RelativeStrengthIndex](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) são utilizadas para uma entrada precisa na direção da tendência.

## Componentes Principais

A estratégia herda de [Strategy](xref:StockSharp.Algo.Strategies.Strategy) e utiliza parâmetros para configuração:

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// Direção da tendência no período superior
	private Sides? _hourlyTrend;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **FastSmaLength** - período da média móvel rápida para o gráfico horário (predefinição 10)
- **SlowSmaLength** - período da média móvel lenta para o gráfico horário (predefinição 30)
- **RsiLength** - período do RSI para o gráfico de 5 minutos (predefinição 14)
- `TakeProfit` - tamanho do take-profit em percentagem (predefinição 2)
- `StopLoss` - tamanho do stop-loss em percentagem (predefinição 1)

Todos os parâmetros estão disponíveis para otimização com intervalos de valores especificados.

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), os indicadores são criados e as subscrições de velas são configuradas para dois períodos:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// Velas horárias para deteção de tendência (cruzamento de SMA)
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

	// Velas de 5 minutos para entrada precisa (RSI)
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// Configurar proteção da posição (take-profit e stop-loss)
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// Configurar visualização no gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Processamento de Velas Horárias

O método `ProcessHourlyCandle` determina a direção da tendência no período superior:

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Determinar tendência pelo cruzamento de médias móveis
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## Processamento de Velas de 5 Minutos

O método `ProcessEntryCandle` implementa a entrada em posição com base no sinal RSI na direção da tendência:

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// Comprar: tendência de alta e RSI em zona de sobrevenda
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Vender: tendência de baixa e RSI em zona de sobrecompra
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Lógica de Negociação

- **Deteção de tendência**: SMA rápida acima da SMA lenta no gráfico horário indica tendência de alta; abaixo indica tendência de baixa
- **Sinal de compra**: tendência de alta no gráfico horário e RSI < 30 no gráfico de 5 minutos quando não existe posição longa
- **Sinal de venda**: tendência de baixa no gráfico horário e RSI > 70 no gráfico de 5 minutos quando não existe posição curta
- **Proteção da posição**: take-profit e stop-loss automáticos através de `StartProtection`

## Funcionalidades

- A estratégia utiliza dois períodos: horário para tendência e 5 minutos para entrada
- A entrada em posição é executada apenas na direção da tendência do período superior
- O RSI é utilizado como filtro para encontrar pontos de entrada ideais (sobrevenda/sobrecompra)
- As posições são protegidas automaticamente com stop-loss e take-profit
- A estratégia trabalha apenas com velas concluídas
- Indicadores e transações são visualizados no gráfico quando existe uma área gráfica disponível
- A otimização de parâmetros é suportada para encontrar definições ideais da estratégia
