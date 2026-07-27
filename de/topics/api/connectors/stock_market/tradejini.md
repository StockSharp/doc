# Tradejini

Der **Tradejini-Konnektor** verbindet StockSharp mit einem Broker oder elektronischen Handelsplatz für Finanzmärkte. Er übersetzt anbieterspezifische Daten und Vorgänge in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Handelsplätze verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien, Futures, Optionen, Forex und Rohstoffe.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Adapter unterstützte Marktdaten: Kerzen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Vom Anbieter unterstützte Abläufe für Orderübermittlung und Ausführungen.
- Aktualisierungen von Portfolios, Salden, Positionen und Ausführungsstatus.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Live-Strategien, Handelsterminals, Ordermanagement-Dienste und Überwachungswerkzeuge mit direktem Anbieterzugang.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von Tradejini, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](tradejini/configuration_tradejini.md)

[Grafische Konfiguration](tradejini/graphical_configuration_tradejini.md)

[Adapterinitialisierung](tradejini/adapter_initialization_tradejini.md)
