# SEC API

Der **SEC API-Konnektor** verbindet StockSharp mit einem Dienst für Finanzdaten und Referenzinformationen. Er übersetzt anbieterspezifische Daten in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Datenquellen verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien und Emittentenreferenzdaten.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Anbieter unterstützte Markt-, Unternehmens-, Einreichungs-, Offenlegungs- und Referenzdaten.
- Vom Adapter unterstützte Marktdaten: Finanznachrichten und Finanzveröffentlichungen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Echtzeitabonnements über den Datenstrom des Anbieters.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Orders weiter.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Wertpapierstammdaten, Offenlegungsüberwachung, Emittentenrecherche, Compliance-Abläufe und historische Analysen.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von SEC API, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](sec_api/configuration_sec_api.md)

[Grafische Konfiguration](sec_api/graphical_configuration_sec_api.md)

[Adapterinitialisierung](sec_api/adapter_initialization_sec_api.md)
