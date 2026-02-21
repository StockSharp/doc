# Instrument Lookup

When creating your own adapter for working with an exchange, you need to implement the instrument lookup method. This method is called when sending a [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) message and returns information about instruments through [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) messages.

## Implementing the SecurityLookupAsync Method

The **SecurityLookupAsync** method usually performs the following actions:

1. Retrieves the list of supported instrument types from the incoming message.
2. Requests the list of instruments from the exchange via API.
3. For each received instrument, creates a [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) message, filling it with instrument data.
4. Checks if the instrument matches the search criteria.
5. Sends the created [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) message via the **SendOutMessageAsync** method.
6. After processing all instruments, sends a message about the completion of the search.

Below is an example of the implementation of the SecurityLookupAsync method based on the adapter for the Coinbase exchange. When creating your own adapter, you need to adapt this code to the API of the exchange being used.

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

				// It is necessary to fill in the subscription identifier
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

This method allows you to retrieve information about the instruments available on the exchange, including their main characteristics such as instrument type, minimum volume, price step, etc.