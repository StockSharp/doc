> [!CAUTION]
> **Биржа CoinBene прекратила работу. Коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера CoinBene

Код ниже демонстрирует, как инициализировать [CoinBeneMessageAdapter](xref:StockSharp.CoinBene.CoinBeneMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new CoinBeneMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
