> [!CAUTION]
> **BitMEX 交易所将于 2026 年 9 月 23 日关闭，新用户注册已停止。关闭后，该连接器将无法使用。**

# BitMEX 适配器初始化

下面的代码演示如何初始化 [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<您的 API 访问密钥>".To<SecureString>(),
				Secret = "<您的 API 私密密钥>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
