# Initialisierung des CTP-Adapters

Der folgende Code zeigt, wie [CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Benutzername>",
	Password = "<Passwort>".ToSecureString(),
	BrokerId = "<Brokerkennung>",
	InvestorId = "<Anlegerkennung>",
	MarketDataAddress = "tcp://<Marktdatenadresse>",
	TraderAddress = "tcp://<Handelsserveradresse>",
	AppId = "<Anwendungskennung>",
	AuthCode = "<Authentifizierungscode>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
