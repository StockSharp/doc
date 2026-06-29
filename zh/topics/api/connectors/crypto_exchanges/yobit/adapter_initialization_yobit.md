> [!WARNING]
> 该交易所已永久关闭（~2023 — 实际上已停止运作）。该连接器不再可用。文档已保留以作历史参考。

# 适配器初始化 Yobit

下面的代码演示了如何初始化 [YobitMessageAdapter](xref:StockSharp.Yobit.YobitMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new YobitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
