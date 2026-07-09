# Grafische Konfiguration Interactive Brokers

Für alle [S#](../../../../api.md)-Produkte wird die grafische Konfiguration der Verbindung im [Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) vorgenommen:

![API GUI Settings Interactive Brokers](../../../../../images/api_gui_settings_interactivebrokers.png)

- **Adresse** - TWS-Adresse.
- **Identifier** - Eindeutige ID. Wird verwendet, wenn mehrere Clients mit einem Terminal oder Gateway verbunden sind.
- **Echtzeit** - Legt fest, ob Echtzeitdaten oder auf dem Brokerserver eingefrorene Daten verwendet werden.
- **Protokollierungsstufe** - Protokollierungsstufe für Servermeldungen.
- **Market data fields** - Marktdatenfelder, die mit abonnierten Level1-Nachrichten empfangen werden.
- **Protocol** - SSL-Protokoll zum Herstellen der Verbindung.
- **Zertifikat** - SSL-Zertifikat.
- **Passwort** - Passwort des SSL-Zertifikats.
- **Check revocation** - Zertifikatsperrung prüfen.
- **Validate remote** - Remote-Zertifikate validieren.
- **Host name** - Name des Servers, der die SSL-Verbindung bereitstellt.
- **MaxVersion** - MaxVersion
- **Verbindungsprüfung** - Intervall zur Serverpruefung, um zu verfolgen, ob die Verbindung aktiv ist. Standardmaessig 1 Minute.
- **Einstellungen für die Wiederverbindung** - Mechanismus für Einstellungen zur Überwachung von Verbindungen mit dem Handelssystem. ([Einstellungen für die Wiederverbindung](../../reconnection_settings.md))

## Empfohlene Inhalte

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Eigenen Connector erstellen](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
