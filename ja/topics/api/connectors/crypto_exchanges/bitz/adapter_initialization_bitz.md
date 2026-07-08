> [!WARNING]
> この取引所は完全に閉鎖されています（2021 年頃に閉鎖）。このコネクタは現在動作しません。ドキュメントは履歴参照用として保持されています。

# BitZ アダプターの初期化

以下のコードは、[BitZMessageAdapter](xref:StockSharp.BitZ.BitZMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitZMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
