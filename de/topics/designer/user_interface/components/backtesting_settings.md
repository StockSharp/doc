# Backtesting-Einstellungen

Das Panel **Eigenschaften** ist standardmäßig auf der rechten Seite des Strategietabs minimiert. Dieses Panel ist eine Tabelle mit Emulations- oder Live-Trade-Eigenschaften. Wenn Sie eine bestimmte Eigenschaft auswählen, erscheint unten in der Tabelle eine detaillierte Beschreibung dieser Eigenschaft. Alle Eigenschaften sind in Gruppen zusammengefasst:

![Designer Properties emulation 00](../../../../images/designer_properties_emulation_00.png)

**Einstellungen**

- **Marktdaten** - der Datenspeicher.
- **Speicherformat** - das Speicherformat.
- **Datentyp** - der Datentyp.
- **Zeitrahmen** - Kerzen mit dem angegebenen Zeitrahmen verwenden.
- **Maximales Quote-Volumen in generierter Tiefe** - das maximale Quote-Volumen im generierten Orderbuch.
- **Intervall** - das Zeitintervall.
- **Nicht realisierter P\/L** - das Intervall zur Neuberechnung des nicht realisierten Gewinns.
- **Trades** - welche Trades verwendet werden.
- **Markttiefe** - welche Orderbücher verwendet werden.
- **Order-Log** - das Order-Log verwenden.
- **Anzahl der Strategien** - die Anzahl gleichzeitig getesteter Strategien.
- **Protokollierungsstufe** - der Logging-Level.
- **Bei Berührung zusammenführen** - während der Emulation Orders zusammenführen, wenn der Trade-Preis den Orderpreis berührt (d. h. ihm entspricht).
- **Markttiefe (Lebensdauer)** - die maximale Zeit, während der sich das Orderbuch im Emulator befindet. Wenn es während dieser Zeit keine Aktualisierung gab, wird das Orderbuch gelöscht. Diese Eigenschaft kann verwendet werden, um alte Orderbücher zu entfernen, wenn die Daten Lücken enthalten.
- **Fehlerprozentsatz** - der Prozentwert des Fehlers bei der Registrierung neuer Orders. Der Wert kann von 0 (keine Fehler) bis 100 reichen.
- **Latenz** - der Mindestwert der registrierten Orderlatenz.
- **Erneute Registrierung** - ob das erneute Registrieren von Orders als einzelner Trade unterstützt wird.
- **Pufferungszeitraum** - Antworten in einem einzelnen Paket in Intervallen senden. Die Netzwerklatenz und der gepufferte Betrieb des Börsenkerns werden emuliert.
- **Order-ID** - die Nummer, ab der der Emulator Kennungen für Orders generiert.
- **Trade-ID** - die Nummer, ab der der Emulator Kennungen für Trades generiert.
- **Transaktion** - die Nummer, ab der der Emulator Kennungen für Ordertransaktionen generiert.
- **Spread-Größe** - die Spread-Größe in Preisschritten. Sie wird beim Angeben des Spreads für die Generierung eines Orderbuchs aus Tick-Trades verwendet.
- **Orderbuchtiefe** - die maximale Tiefe des Orderbuchs, das aus den Ticks generiert wird.
- **Anzahl der Volumenschritte** - die Anzahl der Volumenschritte, um die die Order größer ist als der Tick-Trade. Wird beim Testen auf Tick-Trades verwendet.
- **Portfoliointervall** - das Intervall für die Neuberechnung des Portfolios. Wenn das Intervall null ist, wird keine Neuberechnung durchgeführt.
- **Zeit ändern** - die Zeit für Orders und Trades in Börsenzeit ändern.
- **Zeitzone** - Informationen über die Zeitzone, in der sich die Börse befindet.
- **Preisverschiebung** - die Preisverschiebung vom letzten Trade, mit der die Grenzen der maximalen und minimalen Preise für die nächste Sitzung angegeben werden.
- **Zusätzliches Volumen hinzufügen** - zusätzliches Volumen zum Orderbuch hinzufügen, wenn Orders mit großem Volumen registriert werden.
- **[Kommissionen](../commissions.md)** - die Provision (Brokerage, Börse usw.).

**Protokollierung**

- **Protokollierungsstufe** - der Logging-Level für dieses Element.

**Einstellung**

- **[Risikomanagement](../risk_management.md)** - die Risikomanagement-Einstellungen.

**Diagrammparameter**

- **Instrument** - das Instrument.
- **Portfolio** - das Portfolio.

Wenn Sie die **Diagrammparameter** nicht ausfüllen, wird bei der Emulation das Instrument aus dem Feld **Instrument** der Registerkarte **Emulation** verwendet; als Portfolio wird standardmäßig das Testportfolio verwendet.

## Empfohlene Inhalte

[Chart](chart.md)

