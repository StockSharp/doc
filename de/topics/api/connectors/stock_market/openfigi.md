# OpenFIGI

**OpenFIGI** verbindet StockSharp mit OpenFIGI API v3 zur globalen Suche von Instrumentenkennungen.

## Wichtige Funktionen

- Im Quellcode geprüfte Funktionen: Wertpapiersuche und Filterung nach Börse, MIC, Währung, Marktsektor und Wertpapiertyp. Der Adapter liefert Instrumentenmetadaten, aber keine Kurse oder Handelsoperationen.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet zur Zuordnung externer Kennungen zu FIGI-Datensätzen und zum Aufbau eines normalisierten Instrumentenkatalogs.

Ergebnisabdeckung, Anfragelimits sowie anonymer oder schlüsselbasierter Zugriff werden von OpenFIGI bestimmt.

## Siehe auch

[Connector-Konfiguration](openfigi/configuration_openfigi.md)

[Grafische Konfiguration](openfigi/graphical_configuration_openfigi.md)

[Adapterinitialisierung](openfigi/adapter_initialization_openfigi.md)
