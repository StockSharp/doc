> [!CAUTION]
> **OKCoin 的交易服务已在平台迁移至 OKX 后停止。此连接器已无法使用；文档仅保留供参考。**

# OKCoin 适配器初始化

下面的代码演示如何初始化 [OkcoinMessageAdapter](xref:StockSharp.Okcoin.OkcoinMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new OkcoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<您的 API 访问密钥>".To<SecureString>(),
				Secret = "<您的 API 私密密钥>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
