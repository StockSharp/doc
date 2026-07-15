# 交易品种查询

在为交易所创建自己的适配器时，需要实现交易品种查询方法。发送 [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) 消息时会调用此方法，并通过 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 消息返回有关交易品种的信息。

## 实现 SecurityLookupAsync 方法

**SecurityLookupAsync** 方法通常执行以下操作：

1. 从传入消息中获取支持的交易品种类型列表。
2. 通过 API 向交易所请求交易品种列表。
3. 为每个接收到的交易品种创建一个 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 消息，并填充相应的交易品种数据。
4. 检查该交易品种是否符合搜索条件。
5. 通过 **SendOutMessageAsync** 方法发送创建好的 [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) 消息。
6. 处理完所有交易品种后，发送搜索完成的消息。

下面是基于 Coinbase 交易所适配器的 SecurityLookupAsync 方法实现示例。在创建自己的适配器时，需要将此代码适配到所使用交易所的 API。

```cs
public override async ValueTask SecurityLookupAsync(SecurityLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	// 获取要查找的交易品种类型列表
	var secTypes = lookupMsg.GetSecurityTypes();
	
	// 确定要搜索的最大交易品种数量
	var left = lookupMsg.Count ?? long.MaxValue;

	// 遍历交易所支持的交易品种类型
	foreach (var type in new[] { "SPOT", "FUTURE" })
	{
		// 从交易所请求交易品种列表
		var products = await _restClient.GetProducts(type, cancellationToken);

		foreach (var product in products)
		{
			// 创建交易品种标识符
			var secId = product.ProductId.ToStockSharp();

			// 创建包含交易品种信息的消息
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

				// 需要填写订阅标识符
				// 以便外部代码理解数据属于哪个订阅
				OriginalTransactionId = lookupMsg.TransactionId,
			}
			.TryFillUnderlyingId(product.BaseCurrencyId.ToUpperInvariant());

			// 检查交易品种是否符合搜索条件
			if (!secMsg.IsMatch(lookupMsg, secTypes))
				continue;

			// 发送包含交易品种信息的消息
			await SendOutMessageAsync(secMsg, cancellationToken);

			// 减少剩余交易品种计数器
			if (--left <= 0)
				break;
		}

		if (left <= 0)
			break;
	}

	// 发送搜索完成消息
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

此方法可以获取交易所可用交易品种的相关信息，包括交易品种类型、最小交易量、价格步长等主要特征。
