> [!CAUTION]
> **QUOINEX は Liquid に改称され、その Liquid も後に運営を終了しました。このコネクターは動作しません。ドキュメントは参照用としてのみ保持されています。**

# Quoinex アダプターの初期化

以下のコードは、[QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
