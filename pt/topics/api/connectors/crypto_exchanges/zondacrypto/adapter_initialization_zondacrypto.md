# Inicialização do adaptador: zondacrypto

O código a seguir inicializa [ZondaCryptoMessageAdapter](xref:StockSharp.ZondaCrypto.ZondaCryptoMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZondaCryptoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_zondacrypto.md).

## Veja também

[Configuração do conector](configuration_zondacrypto.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
