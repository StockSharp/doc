> [!WARNING]
> この取引所は完全に閉鎖されています（2017年7月に差し押さえ）。このコネクタは現在動作しません。ドキュメントは履歴参照用に保持されています。

# WEX (BTC-e) アダプターの初期化

以下のコードは、[BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
