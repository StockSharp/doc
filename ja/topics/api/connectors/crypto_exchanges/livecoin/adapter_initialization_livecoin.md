> [!WARNING]
> この取引所は恒久的に閉鎖されています（2020年12月 - ハッキングを受けて閉鎖）。このコネクターは現在動作しません。ドキュメントは履歴参照のために保存されています。

# Livecoin アダプターの初期化

以下のコードは、[LiveCoinMessageAdapter](xref:StockSharp.LiveCoin.LiveCoinMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiveCoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

