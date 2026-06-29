> [!WARNING]
> 该交易所已永久关闭（2017年7月 —— 被查封）。此连接器不再可用。文档已保留以供历史参考。

# 适配器初始化 WEX（BTC\-e）

下面的代码演示了如何初始化 [BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
