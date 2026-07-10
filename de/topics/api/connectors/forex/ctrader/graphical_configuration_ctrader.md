# Grafische Konfiguration von cTrader

Für alle StockSharp-Produkte erfolgt die grafische Verbindungseinrichtung im [Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md):

![API-GUI-Einstellungen cTrader](../../../../../images/api_gui_settings_ctrader.png)

- **Demo** - Verbindung zum Demo-Handel.

OAuth-Autorisierung:

cTrader stellt ausschließlich OAuth als Autorisierungsmethode bereit.

Ablauf der OAuth-Autorisierung:

1. Wenn Sie auf die Schaltfläche "Check" klicken, öffnet sich ein Fenster:

   ![OAuth Start](../../../../../images/oauth_start.png)

2. Nach einem Klick auf "Start" wird der Benutzer zur cTrader-Website weitergeleitet, um sich anzumelden. Auf der cTrader-Website müssen Sie der StockSharp-Anwendung Zugriff auf Handelsoperationen erlauben:

   ![cTrader Anmeldung](../../../../../images/api_gui_settings_ctrader_2.png)

3. Danach werden Sie zur StockSharp-Website zurückgeleitet, und das Programm meldet sich automatisch an.

## Siehe auch

[Connectoren](../../../connectors.md)

[OAuth](../../oauth.md)

[Grafische Konfiguration](../../graphical_configuration.md)

[Eigenen Connector erstellen](../../creating_own_connector.md)

[Einstellungen speichern und laden](../../save_and_load_settings.md)
