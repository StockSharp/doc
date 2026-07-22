# ProBit Global 适配器初始化

下面的代码演示如何初始化 [ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 OAuth 客户端标识符>".To<SecureString>(),
	Secret = "<您的 OAuth 客户端密钥>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

如果只需要公共市场数据，请省略 `Key` 和 `Secret`。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
