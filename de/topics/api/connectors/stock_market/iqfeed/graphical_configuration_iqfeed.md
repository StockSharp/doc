# Grafische Konfiguration IQFeed

Für alle [S#](../../../../api.md)-Produkte wird die grafische Konfiguration der Verbindung im [Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) vorgenommen:

![API GUI Settings IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Level1-Server** - Adresse zum Abrufen von Daten auf Level1.
- **Level2-Server** - Adresse zum Abrufen von Daten auf Level2.
- **Lookup-Server** - Adresse zum Abrufen historischer Daten.
- **Admin-Server** - Adresse zum Abrufen von Servicedaten.
- **Derivate** - Adresse zum Abrufen von Derivatedaten.
- **Daten für Level1** - Alle Datentypen für Level1, die übertragen werden müssen.
- **Datentyp** - Wertpapiertypen, für die Daten empfangen werden müssen.
- **Wertpapiere laden** - Legt fest, ob der gesamte Satz von Wertpapieren aus dem IQFeed-Websitearchiv geladen werden soll.
- **Datei mit Wertpapieren** - Pfad zur Datei mit der von der Website heruntergeladenen IQFeed-Wertpapierliste. Wenn ein Pfad angegeben ist, erfolgt kein zweiter Download von der Website, und nur die lokale Kopie wird geparst.
- **Version** - Version.
- **Verbindungsprüfung** - Intervall zur Serverprüfung, um zu verfolgen, ob die Verbindung aktiv ist. Standardmäßig 1 Minute.
- **Einstellungen für die Wiederverbindung** - Mechanismus für Einstellungen zur Überwachung von Verbindungen mit dem Handelssystem. ([Einstellungen für die Wiederverbindung](../../reconnection_settings.md))

## Empfohlene Inhalte

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Eigenen Connector erstellen](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
