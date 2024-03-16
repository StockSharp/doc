# Инициализация адаптера Alor

Код ниже демонстрирует как инициализировать [AlorMessageAdapter](xref:StockSharp.Alor.AlorMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlorMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>",
	// раскоментировать для подключения к демо торгам
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
