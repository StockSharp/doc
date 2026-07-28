# Inicialização do adaptador: KyberSwap

O código a seguir inicializa [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new KyberSwapMessageAdapter(connector.TransactionIdGenerator)
{
	ClientId = "<Seu identificador de cliente>",
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_kyber_swap.md).

## Veja também

[Configuração do conector](configuration_kyber_swap.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
