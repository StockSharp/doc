# Inicialização do adaptador: Tradejini

O código a seguir inicializa [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) e o adiciona ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradejiniMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<key>".ToSecureString(),
	Password = "<secret>".ToSecureString(),
	TwoFactorCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Informe as credenciais e as demais propriedades necessárias descritas na página de [Configuração do conector](configuration_tradejini.md).

## Veja também

[Configuração do conector](configuration_tradejini.md)

[janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)
