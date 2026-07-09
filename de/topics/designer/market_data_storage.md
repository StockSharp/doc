# Marktdatenspeicher

Der Speicher für historische Daten ist dafür vorgesehen, Marktdaten (Instrumente, Kerzen, Tick-Trades und Orderbücher) aus verschiedenen Quellen zu laden und in einem lokalen oder entfernten Speicher abzulegen. [Designer](../designer.md) kann Quellen sowohl für historische als auch für Echtzeitdaten verwenden ([Konnektoren](../api/connectors.md)). Die gespeicherten Informationen stehen anschließend Handelsstrategien zur Verfügung.

Um den Tab **Marktdaten** zu öffnen, wechseln Sie zum Tab **Allgemein** und klicken auf die Schaltfläche **Marktdaten**. Der Bereich **Marktdaten** ist in drei Bereiche unterteilt. Der linke Bereich enthält die Liste aller empfangenen Instrumente aus allen Quellen, die jemals verbunden waren. Die mittleren Bereiche enthalten die aktiven Instrumente. Mit diesen Instrumenten können Sie die heruntergeladene Historie herunterladen oder anzeigen. Der rechte Bereich zeigt die verfügbaren Daten für das im mittleren Bereich ausgewählte Instrument; außerdem können Sie Daten für das ausgewählte Instrument herunterladen.

![Designer Repository of historical data 00](../../images/designer_repository_of_historical_data_00.png)

## Empfohlene Inhalte

[Erste Schritte](market_data_storage/getting_started.md)
