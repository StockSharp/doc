# 銘柄検索

取引所と連携する独自アダプターを作成する場合、銘柄検索メソッドを実装する必要があります。このメソッドは [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) メッセージの送信時に呼び出され、[SecurityMessage](xref:StockSharp.Messages.SecurityMessage) メッセージを通じて銘柄に関する情報を返します。

## SecurityLookupAsync メソッドの実装

**SecurityLookupAsync** メソッドは通常、次の処理を行います。

1. 受信メッセージから、サポートされている銘柄タイプの一覧を取得します。
2. API を介して取引所から銘柄一覧を要求します。
3. 受信した各銘柄について、銘柄データを設定した [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) メッセージを作成します。
4. 銘柄が検索条件に一致するか確認します。
5. 作成した [SecurityMessage](xref:StockSharp.Messages.SecurityMessage) メッセージを **SendOutMessageAsync** メソッドで送信します。
6. すべての銘柄の処理後、検索完了を示すメッセージを送信します。

以下は、Coinbase 取引用アダプターに基づく SecurityLookupAsync メソッドの実装例です。独自アダプターを作成する場合は、このコードを使用する取引所の API に合わせて調整する必要があります。

```cs
public override async ValueTask SecurityLookupAsync(SecurityLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	// 検索する銘柄タイプの一覧を取得
	var secTypes = lookupMsg.GetSecurityTypes();
	
	// 検索する銘柄数の上限を決定
	var left = lookupMsg.Count ?? long.MaxValue;

	// 取引所がサポートする銘柄タイプを反復処理
	foreach (var type in new[] { "SPOT", "FUTURE" })
	{
		// 取引所から銘柄一覧を要求
		var products = await _restClient.GetProducts(type, cancellationToken);

		foreach (var product in products)
		{
			// 銘柄識別子を作成
			var secId = product.ProductId.ToStockSharp();

			// 銘柄情報を含むメッセージを作成
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

				// サブスクリプション識別子を設定する必要があります
				// これにより、外部コードはどのサブスクリプション向けにデータを受信したかを判別できます
				OriginalTransactionId = lookupMsg.TransactionId,
			}
			.TryFillUnderlyingId(product.BaseCurrencyId.ToUpperInvariant());

			// 銘柄が検索条件に一致するか確認
			if (!secMsg.IsMatch(lookupMsg, secTypes))
				continue;

			// 銘柄情報を含むメッセージを送信
			await SendOutMessageAsync(secMsg, cancellationToken);

			// 残りの銘柄数カウンターを減らす
			if (--left <= 0)
				break;
		}

		if (left <= 0)
			break;
	}

	// 検索完了を示すメッセージを送信
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

このメソッドにより、銘柄タイプ、最小数量、価格ステップなどの主な特性を含め、取引所で利用可能な銘柄に関する情報を取得できます。
