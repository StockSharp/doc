# OpenECry アダプターの初期化

以下のコードは、[OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OpenECryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
	EnableOECLogging = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
