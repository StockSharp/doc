# TWSE

Der **TWSE-Konnektor** verbindet StockSharp mit einem professionellen Markt- und Analysedienst. Er übersetzt anbieterspezifische Daten und Vorgänge in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Handelsplätze verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Anbieter unterstützte Markt-, Unternehmens-, Einreichungs-, Offenlegungs- und Referenzdaten.
- Vom Adapter unterstützte Marktdaten: Level-1-Kurse und Kerzen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Orders weiter.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Charts, Marktdatenspeicher, Analysen, Forschung und Strategietests mit Daten des Anbieters.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von TWSE, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](twse_openapi/configuration_twse_openapi.md)

[Grafische Konfiguration](twse_openapi/graphical_configuration_twse_openapi.md)

[Adapterinitialisierung](twse_openapi/adapter_initialization_twse_openapi.md)
