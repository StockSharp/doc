# Einstellungen

[Hydra](../../hydra.md) kann im Servermodus verwendet werden. In diesem Modus koennen Sie sich remote mit [Hydra](../../hydra.md) verbinden und vorhandene Daten aus dem Speicher abrufen. Sie koennen sich aus [Designer](../../designer.md) mit [Hydra](../../hydra.md) im Servermodus verbinden (siehe [Erste Schritte](../../designer/market_data_storage/getting_started.md) in der Dokumentation von [Designer](../../designer.md)). Sie koennen auch ueber die [API](../../api.md) eine Verbindung zu [Hydra](../../hydra.md) herstellen (Details finden Sie im Abschnitt [FIX/FAST connectivity](fix_fast_connectivity.md)).

Im Servermodus ermoeglicht das Programm [Hydra](../../hydra.md) dem Benutzer, mit einer Verbindung gleichzeitig in mehreren Programmen zu arbeiten. Durch Festlegen des Zugriffsschluessels in den Programmeinstellungen kann der Benutzer gleichzeitig mit einer Quelle unter einem Konto arbeiten.

Tatsaechlich erfolgt die Verbindung zur Quelle ueber [Hydra](../../hydra.md), mit dem zum Beispiel [Designer](../../designer.md) und [Terminal](../../terminal.md) gleichzeitig verbunden sind. Diese Methode ermoeglicht es, erneute Verbindungen zwischen Programmen und den Kauf einer zusaetzlichen Verbindung zu vermeiden. Bei einer solchen Arbeit sind Konflikte ausgeschlossen, die durch Orderregistrierungen oder Trades aus unterschiedlichen Programmen entstehen koennen. [Hydra](../../hydra.md) empfaengt das Signal und gibt das Ergebnis an das Programm zurueck, von dem es empfangen wurde, waehrend der Ablauf anderer Arbeiten nicht gestoert wird.

Um den Servermodus von [Hydra](../../hydra.md) zu aktivieren, waehlen Sie im oberen Menue des Programms die Registerkarte **Server mode**.

![hydra server menu](../../../images/hydra_server_menu.png)

Klicken Sie danach auf die Schaltflaeche **Settings**, um das Einstellungsfenster fuer den Servermodus zu oeffnen.

![hydra server](../../../images/hydra_server.png)

**Hydra Server**

- **FIX server** - [Hydra](../../hydra.md) in den Servermodus schalten, der Live-Handelsdaten und historische Daten ueber das FIX-Protokoll verteilt.

  In diesem Abschnitt konfigurieren Sie die Verbindung fuer die Arbeit mit Quellen:
  1. **ConvertToLatin** - Kyrillisch in Latein umwandeln.
  2. **QuotesInterval** - Zeitraum fuer Quotes-Aktualisierungen.
  3. **TransactionSession** - Einstellung einer Handelssitzung. Einrichtung fuer den Handel ueber das Programm [Hydra](../../hydra.md).

     Mit dieser Einstellung koennen Sie Dialect des FIX-Protokolls, Sender und Recipient, Datenformat und weitere Einstellungen konfigurieren. Details finden Sie unter [FIXServer properties](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm).
  4. **MarketDataSession** - Einstellungen fuer die Uebertragung von Marktdaten, die mit [Hydra](../../hydra.md) empfangen wurden. Details finden Sie unter [FIXServer properties](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm).
  5. **KeepSubscriptionsOnDisconnect** - Subscriptions beim Trennen der Verbindung zur Quelle beibehalten.
  6. **DeadSessionCleanupInterval** - nach welchem Zeitintervall Informationen geloescht werden, wenn die Verbindung getrennt ist.
- **Authorization** - Autorisierung fuer den Zugriff auf den Hydra-Server.
- **Number of securities** - die maximale Anzahl von Instrumenten, die vom Server angefordert werden kann.
- **Candles (days)** - die maximale Anzahl von Tagen, die zum Herunterladen der Kerzenhistorie verfuegbar ist.
- **Ticks (days)** - die maximale Anzahl von Tagen, die zum Herunterladen der Tick-Datenhistorie verfuegbar ist.
- **Order books (days)** - die maximale Anzahl von Tagen, die zum Herunterladen der Order-Book-Historie verfuegbar ist.
- **OL (days)** - die maximale Anzahl von Tagen, die zum Herunterladen der OL-Datenhistorie verfuegbar ist.
- **Transactions (days)** - die maximale Anzahl von Tagen, die zum Herunterladen der Transaktionshistorie verfuegbar ist.
- **Simulator** - Simulatormodus einschalten.
- **Security mapping** - Uebertragungsmodus nur fuer angegebene Instrumente aktivieren.

Wenn Sie **Authorization** auf einen anderen Wert als **Anonymous** setzen, erscheint auf der Registerkarte **Common** die Schaltflaeche **Users**. Nach dem Klicken darauf erscheint das Fenster **Users**.

![hydra users](../../../images/hydra_users.png)

Auf der linken Seite des Fensters koennen Sie einen neuen Benutzer hinzufuegen, und rechts koennen Sie dessen Zugriffsrechte festlegen.
