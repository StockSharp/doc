# Einstellungen

[Hydra](../../hydra.md) kann im Servermodus verwendet werden. In diesem Modus können Sie sich remote mit [Hydra](../../hydra.md) verbinden und vorhandene Daten aus dem Speicher abrufen. Sie können sich aus [Designer](../../designer.md) mit [Hydra](../../hydra.md) im Servermodus verbinden (siehe [Erste Schritte](../../designer/market_data_storage/getting_started.md) in der Dokumentation von [Designer](../../designer.md)). Sie können auch über die [API](../../api.md) eine Verbindung zu [Hydra](../../hydra.md) herstellen (Details finden Sie im Abschnitt [FIX/FAST connectivity](fix_fast_connectivity.md)).

Im Servermodus ermöglicht das Programm [Hydra](../../hydra.md) dem Benutzer, mit einer Verbindung gleichzeitig in mehreren Programmen zu arbeiten. Durch Festlegen des Zugriffsschlüssels in den Programmeinstellungen kann der Benutzer gleichzeitig mit einer Quelle unter einem Konto arbeiten.

Tatsächlich erfolgt die Verbindung zur Quelle über [Hydra](../../hydra.md), mit dem zum Beispiel [Designer](../../designer.md) und [Terminal](../../terminal.md) gleichzeitig verbunden sind. Diese Methode ermöglicht es, erneute Verbindungen zwischen Programmen und den Kauf einer zusätzlichen Verbindung zu vermeiden. Bei einer solchen Arbeit sind Konflikte ausgeschlossen, die durch Orderregistrierungen oder Trades aus unterschiedlichen Programmen entstehen können. [Hydra](../../hydra.md) empfängt das Signal und gibt das Ergebnis an das Programm zurück, von dem es empfangen wurde, während der Ablauf anderer Arbeiten nicht gestört wird.

Um den Servermodus von [Hydra](../../hydra.md) zu aktivieren, wählen Sie im oberen Menü des Programms die Registerkarte **Servermodus**.

![hydra server menu](../../../images/hydra_server_menu.png)

Klicken Sie danach auf die Schaltfläche **Einstellungen**, um das Einstellungsfenster für den Servermodus zu öffnen.

![hydra server](../../../images/hydra_server.png)

**Hydra-Server**

- **FIX-Server** - [Hydra](../../hydra.md) in den Servermodus schalten, der Live-Handelsdaten und historische Daten über das FIX-Protokoll verteilt.

  In diesem Abschnitt konfigurieren Sie die Verbindung für die Arbeit mit Quellen:
  1. **ConvertToLatin** - Kyrillisch in Latein umwandeln.
  2. **QuotesInterval** - Zeitraum für Quotes-Aktualisierungen.
  3. **TransactionSession** - Einstellung einer Handelssitzung. Einrichtung für den Handel über das Programm [Hydra](../../hydra.md).

     Mit dieser Einstellung können Sie Dialect des FIX-Protokolls, Sender und Recipient, Datenformat und weitere Einstellungen konfigurieren. Details finden Sie unter [FIXServer properties](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm).
  4. **MarketDataSession** - Einstellungen für die Übertragung von Marktdaten, die mit [Hydra](../../hydra.md) empfangen wurden. Details finden Sie unter [FIXServer properties](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm).
  5. **KeepSubscriptionsOnDisconnect** - Subscriptions beim Trennen der Verbindung zur Quelle beibehalten.
  6. **DeadSessionCleanupInterval** - nach welchem Zeitintervall Informationen gelöscht werden, wenn die Verbindung getrennt ist.
- **Autorisierung** - Autorisierung für den Zugriff auf den Hydra-Server.
- **Anzahl der Instrumente** - die maximale Anzahl von Instrumenten, die vom Server angefordert werden kann.
- **Kerzen (Tage)** - die maximale Anzahl von Tagen, die zum Herunterladen der Kerzenhistorie verfügbar ist.
- **Ticks (Tage)** - die maximale Anzahl von Tagen, die zum Herunterladen der Tick-Datenhistorie verfügbar ist.
- **Orderbücher (Tage)** - die maximale Anzahl von Tagen, die zum Herunterladen der Order-Book-Historie verfügbar ist.
- **OL (Tage)** - die maximale Anzahl von Tagen, die zum Herunterladen der OL-Datenhistorie verfügbar ist.
- **Transaktionen (Tage)** - die maximale Anzahl von Tagen, die zum Herunterladen der Transaktionshistorie verfügbar ist.
- **Simulator** - Simulatormodus einschalten.
- **Instrumentzuordnung** - Übertragungsmodus nur für angegebene Instrumente aktivieren.

Wenn Sie **Autorisierung** auf einen anderen Wert als **Anonym** setzen, erscheint auf der Registerkarte **Allgemein** die Schaltfläche **Benutzer**. Nach dem Klicken darauf erscheint das Fenster **Benutzer**.

![hydra users](../../../images/hydra_users.png)

Auf der linken Seite des Fensters können Sie einen neuen Benutzer hinzufügen, und rechts können Sie dessen Zugriffsrechte festlegen.
