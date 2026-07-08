> [!WARNING]
> この取引所は恒久的に閉鎖されています（~2021 — 閉鎖）。このコネクターは現在動作しません。ドキュメントは履歴参照用に保持されています。

# CoinBene アダプターの初期化

以下のコードは、[CoinBeneMessageAdapter](xref:StockSharp.CoinBene.CoinBeneMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new CoinBeneMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

