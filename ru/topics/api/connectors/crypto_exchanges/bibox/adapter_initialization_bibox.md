> [!CAUTION]
> **API биржи Bibox, который использует этот коннектор, больше недоступен. Коннектор не работает; документация сохранена только для справки.**

# Инициализация адаптера Bibox

Код ниже демонстрирует, как инициализировать [BiboxMessageAdapter](xref:StockSharp.Bibox.BiboxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BiboxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
