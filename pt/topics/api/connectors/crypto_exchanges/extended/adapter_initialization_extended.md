# Inicialização do adaptador Extended

O código abaixo demonstra como inicializar o [ExtendedMessageAdapter](xref:StockSharp.Extended.ExtendedMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ExtendedMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Seu valor>".To<SecureString>(),
	PrivateKey = "<Seu valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
