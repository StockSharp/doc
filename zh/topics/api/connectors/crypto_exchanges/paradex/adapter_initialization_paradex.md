# Paradex 适配器初始化

下面的代码演示如何初始化 [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ParadexMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<您的 API 访问密钥>".To<SecureString>(),
	Secret = "<您的 API 私密密钥>".To<SecureString>(),
	StarknetAccount = "<您的 Starknet 账户>",
	StarknetPrivateKey = "<您的 Starknet 私钥>".To<SecureString>(),
	Section = ParadexSections.Derivatives,
	AuthPath = "/v1/auth",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
