# Grafische Konfiguration FIX

Für alle [S#](../../../../api.md)-Produkte erfolgt die grafische Konfiguration der Verbindung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings FIX](../../../../../images/api_gui_settings_fix.png)

- **Address** - Adresse.
- **Dialect** - Dialekt des FIX-Protokolls.
- **Sender** - Absenderkennung.
- **Target** - Zielkennung.
- **Login** - Login.
- **Password** - Passwort.
- **Portfolios** - Alle Portfolios beim Start anfordern.
- **Instruments** - Alle Instrumente bei der Verbindung anfordern.
- **Encoding** - Für die Datenübertragung verwendete Kodierung.
- **Sequence reset** - Ob der Bezeichnerzähler zurückgesetzt werden soll.
- **Date format** - Datumsformat.
- **Date and time format** - Datums- und Zeitformat.
- **Time format** - Zeitformat.
- **Receive timeout** - Timeout für den Datenempfang.
- **Send timeout** - Timeout für das Senden von Daten.
- **Unknown transactions** - Verarbeitung unbekannter, von Dritten erzeugter Ausführungen.
- **Protocol** - SSL-Protokoll für den Verbindungsaufbau.
- **Certificate** - SSL-Zertifikat.
- **Password** - Passwort für das SSL-Zertifikat.
- **Revocation check** - Prüfung des Zertifikatswiderrufs.
- **Check remote** - Prüfung der Remote-Zertifikate.
- **Server name** - Servername, der die SSL-Verbindung verwendet.
- **Reconnection settings** - Einstellungen des Mechanismus zur Überwachung der Verbindung mit dem Handelssystem ([Wiederverbindungseinstellungen](../../reconnection_settings.md)).
- **Heartbeat interval** - Intervall zur Benachrichtigung des Servers, dass die Verbindung noch aktiv ist. Der Standardwert beträgt 1 Minute.
- **Unified board code** - Board-Code für das vereinheitlichte Instrument.

## Siehe auch

[Konnektoren](../../../connectors.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Erstellung eines eigenen Connectors](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
