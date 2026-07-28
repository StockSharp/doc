# Inicialização do adaptador: AltCoinTrader

O código a seguir inicializa [AltCoinTraderMessageAdapter](xref:StockSharp.AltCoinTrader.AltCoinTraderMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new AltCoinTraderMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Sua chave de API>".To<SecureString>(),
	Secret = "<Seu segredo de API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_altcointrader.md).

## Veja também

[Configuração do conector](configuration_altcointrader.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
