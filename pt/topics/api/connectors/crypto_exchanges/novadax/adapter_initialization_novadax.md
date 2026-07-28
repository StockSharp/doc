# Inicialização do adaptador: NovaDAX

O código a seguir inicializa [NovaDaxMessageAdapter](xref:StockSharp.NovaDax.NovaDaxMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new NovaDaxMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
	AccountId = "<Seu identificador de conta>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_novadax.md).

## Veja também

[Configuração do conector](configuration_novadax.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
