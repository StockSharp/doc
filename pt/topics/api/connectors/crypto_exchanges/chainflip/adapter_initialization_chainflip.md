# Inicialização do adaptador: Chainflip

O código a seguir inicializa [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<O endereço da sua carteira EVM>",
	PrivateKey = "<Sua chave privada EVM>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os dados da carteira, os endereços de destino e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_chainflip.md).

## Veja também

[Configuração do conector](configuration_chainflip.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
