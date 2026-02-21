# Инициализация адаптера FIX

Код ниже демонстрирует, как инициализировать [FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

Альтернативный, более удобный способ -- использование метода расширения `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<FixMessageAdapter>(a =>
{
	a.Login = "<Your Login>";
	a.Password = "<Your Password>".To<SecureString>();
	a.Address = "<Address>".To<EndPoint>();
});
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
