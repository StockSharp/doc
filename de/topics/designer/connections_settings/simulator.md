# Simulator

[Designer](../../designer.md) ermöglicht das Ausführen erstellter Strategien im Modus **Simulation**. Um die **Simulation** anzupassen, führen Sie die folgenden Schritte aus:

1. Wenn Sie auf den Pfeil neben der Schaltfläche **Verbinden** ![Designer Symbolleiste für den Schnellzugriff 00](../../../images/designer_quick_access_toolbar_00.png) klicken, erscheint die Schaltfläche **Emulatoreinstellungen**:

![Designer Verbindungseinstellungen 00](../../../images/designer_connection_settings_00.png)

2. Wenn Sie auf die Schaltfläche **Emulatoreinstellungen** klicken, öffnet sich das Fenster **Emulatoreinstellungen**:

![Designer Emulationseigenschaften 00](../../../images/designer_properties_emulation_00.png)

1. **Simulator**

- **Emulator verwenden** - Emulator verwenden.
- **Instrumente** - Instrumente.

2. **Einstellungen**

- **Bei Berührung zusammenführen** - Während der Emulation Trades zusammenführen, wenn der Trade-Preis den Orderpreis berührt, also dem Orderpreis entspricht.
- **Markttiefe (Lebensdauer)** - Maximale Zeit, während der sich das Orderbuch im Emulator befindet. Wenn in dieser Zeit kein Update erfolgt, wird das Orderbuch gelöscht. Diese Eigenschaft kann verwendet werden, um alte Orderbücher zu entfernen, wenn Datenlücken vorhanden sind.
- **Fehlerprozentsatz** - Prozentwert des Fehlers bei der Registrierung neuer Orders. Der Wert kann von 0 (keine Fehler) bis 100 reichen.
- **Latenz** - Mindestwert der Verzögerung für registrierte Orders.
- **Erneute Registrierung** - Wird die erneute Registrierung von Orders in Form eines einzelnen Trades unterstützt?
- **Pufferungszeitraum** - Antworten in Batches in einem einzelnen Paket senden. Die Netzwerkverzögerung und die gepufferte Arbeit des Börsenkerns werden emuliert.
- **Order-ID** - Die Nummer, ab der der Emulator Identifikatoren für Orders generiert.
- **Trade-ID** - Die Nummer, ab der der Emulator Identifikatoren für Trades generiert.
- **Transaktion** - Die Nummer, ab der der Emulator Identifikatoren für Ordertransaktionen generiert.
- **Spread-Größe** - Spread-Größe in Preisschritten. Wird bei der Bestimmung des Spreads für die Orderbuchgenerierung aus Tick-Trades verwendet.
- **Orderbuchtiefe** - Die maximale Tiefe des Orderbuchs, das aus Ticks generiert wird.
- **Anzahl der Volumenschritte** - Die Anzahl der Volumenschritte, um die die Order größer als der Tick-Trade ist. Wird beim Testing von Tick-Trades verwendet.
- **Portfoliointervall** - Intervall zur Portfolioneuberechnung. Wenn das Intervall null ist, wird keine Neuberechnung durchgeführt.
- **Zeit ändern** - Zeitänderung für Orders und Trades mit Börsenzeit.
- **Zeitzone** - Informationen zur Zeitzone der Börse.
- **Preisverschiebung** - Preisverschiebung vom letzten Trade, die Grenzen der maximalen und minimalen Preise für die nächste Sitzung bestimmt.
- **Zusätzliches Volumen hinzufügen** - Zusätzliches Volumen zur Order im Orderbuch hinzufügen, wenn Orders mit großem Volumen registriert werden.

## Empfohlene Inhalte

[Chart](../user_interface/components/chart.md)
