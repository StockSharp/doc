# Import (auto)

Die Aufgabe fuehrt den automatischen Import von Boersendaten aus Dateien im angegebenen Verzeichnis gemaess der angegebenen Dateimaske aus.

Fuer jeden ausgewaehlten Marktdatentyp wird die Vorlage auf der Registerkarte [Importing](../importing.md) konfiguriert.

![hydra tasks import](../../../images/hydra_tasks_import.png)

Am unteren Rand des Panels koennen Sie die Instrumente auswaehlen, fuer die Daten importiert werden, sowie den zu importierenden Datentyp.

Fuer jedes Instrument koennen Sie die folgenden Datenimporteigenschaften angeben:

![hydra tasks proper import](../../../images/hydra_tasks_proper_import.png)

**Import (auto)**

**Settings**

- **Data type** - Typ der importierten Daten.
- **Filename** - vollstaendiger Pfad zur Datei.
- **Data directory** - Datenverzeichnis.
- **File mask** - Dateimaske, die beim Scannen des Verzeichnisses verwendet wird. Zum Beispiel candles\*.csv.
- **Subdirectories** - Unterverzeichnisse einschliessen.
- **Column separator** - Spaltentrennzeichen. Tabulator wird als TAB bezeichnet.
- **Indent from the beginning** - Anzahl der Zeilen, die am Anfang der Datei uebersprungen werden sollen (wenn sie Metainformationen enthalten).
- **Time zone** - Zeitzone.
- **Interval** - Haeufigkeit der Datenaktualisierung.
- **Extended information** - erweiterte importierte Felder im Speicher fuer erweiterte Informationen speichern.
- **Duplicates** - ob doppelte Instrumente aktualisiert werden sollen, wenn sie bereits existieren.
- **Ignore without ID** - Instrumente ohne Kennung ignorieren.

**General**

- **Header** - Converter.
- **Working hours** - Einrichtung des Arbeitszeitplans des Boards. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - das Ausfuehrungsintervall.
- **Data directory** - Datenverzeichnis, aus dem die Daten fuer die Konvertierung gelesen werden.
- **Format** - Format der konvertierten Daten: BIN\/CSV.
- **Max. errors** - maximale Anzahl von Fehlern, bei deren Erreichen die Aufgabe gestoppt wird. Standardmaessig 0, die Anzahl der Fehler wird ignoriert.
- **Dependency** - eine Aufgabe, die vor dem Start der aktuellen Aufgabe ausgefuehrt werden muss.

**Logging**

- **Identifier** - die Kennung.
- **Logging level** - der Logging-Level.
