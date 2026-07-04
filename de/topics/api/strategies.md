# Strategien in StockSharp

## Einführung

StockSharp stellt eine leistungsfähige Infrastruktur zum Erstellen, Testen und Ausführen von Handelsstrategien bereit. Grundlage für die Entwicklung algorithmischer Handelsstrategien ist die Basisklasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy), die eine Reihe von Standardfunktionen und Abstraktionen für die Arbeit mit Marktdaten, die Ausführung von Handelsoperationen und die Analyse von Ergebnissen bereitstellt.

## Navigation

### Grundlagen von Strategien

- [Marktdatenabonnements in Strategien](strategies/subscriptions.md) - Ausführliche Anleitung zur Verwendung von Marktdatenabonnements in Strategien. Erläutert das Erstellen und Konfigurieren von Abonnements, die Verwaltung ihres Lebenszyklus und die Überwachung ihres Zustands.

- [Indikatoren in Strategien](strategies/indicators.md) - Informationen zur Arbeit mit Indikatoren der technischen Analyse in Strategien. Behandelt das Hinzufügen von Indikatoren zu einer Strategie, die Steuerung ihrer Ausbildung und ihre Verwendung in der Handelslogik.

- [Handelsoperationen in Strategien](strategies/trading_operations.md) - Anleitung zur Ausführung von Handelsoperationen in Strategien. Beschreibt Methoden zum Erstellen und Senden von Orders, Schließen von Positionen und Überwachen ihres Status.

- [Positionsschutz](strategies/take_profit_and_stop_loss.md) - Beschreibung von Mechanismen zum Schutz offener Positionen mit Take Profit und Stop Loss. Betrachtet lokale und serverseitige Ansätze zum Positionsschutz.

- [Strategieparameter](strategies/parameters.md) - Anleitung zur Arbeit mit Strategieparametern über [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1). Beschreibt, wie konfigurierbare Parameter erstellt, ihre Anzeige in der GUI festgelegt und sie in der Optimierung verwendet werden.

- [Logging in Strategien](strategies/logging.md) - Anleitung zur Verwendung des Logging-Mechanismus in Strategien zur Nachverfolgung und Fehlersuche bei der Algorithmusausführung.

### Erweiterte Funktionen

- [Kompatibilität von Strategieplattformen](strategies/compatibility.md) - Empfehlungen zum Erstellen von Strategien, die mit verschiedenen StockSharp-Plattformen kompatibel sind: [Designer](../designer.md), [Shell](../shell.md), [Runner](../runner.md) und Cloud-Testing.

- [High-Level-APIs in Strategien](strategies/high_level_api.md) - Beschreibung von High-Level-Methoden, die die Arbeit mit Abonnements, Indikatoren, Charts und Positionsschutz vereinfachen. Erläutert, wie saubererer Code geschrieben wird, indem der Fokus auf der Handelslogik liegt.

- [Arbeiten mit Charts in Strategien](strategies/chart.md) - Anleitung zur Visualisierung von Strategiedaten in einem Chart. Erläutert den Zugriff auf den Chart, das Erstellen von Bereichen, das Hinzufügen von Elementen und die Darstellung von Daten.

- [Speichern und Laden von Einstellungen](strategies/settings_saving_and_loading.md) - Beschreibung des Mechanismus zum Speichern und Laden von Strategieeinstellungen über die Methoden [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) und [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)).

- [Laden des Zustands](strategies/orders_and_trades_loading.md) - Anleitung zum Laden zuvor ausgeführter Orders und Trades in eine Strategie, beispielsweise beim Neustart einer Strategie während einer Handelssitzung.

- [Preisrundung](strategies/shrink_price.md) - Anleitung zum korrekten Runden von Preisen in Strategien mit der Methode [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)).

- [Unit-Typ](strategies/unit_type.md) - Beschreibung des Datentyps [Unit](xref:StockSharp.Messages.Unit), der arithmetische Operationen mit Größen wie Prozentwerten, Punkten oder Pips vereinfacht.

- [Ereignismodell](strategies/event_model.md) - Erklärung des Strategie-Ereignismodells auf Basis von [IMarketRule](xref:StockSharp.Algo.IMarketRule). Behandelt das Erstellen von Regeln zur Reaktion auf Marktereignisse, das Kombinieren von Bedingungen und die Verwaltung von Regel-Lebenszyklen.

## Einstieg in die Strategieentwicklung

Um mit der Entwicklung Ihrer eigenen Strategie zu beginnen, wird empfohlen:

1. Machen Sie sich mit den Grundlagen der Arbeit mit Strategien vertraut, um die allgemeinen Prinzipien von Strategien in StockSharp zu verstehen.

2. Lesen Sie den Abschnitt [Marktdatenabonnements in Strategien](strategies/subscriptions.md), um den Mechanismus zum Empfangen und Verarbeiten von Marktdaten zu verstehen.

3. Sehen Sie sich den Abschnitt [Indikatoren in Strategien](strategies/indicators.md) an, um die Arbeit mit Indikatoren der technischen Analyse zu verstehen.

4. Erkunden Sie den Abschnitt [Handelsoperationen in Strategien](strategies/trading_operations.md), um die Mechanismen von Handelsoperationen zu verstehen.

5. Prüfen Sie den Abschnitt [Strategieparameter](strategies/parameters.md), um die Mechanismen der Strategiekonfiguration kennenzulernen.

6. Machen Sie sich mit dem Abschnitt [High-Level-APIs in Strategien](strategies/high_level_api.md) vertraut, um Strategiecode mithilfe integrierter High-Level-Funktionen zu vereinfachen.

## Strategietests

StockSharp bietet verschiedene Methoden zum Testen von Strategien:

- **Tests mit historischen Daten** - Ermöglicht die Bewertung der Strategieeffektivität anhand historischer Daten.
- **Parameteroptimierung** - Hilft, optimale Werte für Strategieparameter zu finden.
- **Testen mit virtuellem Konto** - Ermöglicht die Überprüfung der Strategieleistung im Echtzeitmodus, ohne reale Mittel zu riskieren.

Detaillierte Beschreibungen der Testmethoden und der Bewertung der Strategieleistung finden Sie im Abschnitt [Testing](../api/testing.md).
