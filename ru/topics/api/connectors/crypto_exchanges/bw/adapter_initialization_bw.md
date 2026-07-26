> [!CAUTION]
> **Биржа BW прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера BW

Код ниже демонстрирует, как инициализировать [BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
