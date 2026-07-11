# Preisniveau im Orderbuch abrufen

Um die erforderliche Kaufzeile aus dem Orderbuch zu erhalten, kann das folgende Schema verwendet werden:

![Designer Ereignismodell 00](../../../../../images/designer_event_model_00.png)

Für den Würfel [Variable](../elements/data_sources/variable.md) wird der Datentyp **Handelsinstrument** ausgewählt. Wenn das Instrument nicht angegeben ist, aber das Flag **Parameter** der Eigenschaftsgruppe **Allgemein** gesetzt ist, wird es aus der Strategie übernommen. Für den Würfel [Konverter](../elements/converters/converter.md) werden der Datentyp und das entsprechende Feld der Würfelsammlung für Bids-Käufe ausgewählt. Der Indexer-Würfel erhält das erforderliche Element aus der Sammlung der besten Kaufpreise. Um einen bestimmten Preis- oder Volumenwert auf einer Ebene zu erhalten, können Sie den Würfel [Konverter](../elements/converters/converter.md) verwenden.

## Empfohlene Inhalte

[Strategiegalerie](../../../strategy_gallery.md)

