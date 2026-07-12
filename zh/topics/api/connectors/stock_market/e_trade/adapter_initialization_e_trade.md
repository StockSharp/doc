# E\*TRADE 适配器初始化

下面的代码演示了如何初始化 [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new ETradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerSecret = "<您的私密密钥>".To<SecureString>(),
	ConsumerKey = "<您的密钥>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
