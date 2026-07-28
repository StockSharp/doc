# Inicialização do adaptador: CoinGlass

O código a seguir inicializa [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinGlassMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Seu token de acesso>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_coinglass.md).

## Veja também

[Configuração do conector](configuration_coinglass.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
