# Инициализация адаптера: Korean FSC

Следующий код создаёт [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KoreanFscMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_korean_fsc.md).

## См. также

[Настройки коннектора](configuration_korean_fsc.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
