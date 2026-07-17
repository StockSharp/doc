# Inicialização do adaptador Upstox

O código seguinte demonstra como inicializar o [UpstoxMessageAdapter](xref:StockSharp.Upstox.UpstoxMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UpstoxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	IsDemo = true,
	DefaultProduct = UpstoxProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
