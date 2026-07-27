# Inicialización del adaptador: Paytm Money

El siguiente código inicializa [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PaytmMoneyMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ReadAccessToken = "<token>".ToSecureString(),
	PublicAccessToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_paytm_money.md).

## Véase también

[Configuración del conector](configuration_paytm_money.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
