# Инициализация адаптера ByBit

Код ниже демонстрирует, как инициализировать [ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Ваш API-ключ>".To<SecureString>(),
				Secret = "<Ваш API-секрет>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

Альтернативный, более удобный способ -- использование метода расширения `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<Ваш API-ключ>".To<SecureString>();
	a.Secret = "<Ваш API-секрет>".To<SecureString>();
});
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
