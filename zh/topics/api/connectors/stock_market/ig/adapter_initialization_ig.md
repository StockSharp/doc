# IG Markets 适配器初始化

以下代码演示如何初始化 [IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<API 密钥>",
	UserName = "<用户名>",
	Password = "<密码>".ToSecureString(),
	AccountId = "<账户标识符>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)

