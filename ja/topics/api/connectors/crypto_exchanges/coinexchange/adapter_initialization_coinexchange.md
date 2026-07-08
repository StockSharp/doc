> [!WARNING]
> この取引所は完全に閉鎖されています（2019年10月 - 閉鎖）。このコネクターは現在動作しません。ドキュメントは履歴参照用として保存されています。

# CoinExchange アダプターの初期化

以下のコードは、[CoinExchangeMessageAdapter](xref:StockSharp.CoinExchange.CoinExchangeMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CoinExchangeMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[?????????](../../../graphical_user_interface/connection_settings_window.md)
