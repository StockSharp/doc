# Adapter initialisieren: Paytm Money

Der folgende Code initialisiert [PaytmMoneyMessageAdapter](xref:StockSharp.PaytmMoney.PaytmMoneyMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

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

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_paytm_money.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_paytm_money.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
