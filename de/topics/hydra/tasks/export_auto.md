# Export (auto)

Die Aufgabe exportiert Boersendaten in verschiedene Formate: Excel, xml, sql, bin, Json oder txt.

![hydra tasks export](../../../images/hydra_tasks_export.png)

**Database**

- **Connection** - Verbindung zur Datenbank. Wird beim Export über SQL verwendet.
- **Packet** - Groesse des uebertragenen Datenpakets. Standardmaessig betraegt die Groesse 50 Elemente. Wird beim Export über SQL verwendet.
- **Uniqueness** - Pruefung der Dateneindeutigkeit in der Datenbank. Beeinflusst die Performance. Standardmaessig aktiviert. Wird beim Export über SQL verwendet.

> [!TIP]
> Beim Export über SQL müssen die Parameter der Verbindungszeichenfolge gesetzt werden.

**New connection string**

![hydra tasks connstring](../../../images/hydra_tasks_connstring.png)

- **Provider** - Provider-Einstellungen.
- **Server** - Serveradresse oder Pfad zur Datenbank.
- **Database** - Datenbankname. Wird für SQLite nicht verwendet.
- **Login** - Login für den Zugriff auf die Datenbank. Wird für anonymen Zugriff nicht verwendet.
- **Password** - Passwort für den Zugriff auf die Datenbank. Wird für anonymen Zugriff nicht verwendet.
- **Windows** - das aktuelle Windows-Konto für die Verbindung zur Datenbank verwenden.
- **Connection** - fertige Verbindungszeichenfolge.

> [!TIP]
> Sie können die Verbindung zur Datenbank mit der Schaltfläche **Check** pruefen.

**General**

- **Header** - Converter.
- **Working hours** - Einrichtung des Arbeitszeitplans des Boards. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - das Ausfuehrungsintervall.
- **Data directory** - Datenverzeichnis, aus dem die Daten für die Konvertierung gelesen werden.
- **Format** - Format der konvertierten Daten: BIN\/CSV.
- **Max. errors** - maximale Anzahl von Fehlern, bei deren Erreichen die Aufgabe gestoppt wird. Standardmaessig 0, die Anzahl der Fehler wird ignoriert.
- **Dependency** - eine Aufgabe, die vor dem Start der aktuellen Aufgabe ausgeführt werden muss.

**CSV**

- **Templates** - Vorlagen für jeden Typ exportierter Daten.
- **Header** - Kopfzeile in der ersten Zeile. Wenn eine leere Zeichenfolge uebergeben wird, wird dem Dateianfang keine Kopfzeile hinzugefügt.
- **Name format** - Format für das Schreiben des exportierten Dateinamens.

**Export (auto)**

- **Type** - Exporttyp (Format).
- **Start date** - ab welchem Datum der Datenexport gestartet werden soll.
- **Time offset** - Zeitoffset in Tagen.
- **Export directory** - Verzeichnis, in das Daten exportiert werden.
- **Format** - Datenformat.
- **Split** - Aufteilungstyp.

**Logging**

- **Identifier** - die Kennung.
- **Logging level** - der Logging-Level.

Betrachten wir ein Beispiel für den automatischen Export:

1. Wählen Sie ein Instrument aus.
2. Richten Sie die Marktdaten ein, die exportiert werden müssen.![hydra tasks export 00](../../../images/hydra_tasks_export_00.png)
3. Legen Sie den Exportzeitraum fest. Wenn der Download von Marktdaten in Echtzeit konfiguriert ist, können Sie das Enddatum des Zeitraums weglassen. In diesem Fall werden die Daten gemaess dem Arbeitsintervall (Datenaktualisierung) in Echtzeit exportiert. ![hydra tasks export 01](../../../images/hydra_tasks_export_01.png)
4. Richten Sie Verzeichnisse, Ausfuehrungsintervall, Datentyp und Datenformat ein.
5. Starten Sie den Export.![hydra tasks export 02](../../../images/hydra_tasks_export_02.png)

Sehen wir uns die exportierten Daten an.

![hydra tasks export 03](../../../images/hydra_tasks_export_03.png)

**Sehen Sie sich das [Video-Tutorial](../videos/export_task.md) an**
