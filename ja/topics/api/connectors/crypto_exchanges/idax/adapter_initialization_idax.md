> [!CAUTION]
> **IDAX 取引所は運営を終了しました。このコネクターは動作しません。ドキュメントは参照用としてのみ保持されています。**

# Idax アダプターの初期化

以下のコードは、[IdaxMessageAdapter](xref:StockSharp.Idax.IdaxMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new IdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
