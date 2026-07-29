# Inicialização do adaptador: SSI

O código a seguir inicializa [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SSIMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
	ClientId = "<Seu identificador de cliente>",
	PrivateKey = "<Sua chave RSA privada>".To<SecureString>(),
	Otp = "<OTP atual>".To<SecureString>(),
	Account = "<O número da sua conta>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_ssi.md).

## Veja também

[Configuração do conector](configuration_ssi.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
