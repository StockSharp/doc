> [!CAUTION]
> **Сервис GDAX больше недоступен. Его преемник Coinbase Pro также отключён; для Coinbase следует использовать актуальный коннектор Coinbase. Этот коннектор не работает; документация сохранена только для справки.**

# Инициализация адаптера GDAX

Код ниже демонстрирует, как инициализировать [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
