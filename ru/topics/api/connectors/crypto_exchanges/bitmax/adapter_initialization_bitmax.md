> [!CAUTION]
> **Биржа BitMax, позднее переименованная в AscendEX, прекратила работу 1 июля 2026 года. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера BitMax

Код ниже демонстрирует как инициализировать [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
