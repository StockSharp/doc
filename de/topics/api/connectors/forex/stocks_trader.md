# StocksTrader

**StocksTrader** verbindet StockSharp mit der offiziellen StocksTrader-REST-API.

Der Connector unterstützt das Ermitteln von Demo- und Echtkonten, das Abfragen des Kontozustands, die Instrumentsuche sowie die aktuellen Momentaufnahmen von Geld-, Brief- und letztem Preis. Der Handel umfasst Markt-, Limit- und Stop-Aufträge, das Ändern und Stornieren ausstehender Aufträge, das Ändern von Stop-Loss und Take-Profit für offene Positionen, das Schließen von Positionen sowie die Auftrags- und Geschäftshistorie.

StocksTrader bietet keine Marktdaten-API in Echtzeit; eine Level-1-Anfrage liefert daher die zuletzt verfügbare Momentaufnahme und wird sofort abgeschlossen, während Aufträge, Geschäfte und der Kontozustand abgefragt werden.

Erstellen Sie vor dem Verbinden ein Bearer-Token im StocksTrader-Webterminal.

## Siehe auch

[Connector-Konfiguration](stocks_trader/configuration_stocks_trader.md)

[Grafische Konfiguration](stocks_trader/graphical_configuration_stocks_trader.md)

[Adapter initialisieren](stocks_trader/adapter_initialization_stocks_trader.md)

[Offizielle StocksTrader-API-Dokumentation](https://api-doc.stockstrader.com/)
