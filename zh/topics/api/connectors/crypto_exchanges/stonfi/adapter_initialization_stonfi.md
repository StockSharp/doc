# 适配器初始化：STON.fi

以下代码初始化 [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<您的 TON 钱包地址>",
	Mnemonic = "<您的 24 词助记短语>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写钱包凭据以及[连接器配置](configuration_stonfi.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_stonfi.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
