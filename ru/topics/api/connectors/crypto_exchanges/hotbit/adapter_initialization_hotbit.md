> [!CAUTION]
> **Биржа Hotbit прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера Hotbit

Код ниже демонстрирует, как инициализировать [HotbitMessageAdapter](xref:StockSharp.Hotbit.HotbitMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HotbitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
