# Compatibilidade da Estratégia com Plataformas StockSharp

Ao desenvolver estratégias de negociação no StockSharp, é importante considerar a sua compatibilidade com várias plataformas: [Designer](../../designer.md), [Shell](../../shell.md), [Runner](../../runner.md) e [testes na cloud](../../designer/backtesting/cloud_backtesting.md). Ao seguir as recomendações abaixo, criará uma estratégia que funciona correctamente em todos os ambientes.

## Parâmetros do Construtor da Estratégia

### Evitar Parâmetros no Construtor

Para garantir a compatibilidade com as plataformas StockSharp, especialmente com testes na cloud, **não deve adicionar parâmetros ao construtor da estratégia**:

```cs
// Correct: constructor without parameters
public class SmaStrategy : Strategy
{
	public SmaStrategy()
	{
		// Parameters initialization
	}
}

// Incorrect: constructor with parameters
public class SmaStrategy : Strategy
{
	public SmaStrategy(int longLength, int shortLength) // Don't use this approach
	{
		// ...
	}
}
```

As plataformas StockSharp criam instâncias de estratégias usando um construtor sem parâmetros. Se a sua estratégia exigir um construtor com parâmetros, não será inicializada correctamente.

## Usar StrategyParam em Vez de Propriedades Normais

### Vantagens de StrategyParam

Em vez de criar propriedades C# normais e depois sobrepor os métodos `Save` e `Load`, use [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) para todos os parâmetros personalizáveis:

```cs
// Correct: using StrategyParam
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
	get => _longSmaLength.Value;
	set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
	_longSmaLength = Param(nameof(LongSmaLength), 80)
						.SetDisplay("Long SMA length", string.Empty, "Base settings");
}

// Incorrect: using regular properties
private int _longSmaLength = 80; // Don't use this approach

public int LongSmaLength
{
	get => _longSmaLength;
	set => _longSmaLength = value;
}
```

Os parâmetros criados através de [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) irão automaticamente:
- Ser apresentados nas interfaces de utilizador da plataforma
- Ser guardados e carregados sem sobrepor os métodos `Save` e `Load`
- Ser usados na optimização
- Ser serializados correctamente quando enviados para testes na cloud

## Trabalhar com a Interface de Utilizador

### Usar Abstracções em Vez de Acesso Directo à UI

Em vez de aceder directamente aos elementos da interface de utilizador, use as abstracções fornecidas pelo StockSharp:

```cs
// Correct approach: using IChart
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Get the chart provided by the runtime environment
	_chart = GetChart();
	
	if (_chart != null)
	{
		// Chart is available (e.g., in Designer or Shell)
		InitChart();
	}
	else
	{
		// Chart is unavailable (e.g., in Runner or cloud backtesting)
		// Strategy continues to work without visualization
	}
}

private void InitChart()
{
	// Configure chart through the abstract interface
	_chart.ClearAreas();
	var area = _chart.AddArea();
	_chartCandleElement = area.AddCandles();
	// ...
}
```

O método [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) devolve uma interface [IChart](xref:StockSharp.Charting.IChart) se existir um gráfico disponível no ambiente de execução actual. Se a estratégia estiver a correr no [Runner](../../runner.md) de consola ou em testes na cloud, onde não existe interface gráfica, o método devolverá `null`.

A interface [IChart](xref:StockSharp.Charting.IChart) fornece métodos para trabalhar com gráficos:
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - para adicionar uma área ao gráfico
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - para remover uma área
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - para adicionar um elemento ao gráfico
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - para remover um elemento
- [Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - para repor valores dos elementos

### Verificar a Disponibilidade do Gráfico

Verifique sempre a disponibilidade do gráfico antes de o usar:

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
	if (_chart == null) return; // Important check
	
	var data = _chart.CreateData();
	data.Group(candle.OpenTime)
		.Add(_chartCandleElement, candle)
		.Add(_longSmaIndicatorElement, longSma)
		.Add(_shortSmaIndicatorElement, shortSma);
	_chart.Draw(data);
}
```

## Threads e Sincronização

### Evitar Criar Threads Adicionais

No StockSharp, **não precisa de criar threads adicionais** para processamento de dados. Todos os eventos (dados de mercado, transacções) chegam numa única thread:

```cs
// Correct: using standard event handlers
private void ProcessCandle(ICandleMessage candle)
{
	// Process candle in the main thread
	var longSmaIsFormedPrev = _longSma.IsFormed;
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	
	// ...
}

// Incorrect: creating additional threads
private void ProcessCandle(ICandleMessage candle)
{
	// DON'T do this
	Task.Run(() => {
		var longSmaIsFormedPrev = _longSma.IsFormed;
		// ...
	});
}
```

### Evitar Objectos de Sincronização

Como todos os eventos são processados numa única thread, **não há necessidade de usar objectos de sincronização**:

```cs
// Correct: regular processing without synchronization
private void ProcessCandle(ICandleMessage candle)
{
	var ls = _longSma.Process(candle);
	var ss = _shortSma.Process(candle);
	// ...
}

// Incorrect: unnecessary synchronization
private readonly object _syncLock = new object(); // Not needed

private void ProcessCandle(ICandleMessage candle)
{
	lock (_syncLock) // Not needed
	{
		var ls = _longSma.Process(candle);
		// ...
	}
}
```

## Recursos Externos

### Usar a Infraestrutura StockSharp

Em vez de aceder directamente a recursos externos (ficheiros, bases de dados, rede), use as capacidades fornecidas pelas plataformas StockSharp:

```cs
// Correct: using built-in mechanisms for data saving
protected override void OnStopped()
{
	// Data is automatically saved through strategy parameters
	base.OnStopped();
}

// Incorrect: direct access to external resources
protected override void OnStopped()
{
	// DON'T do this
	File.WriteAllText("results.txt", $"PnL: {PnL}");
	
	// or this
	using (var connection = new SqlConnection("..."))
	{
		// ...
	}
	
	base.OnStopped();
}
```

### Armazenamento de Dados

Para guardar resultados da estratégia, use:

- [Parâmetros da estratégia](parameters.md) para definições e configuração
- Mecanismos de armazenamento integrados no [Designer](../../designer.md) e no [Shell](../../shell.md)
- [Estatísticas](xref:StockSharp.Algo.Statistics.StatisticManager) para recolher métricas de negociação

### Métodos Save e Load

Os métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) e [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) foram concebidos especificamente para guardar dados adicionais da estratégia que não são definições ou parâmetros. Este é o local ideal para guardar dados necessários para restaurar o estado da estratégia:

```cs
public override void Save(SettingsStorage settings)
{
	base.Save(settings); // First save strategy parameters
	
	// Then save custom data
	settings.SetValue("CustomState", _customState);
	settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
	base.Load(settings); // First load strategy parameters
	
	// Then load custom data
	if (settings.Contains("CustomState"))
		_customState = settings.GetValue<string>("CustomState");
	
	if (settings.Contains("LastSignalTime"))
		_lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

No entanto, os principais parâmetros configuráveis devem continuar a ser implementados através de [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1), como descrito acima, pois isso garantirá a sua apresentação automática na interface de utilizador.

## Subscrição de Dados de Mercado

### Usar Regras em Vez de Subscrição Directa

Para processar dados de mercado, recomenda-se usar o [Modelo de Eventos](event_model.md) e regras:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
	_longSma = new SimpleMovingAverage { Length = LongSmaLength };

	Indicators.Add(_shortSma);
	Indicators.Add(_longSma);
	
	var subscription = new Subscription(Series, Security);

	// Correct: using rules for data processing
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Subscribe(subscription);
}
```

As regras têm várias vantagens importantes sobre processadores de eventos normais:

1. **Cancelamento automático da subscrição** - as regras cancelam automaticamente a subscrição dos eventos quando a estratégia pára ou quando já não são necessárias. Não precisa de gerir subscrições manualmente.

2. **API de alto nível** - as regras fornecem uma interface mais compreensível e conveniente do que processadores de eventos padrão. Por exemplo, `WhenCandlesFinished` é muito mais claro do que subscrever o evento `CandleReceived` com uma verificação posterior do estado da vela.

3. **Combinação de condições** - as regras podem ser combinadas usando operadores como `And`, `Or` e outros, criando condições de activação complexas:

```cs
// Example of combining rules
var tickSub = new Subscription(DataType.Ticks, Security);

tickSub
	.WhenTickTradeReceived(this)
	.And(Portfolio.WhenChanged(Connector))
	.Do(() => {
		// Code that executes only when there is a new trade
		// and the portfolio balance changes
	})
	.Apply(this);

Subscribe(tickSub);
```

4. **Gestão do ciclo de vida** - as regras podem ser tornadas de utilização única (`Once()`), ter condições de cancelamento definidas (`Until()`), adicionar acções diferidas, etc.

## Exemplo de Estratégia Compatível

Abaixo está um exemplo de uma estratégia que segue todas as recomendações e funcionará correctamente em todas as plataformas StockSharp:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<DataType> _series;
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;

	public DataType Series
	{
		get => _series.Value;
		set => _series.Value = value;
	}

	public int LongSmaLength
	{
		get => _longSmaLength.Value;
		set => _longSmaLength.Value = value;
	}

	public int ShortSmaLength
	{
		get => _shortSmaLength.Value;
		set => _shortSmaLength.Value = value;
	}

	private SimpleMovingAverage _longSma;
	private SimpleMovingAverage _shortSma;
	private IChart _chart;
	private IChartCandleElement _chartCandleElement;
	private IChartIndicatorElement _longSmaIndicatorElement;
	private IChartIndicatorElement _shortSmaIndicatorElement;

	public SmaStrategy()
	{
		_longSmaLength = Param(nameof(LongSmaLength), 80)
							.SetDisplay("Long SMA length", string.Empty, "Base settings")
							.SetCanOptimize(true);
							
		_shortSmaLength = Param(nameof(ShortSmaLength), 30)
							.SetDisplay("Short SMA length", string.Empty, "Base settings")
							.SetCanOptimize(true);
							
		_series = Param(nameof(Series), TimeSpan.FromMinutes(15).TimeFrame())
					.SetDisplay("Series", string.Empty, "Base settings");
	}

	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);

		_longSma = new SimpleMovingAverage { Length = LongSmaLength };
		_shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

		Indicators.Add(_shortSma);
		Indicators.Add(_longSma);
		
		// Initialize chart if available
		_chart = GetChart();
		if (_chart != null)
			InitChart();
		
		var subscription = new Subscription(Series, Security);

		Connector
			.WhenCandlesFinished(subscription)
			.Do(ProcessCandle)
			.Apply(this);

		Subscribe(subscription);
	}

	private void InitChart()
	{
		_chart.ClearAreas();
		var area = _chart.AddArea();
		
		_chartCandleElement = area.AddCandles();
		
		_longSmaIndicatorElement = area.AddIndicator(_longSma);
		_longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
		_longSmaIndicatorElement.DrawStyle = DrawStyles.Line;
		
		_shortSmaIndicatorElement = area.AddIndicator(_shortSma);
		_shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
		_shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
	}

	private void ProcessCandle(ICandleMessage candle)
	{
		var ls = _longSma.Process(candle);
		var ss = _shortSma.Process(candle);
		
		// Draw on chart if available
		if (_chart != null)
		{
			var data = _chart.CreateData();
			data.Group(candle.OpenTime)
				.Add(_chartCandleElement, candle)
				.Add(_longSmaIndicatorElement, ls)
				.Add(_shortSmaIndicatorElement, ss);
			_chart.Draw(data);
		}
		
		if (!_longSma.IsFormed)
			return;
			
		var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
		var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

		if (isShortLessCurrent == isShortLessPrev)
			return;
			
		// Trading logic
		var volume = Volume + Math.Abs(Position);

		if (isShortLessCurrent)
			SellMarket(volume);
		else
			BuyMarket(volume);
	}
}
```

## Ver também

- [Parâmetros da Estratégia](parameters.md)
- [Modelo de Eventos](event_model.md)
- [Logging da Estratégia](logging.md)
