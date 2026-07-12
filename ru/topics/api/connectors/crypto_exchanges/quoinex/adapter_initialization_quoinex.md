> [!NOTE]
> QUOINEX была переименована в Liquid, которая прекратила работу в 2022. Данная документация сохранена для справки.

# Инициализация адаптера Quoinex

Код ниже демонстрирует, как инициализировать [QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
