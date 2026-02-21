> [!NOTE]
> GDAX была переименована в Coinbase Pro, затем в Coinbase Advanced Trade. Данная документация сохранена для справки.

# Инициализация адаптера GDAX

Код ниже демонстрирует, как инициализировать [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
