# Инициализация адаптера Oanda

Код ниже демонстрирует как инициализировать [OandaMessageAdapter](../api/StockSharp.Oanda.OandaMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

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

[Окно настройки подключений](API_UI_ConnectorWindow.md)
