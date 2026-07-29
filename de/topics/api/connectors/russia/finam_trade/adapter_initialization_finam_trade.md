# Adapter initialisieren: Finam Trade API

Der folgende Code initialisiert [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie unter `Token` das Finam-Trade-API-Geheimnis ein. Lassen Sie `AccountId` weg, damit der Adapter das erste für das Token verfügbare Konto verwendet. Weitere Eigenschaften sind auf der Seite [Connector-Konfiguration](configuration_finam_trade.md) beschrieben.

## Siehe auch

[Connector-Konfiguration](configuration_finam_trade.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
