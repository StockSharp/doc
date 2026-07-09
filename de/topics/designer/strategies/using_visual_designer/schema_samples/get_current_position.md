# Aktuelle Position abrufen

Um das Volumen zu erhalten, das zum Drehen der aktuellen Position in die entgegengesetzte Position erforderlich ist, kann das Schema aus dem SMA-Strategiebeispiel verwendet werden:

![Designer Determination of the volume position 00](../../../../../images/designer_determination_of_volume_position_00.png)

Für den Würfel [Variable](../elements/data_sources/variable.md) wird der Datentyp **Instrument** ausgewählt. Wenn das Instrument nicht angegeben ist, aber das Flag **Parameter** der Gruppe **Allgemein** gesetzt ist, wird es aus der Strategie übernommen und anschließend an [Position](../elements/positions/current.md) übergeben.

Für den Würfel [Position](../elements/positions/current.md) ist die Positionseigenschaft ebenfalls nicht angegeben, aber das Flag **Parameter** der Gruppe **Allgemein** ist gesetzt. Das bedeutet, dass die Position für das Portfolio ermittelt wird, das in den Strategieeinstellungen angegeben ist.

Nach dem Übergeben des Instruments und der Positionsänderung wird mit der mathematischen Funktion mit einem Argument (abs(pos)) der absolute Wert berechnet und ein Signal an den Würfel der Variablen (2) gegeben. Dieser Würfel enthält den Faktor 2; um den gespeicherten Wert über den Ausgabeparameter weiterzugeben, wird anschließend mit einer mathematischen Formel mit zwei Argumenten (abs(pos) \* 2) deren Produkt berechnet. Danach wird mit dem zusammengesetzten Würfel Conditional operator (pos \=\= 0 ? 1 : pos) der tatsächliche Wert des erforderlichen Volumens bestimmt, der vom aktuellen Positionswert multipliziert mit 2 abweichen kann. Zum Beispiel beim Start der Strategie, wenn noch keine Orders ausgeführt wurden. In diesem Fall gibt das Element Conditional statement den Standardwert 1 zurück. Da ein Ausgabeparameter nur einmal mit dem Eingabeparameter eines anderen Elements verbunden werden kann, wird ein zusätzlicher Würfel **Combination** hinzugefügt, um denselben Wert zwischen Formel und bedingtem Operator zu übergeben.

## Empfohlene Inhalte

[Preisniveau im Orderbuch abrufen](get_order_book_price_level.md)

