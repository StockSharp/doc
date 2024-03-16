# Инициализация адаптера PolygonIO

Код ниже демонстрирует как инициализировать [PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // подключение через REST к истории торгов
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
