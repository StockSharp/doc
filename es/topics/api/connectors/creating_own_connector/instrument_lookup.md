# Búsqueda de Instrumentos

Al crear su propio adaptador para trabajar con una bolsa, necesita implementar el método de búsqueda de instrumentos. Este método se llama al enviar un mensaje [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) y devuelve información sobre los instrumentos a través de mensajes [SecurityMessage](xref:StockSharp.Messages.SecurityMessage).

## Implementación del Método SecurityLookupAsync

El método **SecurityLookupAsync** generalmente realiza las siguientes acciones:

1. Obtiene la lista de tipos de instrumentos admitidos del mensaje entrante.
2. Solicita la lista de instrumentos de la bolsa a través de la API.
3. Para cada instrumento recibido, crea un mensaje [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), rellenándolo con los datos del instrumento.
4. Comprueba si el instrumento coincide con los criterios de búsqueda.
5. Envía el mensaje [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) creado a través del método **SendOutMessageAsync**.
6. Después de procesar todos los instrumentos, envía un mensaje sobre la finalización de la búsqueda.

A continuación se muestra un ejemplo de la implementación del método SecurityLookupAsync basado en el adaptador para la bolsa Coinbase. Al crear su propio adaptador, necesita adaptar este código a la API de la bolsa que se esté utilizando.

```cs
public override async ValueTask SecurityLookupAsync(SecurityLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	// Obtener la lista de tipos de instrumentos a buscar
	var secTypes = lookupMsg.GetSecurityTypes();

	// Determinar el número máximo de instrumentos a buscar
	var left = lookupMsg.Count ?? long.MaxValue;

	// Iterar por los tipos de instrumentos admitidos por la bolsa
	foreach (var type in new[] { "SPOT", "FUTURE" })
	{
		// Solicitar la lista de instrumentos a la bolsa
		var products = await _restClient.GetProducts(type, cancellationToken);

		foreach (var product in products)
		{
			// Crear identificador del instrumento
			var secId = product.ProductId.ToStockSharp();

			// Crear mensaje con información del instrumento
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

				// es necesario rellenar el identificador de suscripción
				// para que el código externo entienda para qué suscripción se recibieron los datos
				OriginalTransactionId = lookupMsg.TransactionId,
			}
			.TryFillUnderlyingId(product.BaseCurrencyId.ToUpperInvariant());

			// Comprobar si el instrumento coincide con los criterios de búsqueda
			if (!secMsg.IsMatch(lookupMsg, secTypes))
				continue;

			// Enviar mensaje con información del instrumento
			await SendOutMessageAsync(secMsg, cancellationToken);

			// Disminuir contador de instrumentos restantes
			if (--left <= 0)
				break;
		}

		if (left <= 0)
			break;
	}

	// Enviar mensaje de finalización de la búsqueda
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

Este método le permite recuperar información sobre los instrumentos disponibles en la bolsa, incluyendo sus características principales como el tipo de instrumento, el volumen mínimo, el paso de precio, etc.
</content>
