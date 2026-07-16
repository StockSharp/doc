# Inicialização do adaptador Angel One

O código seguinte demonstra como inicializar o [AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Nome de utilizador>",
	Password = "<Palavra-passe>".ToSecureString(),
	ApiKey = "<Chave de API>".ToSecureString(),
	TotpSecret = "<Segredo TOTP>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<Endereço IP público do cliente>",
	MacAddress = "<Endereço MAC>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelas credenciais e pelos endereços dos servidores emitidos para a sua conta.

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

