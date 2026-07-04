# Verbindung mehrerer Algorithmen

Je nach konkretem Benutzer bzw. konkreter Anwendung unterstützt der OEC-Server möglicherweise keine gleichzeitige Verbindung mehrerer Anwendungen. In diesem Fall können andere Verbindungen unterbrochen werden. Um diese Einschränkungen zu umgehen, unterstützt diese [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader)-Implementierung den gleichzeitigen Betrieb mehrerer Anwendungen über eine einzige Verbindung zum OEC-Server - [OECRemoting](https://gainfutures.com/gainfuturesapi).

Die folgenden Modi von [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) werden unterstützt:

- [None](xref:StockSharp.OpenECry.OpenECryRemoting.None) - [OpenECryRemoting](xref:StockSharp.OpenECry.OpenECryRemoting) ist getrennt. Die Anwendung erstellt ihre eigene Verbindung zum OEC-Server. Die Anwendung kann anderen Anwendungen nicht als [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) dienen.
- [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) - die Anwendung erstellt ihre eigene Verbindung zum OEC-Server.
- [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary) - sucht bei der Initialisierung nach lokalen Anwendungen, die im Modus [Primary](xref:StockSharp.OpenECry.OpenECryRemoting.Primary) ausgeführt werden. Werden solche Anwendungen gefunden, verwendet sie deren Verbindung zum OEC-Server. Andernfalls wechselt die Anwendung in den Modus [None](xref:StockSharp.OpenECry.OpenECryRemoting.None).

Um den Modus [OECRemoting](https://gainfutures.com/gainfuturesapi) explizit festzulegen, geben Sie den gewünschten Modus unmittelbar nach dem Erstellen des [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader)-Objekts an. Beispiel für den Modus [Secondary](xref:StockSharp.OpenECry.OpenECryRemoting.Secondary):

```cs
Trader.RemotingRequired = OECRemoting.Secondary;
		
```

Standardmäßig arbeitet der [OpenECryTrader](xref:StockSharp.OpenECry.OpenECryTrader)-Adapter im Modus [OpenECryRemoting.None](xref:StockSharp.OpenECry.OpenECryRemoting.None).
