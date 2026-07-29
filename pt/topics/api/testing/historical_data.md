# Dados históricos

O teste com dados históricos permite tanto a análise de mercado para encontrar padrões como a [otimização de parâmetros da estratégia](optimization.md). O trabalho principal é realizado pela classe [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector), que obtém os dados armazenados num repositório local através de uma [API](../market_data_storage/api.md) especial. Parâmetros adicionais são descritos na secção [Definições de Teste](extended_settings.md).

O teste pode ser realizado usando vários tipos de dados de mercado:
- Transações tick ([ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage))
- Livros de ordens ([IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage))
- Velas de diferentes períodos
- [OrderLog](xref:StockSharp.Messages.IOrderLogMessage)
- [Level1](xref:StockSharp.Messages.Level1ChangeMessage) (melhores preços bid e ask)
- Combinações de diferentes tipos de dados

Se não existirem livros de ordens guardados para o período de teste, estes podem ser gerados com base nas transações usando [MarketDepthGenerator](xref:StockSharp.Algo.Testing.MarketDepthGenerator) ou reconstruídos a partir do registo de ordens usando [OrderLogMarketDepthBuilder](xref:StockSharp.Messages.OrderLogMarketDepthBuilder).

Os dados para testes históricos devem ser descarregados e guardados antecipadamente num formato especial [S#](../../api.md). Isto pode ser feito manualmente usando [Conectores](../connectors.md) e a [Storage API](../market_data_storage/api.md), ou configurando e executando a aplicação especial [Hydra](../../hydra.md).

## Etapas principais do teste histórico

### 1. Configurar o armazenamento de dados

O primeiro passo é criar um objeto [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) através do qual [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) irá aceder aos dados históricos:

```csharp
// armazenamento para aceder a dados históricos
var storageRegistry = new StorageRegistry
{
	// definir caminho para o diretório com dados históricos
	DefaultDrive = new LocalMarketDataDrive(HistoryPath.Folder)
};
```

> [!CAUTION]
> O construtor [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive) recebe o caminho para o diretório raiz onde o histórico de **todos os instrumentos** está armazenado, não para um diretório com um instrumento específico. Por exemplo, se o ficheiro HistoryData.zip foi descompactado para o diretório *C:\\MarketData\\AAPL@NASDAQ\\*, deve passar o caminho *C:\\MarketData\\* para [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive). Mais detalhes na secção [API](../market_data_storage/api.md).

### 2. Criar instrumentos e carteiras

```csharp
// criar instrumento de teste para o teste
var security = new Security
{
	Id = SecId.Text, // o ID do instrumento corresponde ao nome da pasta com dados históricos
	Code = secCode,
	Board = board,
};

// carteira de teste
var portfolio = new Portfolio
{
	Name = "conta de teste",
	BeginValue = 1000000,
};
```

### 3. Criar o conector de emulação

```csharp
// criar conector para emulação
var connector = new HistoryEmulationConnector(
	new[] { security },
	new[] { portfolio })
{
	EmulationAdapter =
	{
		Emulator =
		{
			Settings =
			{
				// executar a ordem se o preço histórico tocar no preço da nossa ordem limitada
				// Por predefinição está desativado; o preço deve atravessar o preço da ordem limitada
				// (modo de teste mais rigoroso)
				MatchOnTouch = false,
				
				// comissão para transações
				CommissionRules = new ICommissionRule[]
				{
					new CommissionPerTradeRule { Value = 0.01m },
				}
			}
		}
	},
	UseExternalCandleSource = emulationInfo.UseCandle != null,
	CreateDepthFromOrdersLog = emulationInfo.UseOrderLog,
	CreateTradesFromOrdersLog = emulationInfo.UseOrderLog,
	HistoryMessageAdapter =
	{
		StorageRegistry = storageRegistry,
		// definir intervalo de teste
		StartDate = startTime,
		StopDate = stopTime,
		OrderLogMarketDepthBuilders =
		{
			{ secId, new ItchOrderLogMarketDepthBuilder(secId) }
		}
	},
	// definir intervalo de atualização da hora de mercado
	MarketTimeChangedInterval = timeFrame,
};
```

### 4. Subscrever eventos e configurar a geração de dados

Ao ligar, configuramos a receção dos dados necessários dependendo dos parâmetros de teste:

```csharp
connector.SecurityReceived += (subscr, s) =>
{
	if (s != security)
		return;
		
	// preencher valores Level1
	connector.EmulationAdapter.SendInMessage(level1Info);
	
	// subscrever os dados necessários dependendo das definições de teste
	if (emulationInfo.UseMarketDepth)
	{
		connector.Subscribe(new(DataType.MarketDepth, security));
		
		// se for necessário gerar livros de ordens
		if (generateDepths || emulationInfo.UseCandle != null)
		{
			// se não houver dados históricos de livro de ordens disponíveis, mas forem exigidos pela estratégia,
			// usar o gerador baseado nos últimos preços
			connector.RegisterMarketDepth(new TrendMarketDepthGenerator(connector.GetSecurityId(security))
			{
				Interval = TimeSpan.FromSeconds(1), // frequência de atualização do livro de ordens - 1 s
				MaxAsksDepth = maxDepth,
				MaxBidsDepth = maxDepth,
				UseTradeVolume = true,
				MaxVolume = maxVolume,
				MinSpreadStepCount = 2,
				MaxSpreadStepCount = 5,
				MaxPriceStepCount = 3
			});
		}
	}
	
	if (emulationInfo.UseOrderLog)
	{
		connector.Subscribe(new(DataType.OrderLog, security));
	}
	
	if (emulationInfo.UseTicks)
	{
		connector.Subscribe(new(DataType.Ticks, security));
	}
	
	if (emulationInfo.UseLevel1)
	{
		connector.Subscribe(new(DataType.Level1, security));
	}
	
	// iniciar a estratégia antes de a emulação começar
	strategy.Start();
	
	// iniciar o carregamento de dados históricos
	connector.Start();
};
```

### 5. Criar e configurar a estratégia

```csharp
// criar estratégia de negociação baseada em médias móveis com períodos 80 e 10
var strategy = new SmaStrategy
{
	LongSma = 80,
	ShortSma = 10,
	Volume = 1,
	Portfolio = portfolio,
	Security = security,
	Connector = connector,
	LogLevel = DebugLogCheckBox.IsChecked == true ? LogLevels.Debug : LogLevels.Info,
	// o intervalo predefinido é 1 min, o que é excessivo para um intervalo de vários meses
	UnrealizedPnLInterval = ((stopTime - startTime).Ticks / 1000).To<TimeSpan>()
};

// configurar o tipo de dados usado para construir velas
if (emulationInfo.UseCandle != null)
{
	strategy.CandleType = emulationInfo.UseCandle;
	
	if (strategy.CandleType != TimeSpan.FromMinutes(1).TimeFrame())
	{
		strategy.BuildFrom = TimeSpan.FromMinutes(1).TimeFrame();
	}
}
else if (emulationInfo.UseTicks)
	strategy.BuildFrom = DataType.Ticks;
else if (emulationInfo.UseLevel1)
{
	strategy.BuildFrom = DataType.Level1;
	strategy.BuildField = emulationInfo.BuildField;
}
else if (emulationInfo.UseOrderLog)
	strategy.BuildFrom = DataType.OrderLog;
else if (emulationInfo.UseMarketDepth)
	strategy.BuildFrom = DataType.MarketDepth;
```

### 6. Visualizar resultados

Para apresentar visualmente os resultados do teste, subscrevemos alterações de P&L e de posição:

```csharp
var pnlCurve = equity.CreateCurve(LocalizedStrings.PnL + " " + emulationInfo.StrategyName, Colors.Green, Colors.Red, DrawStyles.Area);
var realizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLRealized + " " + emulationInfo.StrategyName, Colors.Black, DrawStyles.Line);
var unrealizedPnLCurve = equity.CreateCurve(LocalizedStrings.PnLUnreal + " " + emulationInfo.StrategyName, Colors.DarkGray, DrawStyles.Line);
var commissionCurve = equity.CreateCurve(LocalizedStrings.Commission + " " + emulationInfo.StrategyName, Colors.Red, DrawStyles.DashedLine);

strategy.PnLReceived2 += (s, pf, t, r, u, c) =>
{
	var data = equity.CreateData();

	data
		.Group(t)
		.Add(pnlCurve, r - (c ?? 0))
		.Add(realizedPnLCurve, r)
		.Add(unrealizedPnLCurve, u ?? 0)
		.Add(commissionCurve, c ?? 0);

	equity.Draw(data);
};

var posItems = pos.CreateCurve(emulationInfo.StrategyName, emulationInfo.CurveColor, DrawStyles.Line);

strategy.PositionReceived += (s, p) =>
{
	var data = pos.CreateData();

	data
		.Group(p.LocalTime)
		.Add(posItems, p.CurrentValue);

	pos.Draw(data);
};

// subscrever atualizações de progresso
connector.ProgressChanged += steps => this.GuiAsync(() => progressBar.Value = steps);
```

### 7. Iniciar o teste

```csharp
// iniciar emulação
connector.Connect();
```

## Implementação moderna de testes históricos

Nas versões mais recentes de [S#](../../api.md), o exemplo de teste histórico foi significativamente modernizado e agora permite testar estratégias usando vários tipos de dados de mercado:

- Ticks (transações)
- Livros de ordens
- Velas de diferentes períodos
- Registo de ordens
- Dados Level1 (melhores preços)
- Combinações de diferentes tipos de dados

É criado um separador separado com gráficos e estatísticas para cada tipo de dados:

```csharp
// criar modos de teste
_settings = new[]
{
	(
		TicksCheckBox,
		TicksProgress,
		TicksParameterGrid,
		// ticks
		new EmulationInfo
		{
			UseTicks = true,
			CurveColor = Colors.DarkGreen,
			StrategyName = LocalizedStrings.Ticks
		},
		TicksChart,
		TicksEquity,
		TicksPosition
	),

	(
		TicksAndDepthsCheckBox,
		TicksAndDepthsProgress,
		TicksAndDepthsParameterGrid,
		// ticks + livros de ordens
		new EmulationInfo
		{
			UseTicks = true,
			UseMarketDepth = true,
			CurveColor = Colors.Red,
			StrategyName = LocalizedStrings.TicksAndDepths
		},
		TicksAndDepthsChart,
		TicksAndDepthsEquity,
		TicksAndDepthsPosition
	),
	
	// outras combinações de tipos de dados
};
```

Esta abordagem permite comparar visualmente o desempenho da estratégia ao usar diferentes fontes de dados.

## Estratégia SMA melhorada

A estratégia de média móvel (SMA) foi redesenhada e agora usa uma abordagem mais moderna para a subscrição de dados e o processamento de velas:

```csharp
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// criar subscrição de velas do tipo necessário
	var dt = CandleTimeFrame is null
		? CandleType
		: DataType.Create(CandleType.MessageType, CandleTimeFrame);

	var subscription = new Subscription(dt, Security)
	{
		MarketData =
		{
			IsFinishedOnly = true,
			BuildFrom = BuildFrom,
			BuildMode = BuildFrom is null ? MarketDataBuildModes.LoadAndBuild : MarketDataBuildModes.Build,
			BuildField = BuildField,
		}
	};

	// criar indicadores
	var longSma = new SMA { Length = LongSma };
	var shortSma = new SMA { Length = ShortSma };

	// subscrever velas e ligá-los aos indicadores
	SubscribeCandles(subscription)
		.Bind(longSma, shortSma, OnProcess)
		.Start();

	// configurar apresentação no gráfico
	var area = CreateChartArea();

	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
		DrawIndicator(area, longSma);
		DrawOwnTrades(area);
	}

	// configurar proteção da posição
	StartProtection(TakeValue, StopValue);
}
```

O processamento de velas e as decisões de negociação estão agora separados num método dedicado:

```csharp
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	LogInfo(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

	// verificar se a vela está concluído
	if (candle.State != CandleStates.Finished)
		return;

	// analisar o cruzamento dos indicadores
	var isShortLessThenLong = shortValue < longValue;

	if (_isShortLessThenLong == null)
	{
		_isShortLessThenLong = isShortLessThenLong;
	}
	else if (_isShortLessThenLong != isShortLessThenLong) // ocorreu cruzamento
	{
		// se a curta for inferior à longa - vender; caso contrário, comprar
		var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;

		// calcular volume para abertura de posição ou reversão
		var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;

		// usar o preço de fecho da vela
		var price = candle.ClosePrice;

		if (direction == Sides.Buy)
			BuyLimit(price, volume);
		else
			SellLimit(price, volume);

		_isShortLessThenLong = isShortLessThenLong;
	}
}
```

## Definições adicionais de teste

Definições alargadas para testes estão disponíveis em [S#](../../api.md), incluindo:

- Geração de livros de ordens com parâmetros especificados
- Definições de comissão
- Definições de deslizamento de preço
- Emulação de atraso de execução

Estas definições são descritas com mais detalhe na secção [Definições de Teste](extended_settings.md).
