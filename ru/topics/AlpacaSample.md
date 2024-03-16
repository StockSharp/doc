# Инициализация адаптера Alpaca

Код ниже демонстрирует как инициализировать [AlpacaMessageAdapter](xref:StockSharp.Alpaca.AlpacaMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlpacaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	// раскомментировать для подключения к демо торгам
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
