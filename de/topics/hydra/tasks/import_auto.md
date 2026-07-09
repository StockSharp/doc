# Automatischer Import

Die Aufgabe führt den automatischen Import von Börsendaten aus Dateien im angegebenen Verzeichnis gemäß der angegebenen Dateimaske aus.

Für jeden ausgewählten Marktdatentyp wird die Vorlage auf der Registerkarte [Import](../importing.md) konfiguriert.

![hydra tasks import](../../../images/hydra_tasks_import.png)

Am unteren Rand des Panels können Sie die Instrumente auswählen, für die Daten importiert werden, sowie den zu importierenden Datentyp.

Für jedes Instrument können Sie die folgenden Datenimporteigenschaften angeben:

![hydra tasks proper import](../../../images/hydra_tasks_proper_import.png)

**Import (automatisch)**

**Einstellungen**

- **Datentyp** - Typ der importierten Daten.
- **Dateiname** - vollstaendiger Pfad zur Datei.
- **Datenverzeichnis** - Datenverzeichnis.
- **Dateimaske** - Dateimaske, die beim Scannen des Verzeichnisses verwendet wird. Zum Beispiel candles\*.csv.
- **Unterverzeichnisse** - Unterverzeichnisse einschliessen.
- **Spaltentrennzeichen** - Spaltentrennzeichen. Tabulator wird als TAB bezeichnet.
- **Einzug vom Anfang** - Anzahl der Zeilen, die am Anfang der Datei übersprungen werden sollen (wenn sie Metainformationen enthalten).
- **Zeitzone** - Zeitzone.
- **Intervall** - Häufigkeit der Datenaktualisierung.
- **Erweiterte Informationen** - erweiterte importierte Felder im Speicher für erweiterte Informationen speichern.
- **Duplikate** - ob doppelte Instrumente aktualisiert werden sollen, wenn sie bereits existieren.
- **Ohne ID ignorieren** - Instrumente ohne Kennung ignorieren.

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
