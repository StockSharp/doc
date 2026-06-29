> [!NOTE]
> GDAX 已被更名为 Coinbase Pro，后来又更名为 Coinbase 高级交易。此文档为历史参考而保留。

# 适配器初始化 GDAX

下面的代码演示了如何初始化 [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
