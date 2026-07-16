# Inicialização do adaptador Longbridge OpenAPI

O código seguinte demonstra como inicializar o [LongbridgeMessageAdapter](xref:StockSharp.Longbridge.LongbridgeMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LongbridgeMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Chave da aplicação>",
	AppSecret = "<Segredo da aplicação>".ToSecureString(),
	AccessToken = "<Token de acesso>".ToSecureString(),
	Portfolio = "Longbridge",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

