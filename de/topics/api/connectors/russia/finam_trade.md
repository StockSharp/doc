# Finam Trade API

Der **Finam-Trade-API-Konnektor** verbindet StockSharp-Anwendungen mit den von Finam bereitgestellten Brokerkonten und Marktdaten. Instrumente, Kurse, Orders, Ausführungen und Portfoliostände werden in das einheitliche Nachrichtenmodell von StockSharp übertragen.

## Wichtigste Funktionen

- Instrumentensuche für verfügbare Aktien, Anleihen, Währungen, Fonds, Futures und Optionen.
- Level-1-Kurse, Orderbücher, öffentliche Trades und Zeitintervall-Kerzen.
- Historische Kerzenabfragen und Echtzeitabonnements für Marktdaten.
- Markt-, Limit-, Stop- und Stop-Limit-Orders sowie Orderstornierung.
- Aktualisierungen von Orderstatus, eigenen Trades, Barbeständen und Positionen.
- Automatischer Austausch des API-Geheimnisses gegen ein kurzlebiges Sitzungstoken.
- Konfigurierbare REST- und WebSocket-Adressen für kompatible Gateways und Testumgebungen.

## Typische Verwendung

Der Konnektor eignet sich für Handelsroboter, Terminals, Portfolioüberwachung und Ordermanagement-Dienste, die Finam über eine einheitliche StockSharp-Schnittstelle nutzen.

Ein Finam-Trade-API-Geheimnis ist erforderlich. Ein Konto kann ausdrücklich gewählt werden; bei leerer Kontokennung verwendet der Konnektor das erste für das Token verfügbare Konto. Instrumente werden im Finam-Format `ticker@MIC` angegeben. Verfügbare Märkte, Historientiefe, Echtzeitdaten, Handelsrechte und Anfragelimits hängen vom Konto und den Finam-Bedingungen ab.

## Siehe auch

[Connector-Konfiguration](finam_trade/configuration_finam_trade.md)

[Grafische Konfiguration](finam_trade/graphical_configuration_finam_trade.md)

[Adapterinitialisierung](finam_trade/adapter_initialization_finam_trade.md)
