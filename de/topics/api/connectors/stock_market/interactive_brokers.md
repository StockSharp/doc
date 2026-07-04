# Interactive Brokers

**Interactive Brokers** - Handelsplattform fuer den Handel mit Finanzanlagen, darunter Aktien, Optionen, Futures, EFPs, Futures-Optionen, Forex, Anleihen und Fonds.

Bevor Sie Handelsroboter fuer diese Handelsplattform schreiben, lesen Sie die Links unter [Connectors](../../connectors.md).

## TWS-Konfiguration Interactive Brokers

1. Sie muessen Verbindungen von anderen Programmen erlauben (z. B. vom Handelsalgorithmus auf [S#](../../../api.md)). Oeffnen Sie dazu das Einstellungsmenue "File -> Global configuration...". Waehlen Sie im neuen Fenster "Configuration -> API -> Settings" aus:

   ![ib settings](../../../../images/ib_settings.png)
2. Aktivieren Sie den Modus "Enable ActiveX and Socket Clients".
3. Fuegen Sie ausserdem die Adresse des Computers hinzu, auf dem der Algorithmus ausgefuehrt wird (lokale Adresse: 127.0.0.1). Dadurch muss die Berechtigung fuer die Terminalverbindung nicht bei jedem Start des Algorithmus erneut bestaetigt werden.

## Siehe auch

[Connectors](../../connectors.md)

[Grafische Konfiguration](../graphical_configuration.md)

[Einstellungen speichern und laden](../save_and_load_settings.md)

[Eigenen Connector erstellen](../creating_own_connector.md)

[Auftragsverwaltung](../../orders_management.md)

[Neuen Auftrag erstellen](../../orders_management/create_new_order.md)

[Neue Stop-Order erstellen](../../orders_management/create_new_stop_order.md)

[Adapterinitialisierung Interactive Brokers](interactive_brokers/adapter_initialization_interactive_brokers.md)
