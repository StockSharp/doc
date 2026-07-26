> [!CAUTION]
> **Биржа PrizmBit и её API больше недоступны. Коннектор не работает; документация сохранена только для справки.**

# Инициализация адаптера PrizmBit

Код ниже демонстрирует, как инициализировать [PrizmBitMessageAdapter](xref:StockSharp.PrizmBit.PrizmBitMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new PrizmBitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
