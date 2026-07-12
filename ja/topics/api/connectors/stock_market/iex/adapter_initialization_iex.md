# IEX アダプターの初期化

以下のコードは、[IEXMessageAdapter](xref:StockSharp.IEX.IEXMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token  = "<トークン>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
