# Open DART

Der **Open DART-Konnektor** verbindet StockSharp mit einem Dienst für Finanzdaten und Referenzinformationen. Er übersetzt anbieterspezifische Daten in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Datenquellen verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien und Emittentenreferenzdaten.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Anbieter unterstützte Markt-, Unternehmens-, Einreichungs-, Offenlegungs- und Referenzdaten.
- Vom Adapter unterstützte Marktdaten: Level-1-Kurse, Finanznachrichten und Finanzveröffentlichungen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Orders weiter.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Wertpapierstammdaten, Offenlegungsüberwachung, Emittentenrecherche, Compliance-Abläufe und historische Analysen.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von Open DART, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](open_dart/configuration_open_dart.md)

[Grafische Konfiguration](open_dart/graphical_configuration_open_dart.md)

[Adapterinitialisierung](open_dart/adapter_initialization_open_dart.md)
