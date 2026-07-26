> [!CAUTION]
> **GDAX サービスは利用できなくなりました。後継の Coinbase Pro も終了しているため、Coinbase には現在の Coinbase コネクターを使用してください。このコネクターは動作しません。ドキュメントは参照用としてのみ保持されています。**

# GDAX アダプターの初期化

以下のコードは、[GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

