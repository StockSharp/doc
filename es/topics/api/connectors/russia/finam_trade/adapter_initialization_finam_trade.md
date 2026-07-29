# Inicialización del adaptador: API de negociación de Finam

El siguiente código inicializa [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Asigne a `Token` el secreto de la API de negociación de Finam. Omita `AccountId` para que el adaptador utilice la primera cuenta disponible para el token. Las propiedades adicionales se describen en la página de [Configuración del conector](configuration_finam_trade.md).

## Véase también

[Configuración del conector](configuration_finam_trade.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
