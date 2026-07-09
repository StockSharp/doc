# Hydra Client verbinden

Im Servermodus kann ein weiteres Hydra-Programm verbunden werden, das als Client arbeitet und Daten zu sich selbst herunterlaedt. Im Unterschied zu [Verbindung über FIX protocol](fix_fast_connectivity.md) werden Daten in Form von Dateien im StockSharp-Format uebertragen. Dadurch eignet sich die Quelle für die Uebertragung größer Mengen historischer Daten.

Für die Verbindung wird eine spezielle Quelle verwendet:

![hydra tasks server](../../../images/hydratasksserver_1.png)

**Einstellungen**

![hydra tasks server](../../../images/hydratasksserver_2.png)

- **Address** - die Adresse des Hydra-Servers.
- **Login** - Login (erforderlich, wenn der Server Autorisierung verlangt).
- **Password** - Passwort (erforderlich, wenn der Server Autorisierung verlangt).
- **Time Offset** - ein Zeitoffset in Tagen ab dem aktuellen Datum, erforderlich, um das Herunterladen unvollstaendiger Daten für die aktuelle Handelssitzung zu verhindern.
- **Weekends** - ob Daten für Wochenenden heruntergeladen werden sollen.

**Main**

- **Title** - der Titel der Aufgabe.
- **Working Hours** - Einstellung des Plattformbetriebs.
- **Interval of Operation** - Ausfuehrungsintervall.
- **Data Directory** - das Datenverzeichnis, in dem die finalen Dateien im [S#](../../api.md)-Format gespeichert werden.
- **Format** - Datenformat: BIN/CSV.
- **Max. Errors** - die maximale Anzahl von Fehlern, bei deren Erreichen die Aufgabe gestoppt wird. Standardmaessig 0, die Anzahl der Fehler wird ignoriert.
- **Dependency** - eine Aufgabe, die vor dem Start der aktuellen Aufgabe abgeschlossen sein muss.

**Logging**

- **Identifier** - Kennung.
- **Logging Level** - Logging-Level.
