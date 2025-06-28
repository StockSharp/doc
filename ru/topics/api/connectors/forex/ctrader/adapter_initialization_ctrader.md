# Инициализация адаптера cTrader

Код ниже демонстрирует как инициализировать [cTraderMessageAdapter](xref:StockSharp.cTrader.cTraderMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new cTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
