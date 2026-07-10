# Beispiele mit dem Orderbuch

## Beste Preise abrufen

Um die besten Preise aus dem Orderbuch zu erhalten, ist es wichtig, sich auf die ersten Elemente der Listen für Kauforders ([Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)) und Verkaufsorders ([Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)) zu konzentrieren, da sie die günstigsten verfügbaren Preise für Transaktionen darstellen:

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Bester Kaufpreis: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Bester Verkaufspreis: {bestAsk.Price}");
}
```

Oder verwenden Sie die fertigen Erweiterungsmethoden [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) und [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage)):

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Bester Kaufpreis: {bestBid.Price}, Volumen: {bestBid.Volume}");
}
else
{
	Console.WriteLine("Keine besten Kauforders.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Bester Verkaufspreis: {bestAsk.Price}, Volumen: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("Keine besten Verkaufsorders.");
}
```

## Tiefe des Orderbuchs analysieren

Zur Analyse der Tiefe des Orderbuchs können Sie die Elemente in den Listen [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) und [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks) von Anfang an durchlaufen. Dadurch erhalten Sie einen Überblick über die Verteilung von Orders auf verschiedenen Preisniveaus und können mögliche Unterstützungs- und Widerstandsniveaus erkennen:

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Kaufpreis: {bid.Price}, Volumen: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Verkaufspreis: {ask.Price}, Volumen: {ask.Volume}");
}
```

## Suche nach Volumina im Orderbuch

Ein Algorithmus zur Suche nach signifikanten Volumina im Orderbuch hilft, Niveaus zu erkennen, an denen sich große Orders ansammeln. Dies kann auf das Interesse großer Marktteilnehmer hinweisen und als zusätzliches Signal bei Handelsentscheidungen dienen.

Algorithmus:

1. Bestimmen Sie einen Volumenschwellenwert, der als signifikant gelten soll.
2. Durchlaufen Sie die Orders in den Listen [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) und [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks), und vergleichen Sie das Volumen jeder Order mit dem Schwellenwert.
3. Zeichnen Sie die Preisniveaus auf, auf denen Orders mit einem Volumen oberhalb des Schwellenwerts gefunden wurden.

```cs
double significantVolumeThreshold = 10000; // Beispiel für einen Schwellenwert

Console.WriteLine("Signifikante Volumina im Orderbuch:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Kauf: Preis {bid.Price}, Volumen {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Verkauf: Preis {ask.Price}, Volumen {ask.Volume}");
	}
}
```

Dieser Algorithmus hilft, Niveaus mit signifikanten Volumina hervorzuheben, die eine wichtige Rolle in Marktpreisbewegungen spielen können.
