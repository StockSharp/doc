# Один логин \- несколько роботов

Роутер [Plaza II](../plaza.md) позволяет одновременно подключаться нескольким роботам. Для этого необходимо, чтобы каждое из подключений имело уникальное имя. Имя для подключения задаётся через свойство [PlazaMessageAdapter.AppName](xref:StockSharp.Plaza.PlazaMessageAdapter.AppName)

```cs
var messageAdapter = new PlazaMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = SmartComAddresses.Demo,
	AppName = "robot_PRADA",
};      
		
```
