# Inicialização do adaptador: Toss Securities

O código a seguir inicializa [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TossSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_toss_securities.md).

## Veja também

[Configuração do conector](configuration_toss_securities.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
