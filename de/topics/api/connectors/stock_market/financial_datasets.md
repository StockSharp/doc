# Financial Datasets

Der **Financial Datasets-Konnektor** verbindet StockSharp mit einem professionellen Markt- und Analysedienst. Er übersetzt anbieterspezifische Daten und Vorgänge in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Handelsplätze verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Anbieter unterstützte Markt-, Unternehmens-, Einreichungs-, Offenlegungs- und Referenzdaten.
- Vom Adapter unterstützte Marktdaten: Level-1-Kurse, Kerzen, Finanznachrichten und Finanzveröffentlichungen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Echtzeitabonnements über den Datenstrom des Anbieters.
- Dieser Adapter ist für den Datenzugriff vorgesehen und leitet keine Orders weiter.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Charts, Marktdatenspeicher, Analysen, Forschung und Strategietests mit Daten des Anbieters.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von Financial Datasets, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](financial_datasets/configuration_financial_datasets.md)

[Grafische Konfiguration](financial_datasets/graphical_configuration_financial_datasets.md)

[Adapterinitialisierung](financial_datasets/adapter_initialization_financial_datasets.md)
