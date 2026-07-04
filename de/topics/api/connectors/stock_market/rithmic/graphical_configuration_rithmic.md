# Grafische Konfiguration Rithmic

Für alle [S#](../../../../api.md)-Produkte erfolgt die grafische Konfiguration der Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings Rithmic](../../../../../images/api_gui_settings_rithmic.png)

- **Login** - Login.
- **Password** - Password.
- **Certificate** - Pfad zur Zertifikatsdatei, erforderlich für die Verbindung zum Rithmic-System.
- **File log** - Pfad zur Logdatei.
- **Server type** - Servertyp.
- **Point (admin)** - Verbindungspunkt für administrative Funktionen (Initialisierung/Deinitialisierung).
- **Point (data)** - Verbindungspunkt für Marktdaten.
- **Login (trans)** - Zusätzliches Login. Wird verwendet, wenn Transaktionen an einen separaten Server gesendet werden.
- **Point (transactions)** - Verbindungspunkt zum System für die Transaktionsausführung.
- **Password (trans)** - Zusätzliches Passwort. Wird verwendet, wenn Transaktionen an einen separaten Server gesendet werden.
- **Point (positions)** - Verbindungspunkt für den Zugriff auf Portfolio- und Positionsinformationen.
- **Point (history)** - Verbindungspunkt für den Zugriff auf historische Daten.
- **Domain (address)** - Domainadresse.
- **Domain (name)** - Domainname.
- **Licenses** - Adresse des Lizenzservers.
- **Broker** - Brokeradresse.
- **Log (address)** - Logger-Adresse.
- **User name (hist)** - Zusätzliches Login. Benutzer-ID für die Authentifizierung beim History Plant.
- **Password (hist)** - Zusätzliches Passwort. Passwort für die Authentifizierung beim History Plant.
- **Heart beat** - Intervall zur Serverprüfung, um die Verbindung zu überwachen. Standardmäßig 1 Minute.
- **Reconnection settings** - Mechanismus zur Überwachung von Verbindungen mit den Einstellungen des Handelssystems. ([Reconnection settings](../../reconnection_settings.md))

## Empfohlene Inhalte

[Connectoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Eigenen Connector erstellen](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
