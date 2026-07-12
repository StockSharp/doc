> [!WARNING]
> この取引所は恒久的に閉鎖されています（~2022 — 閉鎖）。このコネクターは現在動作しません。ドキュメントは履歴参照用として保持されています。

# FatBTC アダプターの初期化

以下のコードは、[FatBtcMessageAdapter](xref:StockSharp.FatBTC.FatBtcMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new FatBtcMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

