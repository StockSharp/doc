# Exemplos com o Livro de Ofertas

## Obter os Melhores Preços

Para obter os melhores preços do livro de ofertas, é importante focar-se nos primeiros elementos das listas de ordens de compra ([Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)) e ordens de venda ([Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)), uma vez que estes representam os preços disponíveis mais favoráveis para transações:

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}");
}
```

Ou use os métodos de extensão prontos a usar [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) e [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage)):

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}, volume: {bestBid.Volume}");
}
else
{
	Console.WriteLine("No best buy orders.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}, volume: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("No best sell orders.");
}
```

## Analisar a Profundidade do Livro de Ofertas

Para analisar a profundidade do livro de ofertas, pode percorrer os itens nas listas [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) e [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), começando pelo início da lista. Isto fornece uma visão geral da distribuição das ordens em diferentes níveis de preço e ajuda a identificar potenciais níveis de suporte e resistência:

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Buy price: {bid.Price}, volume: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Sell price: {ask.Price}, volume: {ask.Volume}");
}
```

## Procurar Volumes no Livro de Ofertas

Um algoritmo para procurar volumes significativos no livro de ofertas ajuda a identificar níveis onde se acumulam ordens grandes. Isto pode indicar o interesse de grandes participantes e servir como sinal adicional ao tomar decisões de negociação.

Algoritmo:

1. Determinar um limiar de volume que será considerado significativo.
2. Percorrer as ordens nas listas [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) e [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), comparando o volume de cada ordem com o valor limiar.
3. Registar os níveis de preço onde foram encontradas ordens com volume acima do limiar.

```cs
double significantVolumeThreshold = 10000; // Exemplo de valor de limiar

Console.WriteLine("Volumes significativos no livro de ordens:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Buy: Price {bid.Price}, volume {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Sell: Price {ask.Price}, volume {ask.Volume}");
	}
}
```

Este algoritmo ajuda a destacar níveis com volumes significativos, que podem desempenhar um papel importante nos movimentos dos preços de mercado.
