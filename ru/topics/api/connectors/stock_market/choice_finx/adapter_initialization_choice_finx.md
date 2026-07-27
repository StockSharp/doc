# Инициализация адаптера: Choice FinX

Следующий код создаёт [ChoiceFinXMessageAdapter](xref:StockSharp.ChoiceFinX.ChoiceFinXMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ChoiceFinXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_choice_finx.md).

## См. также

[Настройки коннектора](configuration_choice_finx.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
