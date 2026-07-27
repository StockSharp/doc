# Inicialização do adaptador: HDFC Securities

O código a seguir inicializa [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new HdfcMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_hdfc_securities.md).

## Veja também

[Configuração do conector](configuration_hdfc_securities.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
