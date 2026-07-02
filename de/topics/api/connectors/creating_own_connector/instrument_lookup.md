# Instrumentensuche

Beim Erstellen eines eigenen Adapters für die Arbeit mit einer Börse müssen Sie die Methode zur Instrumentensuche implementieren. Diese Methode wird beim Senden einer Nachricht [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) aufgerufen und gibt Informationen über Instrumente über Nachrichten [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) zurück.

## Implementierung der Methode SecurityLookupAsync

Die Methode **SecurityLookupAsync** führt in der Regel die folgenden Aktionen aus:

1. Ruft die Liste der unterstützten Instrumententypen aus der eingehenden Nachricht ab.
2. Fordert die Liste der Instrumente von der Börse über die API an.
3. Erstellt für jedes empfangene Instrument eine Nachricht [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) und füllt sie mit den Instrumentendaten.
4. Prüft, ob das Instrument den Suchkriterien entspricht.
5. Sendet die erstellte Nachricht [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) über die Methode **SendOutMessageAsync**.
6. Sendet nach der Verarbeitung aller Instrumente eine Nachricht über den Abschluss der Suche.

Nachfolgend finden Sie ein Beispiel für die Implementierung der Methode SecurityLookupAsync anhand des Adapters für die Coinbase-Börse. Beim Erstellen Ihres eigenen Adapters müssen Sie diesen Code an die API der verwendeten Börse anpassen.

```cs
public override async ValueTask SecurityLookupAsync(SecurityLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	// Get the list of instrument types to find
	var secTypes = lookupMsg.GetSecurityTypes();

	// Determine the maximum number of instruments to search for
	var left = lookupMsg.Count ?? long.MaxValue;

	// Iterate over the instrument types supported by the exchange
	foreach (var type in new[] { "SPOT", "FUTURE" })
	{
		// Request the list of instruments from the exchange
		var products = await _restClient.GetProducts(type, cancellationToken);

		foreach (var product in products)
		{
			// Create the instrument identifier
			var secId = product.ProductId.ToStockSharp();

			// Create a message with instrument information
			var secMsg = new SecurityMessage
			{
				SecurityType = product.ProductType.ToSecurityType(),
				SecurityId = secId,
				Name = product.DisplayName,
				PriceStep = product.QuoteIncrement?.ToDecimal(),
				VolumeStep = product.BaseIncrement?.ToDecimal(),
				MinVolume = product.BaseMinSize?.ToDecimal(),
				MaxVolume = product.BaseMaxSize?.ToDecimal(),
				ExpiryDate = product.FutureProductDetails?.ContractExpiry,
				Multiplier = product.FutureProductDetails?.ContractSize?.ToDecimal(),

				// you need to fill in the subscription identifier
				// so that the external code can understand which subscription the data was received for
				OriginalTransactionId = lookupMsg.TransactionId,
			}
			.TryFillUnderlyingId(product.BaseCurrencyId.ToUpperInvariant());

			// Check if the instrument matches the search criteria
			if (!secMsg.IsMatch(lookupMsg, secTypes))
				continue;

			// Send a message with instrument information
			await SendOutMessageAsync(secMsg, cancellationToken);

			// Decrease the counter of remaining instruments
			if (--left <= 0)
				break;
		}

		if (left <= 0)
			break;
	}

	// Send a message about the completion of the search
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

Diese Methode ermöglicht es Ihnen, Informationen über die an der Börse verfügbaren Instrumente abzurufen, einschließlich ihrer Haupteigenschaften wie Instrumententyp, Mindestvolumen, Preisschritt usw.
