# Médias Móveis com Martingale

## Visão Geral

`SmaStrategyMartingaleStrategy` é uma estratégia de negociação baseada no cruzamento de duas médias móveis simples ([SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)) com elementos de martingale. A estratégia utiliza SMAs longa e curta para determinar sinais de entrada e saída, aumentando o tamanho da posição a cada nova transação.

## Componentes Principais

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// Variáveis para armazenar valores anteriores dos indicadores
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## Parâmetros da Estratégia

A estratégia permite personalizar os seguintes parâmetros:

- **LongSmaLength** - período da média móvel longa (predefinição 80)
- **ShortSmaLength** - período da média móvel curta (predefinição 30)
- **CandleType** - tipo de vela com que trabalhar (predefinição 5 minutos)

Todos os parâmetros estão disponíveis para otimização com intervalos de valores especificados.

## Inicialização da Estratégia

No método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)), são criados indicadores SMA, a subscrição de velas é configurada e a visualização é preparada:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Criar indicadores
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// Adicionar indicadores à coleção da estratégia para acompanhamento automático de IsFormed
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// Criar subscrição e associar indicadores
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// Configurar visualização no gráfico
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Processamento de Velas

O método `ProcessCandle` é chamado para cada vela concluída e implementa a lógica de negociação:

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Ignorar velas incompletas
	if (candle.State != CandleStates.Finished)
		return;

	// Verificar se a estratégia está pronta para negociar
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Para o primeiro valor, apenas guardar dados sem gerar sinais
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// Obter a comparação atual e anterior dos valores dos indicadores
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// Guardar valores atuais como anteriores para a próxima vela
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// Verificar cruzamento (sinal)
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// Cancelar ordens ativas antes de colocar novas
	CancelActiveOrders();

	// Determinar direção da transação
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// Calcular tamanho da posição (aumentar posição a cada transação - abordagem martingale)
	var volume = Volume + Math.Abs(Position);

	// Criar e registar uma ordem com o preço apropriado
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## Lógica de Negociação

- **Sinal de compra**: a SMA curta cruza a SMA longa de baixo para cima
- **Sinal de venda**: a SMA curta cruza a SMA longa de cima para baixo
- O tamanho da posição aumenta pelo valor da posição atual a cada nova transação (elemento martingale)
- O preço da ordem é definido para o valor atual da SMA curta, arredondado ao tamanho do tick do instrumento

## Funcionalidades

- A estratégia determina automaticamente os instrumentos com que trabalhar através do método `GetWorkingSecurities()`
- A estratégia trabalha apenas com velas concluídas
- A estratégia acompanha cruzamentos de indicadores comparando a relação atual e anterior entre as SMAs
- Todas as ordens ativas são canceladas antes de colocar novas
- O princípio de martingale é implementado - aumentando o tamanho da posição a cada nova transação
- Indicadores e transações são visualizados no gráfico quando existe uma área gráfica disponível
- A otimização de parâmetros é suportada para encontrar definições ideais da estratégia
