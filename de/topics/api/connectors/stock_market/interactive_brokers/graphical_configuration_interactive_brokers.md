# Grafische Konfiguration Interactive Brokers

Fuer alle [S#](../../../../api.md)-Produkte wird die grafische Konfiguration der Verbindung im [Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md) vorgenommen:

![API GUI Settings Interactive Brokers](../../../../../images/api_gui_settings_interactivebrokers.png)

- **Address** - TWS-Adresse.
- **Identifier** - Eindeutige ID. Wird verwendet, wenn mehrere Clients mit einem Terminal oder Gateway verbunden sind.
- **Real-time** - Legt fest, ob Echtzeitdaten oder auf dem Brokerserver eingefrorene Daten verwendet werden.
- **Logging level** - Protokollierungsstufe fuer Servermeldungen.
- **Market data fields** - Marktdatenfelder, die mit abonnierten Level1-Nachrichten empfangen werden.
- **Protocol** - SSL-Protokoll zum Herstellen der Verbindung.
- **Certificate** - SSL-Zertifikat.
- **Password** - Passwort des SSL-Zertifikats.
- **Check revocation** - Zertifikatsperrung pruefen.
- **Validate remote** - Remote-Zertifikate validieren.
- **Host name** - Name des Servers, der die SSL-Verbindung bereitstellt.
- **MaxVersion** - MaxVersion
- **Heart beat** - Intervall zur Serverpruefung, um zu verfolgen, ob die Verbindung aktiv ist. Standardmaessig 1 Minute.
- **Reconnection settings** - Mechanismus fuer Einstellungen zur Ueberwachung von Verbindungen mit dem Handelssystem. ([Einstellungen für die Wiederverbindung](../../reconnection_settings.md))

## Empfohlene Inhalte

[Connectors](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Eigenen Connector erstellen](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
