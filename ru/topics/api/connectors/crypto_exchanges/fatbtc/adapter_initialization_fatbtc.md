> [!CAUTION]
> **Биржа FatBTC прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера FatBTC

Код ниже демонстрирует, как инициализировать [FatBtcMessageAdapter](xref:StockSharp.FatBTC.FatBtcMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new FatBtcMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
