# Inicialização do adaptador QFEX

O código abaixo demonstra como inicializar o [QFEXMessageAdapter](xref:StockSharp.QFEX.QFEXMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QFEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Seu valor>",
	Secret = "<Seu valor>".To<SecureString>(),
	AccountId = "<Seu valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
