# Инициализация адаптера Plaza II

Код ниже демонстрирует как инициализировать [PlazaMessageAdapter](../api/StockSharp.Plaza.PlazaMessageAdapter.html) и передать его в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var address \= "\<Address\>".To\<IPAddress\>();
var messageAdapter \= new PlazaMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
    Address \= SmartComAddresses.Demo,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

> [!CAUTION]
> Если при установке роутера был введен логин и пароль, то в коде их указывать **не надо**.

> [!TIP]
> При соединении между клиентским приложением и роутером в платформе [Plaza II](Plaza.md) возможно использование протокола TCP или LRPCQ. Последний является простым транспортом, основанным на использовании shared\-memory в ОС Windows. Использование LRPCQ возможно только при запуске приложения\-клиента и роутера на одном компьютере. Протокол LRPCQ имеет меньшие накладные расходы, чем TCP, передача сообщений между приложением и роутером с использованием LRPCQ происходит быстрее. Чтобы использовать протокол LRPCQ, нужно присвоить свойству [PlazaMessageAdapter.UseLocalProtocol](../api/StockSharp.Plaza.PlazaMessageAdapter.UseLocalProtocol.html) значение true. По умолчанию оно равно false, то есть используется протокол TCP. 

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
