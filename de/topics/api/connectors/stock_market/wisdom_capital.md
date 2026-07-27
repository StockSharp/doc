# Wisdom Capital

Der **Wisdom Capital-Konnektor** verbindet StockSharp mit einem Broker oder elektronischen Handelsplatz für Finanzmärkte. Er übersetzt anbieterspezifische Daten und Vorgänge in das einheitliche StockSharp-Nachrichtenmodell, sodass Anwendungen dieselben Abonnements und Abläufe für verschiedene Handelsplätze verwenden können.

## Wichtige Funktionen

- Typische Abdeckung: Aktien, Futures, Optionen, Forex und Rohstoffe.
- Instrumentensuche und Referenzdaten des Anbieters.
- Vom Adapter unterstützte Marktdaten: Level-1-Kurse, Tick-Trades, Orderbücher und Kerzen.
- Historische Datenabfragen für Diagramme, Analysen und Strategietests.
- Vom Anbieter unterstützte Abläufe für Orderübermittlung und Ausführungen.
- Aktualisierungen von Portfolios, Salden, Positionen und Ausführungsstatus.
- Echtzeitabonnements über den Datenstrom des Anbieters.
- Anbieterspezifische Transporte, Sitzungen und Datenformate werden hinter der standardisierten StockSharp-API verborgen.

## Typische Verwendung

Geeignet für Live-Strategien, Handelsterminals, Ordermanagement-Dienste und Überwachungswerkzeuge mit direktem Anbieterzugang.

Instrumente, Datentiefe, Handelsrechte, Limits und Verfügbarkeit werden von Wisdom Capital, dem API-Tarif und den Berechtigungen des verbundenen Kontos bestimmt.

## Siehe auch

[Connector-Konfiguration](wisdom_capital/configuration_wisdom_capital.md)

[Grafische Konfiguration](wisdom_capital/graphical_configuration_wisdom_capital.md)

[Adapterinitialisierung](wisdom_capital/adapter_initialization_wisdom_capital.md)
