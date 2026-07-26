> [!CAUTION]
> **CoinHub 取引所とその API は利用できなくなりました。このコネクターは動作しません。ドキュメントは参照用としてのみ保持されています。**

# CoinHub アダプターの初期化

以下のコードは、[CoinHubMessageAdapter](xref:StockSharp.CoinHub.CoinHubMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new CoinHubMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
