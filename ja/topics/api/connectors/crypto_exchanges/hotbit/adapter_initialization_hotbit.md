> [!WARNING]
> この取引所は恒久的に閉鎖されました（2023年5月 - 閉鎖）。このコネクターは現在動作しません。ドキュメントは履歴参照用として保持されています。

# アダプターの初期化 Hotbit

以下のコードは、[HotbitMessageAdapter](xref:StockSharp.Hotbit.HotbitMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送る方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HotbitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
