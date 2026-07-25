# Connector-Konfiguration: Deriv

Erstellen Sie ein Deriv-Token und geben Sie die Verbindungsparameter an.

- `Token` - persönliches Zugriffs- oder OAuth-Token. Für private Operationen erforderlich.
- `AppId` - Anwendungskennung, die mit authentifizierten REST-Anfragen gesendet wird.
- `AccountId` - Kennung des Optionskontos. Optional, wenn genau ein aktives Konto zum gewählten Modus passt.
- `IsDemo` - wählt das Demokonto. Standardwert: `true`.
- `RestAddress` - REST-Endpunkt. Standardwert: `https://api.derivws.com`.
- `PublicWebSocketAddress` - öffentlicher Options-WebSocket-Endpunkt. Standardwert: `wss://api.derivws.com/trading/v1/options/ws/public`.

Öffentliche Marktdatensitzungen laufen ohne Token, während Kontrakte, Salden und Transaktionen das Token und die Anwendungskennung erfordern. Abonnements werden automatisch über eine neue einmalige WebSocket-Adresse wiederhergestellt.

Kontraktparameter werden über [DerivOrderCondition](xref:StockSharp.Deriv.DerivOrderCondition) übergeben: Kontrakttyp, ob der Betrag ein Einsatz oder eine Auszahlung ist, Kontraktwährung, Laufzeit, Barrieren sowie die schützenden Stop-Loss- und Take-Profit-Preise.

## Siehe auch

[Offizielle Deriv-API-Dokumentation](https://developers.deriv.com/docs/)
