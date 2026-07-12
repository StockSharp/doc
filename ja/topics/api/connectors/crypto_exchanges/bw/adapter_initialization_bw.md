> [!WARNING]
> この取引所は恒久的に閉鎖されました（~2021 - 閉鎖）。このコネクタは現在動作しません。ドキュメントは履歴参照用に保持されています。

# アダプターの初期化 BW

以下のコードは、[BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
