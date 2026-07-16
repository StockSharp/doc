# Inicialização do adaptador DhanHQ

O código seguinte demonstra como inicializar o [DhanMessageAdapter](xref:StockSharp.Dhan.DhanMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DhanMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Identificador do cliente>",
	Token = "<Token>".ToSecureString(),
	DefaultProduct = DhanProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

