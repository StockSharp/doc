> [!CAUTION]
> **Биржа BitMEX закрывается 23 сентября 2026 года; регистрация новых пользователей уже остановлена. После закрытия коннектор перестанет работать.**

# Инициализация адаптера BitMEX

Код ниже демонстрирует как инициализировать [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
