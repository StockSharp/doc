# Inicialização do adaptador: EDINET

O código a seguir inicializa [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EdinetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_edinet.md).

## Veja também

[Configuração do conector](configuration_edinet.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
