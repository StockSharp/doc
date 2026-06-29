# 适配器初始化 CQG

下面的代码演示了如何初始化[CqgComMessageAdapter](xref:StockSharp.Cqg.Com.CqgComMessageAdapter)和[CqgContinuumMessageAdapter](xref:StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter)并将其发送到[Connector](xref:StockSharp.Algo.Connector)。

1. **CQG COM**，通过本地 **CQG Integrated Client** 连接：

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

2. **CQG Continuum**，直接连接到服务器：

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Address>".To<IPAddress>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
