# 适配器初始化 PolygonIO

下面的代码演示了如何初始化 [PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // connection for REST data sources
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
