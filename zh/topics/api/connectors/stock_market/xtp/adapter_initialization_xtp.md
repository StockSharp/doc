# Zhongtai XTP 适配器初始化

以下代码演示如何初始化 [XtpMessageAdapter](xref:StockSharp.Xtp.XtpMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new XtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<用户名>",
	Password = "<密码>".ToSecureString(),
	ClientId = 1,
	QuoteAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6001),
	TransactionAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6002),
	Protocol = XtpProtocols.Tcp,
	SoftwareKey = "<软件密钥>",
	SoftwareVersion = "1.0",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
