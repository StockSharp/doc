# Criação de velas pela cesta de instrumentos

Para criar velas para [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity), [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) ou [ExpressionIndexSecurity](xref:StockSharp.Algo.Expressions.ExpressionIndexSecurity), é usado o mesmo mecanismo de assinatura que para instrumentos [Security](xref:StockSharp.BusinessEntities.Security) comuns.

Abaixo está um exemplo de criação de velas de 1 minuto para o spread AAPL - MSFT:

```cs
private Connector _connector;
private Security _instr1;
private Security _instr2;
private WeightedIndexSecurity _indexInstr;
private Subscription _indexSubscription;
private const string _secCode1 = "AAPL";
private const string _secCode2 = "MSFT";
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candleElement;

// Configuração da ligação e do conector
private void ConfigureConnector()
{
	if (_connector.Configure(this))
	{
		_connector.Save().Serialize(_connectorFile);
	}
}

// Configuração do gráfico
private void SetupChart()
{
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	_candleElement = new ChartCandleElement();
	_area.Elements.Add(_candleElement);

	// Assinar o evento de recebimento de velas
	_connector.CandleReceived += OnCandleReceived;
}

// Registro de serviços
private void RegisterServices()
{
	ConfigManager.RegisterService<ISecurityProvider>(_connector);
	ConfigManager.RegisterService<ICompilerService>(new RoslynCompilerService());
}

// Criar instrumento de índice e assinar velas
private void CreateIndexAndSubscribe()
{
	// Criar instrumento de índice (spread)
	_indexInstr = new WeightedIndexSecurity()
	{
		Board = ExchangeBoard.Nyse,
		Id = "IndexInstr"
	};

	// Adicionar instrumentos com pesos (1 e -1 para o spread)
	_indexInstr.Weights.Add(_instr1, 1);
	_indexInstr.Weights.Add(_instr2, -1);

	// Criar assinatura de velas do instrumento de índice
	_indexSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),  // velas de 1 minuto
		_indexInstr)  // Nosso instrumento de índice
	{
		MarketData =
		{
			// Configurar a assinatura para construir velas a partir de ticks
			BuildMode = MarketDataBuildModes.Build,
			BuildFrom = DataType.Ticks,

			// Solicitar dados históricos de 30 dias
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};

	// Adicionar elemento ao gráfico e vinculá-lo à assinatura
	_chart.AddElement(_area, _candleElement, _indexSubscription);

	// Iniciar assinatura
	_connector.Subscribe(_indexSubscription);
}

// Manipulador do evento de recebimento de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Verificar se a vela pertence à nossa assinatura
	if (subscription != _indexSubscription)
		return;

	// Se necessário, limitar o processamento apenas às velas concluídas
	if (candle.State != CandleStates.Finished)
		return;

	// Desenhar a vela no gráfico
	var chartData = new ChartDrawData();
	chartData.Group(candle.OpenTime).Add(_candleElement, candle);

	this.GuiAsync(() => _chart.Draw(chartData));
}

// Cancelar a assinatura ao fechar o aplicativo
private void Unsubscribe()
{
	if (_indexSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_indexSubscription);
		_indexSubscription = null;
	}
}
```

## Outros Casos de Uso para Assinaturas de Índice

### Criar uma subscrição para velas de índice a partir de velas de componentes

```cs
// Criar assinatura para construir velas de índice a partir das velas dos componentes
var indexFromCandlesSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	_indexInstr)
{
	MarketData =
	{
		// Configurar a assinatura para construir a partir das velas dos componentes
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Iniciar assinatura
_connector.Subscribe(indexFromCandlesSubscription);
```

### Criar uma subscrição para velas de índice a partir de livros de ofertas

```cs
// Criar assinatura para construir velas de índice a partir de livros de ofertas
var indexFromDepthSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	_indexInstr)
{
	MarketData =
	{
		// Configurar a assinatura para construir a partir de livros de ofertas
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle,  // Usar o meio do spread
		From = DateTime.Today.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

// Iniciar assinatura
_connector.Subscribe(indexFromDepthSubscription);
```

### Trabalhando com Índice de Volatilidade

```cs
// Criar índice de volatilidade baseado em expressão
var volatilityIndex = new ExpressionIndexSecurity
{
	Board = ExchangeBoard.Nyse,
	Id = "VOLX",
	Expression = "StdDev({0}, 20) / SMA({0}, 20) * 100",  // Fórmula para calcular a volatilidade
};

// Adicionar o instrumento principal ao índice
volatilityIndex.InnerSecurityIds.Add(_instr1.ToSecurityId());

// Criar assinatura de velas do índice de volatilidade
var volatilitySubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	volatilityIndex)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};

// Iniciar assinatura
_connector.Subscribe(volatilitySubscription);
```

## Veja também

[Futuros contínuos](../instruments/continuous_futures.md)

[Índice](../instruments/index.md)
