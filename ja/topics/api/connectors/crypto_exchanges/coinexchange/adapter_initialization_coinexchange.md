> [!WARNING]
> この取引所は完全に閉鎖されています（2019年10月 - 閉鎖）。このコネクターは現在動作しません。ドキュメントは履歴参照用として保存されています。

# CoinExchange アダプターの初期化

以下のコードは、[CoinExchangeMessageAdapter](xref:StockSharp.CoinExchange.CoinExchangeMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CoinExchangeMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
