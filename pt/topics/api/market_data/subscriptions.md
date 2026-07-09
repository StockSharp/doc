# Subscrições

A **API StockSharp** oferece um modelo de aquisição de dados baseado em subscrições. Este é um mecanismo universal para receber tanto dados de mercado como informação de transacções. Esta abordagem tem vantagens significativas:

- **Isolamento da subscrição** — cada subscrição funciona de forma independente, permitindo executar em paralelo qualquer número de subscrições com diferentes parâmetros (com ou sem pedido de histórico).
- **Acompanhamento de estado** — as subscrições têm estados específicos que permitem controlar se os dados históricos estão actualmente a ser transmitidos ou se a subscrição mudou para modo em tempo real.
- **Universalidade** — o código para trabalhar com subscrições é o mesmo independentemente dos tipos de dados pedidos, tornando o desenvolvimento mais eficiente.

Para trabalhar com subscrições, tem de utilizar a classe [Subscription](xref:StockSharp.BusinessEntities.Subscription). Vejamos exemplos de utilização de subscrições para obter vários tipos de dados.

## Exemplo de Subscrição de Velas

```cs
// Criar uma subscrição para velas de 5 minutos
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	// Configurar parâmetros da subscrição através da propriedade MarketData
	MarketData =
	{
		// Pedir dados dos últimos 30 dias
		From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
		// null significa que a subscrição mudará para modo em tempo real após receber o histórico
		To = null
	}
};

// Processar velas recebidas
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;

	// Processar a vela
	Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// Tratar a transição da subscrição para modo online
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine("Subscription switched to real-time mode");
};

// Tratar erros da subscrição
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine($"Erro de subscrição: {error}");
};

// Iniciar a subscrição
_connector.Subscribe(subscription);
```

## Exemplo de Subscrição de Livro de Ofertas

```cs
// Criar uma subscrição para o livro de ofertas do instrumento seleccionado
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// Processar livros de ofertas recebidos
_connector.OrderBookReceived += (sub, depth) =>
{
	if (sub != depthSubscription)
		return;

	// Processar o livro de ofertas
	Console.WriteLine($"Livro de ofertas: {depth.SecurityId}, hora: {depth.ServerTime}");
	Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// Iniciar a subscrição
_connector.Subscribe(depthSubscription);
```

## Exemplo de Subscrição de Negócios Tick

```cs
// Criar uma subscrição de negócios tick para o instrumento seleccionado
var tickSubscription = new Subscription(DataType.Ticks, security);

// Processar ticks recebidos
_connector.TickTradeReceived += (sub, tick) =>
{
	if (sub != tickSubscription)
		return;

	// Processar o tick
	Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// Iniciar a subscrição
_connector.Subscribe(tickSubscription);
```

## Exemplo de Subscrição com Configuração do Modo de Construção de Velas

```cs
// Subscrição de velas de 5 minutos que serão construídas a partir de ticks
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	MarketData =
	{
		// Especificar o modo de construção e a fonte de dados
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		// Também pode activar a construção do perfil de volume
		IsCalcVolumeProfile = true,
	}
};

_connector.Subscribe(candleSubscription);
```

## Exemplo de Subscrição Level1 (Informação Básica do Instrumento)

```cs
// Criar uma subscrição para informação básica do instrumento
var level1Subscription = new Subscription(DataType.Level1, security);

// Processar dados Level1 recebidos
_connector.Level1Received += (sub, level1) =>
{
	if (sub != level1Subscription)
		return;

	Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");

	// Escrever valores dos campos Level1
	foreach (var pair in level1.Changes)
	{
		Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
	}
};

// Iniciar a subscrição
_connector.Subscribe(level1Subscription);
```

## Anular Subscrição de Dados

Para parar de receber dados, utilize o método `UnSubscribe`:

```cs
// Anular a subscrição de uma subscrição específica
_connector.UnSubscribe(subscription);

// Ou pode anular todas as subscrições
foreach (var sub in _connector.Subscriptions)
{
	_connector.UnSubscribe(sub);
}
```

## Estados da Subscrição

As subscrições podem estar nos seguintes estados:

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) — a subscrição está inactiva (parada ou não iniciada).
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) — a subscrição está activa e pode transmitir dados históricos até mudar para modo em tempo real ou concluir.
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) — a subscrição está inactiva e em estado de erro.
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) — a subscrição concluiu o seu trabalho (todos os dados recebidos).
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) — a subscrição mudou para modo em tempo real e transmite apenas dados actuais.
