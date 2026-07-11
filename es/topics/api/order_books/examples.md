# Ejemplos con el libro de órdenes

## Obtención de los mejores precios

Para obtener los mejores precios del libro de órdenes, es importante centrarse en los primeros elementos de las listas de órdenes de compra ([Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)) y órdenes de venta ([Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)), ya que representan los precios disponibles más favorables para las transacciones:

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Mejor precio de compra: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Mejor precio de venta: {bestAsk.Price}");
}
```

O use los métodos de extensión ya preparados [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) y [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage)):

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Mejor precio de compra: {bestBid.Price}, volumen: {bestBid.Volume}");
}
else
{
	Console.WriteLine("No hay mejores órdenes de compra.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Mejor precio de venta: {bestAsk.Price}, volumen: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("No hay mejores órdenes de venta.");
}
```

## Análisis de profundidad del libro de órdenes

Para analizar la profundidad del libro de órdenes, puede iterar por los elementos de las listas [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) y [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), empezando desde el inicio de la lista. Esto proporciona una visión general de la distribución de órdenes en distintos niveles de precio y ayuda a identificar posibles niveles de soporte y resistencia:

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Precio de compra: {bid.Price}, volumen: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Precio de venta: {ask.Price}, volumen: {ask.Volume}");
}
```

## Búsqueda de volúmenes en el libro de órdenes

Un algoritmo para buscar volúmenes significativos en el libro de órdenes ayuda a identificar niveles donde se acumulan órdenes grandes. Esto puede indicar el interés de participantes importantes y servir como señal adicional al tomar decisiones de trading.

Algoritmo:

1. Determine un umbral de volumen que se considerará significativo.
2. Itere por las órdenes en las listas [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) y [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), comparando el volumen de cada orden con el valor umbral.
3. Registre los niveles de precio donde se encontraron órdenes con volumen por encima del umbral.

```cs
double significantVolumeThreshold = 10000; // Ejemplo de valor umbral

Console.WriteLine("Volúmenes significativos en el libro de órdenes:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Compra: precio {bid.Price}, volumen {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Venta: precio {ask.Price}, volumen {ask.Volume}");
	}
}
```

Este algoritmo ayuda a destacar niveles con volúmenes significativos, que pueden desempeñar un papel clave en los movimientos de precio del mercado.
