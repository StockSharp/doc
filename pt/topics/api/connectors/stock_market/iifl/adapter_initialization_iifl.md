# Inicialização do adaptador: IIFL

O código a seguir inicializa [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
	ClientId = "<Seu identificador de cliente>",
	AuthorizationCode = "<Seu código de autorização>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os valores de acesso e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_iifl.md).

## Veja também

[Configuração do conector](configuration_iifl.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
