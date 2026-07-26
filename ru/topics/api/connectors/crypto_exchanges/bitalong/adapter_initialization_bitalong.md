> [!CAUTION]
> **Биржа Bitalong и её API больше недоступны. Коннектор не работает; документация сохранена только для справки.**

# Инициализация адаптера Bitalong

Код ниже демонстрирует, как инициализировать [BitalongMessageAdapter](xref:StockSharp.Bitalong.BitalongMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitalongMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
