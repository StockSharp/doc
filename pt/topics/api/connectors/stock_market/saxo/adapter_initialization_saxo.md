# Inicialização do adaptador Saxo OpenAPI

O código seguinte demonstra como inicializar o [SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Token de acesso>".ToSecureString(),
	RefreshToken = "<Token de atualização>".ToSecureString(),
	ClientId = "<Identificador do cliente>",
	ClientSecret = "<Segredo do cliente>".ToSecureString(),
	RedirectUri = "<URI de redirecionamento>",
	AccountKey = "<Chave da conta>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

