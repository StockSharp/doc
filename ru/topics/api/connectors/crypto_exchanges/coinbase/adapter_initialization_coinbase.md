# Инициализация адаптера Coinbase

Код ниже демонстрирует, как инициализировать [CoinbaseMessageAdapter](xref:StockSharp.Coinbase.CoinbaseMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new  CoinbaseMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

Альтернативный, более удобный способ -- использование метода расширения `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<CoinbaseMessageAdapter>(a =>
{
	a.Key = "<Your API Key>".To<SecureString>();
	a.Secret = "<Your API Secret>".To<SecureString>();
});
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
