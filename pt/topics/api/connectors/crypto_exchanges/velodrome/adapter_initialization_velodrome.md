# Inicialização do adaptador: Velodrome

O código a seguir inicializa [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new VelodromeMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_velodrome.md).

## Veja também

[Configuração do conector](configuration_velodrome.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
