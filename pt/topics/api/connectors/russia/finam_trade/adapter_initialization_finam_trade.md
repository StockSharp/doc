# Inicialização do adaptador: API de negociação da Finam

O código a seguir inicializa [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Defina `Token` com o segredo da API de negociação da Finam. Omita `AccountId` para que o adaptador use a primeira conta disponível para o token. As propriedades adicionais estão descritas na página de [Configuração do conector](configuration_finam_trade.md).

## Veja também

[Configuração do conector](configuration_finam_trade.md)

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
