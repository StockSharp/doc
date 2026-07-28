# Inicialização do adaptador: Pendle

O código a seguir inicializa [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new PendleMessageAdapter(connector.TransactionIdGenerator)
{
	Chain = PendleChains.Ethereum,
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os dados da carteira e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_pendle.md).

## Veja também

[Configuração do conector](configuration_pendle.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
