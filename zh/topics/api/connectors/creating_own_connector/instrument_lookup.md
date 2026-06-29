# 乐器查找

在为交易所创建您自己的适配器时，您需要实现工具查询方法。发送 [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) 消息时会调用此方法，并通过 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 消息返回有关工具的信息。

## 实现 SecurityLookupAsync 方法

**SecurityLookupAsync** 方法通常执行以下操作：

1. 从传入消息中检索支持的乐器类型列表。
2. 通过 API 向交易所请求工具列表。
3. 对于每个接收到的工具，创建一个 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 消息，并用工具数据填充它。
4. 检查仪器是否符合搜索条件。
5. 通过 **SendOutMessageAsync** 方法发送创建的 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 消息。
6. 处理完所有仪器后，发送关于搜索完成的消息。

下面是一个基于Coinbase交易所适配器的SecurityLookupAsync方法实现示例。在创建你自己的适配器时，你需要将此代码适配到所使用交易所的API。

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

此方法允许您获取有关交易所可用工具的信息，包括其主要特征，例如工具类型、最小交易量、价格步长等。