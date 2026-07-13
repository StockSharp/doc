# Definições de Teste

Esta secção descreve as principais definições de [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) para testar estratégias de negociação.

## Definições Básicas do Emulador

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - intervalo para a chegada do evento de alteração de tempo. Se forem usados geradores de negócios, os negócios serão gerados com esta frequência. A predefinição é 1 minuto.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - valor mínimo de atraso para ordens submetidas. A predefinição é TimeSpan.Zero, o que significa aceitação instantânea das ordens submetidas pela bolsa.
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - satisfazer ordens se o preço "tocar" no nível (esta suposição é por vezes demasiado "otimista" e deve ser desativada para testes realistas). Se desativada, as ordens limite serão executadas apenas se o preço "passar através delas" por pelo menos 1 passo. Esta opção funciona em todos os modos exceto no modo de registo de ordens. Está desativada por predefinição.
- [MarketEmulatorSettings.CandlePrice](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CandlePrice) - preço da vela usado para execução de ordens: Middle, Open, High, Low ou Close. A predefinição é Middle.
- [MarketEmulatorSettings.Failing](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Failing) - percentagem de falhas no registo de novas ordens. Os valores variam de 0 (sem falhas) a 100. A predefinição é desativado (0).
- [MarketEmulatorSettings.InitialOrderId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialOrderId) - número inicial a partir do qual o emulador começa a gerar identificadores de ordens.
- [MarketEmulatorSettings.InitialTradeId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialTradeId) - número inicial a partir do qual o emulador começa a gerar identificadores de negócios.
- [MarketEmulatorSettings.SpreadSize](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.SpreadSize) - tamanho do spread em passos de preço. Usado ao gerar um livro de ordens a partir de negócios tick. A predefinição é 2.
- [MarketEmulatorSettings.MaxDepth](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MaxDepth) - profundidade máxima do livro de ordens gerado a partir de ticks. A predefinição é 5.
- [MarketEmulatorSettings.PortfolioRecalcInterval](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PortfolioRecalcInterval) - intervalo de recálculo dos dados do portefólio. Se for igual a TimeSpan.Zero, o recálculo não é realizado.
- [MarketEmulatorSettings.ConvertTime](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.ConvertTime) - converter os carimbos temporais de ordens e negócios para a hora da bolsa. A predefinição é desativado.
- [MarketEmulatorSettings.TimeZone](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.TimeZone) - informação do fuso horário da bolsa.
- [MarketEmulatorSettings.PriceLimitOffset](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PriceLimitOffset) - desvio de preço em relação ao negócio anterior que define os limites máximo e mínimo de preço para a sessão seguinte. A predefinição é 40%.
- [MarketEmulatorSettings.IncreaseDepthVolume](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.IncreaseDepthVolume) - adicionar volume extra ao livro de ordens ao registar ordens de grande volume. Está ativada por predefinição.
- [MarketEmulatorSettings.CheckTradingState](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradingState) - verificar o estado da sessão de negociação. Está desativada por predefinição.
- [MarketEmulatorSettings.CheckMoney](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckMoney) - verificar saldo de caixa. Está desativada por predefinição.
- [MarketEmulatorSettings.CheckShortable](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckShortable) - verificar se posições curtas são permitidas. Está desativada por predefinição.
- [MarketEmulatorSettings.CheckTradableDates](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradableDates) - verificar se as datas carregadas são datas de negociação. Está desativada por predefinição.
- [MarketEmulatorSettings.CommissionRules](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CommissionRules) - regras de cálculo de comissão.

## Subscrições de Dados de Mercado

Para testar corretamente a estratégia, é necessário configurar subscrições para os tipos de dados de mercado necessários. Mesmo que a estratégia seja testada em velas, para uma emulação correta dos negócios é necessária uma subscrição de negócios tick:

```cs
// Criar uma subscrição de negócios tick
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

Se a estratégia exigir dados do livro de ordens:

```cs
// Criar uma subscrição de livros de ordens
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## Geração de Livro de Ordens para Testes

Se não existirem livros de ordens históricos, mas eles forem necessários para testar a estratégia, pode ativar a geração de livros de ordens:

```cs
// Criar um gerador de livro de ordens com comportamento de tendência
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Enviar uma mensagem de subscrição para o gerador
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});
```

### Definições do Gerador de Livro de Ordens

- Intervalo de atualização do livro de ordens ([MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)) - as atualizações não podem ser mais frequentes do que a chegada de negócios tick, pois os livros de ordens são gerados antes de cada negócio:

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- Profundidade do livro de ordens ([MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) e [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)) - quanto maior a profundidade, mais lento será o teste:

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- Para obter um volume de nível realista no livro de ordens, pode usar a opção [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume), que usa os volumes das melhores cotações a partir do volume do negócio que está a ser gerado:

```cs
mdGenerator.UseTradeVolume = true;
```

- Intervalo de volume dos níveis ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) e [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Definições de spread - o spread mínimo gerado é igual a [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). Recomenda-se não gerar um spread entre as melhores cotações superior a 5 passos de preço, para que, ao gerar a partir de velas, o spread não se torne demasiado amplo:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Exemplo Completo de Configuração de Testes

```cs
// Criar uma ligação histórica
var connector = new HistoryEmulationConnector();

// Configurar parâmetros básicos
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// Carregar dados históricos
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Criar uma subscrição de velas
var candleSubscription = new Subscription(
	TimeSpan.FromMinutes(5).TimeFrame(),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,
		BuildMode = MarketDataBuildModes.Load
	}
};
connector.Subscribe(candleSubscription);

// Criar uma subscrição de ticks para emulação correta
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// Configurar geração de livro de ordens
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId())
{
	Interval = TimeSpan.FromSeconds(1),
	MaxAsksDepth = 5,
	MaxBidsDepth = 5,
	UseTradeVolume = true,
	MinVolume = 1,
	MaxVolume = 100,
	MinSpreadStepCount = 1,
	MaxSpreadStepCount = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});

// Subscrever receção de dados
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// Iniciar testes
connector.Connect();
```

## Tratamento de Eventos

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Processar velas recebidas
	Console.WriteLine($"Vela: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Processar ticks recebidos
	Console.WriteLine($"Tick: {tick.ServerTime}, Preço: {tick.Price}, Volume: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Usar métodos de extensão para IOrderBookMessage
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
	
	// Processar livros de ordens recebidos
	Console.WriteLine($"Livro de ordens: {orderBook.ServerTime}, melhor compra: {bestBid?.Price}, melhor venda: {bestAsk?.Price}, meio do spread: {spreadMiddle}");
	
	// Obter preço por lado da ordem
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);
	
	Console.WriteLine($"Preço de compra: {bidPrice}, preço de venda: {askPrice}");
}
```

## Trabalhar com Dados do Livro de Ordens

Ao trabalhar com livros de ordens como IOrderBookMessage, pode usar os seguintes métodos de extensão:

```cs
// Obter o melhor bid
var bestBid = orderBook.GetBestBid();

// Obter o melhor ask
var bestAsk = orderBook.GetBestAsk();

// Obter o meio do spread
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// Obter o preço por lado da ordem
var price = orderBook.GetPrice(Sides.Buy); // ou Sides.Sell, ou null para o meio do spread
```

Ao trabalhar com dados Level1, também pode obter o meio do spread:

```cs
// Obter o meio do spread a partir de uma mensagem Level1
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

Estes métodos de extensão simplificam o acesso a dados do livro de ordens e permitem escrever código mais limpo e compreensível ao trabalhar com cotações da bolsa.
