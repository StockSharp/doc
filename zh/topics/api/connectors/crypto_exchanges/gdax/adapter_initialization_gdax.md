> [!NOTE]
> GDAX 已被更名为 Coinbase Pro，后来又更名为 Coinbase 高级交易。此文档为历史参考而保留。

# GDAX 适配器初始化

下面的代码演示如何初始化 [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<您的 API 访问密钥>".To<SecureString>(),
				Secret = "<您的 API 私密密钥>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
