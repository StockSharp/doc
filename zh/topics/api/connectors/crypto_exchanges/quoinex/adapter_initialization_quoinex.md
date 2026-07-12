> [!NOTE]
> QUOINEX 已更名为 Liquid，该公司于 2022 年关闭。本文件保留以供历史参考。

# Quoinex 适配器初始化

下面的代码演示如何初始化 [QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<您的 API 访问密钥>".To<SecureString>(),
				Secret = "<您的 API 私密密钥>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
