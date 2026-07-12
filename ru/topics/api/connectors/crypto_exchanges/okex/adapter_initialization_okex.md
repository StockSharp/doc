# Инициализация адаптера OKEx

Код ниже демонстрирует, как инициализировать [OkexMessageAdapter](xref:StockSharp.Okex.OkexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new OkexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
