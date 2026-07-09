# Trabalhar com Gráficos em Estratégias

No StockSharp, a classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) fornece uma interface conveniente para visualizar a actividade de negociação num gráfico. Neste artigo, veremos como aceder a um gráfico a partir de uma estratégia, criar áreas (ChartArea), adicionar vários elementos (velas, indicadores, negócios) e renderizar dados.

## Aceder ao Gráfico

### Método GetChart

Para aceder ao gráfico a partir de uma estratégia, use o método [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart):

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Obtendo o gráfico
	_chart = GetChart();
	
	// Verificando disponibilidade do gráfico
	if (_chart != null)
	{
		// Inicializando o gráfico
		InitializeChart();
	}
	else
	{
		// O gráfico está indisponível, por exemplo, ao executar em modo console
		LogInfo("O gráfico não está disponível. Visualização desativada.");
	}
}
```

O método [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) devolve uma interface [IChart](xref:StockSharp.Charting.IChart) que fornece acesso às funções do gráfico. É importante verificar se o resultado é `null`, pois o gráfico pode estar indisponível, por exemplo, ao executar uma estratégia em modo de consola ou em testes na cloud.

### Método SetChart

Em alguns casos, o gráfico pode ser definido a partir do exterior. Para isso, use o método [Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)):

```cs
// Definindo gráfico a partir de fonte externa
public void ConfigureVisualization(IChart chart)
{
	SetChart(chart);
	
	if (chart != null)
	{
		InitializeChart();
	}
}
```

## Criar Áreas de Gráfico

Depois de obter acesso ao gráfico, pode criar uma ou mais áreas para apresentar vários dados. Use o método [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea):

```cs
private void InitializeChart()
{
	// Criando área principal para velas e indicadores
	_mainArea = CreateChartArea();
	
	// Criando área adicional para volume
	_volumeArea = CreateChartArea();
	
	// Configurando áreas e adicionando elementos
	ConfigureChartElements();
}
```

Também pode usar directamente o método [IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart)):

```cs
private void InitializeChart()
{
	// Limpar áreas existentes se necessário
	foreach (var area in _chart.Areas.ToArray())
		_chart.RemoveArea(area);
	
	// Criar área principal para velas e indicadores
	_mainArea = _chart.AddArea();
	
	// Criar área adicional para volume
	_volumeArea = _chart.AddArea();
	
	// Configurar áreas e adicionar elementos
	ConfigureChartElements();
}
```

## Adicionar Elementos ao Gráfico

Depois de criar áreas de gráfico, pode adicionar vários elementos para apresentar dados. O StockSharp suporta diferentes tipos de elementos, como velas, indicadores, negócios e ordens.

### Adicionar Velas

Para apresentar velas, use o método [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) da área do gráfico:

```cs
private void ConfigureChartElements()
{
	// Adicionando elemento de vela à área principal
	_candleElement = _mainArea.AddCandles();
	
	// Configurando exibição de velas
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick; // Japanese candles
	_candleElement.AntiAliasing = true; // Smoothing
	_candleElement.UpFillColor = Color.Green; // Rising candle body color
	_candleElement.DownFillColor = Color.Red; // Falling candle body color
	_candleElement.UpBorderColor = Color.DarkGreen; // Rising candle border color
	_candleElement.DownBorderColor = Color.DarkRed; // Falling candle border color
	_candleElement.StrokeThickness = 1; // Line thickness
	_candleElement.ShowAxisMarker = true; // Show Y-axis marker
}
```

A interface [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) fornece muitas propriedades para configurar a apresentação das velas:

- **DrawStyle** - estilo de apresentação das velas:
  - **CandleStick** - velas japonesas
  - **Ohlc** - barras
  - **LineOpen/LineHigh/LineLow/LineClose** - linhas para os respectivos preços
  - **BoxVolume** - caixas de volume
  - **ClusterProfile** - perfil de cluster
  - **Area** - área
  - **PnF** - gráfico de ponto e figura

- **Definições de cor**:
  - **UpFillColor/DownFillColor** - cor do corpo da vela ascendente/descendente
  - **UpBorderColor/DownBorderColor** - cor da margem da vela ascendente/descendente
  - **LineColor** - cor da linha para gráficos do tipo linha
  - **AreaColor** - cor da área para o tipo Area

- **Outras definições**:
  - **StrokeThickness** - espessura da linha
  - **AntiAliasing** - suavização
  - **ShowAxisMarker** - mostrar marcador do eixo Y

### Adicionar Indicadores

Para apresentar indicadores, use o método [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})):

```cs
// Criar indicadores
_sma = new SimpleMovingAverage { Length = SmaLength };
_bollinger = new BollingerBands
{
	Length = BollingerLength,
	Deviation = BollingerDeviation
};

// Adicionando indicadores à coleção da estratégia
Indicators.Add(_sma);
Indicators.Add(_bollinger);

// Visualizando indicadores
_smaElement = DrawIndicator(_mainArea, _sma, Color.Blue);
_bollingerUpperElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerLowerElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerMiddleElement = DrawIndicator(_mainArea, _bollinger, Color.Gray);
```

O método [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) cria automaticamente um elemento de indicador e adiciona-o à área de gráfico especificada. Pode especificar uma cor e uma cor adicional para apresentação.

Também pode adicionar um elemento de indicador directamente através do método [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) da área do gráfico:

```cs
// Adicionando SMA diretamente pela área do gráfico
var smaElement = _mainArea.AddIndicator(_sma);
smaElement.Color = Color.Blue;
smaElement.StrokeThickness = 2;
smaElement.DrawStyle = DrawStyles.Line;
smaElement.AntiAliasing = true;
smaElement.ShowAxisMarker = true;
smaElement.AutoAssignYAxis = true; // Atribuir automaticamente o eixo Y
```

A interface [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) fornece as seguintes propriedades para configuração:

- **Color** - cor principal do indicador
- **AdditionalColor** - cor adicional (para indicadores com duas linhas)
- **StrokeThickness** - espessura da linha
- **AntiAliasing** - suavização
- **DrawStyle** - estilo de desenho (linha, pontos, histograma, etc.)
- **ShowAxisMarker** - mostrar marcador do eixo Y
- **AutoAssignYAxis** - atribuir automaticamente o eixo Y

### Adicionar Negócios

Para apresentar negócios, use o método [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)):

```cs
// Adicionando elemento para exibir negociações
_tradesElement = DrawOwnTrades(_mainArea);

// Configurando exibição de negociações
_tradesElement.BuyBrush = Color.Green;  // Buy color
_tradesElement.SellBrush = Color.Red;   // Sell color
_tradesElement.PointSize = 10;          // Point size
```

### Adicionar Ordens

Para apresentar ordens, use o método [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)):

```cs
// Adicionando elemento para exibir ordens
_ordersElement = DrawOrders(_mainArea);

// Configurando exibição de ordens
_ordersElement.ActiveBrush = Color.Blue;     // Active orders color
_ordersElement.CanceledBrush = Color.Gray;   // Canceled orders color
_ordersElement.DoneBrush = Color.Green;      // Completed orders color
_ordersElement.ErrorColor = Color.Red;       // Error color
_ordersElement.PointSize = 8;                // Point size
```

A interface [IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) fornece as seguintes propriedades para configuração:

- **ActiveBrush** - cor das ordens activas
- **CanceledBrush** - cor das ordens canceladas
- **DoneBrush** - cor das ordens concluídas
- **ErrorColor** - cor de erro
- **ErrorStrokeColor** - cor da margem de erro
- **Filter** - filtro de apresentação de ordens

## Desenhar Dados no Gráfico

Depois de configurar todos os elementos do gráfico, pode prosseguir para desenhar os dados. São usados métodos diferentes consoante o tipo de dados.

### Desenhar Velas e Indicadores

A forma mais eficiente de desenhar dados é usar o método [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) com um objecto [IChartDrawData](xref:StockSharp.Charting.IChartDrawData):

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Processando vela nos indicadores
	var smaValue = _sma.Process(candle);
	var bollingerValue = _bollinger.Process(candle);
	
	// Se o gráfico não estiver disponível, ignorar o desenho
	if (_chart == null)
		return;
	
	// Criar dados para desenho
	var drawData = _chart.CreateData();
	
	// Agrupar dados pelo horário da vela
	var group = drawData.Group(candle.OpenTime);
	
	// Adicionar candle
	group.Add(_candleElement, 
		candle.DataType, 
		candle.SecurityId, 
		candle.OpenPrice, 
		candle.HighPrice, 
		candle.LowPrice, 
		candle.ClosePrice, 
		candle.PriceLevels, 
		candle.State);
	
	// Adicionar valores dos indicadores
	group.Add(_smaElement, smaValue);
	
	if (bollingerValue != null)
	{
		group.Add(_bollingerUpperElement, bollingerValue);
		group.Add(_bollingerMiddleElement, bollingerValue);
		group.Add(_bollingerLowerElement, bollingerValue);
	}
	
	// Desenhar dados no gráfico
	_chart.Draw(drawData);
}
```

O método [IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) cria um objecto [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) usado para agrupar e adicionar dados para diferentes elementos do gráfico. O agrupamento de dados é feito por carimbo temporal usando o método [Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset)).

Para adicionar dados de diferentes tipos, são usadas várias sobrecargas do método [Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates)) do objecto [IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem).

### Desenhar Negócios e Ordens

Para desenhar negócios e ordens, normalmente é usado um mecanismo automático accionado quando são recebidos novos negócios ou quando as ordens mudam. No entanto, se for necessário desenho manual, pode usar o seguinte código:

```cs
// Desenhando uma negociação
var tradeDrawData = _chart.CreateData();
var tradeGroup = tradeDrawData.Group(trade.Time);
tradeGroup.Add(_tradesElement, trade.Id, trade.StringId, trade.Side, trade.Price, trade.Volume);
_chart.Draw(tradeDrawData);

// Desenhando uma ordem
var orderDrawData = _chart.CreateData();
var orderGroup = orderDrawData.Group(order.Time);
orderGroup.Add(_ordersElement, order.Id, order.StringId, order.Side, order.Price, order.Volume);
_chart.Draw(orderDrawData);
```

## Exemplo Completo de Renderização de Gráfico numa Estratégia

Abaixo está um exemplo completo de uma estratégia com configuração e renderização de gráfico:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _smaLength;
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	
	private SimpleMovingAverage _sma;
	private BollingerBands _bollinger;
	
	private IChart _chart;
	private IChartArea _mainArea;
	private IChartArea _volumeArea;
	
	private IChartCandleElement _candleElement;
	private IChartIndicatorElement _smaElement;
	private IChartIndicatorElement _bollingerUpperElement;
	private IChartIndicatorElement _bollingerMiddleElement;
	private IChartIndicatorElement _bollingerLowerElement;
	private IChartOrderElement _ordersElement;
	private IChartTradeElement _tradesElement;
	
	public SmaStrategy()
	{
		_smaLength = Param(nameof(SmaLength), 20);
		_bollingerLength = Param(nameof(BollingerLength), 20);
		_bollingerDeviation = Param(nameof(BollingerDeviation), 2m);
	}
	
	public int SmaLength
	{
		get => _smaLength.Value;
		set => _smaLength.Value = value;
	}
	
	public int BollingerLength
	{
		get => _bollingerLength.Value;
		set => _bollingerLength.Value = value;
	}
	
	public decimal BollingerDeviation
	{
		get => _bollingerDeviation.Value;
		set => _bollingerDeviation.Value = value;
	}
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// Criar indicadores
		_sma = new SimpleMovingAverage { Length = SmaLength };
		_bollinger = new BollingerBands
		{
			Length = BollingerLength,
			Deviation = BollingerDeviation
		};
		
		// Adicionando indicadores à coleção da estratégia
		Indicators.Add(_sma);
		Indicators.Add(_bollinger);
		
		// Obtendo o gráfico
		_chart = GetChart();
		
		// Inicializando o gráfico se disponível
		if (_chart != null)
		{
			InitializeChart();
		}
		
		// Assinando velas
		var subscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			Security);
		
		subscription
			.WhenCandlesFinished(this)
			.Do(ProcessCandle)
			.Apply(this);
		
		Subscribe(subscription);
	}
	
	private void InitializeChart()
	{
		// Limpar áreas existentes
		foreach (var area in _chart.Areas.ToArray())
			_chart.RemoveArea(area);
		
		// Criar área principal para velas e indicadores
		_mainArea = _chart.AddArea();
		
		// Criar área adicional para volume
		_volumeArea = _chart.AddArea();
		
		// Configurar elementos do gráfico
		ConfigureChartElements();
	}
	
	private void ConfigureChartElements()
	{
		// Adicionando elemento para exibir velas
		_candleElement = _mainArea.AddCandles();
		_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
		_candleElement.AntiAliasing = true;
		_candleElement.UpFillColor = Color.Green;
		_candleElement.DownFillColor = Color.Red;
		_candleElement.UpBorderColor = Color.DarkGreen;
		_candleElement.DownBorderColor = Color.DarkRed;
		_candleElement.StrokeThickness = 1;
		_candleElement.ShowAxisMarker = true;
		
		// Adicionando elementos para indicadores
		_smaElement = _mainArea.AddIndicator(_sma);
		_smaElement.Color = Color.Blue;
		_smaElement.StrokeThickness = 2;
		
		_bollingerUpperElement = _mainArea.AddIndicator(_bollinger);
		_bollingerUpperElement.Color = Color.Purple;
		_bollingerUpperElement.StrokeThickness = 1;
		
		_bollingerMiddleElement = _mainArea.AddIndicator(_bollinger);
		_bollingerMiddleElement.Color = Color.Gray;
		_bollingerMiddleElement.StrokeThickness = 1;
		
		_bollingerLowerElement = _mainArea.AddIndicator(_bollinger);
		_bollingerLowerElement.Color = Color.Purple;
		_bollingerLowerElement.StrokeThickness = 1;
		
		// Adicionando elementos para ordens e negociações
		_ordersElement = DrawOrders(_mainArea);
		_tradesElement = DrawOwnTrades(_mainArea);
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Processando vela com indicadores
		var smaValue = _sma.Process(candle);
		var bollingerValue = _bollinger.Process(candle);
		
		// Se o gráfico não estiver disponível, ignorar o desenho
		if (_chart == null)
			return;
		
		// Desenhando dados no gráfico
		var drawData = _chart.CreateData();
		var group = drawData.Group(candle.OpenTime);
		
		// Adicionando vela
		group.Add(_candleElement, 
			candle.DataType, 
			candle.SecurityId, 
			candle.OpenPrice, 
			candle.HighPrice, 
			candle.LowPrice, 
			candle.ClosePrice, 
			candle.PriceLevels, 
			candle.State);
		
		// Adicionando valores dos indicadores
		group.Add(_smaElement, smaValue);
		
		if (bollingerValue != null)
		{
			group.Add(_bollingerUpperElement, bollingerValue);
			group.Add(_bollingerMiddleElement, bollingerValue);
			group.Add(_bollingerLowerElement, bollingerValue);
		}
		
		// Desenhando dados no gráfico
		_chart.Draw(drawData);
		
		// Lógica de negociação
		if (!IsFormed)
			return;
			
		// ... implementação da lógica de negociação ...
	}
}
```

## Conclusão

Usar gráficos em estratégias StockSharp permite visualizar a actividade de negociação, o que simplifica significativamente o desenvolvimento, a depuração e a monitorização de estratégias de negociação. A classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) fornece muitos métodos para trabalhar com gráficos, permitindo adicionar facilmente vários elementos e renderizar dados.

Ao desenvolver uma estratégia com interface gráfica, tenha sempre em conta que o gráfico pode estar indisponível, por exemplo, ao executar em modo de consola ou em testes na cloud. Por isso, é importante verificar se o resultado do método [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) é `null` e fornecer um cenário alternativo para a estratégia funcionar sem visualização.

## Ver Também

- [Indicadores em Estratégias](indicators.md)
- [Operações de Negociação em Estratégias](trading_operations.md)
