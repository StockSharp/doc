# Búsqueda de Instrumentos

Al crear su propio adaptador para trabajar con un exchange, necesita implementar el método de búsqueda de instrumentos. Este método se llama al enviar un mensaje [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) y devuelve información sobre los instrumentos a través de mensajes [SecurityMessage](xref:StockSharp.Messages.SecurityMessage).

## Implementación del Método SecurityLookupAsync

El método **SecurityLookupAsync** generalmente realiza las siguientes acciones:

1. Obtiene la lista de tipos de instrumentos admitidos del mensaje entrante.
2. Solicita la lista de instrumentos del exchange a través de la API.
3. Para cada instrumento recibido, crea un mensaje [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), rellenándolo con los datos del instrumento.
4. Comprueba si el instrumento coincide con los criterios de búsqueda.
5. Envía el mensaje [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) creado a través del método **SendOutMessageAsync**.
6. Después de procesar todos los instrumentos, envía un mensaje sobre la finalización de la búsqueda.

A continuación se muestra un ejemplo de la implementación del método SecurityLookupAsync basado en el adaptador para el exchange Coinbase. Al crear su propio adaptador, necesita adaptar este código a la API del exchange que se esté utilizando.

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

Este método le permite recuperar información sobre los instrumentos disponibles en el exchange, incluyendo sus características principales como el tipo de instrumento, el volumen mínimo, el paso de precio, etc.
</content>
