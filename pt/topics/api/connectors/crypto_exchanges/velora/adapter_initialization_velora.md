# Inicialização do adaptador: Velora

O código a seguir inicializa [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new VeloraMessageAdapter(connector.TransactionIdGenerator)
{
	Partner = "<Seu identificador de parceiro>",
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_velora.md).

## Veja também

[Configuração do conector](configuration_velora.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
