# CoinGlass

**CoinGlass** verbindet StockSharp mit dem Kryptomarktdatendienst CoinGlass. Der Adapter stellt die Anbieterdaten über das standardisierte StockSharp-Nachrichtenmodell bereit.

## Wichtige Funktionen

- Im Quellcode geprüfte Adapterfunktionen: Datenaktualisierungen in Echtzeit, Level-1-Kurse, Kerzendaten, historische Datenabfragen, Terminmärkte, Optionsmärkte.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Aufträge weiter.

## Typische Verwendung

Geeignet für Charts, Marktdatenspeicher, Analysen, Forschung und Strategietests mit Daten des Anbieters.

Instrumente, Datentiefe, Transaktionsrechte, Anfragelimits und Verfügbarkeit werden von CoinGlass, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](coinglass/configuration_coinglass.md)

[Grafische Konfiguration](coinglass/graphical_configuration_coinglass.md)

[Adapterinitialisierung](coinglass/adapter_initialization_coinglass.md)
