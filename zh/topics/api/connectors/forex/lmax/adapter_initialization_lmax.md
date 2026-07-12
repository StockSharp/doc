# LMAX 适配器初始化

下面的代码演示了如何初始化 [LmaxMessageAdapter](xref:StockSharp.LMAX.LmaxMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new LmaxMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<您的登录名>",
	Password = "<您的密码>".To<SecureString>(),
	IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
