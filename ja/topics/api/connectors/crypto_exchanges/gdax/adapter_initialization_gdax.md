> [!NOTE]
> GDAX は Coinbase Pro に名称変更され、その後 Coinbase Advanced Trade に変更されました。このドキュメントは履歴参照用として保持されています。

# GDAX アダプターの初期化

以下のコードは、[GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[?????????](../../../graphical_user_interface/connection_settings_window.md)

