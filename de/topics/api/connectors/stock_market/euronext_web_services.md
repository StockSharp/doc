# Euronext Web Services

Der **Euronext Web Services-Konnektor** verbindet StockSharp mit einem professionellen Markt- und Analysedienst. Er übersetzt anbieterspezifische Daten und Vorgänge in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Handelsplätze verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Anbieter unterstützte Markt-, Unternehmens-, Einreichungs-, Offenlegungs- und Referenzdaten.
- Vom Adapter unterstützte Marktdaten: Level-1-Kurse, Tick-Trades, Orderbücher und Kerzen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Orders weiter.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Charts, Marktdatenspeicher, Analysen, Forschung und Strategietests mit Daten des Anbieters.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von Euronext Web Services, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](euronext_web_services/configuration_euronext_web_services.md)

[Grafische Konfiguration](euronext_web_services/graphical_configuration_euronext_web_services.md)

[Adapterinitialisierung](euronext_web_services/adapter_initialization_euronext_web_services.md)
