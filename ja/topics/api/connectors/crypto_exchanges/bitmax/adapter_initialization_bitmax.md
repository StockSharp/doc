> [!CAUTION]
> **BitMax（後に AscendEX へ改称）は、2026 年 7 月 1 日に運営を終了しました。このコネクターは動作しません。ドキュメントは参照用としてのみ保持されています。**

# BitMax アダプターの初期化

以下のコードは、[BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<APIキー>".To<SecureString>(),
				Secret = "<APIシークレット>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
