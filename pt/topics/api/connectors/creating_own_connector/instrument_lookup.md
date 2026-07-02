# Busca de Instrumentos

Ao criar seu próprio adaptador para trabalhar com uma exchange, você precisa implementar o método de busca de instrumentos. Esse método é chamado ao enviar uma mensagem [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) e retorna informações sobre os instrumentos através de mensagens [SecurityMessage](xref:StockSharp.Messages.SecurityMessage).

## Implementando o Método SecurityLookupAsync

O método **SecurityLookupAsync** geralmente executa as seguintes ações:

1. Recupera a lista de tipos de instrumentos suportados a partir da mensagem de entrada.
2. Solicita a lista de instrumentos da exchange via API.
3. Para cada instrumento recebido, cria uma mensagem [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), preenchendo-a com os dados do instrumento.
4. Verifica se o instrumento corresponde aos critérios de busca.
5. Envia a mensagem [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) criada através do método **SendOutMessageAsync**.
6. Após processar todos os instrumentos, envia uma mensagem sobre a conclusão da busca.

Abaixo está um exemplo da implementação do método SecurityLookupAsync com base no adaptador para a exchange Coinbase. Ao criar seu próprio adaptador, você precisa adaptar esse código à API da exchange utilizada.

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

Esse método permite recuperar informações sobre os instrumentos disponíveis na exchange, incluindo suas principais características, como tipo de instrumento, volume mínimo, passo de preço, etc.
