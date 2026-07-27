# Инициализация адаптера: B3 UP2DATA

Следующий код создаёт [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new B3Up2DataMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_b3_up2data.md).

## См. также

[Настройки коннектора](configuration_b3_up2data.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
