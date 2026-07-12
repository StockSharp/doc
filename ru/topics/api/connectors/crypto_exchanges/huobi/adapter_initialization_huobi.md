# Инициализация адаптера Huobi

Код ниже демонстрирует, как инициализировать [HuobiMessageAdapter](xref:StockSharp.Huobi.HuobiMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HuobiMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
