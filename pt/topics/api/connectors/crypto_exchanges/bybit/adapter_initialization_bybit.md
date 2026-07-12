# Inicialização do adaptador ByBit

O código abaixo demonstra como inicializar o [ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<A sua chave de API>".To<SecureString>(),
	Secret = "<O seu segredo de API>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Uma forma alternativa e mais conveniente é usar o método de extensão `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<A sua chave de API>".To<SecureString>();
	a.Secret = "<O seu segredo de API>".To<SecureString>();
});
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
