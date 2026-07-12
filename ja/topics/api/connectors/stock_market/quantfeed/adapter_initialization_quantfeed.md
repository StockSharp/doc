# QuantFEED アダプターの初期化

以下のコードは、[QuantFeedMessageAdapter](xref:StockSharp.QuantHouse.QuantFeedMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new QuantFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
