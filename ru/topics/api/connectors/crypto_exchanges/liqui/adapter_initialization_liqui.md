> [!CAUTION]
> **Биржа Liqui прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера Liqui

Код ниже демонстрирует, как инициализировать [LiquiMessageAdapter](xref:StockSharp.Liqui.LiquiMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiquiMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
