# Inicialização do adaptador: CoinCatch

O código a seguir inicializa [CoinCatchMessageAdapter](xref:StockSharp.CoinCatch.CoinCatchMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinCatchMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
	Passphrase = "<Sua frase de acesso da API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_coincatch.md).

## Veja também

[Configuração do conector](configuration_coincatch.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
