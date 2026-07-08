> [!NOTE]
> QUOINEX は Liquid に名称変更され、その後 2022 年に終了しました。このドキュメントは履歴参照のために保持されています。

# Quoinex アダプターの初期化

以下のコードは、[QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
