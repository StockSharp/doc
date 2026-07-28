# Inicialização do adaptador: Dexalot

O código a seguir inicializa [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexalotMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os dados da carteira e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_dexalot.md).

## Veja também

[Configuração do conector](configuration_dexalot.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
