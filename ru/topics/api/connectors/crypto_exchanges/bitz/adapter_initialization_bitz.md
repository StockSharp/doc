> [!CAUTION]
> **Биржа BitZ прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера BitZ

Код ниже демонстрирует, как инициализировать [BitZMessageAdapter](xref:StockSharp.BitZ.BitZMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitZMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
