> [!CAUTION]
> **BitMax 交易所（后更名为 AscendEX）已于 2026 年 7 月 1 日停止运营。该连接器已无法使用；文档仅保留供参考。**

# BitMax 适配器初始化

下面的代码演示如何初始化 [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<您的 API 访问密钥>".To<SecureString>(),
				Secret = "<您的 API 私密密钥>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
