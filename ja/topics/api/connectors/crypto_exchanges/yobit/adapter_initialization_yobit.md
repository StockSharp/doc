> [!WARNING]
> この取引所は完全に閉鎖されています（2023年ごろ、実質的に廃止）。このコネクタは現在動作しません。ドキュメントは履歴参照のために保持されています。

# Yobit アダプターの初期化

以下のコードは、[YobitMessageAdapter](xref:StockSharp.Yobit.YobitMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new YobitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
