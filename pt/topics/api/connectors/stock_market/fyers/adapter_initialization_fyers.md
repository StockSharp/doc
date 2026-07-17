# Inicialização do adaptador FYERS

O código seguinte demonstra como inicializar o [FyersMessageAdapter](xref:StockSharp.Fyers.FyersMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FyersMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Identificador do cliente>",
	Token = "<Token>".ToSecureString(),
	DefaultProduct = FyersProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)
