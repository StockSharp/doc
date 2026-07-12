# CEX.IO 适配器初始化

下面的代码演示如何初始化 [CexMessageAdapter](xref:StockSharp.Cex.CexMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<您的 API 访问密钥>".To<SecureString>(),
				Secret = "<您的 API 私密密钥>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
