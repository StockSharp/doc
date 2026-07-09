# Konfiguration OpenECry

Der Interaktionsmechanismus ist in dieser Abbildung dargestellt:

![OECTrader](../../../../../images/oectrader.png)

Wie aus der Abbildung ersichtlich, kommuniziert der [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) über die [GainFutures API](https://gainfutures.com/gainfuturesapi) mit dem OEC-Server. Für die Nutzung der [GainFutures API](https://gainfutures.com/gainfuturesapi) ist kein laufendes OEC Trader-Terminal erforderlich.

Um mit dem Connector zu arbeiten, müssen Sie **Benutzername** und **Passwort** angeben. **Benutzername** und **Passwort** werden vom Broker bereitgestellt. Um API-Zugriff zu erhalten, empfehlen wir, den Broker zu kontaktieren.
