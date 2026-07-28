# Inicialização do adaptador: XRPL DEX

O código a seguir inicializa [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<O endereço da sua conta XRPL>",
	Seed = "<Sua semente familiar secreta>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os dados da conta e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_xrpl.md).

## Veja também

[Configuração do conector](configuration_xrpl.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
