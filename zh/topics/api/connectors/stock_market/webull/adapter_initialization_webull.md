# Webull 适配器初始化

以下代码演示如何初始化 [WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<应用程序密钥>".ToSecureString(),
	Secret = "<应用程序私密密钥>".ToSecureString(),
	Token = "<访问令牌>".ToSecureString(),
	Account = "<账户标识符>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

`Token` 和 `Account` 在不需要时可以省略。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
