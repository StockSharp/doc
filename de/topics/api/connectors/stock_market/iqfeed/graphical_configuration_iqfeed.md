# Grafische Konfiguration IQFeed

Fuer alle [S#](../../../../api.md)-Produkte wird die grafische Konfiguration der Verbindung im [Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) vorgenommen:

![API GUI Settings IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Level1 server** - Adresse zum Abrufen von Daten auf Level1.
- **Level2 server** - Adresse zum Abrufen von Daten auf Level2.
- **Lookup server** - Adresse zum Abrufen historischer Daten.
- **Admin server** - Adresse zum Abrufen von Servicedaten.
- **Derivatives** - Adresse zum Abrufen von Derivatedaten.
- **Data for Level1** - Alle Datentypen fuer Level1, die uebertragen werden muessen.
- **Data type** - Wertpapiertypen, fuer die Daten empfangen werden muessen.
- **Load securities** - Legt fest, ob der gesamte Satz von Wertpapieren aus dem IQFeed-Websitearchiv geladen werden soll.
- **File with securities** - Pfad zur Datei mit der von der Website heruntergeladenen IQFeed-Wertpapierliste. Wenn ein Pfad angegeben ist, erfolgt kein zweiter Download von der Website, und nur die lokale Kopie wird geparst.
- **Version** - Version.
- **Heart beat** - Intervall zur Serverpruefung, um zu verfolgen, ob die Verbindung aktiv ist. Standardmaessig 1 Minute.
- **Reconnection settings** - Mechanismus fuer Einstellungen zur Ueberwachung von Verbindungen mit dem Handelssystem. ([Einstellungen für die Wiederverbindung](../../reconnection_settings.md))

## Empfohlene Inhalte

[Connectors](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Eigenen Connector erstellen](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
