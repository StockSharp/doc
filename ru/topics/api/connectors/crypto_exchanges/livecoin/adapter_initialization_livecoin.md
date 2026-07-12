> [!WARNING]
> Данная биржа прекратила работу (декабрь 2020 — взлом и закрытие). Коннектор более не функционирует. Документация сохранена для справки.

# Инициализация адаптера Livecoin

Код ниже демонстрирует, как инициализировать [LiveCoinMessageAdapter](xref:StockSharp.LiveCoin.LiveCoinMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiveCoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
