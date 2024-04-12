# Инициализация адаптера Oanda

Код ниже демонстрирует как инициализировать [OandaMessageAdapter](xref:StockSharp.Oanda.OandaMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OandaMessageAdapter(Connector.TransactionIdGenerator)
{
    Token = "<Your Token>".To<SecureString>(),
    Server = OandaServers.Practice, // Demo
    //Server = OandaServers.Real,   // Real
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
