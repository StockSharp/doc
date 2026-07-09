# Zusammengesetzte Elemente

Beim Zusammenstellen von Schemas gibt es häufig Elementgruppen, die eine vollständige Funktionalität bilden und in verschiedenen Schemas oder mehrfach in einem Schema mit unterschiedlichen Eigenschaftswerten verwendet werden können. Solche Elementgruppen können in ein eigenes zusammengesetztes Element ausgelagert werden, das anschließend wie jeder gewöhnliche Würfel verwendet wird.

Ein zusammengesetztes Element ist ein gewöhnliches Schema, das wie jedes Strategieschema gespeichert, geladen und bearbeitet wird.

Wenn Sie ein zusammengesetztes Element zu einem Schema hinzufügen, werden ihm alle nicht verbundenen Parameter aller inneren Würfel automatisch hinzugefügt. Nicht verbundene Parameter am Eingang von Würfeln werden als Eingang hinzugefügt, nicht verbundene Parameter am Ausgang als Ausgang. Jeder hinzugefügte Parameter wird genauso benannt wie das Quell-Element und dessen Parameter. Zusätzlich werden für dieses Element die Eigenschaften aller Elemente hinzugefügt, für die die Eigenschaft **Parameter** festgelegt wurde.

Wir betrachten die Verwendung zusammengesetzter Elemente am Beispiel der Strategie für die Kreuzung gleitender Durchschnitte, die die mehrfache Verwendung des zusammengesetzten Elements [Kreuzung](elements/common/crossing.md) zeigt. Die Strategie kann eine Long-Position eröffnen, wenn der kurze gleitende Durchschnitt den langen von unten nach oben kreuzt, und eine Short-Position, wenn der kurze gleitende Durchschnitt den langen von oben nach unten kreuzt. Das Schema des Teils der Strategie zur Kreuzung gleitender Durchschnitte, in dem der Kreuzungszeitpunkt bestimmt wird, ist in der folgenden Abbildung dargestellt:

![Designer Creating a composite elements 00](../../../../images/designer_creating_composite_elements_00.png)

Da sich die Kreuzung gleitender Durchschnitte nur in ihrer möglichen Richtung unterscheidet (short kreuzt von oben nach unten oder von unten nach oben), kann der Teil des Schemas, der den Kreuzungszeitpunkt bestimmt, in ein separates zusammengesetztes Element ausgelagert werden. Wenn Sie dieses Element zum Schema hinzufügen, geben Sie die Eigenschaften an, die den Algorithmus der Kreuzung gleitender Durchschnitte definieren. Das Schema des zusammengesetzten Elements, mit dem die Kreuzung bestimmt wird, ist in der folgenden Abbildung dargestellt:

![Designer Crossing 01](../../../../images/designer_crossing_01.png)

Das Diagramm des zusammengesetzten Elements besteht aus einfachen Elementen und basiert darauf, die aktuellen Werte (Prev In 1 und Prev In 2) zu merken und die Paare aus aktuellen (CurrComparison) und vorherigen (PrevComparison) Werten miteinander zu vergleichen. Da jeder der Eingangswerte in zwei Elementen des Diagramms verwendet wird, werden die Elemente [Kombination](elements/common/combination.md) (In 1, In 2) am Eingang des zusammengesetzten Elements platziert. Sie erlauben es, einen Eingang auf zwei Elemente aufzuteilen und den Eingangswert an die Elemente [Vergleich](elements/common/comparison.md) und [Vorheriger Wert](elements/common/prev_value.md) weiterzugeben. Wenn ein neuer Wert am Eingang eintrifft, werden die aktuellen Werte verglichen und ein neuer Wert an das Element [Vorheriger Wert](elements/common/prev_value.md) übergeben, aus dem der vorherige Wert für den aktuellen Eingang weitergegeben wird. Danach werden die vorherigen Werte verglichen. Wenn beide Bedingungen erfüllt sind, was mit der And [Logische Bedingung](elements/common/logical_condition.md) geprüft wird, wird der Wert des gesetzten Flags an den Ausgang des zusammengesetzten Elements übergeben und kann als Trigger für weitere Aktionen verwendet werden.

Für die Würfel CurrComparison und PrevComparison ist das Flag **Parameter** der Eigenschaftsgruppe **Allgemein** gesetzt. Daher wurden die Eigenschaften dieser Würfel in die Eigenschaften des zusammengesetzten Elements [Kreuzung](elements/common/crossing.md) übernommen, die später bei der Verwendung des zusammengesetzten Elements im Strategieschema angegeben werden.

![Designer Crossing 00](../../../../images/designer_crossing_00.png)

## Empfohlene Inhalte

[Kerzen im Chart anzeigen](schema_samples/display_candles_on_chart.md)

