# Velas

O [S#](../api.md) suporta os seguintes tipos de candles:

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - um candle baseado em um intervalo de tempo, timeframe. Você pode definir tanto intervalos populares (minutos, horas, diário) quanto personalizados. Por exemplo, 21 segundos, 4,5 minutos, etc.
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - um candle de faixa de preço. Um novo candle é criado quando aparece uma negociação com um preço que excede os limites aceitáveis. O limite aceitável é formado a cada vez com base no preço da primeira negociação.
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - um candle é formado até que o volume total de negociações exceda um limite especificado. Se uma nova negociação exceder o volume permitido, ela é incluída em um novo candle.
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - o mesmo que [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage), mas o número de negociações é usado como limitação em vez do volume.
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - um candle de gráfico ponto-e-figura (gráfico X-O).
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - candle Renko.

Como trabalhar com candles é mostrado no exemplo localizado na pasta *Samples\/02\_Candles\/01\_Realtime*.

As imagens a seguir mostram os gráficos [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) e [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage):

![sample timeframecandles](../../images/sample_timeframecandles.png)

![sample rangecandles](../../images/sample_rangecandles.png)

## Iniciando a Obtenção de Dados

1. Para obter candles, crie uma assinatura usando a classe [Subscription](xref:StockSharp.BusinessEntities.Subscription):

```cs
// Criar uma assinatura para velas de 5 minutos
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // Tipo de dados com especificação do timeframe
	security)  // Instrumento
{
	// Configurar parâmetros adicionais pela propriedade MarketData
	MarketData =
	{
		// Período para o qual solicitamos dados históricos (últimos 30 dias)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. Para receber candles, assine o evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived), que sinaliza o aparecimento de um novo valor para processamento:

```cs
// Assinar o evento de recebimento de velas
_connector.CandleReceived += OnCandleReceived;

// Manipulador do evento de recebimento de velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Aqui subscription é o objeto de assinatura que criamos
	// candle — vela recebida

	// Verificar se a vela pertence à nossa assinatura
	if (subscription == _candleSubscription)
	{
		// Desenhar a vela no gráfico
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> O componente gráfico [Chart](xref:StockSharp.Xaml.Charting.Chart) é usado para exibir candles.

3. Em seguida, inicie a assinatura através do método [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Iniciar a assinatura
_connector.Subscribe(subscription);
```

Depois disso, o evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) começará a ser chamado.

4. O evento [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) é chamado não apenas quando um novo candle aparece, mas também quando o atual é alterado.

Se você precisar exibir apenas candles **"completos"**, você precisa verificar a propriedade [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) do candle recebido:

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Verificar se a vela pertence à nossa assinatura
	if (subscription != _candleSubscription)
		return;

	// Verificar se a vela está concluída
	if (candle.State == CandleStates.Finished)
	{
		// Criar dados para desenho
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);

		// Desenhar a vela no gráfico
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. Parâmetros adicionais podem ser configurados para a assinatura:

- **Modo de construção de candles** - determina se dados prontos serão solicitados ou construídos a partir de outro tipo de dado:

```cs
// Solicitar apenas dados prontos
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Construir apenas a partir de outro tipo de dados
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Solicitar dados prontos e, se não estiverem disponíveis, construí-los
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **Fonte para construção de candles** - indica a partir de qual tipo de dado construir candles se eles não estiverem diretamente disponíveis:

```cs
// Construção de velas a partir de negociações tick
subscription.MarketData.BuildFrom = DataType.Ticks;

// Construção de velas a partir do livro de ofertas
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Construção de velas a partir de Level1
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **Campo para construção de candles** - deve ser especificado para determinados tipos de dados:

```cs
// Construção de velas a partir do melhor preço bid em Level1
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Construção de velas a partir do melhor preço ask em Level1
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Construção de velas a partir do meio do spread no livro de ofertas
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **Perfil de volume** - cálculo do perfil de volume para candles:

```cs
// Ativar o cálculo do perfil de volume
subscription.MarketData.IsCalcVolumeProfile = true;
```

## Exemplos de Assinaturas para Diferentes Tipos de Candle

### Candles com Timeframe Padrão

```cs
// Velas de 5 minutos
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### Carregando Apenas Candles Históricos

```cs
// Carregar apenas velas históricas sem transição para tempo real
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // Specify end date
		BuildMode = MarketDataBuildModes.Load  // Only load ready-made data
	}
};
_connector.Subscribe(historicalSubscription);
```

### Construindo Candles de Timeframe Não Padrão a Partir de Ticks

```cs
// Velas com timeframe de 21 segundos construídas a partir de ticks
var customTimeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromSeconds(21)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(customTimeFrameSubscription);
```

### Construindo Candles a Partir de Dados do Livro de Ofertas

```cs
// Velas construídas a partir do meio do spread no livro de ofertas
var depthBasedSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle
	}
};
_connector.Subscribe(depthBasedSubscription);
```

### Candles com Perfil de Volume

```cs
// Velas de 5 minutos com cálculo do perfil de volume
var volumeProfileSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.LoadAndBuild,
		BuildFrom = DataType.Ticks,
		IsCalcVolumeProfile = true
	}
};
_connector.Subscribe(volumeProfileSubscription);
```

### Candles de Volume

```cs
// Velas de volume (cada vela contém volume de 1000 contratos)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // Specify candle type and volume
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(volumeCandleSubscription);
```

### Candles de Contagem de Ticks

```cs
// Velas por contagem de ticks (cada vela contém 1000 negociações)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // Specify candle type and number of trades
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(tickCandleSubscription);
```

### Candles de Faixa de Preço

```cs
// Velas de faixa de preço com intervalo de 0,1 unidade
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // Specify candle type and price range
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(rangeCandleSubscription);
```

### Candles Renko

```cs
// Velas Renko com passo de 0,1
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // Specify candle type and block size
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(renkoCandleSubscription);
```

### Candles Ponto e Figura (P&F)

```cs
// Velas Point and Figure
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // Specify P&F parameters
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(pnfCandleSubscription);
```

## Próximos Passos

[Gráfico](candles/chart.md)

[Tipo Personalizado de Candle](candles/custom_type_of_candle.md)
