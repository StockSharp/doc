# JPX TDnet

Der **JPX TDnet-Konnektor** verbindet StockSharp mit einem Dienst für Finanzdaten und Referenzinformationen. Er übersetzt anbieterspezifische Daten in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Datenquellen verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien und Emittentenreferenzdaten.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Anbieter unterstützte Markt-, Unternehmens-, Einreichungs-, Offenlegungs- und Referenzdaten.
- Vom Adapter unterstützte Marktdaten: Finanznachrichten und Finanzveröffentlichungen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Orders weiter.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Wertpapierstammdaten, Offenlegungsüberwachung, Emittentenrecherche, Compliance-Abläufe und historische Analysen.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von JPX TDnet, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](jpx_tdnet/configuration_jpx_tdnet.md)

[Grafische Konfiguration](jpx_tdnet/graphical_configuration_jpx_tdnet.md)

[Adapterinitialisierung](jpx_tdnet/adapter_initialization_jpx_tdnet.md)
