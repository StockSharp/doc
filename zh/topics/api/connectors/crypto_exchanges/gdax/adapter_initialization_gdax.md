> [!CAUTION]
> **GDAX 服务已不可用，其后继服务 Coinbase Pro 也已停止；如需连接 Coinbase，请使用当前的 Coinbase 连接器。此连接器已无法使用；文档仅保留供参考。**

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
