# Rithmic アダプターの初期化

以下のコードは、[RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
	CertFile = "<証明書ファイルへのパス>",
	Server = RithmicServers.Real,
	//Server = RithmicServers.Test,
	//Server = RithmicServers.Simulator,  
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

別の、より便利な方法として、`AddAdapter<T>()` 拡張メソッドを使用できます。

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<RithmicMessageAdapter>(a =>
{
	a.UserName = "<ログイン名>";
	a.Password = "<パスワード>".To<SecureString>();
	a.CertFile = "<証明書ファイルへのパス>";
	a.Server = RithmicServers.Real;
});
```

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
