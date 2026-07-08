# Backtesting settings

Das Panel **Properties** ist standardmäßig auf der rechten Seite des Strategietabs minimiert. Dieses Panel ist eine Tabelle mit Emulations- oder Live-Trade-Eigenschaften. Wenn Sie eine bestimmte Eigenschaft auswählen, erscheint unten in der Tabelle eine detaillierte Beschreibung dieser Eigenschaft. Alle Eigenschaften sind in Gruppen zusammengefasst:

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Settings**

- **Market data** - der Datenspeicher.
- **Storage format** - das Speicherformat.
- **Data type** - der Datentyp.
- **Time frame** - Kerzen mit dem angegebenen Zeitrahmen verwenden.
- **Maximum quote volume in generated depth** - das maximale Quote-Volumen im generierten Orderbuch.
- **Interval** - das Zeitintervall.
- **Unrealized P\/L** - das Intervall zur Neuberechnung des nicht realisierten Gewinns.
- **Trades** - welche Trades verwendet werden.
- **Marked depth** - welche Orderbücher verwendet werden.
- **Order log** - das Order-Log verwenden.
- **Number of strategies** - die Anzahl gleichzeitig getesteter Strategien.
- **Logging level** - der Logging-Level.
- **Combine on touch** - während der Emulation Orders zusammenführen, wenn der Trade-Preis den Orderpreis berührt (d. h. ihm entspricht).
- **Marked depth (lifetime)** - die maximale Zeit, während der sich das Orderbuch im Emulator befindet. Wenn es während dieser Zeit keine Aktualisierung gab, wird das Orderbuch gelöscht. Diese Eigenschaft kann verwendet werden, um alte Orderbücher zu entfernen, wenn die Daten Lücken enthalten.
- **Errors percentage** - der Prozentwert des Fehlers bei der Registrierung neuer Orders. Der Wert kann von 0 (keine Fehler) bis 100 reichen.
- **Latency** - der Mindestwert der registrierten Orderlatenz.
- **Reregistering** - ob das erneute Registrieren von Orders als einzelner Trade unterstützt wird.
- **Buffering period** - Antworten in einem einzelnen Paket in Intervallen senden. Die Netzwerklatenz und der gepufferte Betrieb des Börsenkerns werden emuliert.
- **Order ID** - die Nummer, ab der der Emulator Kennungen für Orders generiert.
- **Trade ID** - die Nummer, ab der der Emulator Kennungen für Trades generiert.
- **Transaction** - die Nummer, ab der der Emulator Kennungen für Ordertransaktionen generiert.
- **Spread size** - die Spread-Größe in Preisschritten. Sie wird beim Angeben des Spreads für die Generierung eines Orderbuchs aus Tick-Trades verwendet.
- **Depth of book** - die maximale Tiefe des Orderbuchs, das aus den Ticks generiert wird.
- **Number of volume steps** - die Anzahl der Volumenschritte, um die die Order größer ist als der Tick-Trade. Wird beim Testen auf Tick-Trades verwendet.
- **Portfolios interval** - das Intervall für die Neuberechnung des Portfolios. Wenn das Intervall null ist, wird keine Neuberechnung durchgeführt.
- **Change time** - die Zeit für Orders und Trades in Börsenzeit ändern.
- **Time zone** - Informationen über die Zeitzone, in der sich die Börse befindet.
- **Price shift** - die Preisverschiebung vom letzten Trade, mit der die Grenzen der maximalen und minimalen Preise für die nächste Sitzung angegeben werden.
- **Add extra volume** - zusätzliches Volumen zum Orderbuch hinzufügen, wenn Orders mit großem Volumen registriert werden.
- **[Commissions](../commissions.md)** - die Provision (Brokerage, Börse usw.).

**Logging**

- **Logging level** - der Logging-Level für dieses Element.

**Setting**

- **[Risk Management](../risk_management.md)** - die Risikomanagement-Einstellungen.

**Diagram parameters**

- **Security** - das Instrument.
- **Portfolio** - das Portfolio.

Wenn Sie die **Diagram parameters** nicht ausfüllen, wird bei der Emulation das Instrument aus dem Feld **Instrument** der Registerkarte **Emulation** verwendet; als Portfolio wird standardmäßig das Testportfolio verwendet.

## Empfohlene Inhalte

[Chart](chart.md)

