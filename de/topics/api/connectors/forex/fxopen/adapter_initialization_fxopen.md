# Adapter initialisieren: FXOpen TickTrader

Der folgende Code initialisiert [FXOpenMessageAdapter](xref:StockSharp.FXOpen.FXOpenMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new FXOpenMessageAdapter(Connector.TransactionIdGenerator)
{
	WebApiId = "<web-api-id>",
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die Token-Parameter des gewählten Live- oder Demokontos.

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
