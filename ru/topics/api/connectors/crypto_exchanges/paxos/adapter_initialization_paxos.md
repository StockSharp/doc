# Инициализация адаптера Paxos

Код ниже демонстрирует как инициализировать [PaxosMessageAdapter](xref:StockSharp.Paxos.PaxosMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PaxosMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ваше значение>".To<SecureString>(),
	ClientSecret = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
