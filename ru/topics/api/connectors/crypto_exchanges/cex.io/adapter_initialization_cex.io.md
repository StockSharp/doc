# Инициализация адаптера CEX.IO

Код ниже демонстрирует, как инициализировать [CexMessageAdapter](xref:StockSharp.Cex.CexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
