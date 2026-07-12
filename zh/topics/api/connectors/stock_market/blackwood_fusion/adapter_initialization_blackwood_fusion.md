# Blackwood (Fusion) 适配器初始化

下面的代码演示了如何初始化 [BlackwoodMessageAdapter](xref:StockSharp.Blackwood.BlackwoodMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();				
...				
var address = "<Address>".To<IPAddress>();
var messageAdapter = new BlackwoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<您的登录名>",
	Password = "<您的密码>".To<SecureString>(),
	ExecutionAddress = new IPEndPoint(address, BlackwoodAddresses.ExecutionPort),
	MarketDataAddress = new IPEndPoint(address, BlackwoodAddresses.MarketDataPort),
	HistoricalDataAddress = new IPEndPoint(address, BlackwoodAddresses.HistoricalDataPort)
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
