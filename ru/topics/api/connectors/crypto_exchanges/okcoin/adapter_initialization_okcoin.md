> [!CAUTION]
> **Торговые сервисы OKCoin отключены после перехода платформы на OKX. Этот коннектор больше не работает; документация сохранена только для справки.**

# Инициализация адаптера OKCoin

Код ниже демонстрирует, как инициализировать [OkcoinMessageAdapter](xref:StockSharp.Okcoin.OkcoinMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new OkcoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
