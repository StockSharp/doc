# Инициализация адаптера Interactive Brokers

Код ниже демонстрирует, как инициализировать [InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Your Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

Альтернативный, более удобный способ -- использование метода расширения `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<InteractiveBrokersMessageAdapter>(a =>
{
	a.Address = "<Your Address>".To<EndPoint>();
});
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)
