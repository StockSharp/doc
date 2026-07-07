# API de alto nível em estratégias

StockSharp fornece um conjunto de APIs de alto nível para simplificar o trabalho com tarefas comuns em estratégias de negociação. Estas interfaces permitem escrever código mais limpo, centrado na lógica de negociação em vez de detalhes técnicos.

## Gestão Simplificada de Subscrições

Os métodos de alto nível para trabalhar com subscrições ocultam a complexidade da gestão do ciclo de vida da subscrição e do processamento de dados.

### Método SubscribeCandles

Em vez de criar manualmente uma subscrição e configurar manipuladores de eventos, pode usar o método [SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)):

```cs
// Create and configure a candle subscription in a single line
var subscription = SubscribeCandles(CandleType);
```

Este método devolve um objeto do tipo [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1), que fornece uma interface conveniente para configurar posteriormente a subscrição.

### Associação Automática de Indicadores à Subscrição

A API de alto nível facilita a associação de indicadores a uma subscrição de dados:

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
	// Bind indicators to candle subscription
	.Bind(longSma, shortSma, OnProcess)
	// Start processing
	.Start();
```

#### Adição Automática de Indicadores à Coleção Strategy.Indicators

É importante notar que, ao usar o método [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) para ligar indicadores a uma subscrição, **não precisa** de adicionar adicionalmente esses indicadores à coleção [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators), como normalmente é feito na abordagem tradicional (descrita na [documentação de indicadores](indicators.md)). O sistema automaticamente:

1. Adiciona indicadores à coleção [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators)
2. Acompanha o estado de formação dos indicadores
3. Atualiza o estado [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) da estratégia

Isto simplifica significativamente o código e reduz a probabilidade de erros.

Se precisar de receber valores de indicadores mesmo quando alguns deles ainda não têm dados (`IIndicatorValue.IsEmpty` é `true`), use o método `BindWithEmpty`. Neste caso, os argumentos do manipulador devem ser do tipo `decimal?`. Também pode usar `BindEx` para inspecionar diretamente os objetos `IIndicatorValue` brutos.

#### Usar BindEx para Trabalhar com Valores Brutos de Indicadores

Se um indicador devolver valores não padrão (não apenas números), pode usar o método [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)), que fornece acesso ao objeto [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) original:

```cs
subscription
	.BindEx(indicator, OnProcessWithRawValue)
	.Start();

// Handler receives the original IIndicatorValue
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
	// Access to IIndicatorValue properties
	if (value.IsFinal)
	{
		// For indicators returning boolean values
		var boolValue = value.GetValue<bool>();
		
		// Or other data types specific to a particular indicator
		// ...
	}
}
```

O método [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) é particularmente útil nos seguintes casos:

- Trabalhar com indicadores que devolvem valores booleanos (por exemplo, [Fractals](xref:StockSharp.Algo.Indicators.Fractals))
- Aceder a propriedades adicionais do tipo de valor do indicador (por exemplo, o sinalizador [IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal))
- Trabalhar com indicadores que devolvem dados estruturados

#### Trabalhar com Indicadores Complexos (IComplexIndicator)

Para indicadores complexos que contêm vários indicadores internos (por exemplo, [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), [MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence)), a API fornece sobrecargas especiais dos métodos `Bind` e `BindEx`:

```cs
// Create a complex indicator
var bollinger = new BollingerBands 
{ 
	Length = 20, 
	Deviation = 2 
};

// Bind the complex indicator to a subscription
subscription
	.BindEx(bollinger, OnProcessBollinger)
	.Start();

// Handler receives the BollingerBandsValue instance
private void OnProcessBollinger(ICandleMessage candle, IIndicatorValue value)
{
	var typed = (BollingerBandsValue)value;

	// Use Bollinger band values
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
}
```

Para um trabalho mais flexível, pode usar [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) com acesso direto ao valor do indicador complexo:

```cs
subscription.BindEx(bollinger, (candle, indicatorValue) =>
{
	var typed = (BollingerBandsValue)indicatorValue;

	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
});
```

O método [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) para indicadores complexos automaticamente:

1. Processa os dados de entrada através do indicador complexo
2. Passa o `IIndicatorValue` resultante para o manipulador especificado

Converta o valor para o **tipo de valor** dedicado do indicador para trabalhar com os seus campos individuais.

### O método `Bind` estabelece uma ligação entre dados de subscrição e indicadores. Quando uma nova candle é recebida:

1. A candle é enviada automaticamente para processamento pelos indicadores
2. Os resultados do processamento são passados ao manipulador especificado (no exemplo, o método `OnProcess`)
3. Todo o código de sincronização e gestão de estado fica oculto ao programador

O manipulador recebe valores prontos a usar como tipos `decimal` simples. O método é chamado apenas quando todos os indicadores associados devolvem dados:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Work directly with ready-made indicator values
	var isShortLessThenLong = shortValue < longValue;
	
	// Trading logic uses clean numeric values
	// without the need to extract them from IIndicatorValue
	// ...
}
```

Isto simplifica significativamente o código e torna-o mais legível, pois o programador não precisa de:
- Tratar manualmente o evento de receção de uma candle
- Passar dados manualmente para indicadores
- Extrair valores dos resultados dos indicadores

## Gestão Simplificada de Gráficos

### Visualização Automática

A API de alto nível fornece métodos simples para associar subscrições e indicadores a elementos de gráfico:

```cs
var area = CreateChartArea();

// area can be null when running without GUI
if (area != null)
{
	// Automatic binding of candles to chart area
	DrawCandles(area, subscription);

	// Drawing indicators with color customization
	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);
	
	// Drawing own trades
	DrawOwnTrades(area);
	
	// Drawing orders
	DrawOrders(area);
}
```

#### Método DrawCandles

O método [DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) liga automaticamente uma subscrição de candles a um elemento de apresentação de candles no gráfico:

```cs
// Create a chart element for displaying candles
IChartCandleElement candles = DrawCandles(area, subscription);

// Additional element parameters can be configured
candles.DrawOpenClose = true;  // Display open/close lines
candles.DrawHigh = true;       // Display highs
candles.DrawLow = true;        // Display lows
```

O método devolve um elemento de gráfico [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) que pode ser personalizado posteriormente.

#### Método DrawIndicator

O método [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) cria e configura um elemento de gráfico para apresentar valores de indicadores:

```cs
// Simple addition of an indicator to the chart with default color
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// Adding an indicator with a specified primary color
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// Adding an indicator with specified primary and secondary colors
IChartIndicatorElement bollingerElem = DrawIndicator(
	area, 
	bollinger, 
	System.Drawing.Color.Blue,    // Primary color
	System.Drawing.Color.Gray     // Secondary color (for the second line)
);

// Additional element configuration
smaElem.DrawStyle = DrawStyles.Line;           // Drawing style: line
rsiFast.DrawStyle = DrawStyles.Dot;            // Drawing style: dots
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // Drawing style: dash-dot
```

O método devolve um elemento de gráfico [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) que pode ser personalizado. Para indicadores com vários valores (por exemplo, [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)), a cor primária é aplicada ao primeiro valor e a cor secundária ao segundo.

#### Método DrawOwnTrades

O método [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) cria um elemento para apresentar os negócios próprios da estratégia no gráfico:

```cs
// Create an element for displaying trades
IChartTradeElement trades = DrawOwnTrades(area);

// Element configuration
trades.BuyColor = System.Drawing.Color.Green;   // Color for buy trades
trades.SellColor = System.Drawing.Color.Red;    // Color for sell trades
trades.FullTitle = "My Strategy Trades";        // Element title
```

Este método configura automaticamente a apresentação de todos os negócios executados pela estratégia. Os negócios são apresentados no gráfico como marcadores nos pontos em que foram executados, tendo em conta o lado do negócio (buy/sell).

#### Método DrawOrders

O método [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) cria um elemento para apresentar ordens no gráfico:

```cs
// Create an element for displaying orders
IChartOrderElement orders = DrawOrders(area);

// Element configuration
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // Color for active buy orders
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // Color for active sell orders
orders.BuyColor = System.Drawing.Color.Green;              // Color for executed buy orders
orders.SellColor = System.Drawing.Color.Red;               // Color for executed sell orders
orders.CancelColor = System.Drawing.Color.Gray;            // Color for canceled orders
```

Este método configura automaticamente a apresentação de todas as ordens colocadas pela estratégia. As ordens são apresentadas como marcadores nos seus níveis de preço, com codificação por cores diferente para diferentes estados de ordem.

#### Método CreateChartArea

O método [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) cria uma nova área no gráfico da estratégia:

```cs
// Create the first area for candles and indicators
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// Create a second area for separate indicators (e.g., RSI)
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

Dividir o gráfico em áreas permite uma apresentação mais visual de diferentes tipos de dados. Por exemplo, indicadores com um intervalo de valores diferente do preço (RSI, estocástico, etc.) são melhor apresentados em áreas separadas.

Vantagens dos métodos de visualização de alto nível:
- Não é necessário criar manualmente objetos `ChartDrawData`
- Não é necessário gerir o agrupamento de dados por tempo
- Não é necessário chamar `chart.Draw()` para atualizar o gráfico
- Sincronização automática de dados entre subscrições e elementos de gráfico
- Gestão simplificada do aspeto dos elementos gráficos

O sistema atualiza automaticamente o gráfico quando novos dados são recebidos, permitindo que o programador evite concentrar-se em detalhes técnicos de visualização.

## Proteção de Posições

### Método StartProtection

Para proteger posições abertas, StockSharp fornece o método de alto nível [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)):

```cs
// Start position protection with Take Profit and Stop Loss levels
StartProtection(TakeValue, StopValue);
```

Este método configura automaticamente a proteção para todas as posições abertas:
- Acompanha alterações de preço
- Cria automaticamente ordens para fechar posições quando os níveis de Take Profit ou Stop Loss são atingidos
- Suporta vários tipos de unidades de medida (valores absolutos, percentagens, pontos)
- Pode usar trailing stop para proteção adaptativa de posições

Exemplo com parâmetros adicionais:

```cs
// Start protection with trailing stop and market orders
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute), // Take Profit
	stopLoss: new Unit(2, UnitTypes.Percent),     // Stop Loss in percentage
	isStopTrailing: true,                         // Enable trailing stop
	useMarketOrders: true                         // Use market orders
);
```

## Vantagens da API de Alto Nível

A API de alto nível em estratégias StockSharp fornece as seguintes vantagens:

1. **Redução do Volume de Código** - a execução de tarefas comuns requer menos linhas de código

2. **Separação de Responsabilidades** - a lógica de negociação é separada dos detalhes técnicos de processamento e visualização de dados

3. **Melhor Legibilidade** - o código torna-se mais compreensível e expressivo, focado na lógica de negócio

4. **Menor Probabilidade de Erros** - muitos erros típicos são eliminados através da automatização de tarefas rotineiras

5. **Trabalho com Tipos de Dados Simples** - em vez de trabalhar com objetos complexos, pode operar com tipos de dados simples (por exemplo, `decimal`)

## Exemplo de Estratégia Usando a API de Alto Nível

Abaixo está um exemplo completo de uma estratégia que demonstra a utilização da API de alto nível:

```cs
public class SmaStrategy : Strategy
{
	private bool? _isShortLessThenLong;

	public SmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
		_long = Param(nameof(Long), 80);
		_short = Param(nameof(Short), 30);
		_takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
		_stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
	}

	private readonly StrategyParam<DataType> _candleType;
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}

	private readonly StrategyParam<int> _long;
	public int Long
	{
		get => _long.Value;
		set => _long.Value = value;
	}

	private readonly StrategyParam<int> _short;
	public int Short
	{
		get => _short.Value;
		set => _short.Value = value;
	}

	private readonly StrategyParam<Unit> _takeValue;
	public Unit TakeValue
	{
		get => _takeValue.Value;
		set => _takeValue.Value = value;
	}

	private readonly StrategyParam<Unit> _stopValue;
	public Unit StopValue
	{
		get => _stopValue.Value;
		set => _stopValue.Value = value;
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		// Create indicators
		var longSma = new SMA { Length = Long };
		var shortSma = new SMA { Length = Short };

		// Create a candle subscription and bind to indicators
		var subscription = SubscribeCandles(CandleType);
		subscription
			.Bind(longSma, shortSma, OnProcess)
			.Start();

		// Configure visualization
		var area = CreateChartArea();
		if (area != null)
		{
			DrawCandles(area, subscription);
			DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
			DrawIndicator(area, longSma);
			DrawOwnTrades(area);
		}

		// Start position protection
		StartProtection(TakeValue, StopValue);
	}

	private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
	{
		// Process only finished candles
		if (candle.State != CandleStates.Finished)
			return;

		// Trading logic based on indicator crossover
		var isShortLessThenLong = shortValue < longValue;

		if (_isShortLessThenLong == null)
		{
			_isShortLessThenLong = isShortLessThenLong;
		}
		else if (_isShortLessThenLong != isShortLessThenLong)
		{
			// Crossover occurred
			var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
			var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
			var priceStep = GetSecurity().PriceStep ?? 1;
			var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

			// Place an order
			if (direction == Sides.Buy)
				BuyLimit(price, volume);
			else
				SellLimit(price, volume);

			// Save current indicator position
			_isShortLessThenLong = isShortLessThenLong;
		}
	}
}
```

## Conclusão

A API de alto nível em StockSharp simplifica significativamente o desenvolvimento de estratégias de negociação, permitindo que os programadores se concentrem na lógica de negociação em vez de detalhes técnicos. É especialmente útil para casos de utilização típicos em que não é necessária afinação detalhada do processamento de dados ou da visualização.

Combinada com o sistema de parâmetros de estratégia, o modelo de eventos e os mecanismos de proteção de posições, a API de alto nível torna StockSharp uma ferramenta poderosa e conveniente para negociação algorítmica, adequada tanto para iniciantes como para programadores experientes.
