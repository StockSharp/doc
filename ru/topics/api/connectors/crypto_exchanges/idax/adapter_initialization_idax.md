> [!CAUTION]
> **Биржа IDAX прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера Idax

Код ниже демонстрирует, как инициализировать [IdaxMessageAdapter](xref:StockSharp.Idax.IdaxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new IdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
