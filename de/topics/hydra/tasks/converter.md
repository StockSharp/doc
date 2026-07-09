# Konverter

Die Aufgabe konvertiert Börsendaten. Zum Beispiel von Order Logs in Ticks oder von Ticks in Kerzen usw.

![hydra tasks converter](../../../images/hydra_tasks_converter.png)

**Konverter**

- **Konverter** - Converter.
- **From** - welcher Datentyp konvertiert wird.
- **Data format** - Format der konvertierten Daten.
- **Startdatum** - ab welchem Datum die Datenkonvertierung gestartet werden soll.
- **Zeitversatz** - Zeitoffset in Tagen ab dem Datum, an dem die Aufgabe gestartet wurde. Dies verhindert die Konvertierung eines unvollständigen Tages. Wenn die Datenkonvertierung in Echtzeit konfiguriert ist, kann das Aktualisierungsintervall den aktuellen Tag nur teilweise konvertieren. Verwenden Sie den Zeitoffset, um dies zu vermeiden.
- **Where** - das Datenverzeichnis, in dem die konvertierten Daten gespeichert werden.

**Orderbücher**

- **Intervall** - Intervall für die Order-Book-Erzeugung.
- **Depth** - maximale Tiefe der Order-Book-Erzeugung.
- **Order log** - wie Order Books aus dem Order Log erstellt werden.

  Jede Börse hat ihr eigenes **Order Log**-Format. Das Programm [Hydra](../../hydra.md) unterstützt drei Formate:
  - **By default** - wird in den meisten Fällen verwendet.
  - **ITCH** - wird für das ITCH-Protokoll verwendet (Börsen: LSE und Nasdaq).

**Allgemein**

- **Kopfzeile** - Converter.
- **Arbeitszeiten** - Einrichtung des Arbeitszeitplans des Boards. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Betriebsintervall** - das Ausführungsintervall.
- **Datenverzeichnis** - Datenverzeichnis, aus dem die Daten für die Konvertierung gelesen werden.
- **Format** - Format der konvertierten Daten: BIN\/CSV.
- **Max. Fehler** - maximale Anzahl von Fehlern, bei deren Erreichen die Aufgabe gestoppt wird. Standardmäßig 0, die Anzahl der Fehler wird ignoriert.
- **Abhängigkeit** - eine Aufgabe, die vor dem Start der aktuellen Aufgabe ausgeführt werden muss.

**Protokollierung**

- **Kennung** - die Kennung.
- **Protokollierungsstufe** - der Logging-Level.

Betrachten wir ein Beispiel für eine Datenkonvertierung.

1. Wechseln Sie zur Aufgabe **Konverter**. ![hydra tasks converter 00](../../../images/hydra_tasks_converter_00.png)
2. Wählen Sie das Instrument aus und legen Sie im erscheinenden Fenster den Datentyp fest, der bei der Konvertierung entstehen soll, sowie den Datentyp, aus dem konvertiert werden soll. Zum Beispiel müssen Ticks in Kerzen mit einem Time Frame von 15 Minuten konvertiert werden.

   > [!TIP]
> WICHTIG\! Der angeforderte Datenzeitraum muss dem für die Konvertierung verfügbaren Zeitraum entsprechen, andernfalls werden die Daten nicht konvertiert. Geben Sie in den Einstellungen das korrekte Quelldatenformat an, damit es dem Format der zu konvertierenden Daten entspricht.
3. Geben Sie die erforderlichen Verzeichnisse, den Zeitoffset und das Ausführungsintervall an.
4. Starten Sie die Konvertierung.![hydra tasks converter 01](../../../images/hydra_tasks_converter_01.png)

Es ist zu sehen, dass die Daten konvertiert wurden. [Sehen wir uns](../working_with_data/view_and_export.md) die resultierenden Daten an.

![hydra tasks converter 02](../../../images/hydra_tasks_converter_02.png)

Diese Funktion ist vergleichbar mit dem [Abrufen der erforderlichen Marktdaten](../working_with_data/any_market_data_types.md) aus einem anderen Datentyp.

**Sehen Sie sich das [Video-Tutorial](../videos/converter_task.md) an**
