# Inicialização do adaptador: STON.fi

O código a seguir inicializa [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<O endereço da sua carteira TON>",
	Mnemonic = "<Sua frase mnemônica de 24 palavras>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe os dados da carteira e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_stonfi.md).

## Veja também

[Configuração do conector](configuration_stonfi.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
