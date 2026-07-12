# Inicialização do adaptador FIX

O código abaixo demonstra como inicializar o [FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Uma forma alternativa e mais conveniente é usar o método de extensão `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<FixMessageAdapter>(a =>
{
	a.Login = "<O seu login>";
	a.Password = "<A sua palavra-passe>".To<SecureString>();
	a.Address = "<Address>".To<EndPoint>();
});
```

## Veja também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
