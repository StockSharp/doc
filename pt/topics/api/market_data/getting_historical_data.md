# Obter Dados Históricos

A API StockSharp fornece mecanismos convenientes para obter dados históricos, que podem ser utilizados tanto para testar estratégias de negociação como para construir [Indicadores](../indicators.md).

## Obter Dados Históricos via Connector

### Configurar a Ligação

Para obter dados históricos, primeiro tem de configurar uma ligação ao sistema de negociação:

```cs
// Criar uma instância de Connector
var connector = new Connector();

// Adicionar um adaptador para ligação à Binance
var messageAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API Key>",
	Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);

// Ligar
connector.Connect();
```

A ligação também pode ser configurada utilizando a interface gráfica, conforme descrito na secção [Janela de definições de ligação](../graphical_user_interface/connection_settings_window.md).

### Subscrever Velas Históricas

Para receber velas históricas, tem de criar uma subscrição e especificar os parâmetros dos dados pedidos:

```cs
// Criar uma subscrição para velas de 5 minutos para o instrumento seleccionado
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		// Especificar o período para o qual obter dados históricos
		From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now,
		// Definir a flag para receber apenas velas concluídas
		IsFinishedOnly = true
	}
};

// Subscrever o evento de vela recebida
connector.CandleReceived += OnCandleReceived;

// Iniciar a subscrição
connector.Subscribe(subscription);

// Manipulador do evento para receber velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Verificar se a vela pertence à nossa subscrição
	if (subscription != _subscription)
		return;

	// Processar a vela recebida
	Console.WriteLine($"Candle recebido: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");

	// Para apresentação no gráfico, pode utilizar:
	// Chart.Draw(_candleElement, candle);
}
```

### Utilizar Velas em Gráficos

As velas recebidas podem ser apresentadas num gráfico utilizando os componentes gráficos incorporados do StockSharp:

```cs
// Criar e configurar elementos do gráfico
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// Adicionar área e elemento ao gráfico
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// No manipulador do evento CandleReceived, desenhar velas
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Verificar se a vela pertence à nossa subscrição
	if (subscription != _subscription)
		return;

	// Se precisar de apresentar apenas velas concluídas
	if (candle.State == CandleStates.Finished)
	{
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(candleElement, candle);
		chart.Draw(chartData);
	}
}
```

## Obter Outros Tipos de Dados Históricos

De modo semelhante, pode obter outros tipos de dados históricos:

### Obter Ticks Históricos

```cs
var tickSubscription = new Subscription(DataType.Ticks, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
		To = DateTime.Now
	}
};

connector.TickTradeReceived += (subscription, tick) =>
{
	if (subscription == tickSubscription)
		Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

connector.Subscribe(tickSubscription);
```

### Obter Livros de Ofertas Históricos

```cs
var depthSubscription = new Subscription(DataType.MarketDepth, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
		To = DateTime.Now
	}
};

connector.OrderBookReceived += (subscription, depth) =>
{
	if (subscription == depthSubscription)
		Console.WriteLine($"Livro de ofertas: {depth.ServerTime}, melhor bid: {depth.GetBestBid()?.Price}, melhor ask: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## Ver Também

- [Velas](../candles.md)
- [Subscrições](subscriptions.md)
- [Indicadores](../indicators.md)
