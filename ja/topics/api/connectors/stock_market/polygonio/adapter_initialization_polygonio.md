# PolygonIO アダプターの初期化

以下のコードは、[PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<トークン>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // REST データソースへの接続
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
