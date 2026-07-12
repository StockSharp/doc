# FIX アダプターの初期化

以下のコードは、[FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ログイン名>",
	Password = "<パスワード>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

別の、より便利な方法として、`AddAdapter<T>()` 拡張メソッドを使用できます。

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<FixMessageAdapter>(a =>
{
	a.Login = "<ログイン名>";
	a.Password = "<パスワード>".To<SecureString>();
	a.Address = "<Address>".To<EndPoint>();
});
```

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
