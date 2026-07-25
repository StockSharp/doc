# Connector-Konfiguration: StocksTrader

Erzeugen Sie ein Token im StocksTrader-Webterminal und geben Sie die Verbindungsparameter an.

- `Token` - vom Webterminal ausgestelltes Bearer-Token.
- `AccountId` - Kontokennung. Optional, wenn genau ein Konto zum gewählten Modus passt.
- `IsDemo` - wählt das Demokonto. Standardwert: `true`.
- `Address` - REST-Endpunkt. Standardwert: `https://api.stockstrader.com/`.
- `PollingInterval` - Abfrageintervall für Aufträge, Geschäfte und den Kontozustand. Standardwert: 5 Sekunden; kürzere Werte werden auf 2 Sekunden angehoben.

Da der Anbieter keine Echtzeitübertragung bereitstellt, bestimmt `PollingInterval`, wie schnell Auftrags- und Positionsänderungen die Strategie erreichen. Verkürzen Sie es für aktiven Handel und verlängern Sie es, um die Anfragelimits des Anbieters einzuhalten.

Schutzpreise werden über [StocksTraderOrderCondition](xref:StockSharp.StocksTrader.StocksTraderOrderCondition) übergeben: die Stop-Loss- und Take-Profit-Preise des Auftrags oder der offenen Position.

## Siehe auch

[Offizielle StocksTrader-API-Dokumentation](https://api-doc.stockstrader.com/)
