> [!CAUTION]
> **BitMEX は 2026 年 9 月 23 日に閉鎖予定で、新規登録はすでに停止されています。閉鎖後、このコネクターは動作しなくなります。**

# BitMEX アダプターの初期化

以下のコードは、[BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
