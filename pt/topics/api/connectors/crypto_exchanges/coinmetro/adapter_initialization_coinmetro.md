# Inicialização do adaptador: Coinmetro

O código a seguir inicializa [CoinmetroMessageAdapter](xref:StockSharp.Coinmetro.CoinmetroMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinmetroMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Seu token de acesso>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_coinmetro.md).

## Veja também

[Configuração do conector](configuration_coinmetro.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
