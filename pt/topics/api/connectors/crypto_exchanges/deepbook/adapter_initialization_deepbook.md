# Inicialização do adaptador: DeepBook

O código a seguir inicializa [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DeepBookMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<O endereço da sua carteira>",
	PrivateKey = "<Sua chave privada>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_deepbook.md).

## Veja também

[Configuração do conector](configuration_deepbook.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
