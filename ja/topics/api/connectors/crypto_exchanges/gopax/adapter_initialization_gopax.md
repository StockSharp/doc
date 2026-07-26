# アダプターの初期化 Gopax

以下のコードは、[GopaxMessageAdapter](xref:StockSharp.Gopax.GopaxMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送る方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GopaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
