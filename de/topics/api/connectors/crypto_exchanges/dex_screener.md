# DEX Screener

**DEX Screener** verbindet StockSharp mit dem Kryptomarktdatendienst DEX Screener. Der Adapter stellt die Anbieterdaten über das standardisierte StockSharp-Nachrichtenmodell bereit.

## Wichtige Funktionen

- Im Quellcode geprüfte Adapterfunktionen: Datenaktualisierungen in Echtzeit, Level-1-Kurse.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Aufträge weiter.

## Typische Verwendung

Geeignet für Charts, Marktdatenspeicher, Analysen, Forschung und Strategietests mit Daten des Anbieters.

Instrumente, Datentiefe, Transaktionsrechte, Anfragelimits und Verfügbarkeit werden von DEX Screener, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](dex_screener/configuration_dex_screener.md)

[Grafische Konfiguration](dex_screener/graphical_configuration_dex_screener.md)

[Adapterinitialisierung](dex_screener/adapter_initialization_dex_screener.md)
