> [!NOTE]
> BitMax 已更名为 AscendEX。本文件保留用于历史参考。

# 适配器初始化 BitMax

下面的代码演示了如何初始化 [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
