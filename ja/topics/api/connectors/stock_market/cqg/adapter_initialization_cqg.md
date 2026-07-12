# CQG アダプターの初期化

以下のコードは、[CqgComMessageAdapter](xref:StockSharp.Cqg.Com.CqgComMessageAdapter) と [CqgContinuumMessageAdapter](xref:StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

1. **CQG COM**、ローカルの **CQG Integrated Client** による接続:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

2. **CQG Continuum**、サーバーへの直接接続:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
	Address = "<Address>".To<IPAddress>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

