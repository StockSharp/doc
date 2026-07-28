# Inicialização do adaptador: 0x

O código a seguir inicializa [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Sua chave de API>".To<SecureString>(),
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_zero_x.md).

## Veja também

[Configuração do conector](configuration_zero_x.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
