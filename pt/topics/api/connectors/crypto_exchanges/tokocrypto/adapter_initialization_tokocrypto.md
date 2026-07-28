# Inicialização do adaptador: Tokocrypto

O código a seguir inicializa [TokocryptoMessageAdapter](xref:StockSharp.Tokocrypto.TokocryptoMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TokocryptoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_tokocrypto.md).

## Veja também

[Configuração do conector](configuration_tokocrypto.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
