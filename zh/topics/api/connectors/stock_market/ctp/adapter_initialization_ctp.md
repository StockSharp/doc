# CTP 适配器初始化

以下代码演示如何初始化 [CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<用户名>",
	Password = "<密码>".ToSecureString(),
	BrokerId = "<经纪商标识符>",
	InvestorId = "<投资者标识符>",
	MarketDataAddress = "tcp://<市场数据地址>",
	TraderAddress = "tcp://<交易服务器地址>",
	AppId = "<应用程序标识符>",
	AuthCode = "<身份验证代码>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)

