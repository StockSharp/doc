# Инициализация адаптера: MasterLink

Следующий код создаёт [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MasterLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
	NodePath = "<id>",
	GatewayDirectory = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_masterlink.md).

## См. также

[Настройки коннектора](configuration_masterlink.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
