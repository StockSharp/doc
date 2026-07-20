# Dow Jones 适配器初始化

下面的代码演示了如何初始化 [DowJonesMessageAdapter](xref:StockSharp.DowJones.DowJonesMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DowJonesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<您的值>".To<SecureString>(),
	ClientId = "<您的值>",
	Login = "<您的值>",
	Password = "<您的值>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
