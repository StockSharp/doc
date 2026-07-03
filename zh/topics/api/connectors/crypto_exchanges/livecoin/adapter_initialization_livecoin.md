> [!WARNING]
> 该交易所已永久关闭（2020年12月——被黑并关闭）。此连接器不再可用。文档已保留用于历史参考。

# Livecoin 适配器初始化

下面的代码演示如何初始化 [LiveCoinMessageAdapter](xref:StockSharp.LiveCoin.LiveCoinMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiveCoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
