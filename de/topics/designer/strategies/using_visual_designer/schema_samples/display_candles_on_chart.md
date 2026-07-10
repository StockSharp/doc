# Kerzen im Chart anzeigen

Um Kerzen eines Instruments in einem Chart auszugeben, kann das folgende Schema verwendet werden:

![Designer Ausgabe der Kerzen im Chart 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

Für den Würfel [Variable](../elements/data_sources/variable.md) wird der Datentyp **Handelsinstrument** ausgewählt. Wenn das Instrument nicht angegeben ist, aber das Flag **Parameter** der Eigenschaftsgruppe **Allgemein** gesetzt ist, wird es aus der Strategie übernommen und an den Würfel [Kerzen](../elements/data_sources/candles.md) übergeben. Für den Würfel [Kerzen](../elements/data_sources/candles.md) sind die Einstellungen zum Erstellen von 5-Minuten-Kerzen und zum Übergeben nur vollständig gebildeter Kerzen angegeben.

Für den Würfel [Chart](../elements/common/chart.md) wurde ein grafisches Element mit Kerzentyp hinzugefügt, für das der Eingabeparameter automatisch hinzugefügt wurde.

Nach dem Hinzufügen der erforderlichen grafischen Elemente zum Chart-Panel wird die Verbindung zwischen den Elementen [Kerzen](../elements/data_sources/candles.md) und [Chart](../elements/common/chart.md) hinzugefügt. Über diese Verbindung werden die erstellten Kerzen zur Ausgabe an den Chart übergeben.

## Empfohlene Inhalte

[Besten Preis für ein Instrument abrufen](get_best_price_for_instrument.md)

