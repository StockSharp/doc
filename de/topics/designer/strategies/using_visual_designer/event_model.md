# Event model

Der Ansatz zum Erstellen von Schemas im [Designer](../../../designer.md) basiert auf der Erzeugung und anschließenden Verarbeitung von Ereignissen. Beim Erstellen einer Strategie ist nicht bekannt, wann ein Ereignis zur Änderung von Marktdaten eintritt, aber Sie können dieses Ereignis abonnieren und entsprechend verarbeiten.

Jeder Würfel im [Designer](../../../designer.md), der einen Ausgabeparameter hat, ist ein Ereigniserzeuger. Würfel mit einem Eingabeparameter können das Ereignis abonnieren, das vom ausgehenden Parameter erzeugt wird. Ein Ereignis zu abonnieren bedeutet nichts anderes, als eine Verbindungslinie zwischen zwei Würfeln zu erstellen.

Zum Beispiel erzeugt der Würfel [Order book](elements/market_depths/order_book.md) ein Ereignis zur Änderung des Orderbuchs. Im Voraus ist nicht bekannt, wann eine Änderung eintritt. Wenn Sie eine Verbindungslinie zwischen dem Würfel [Order book](elements/market_depths/order_book.md) und dem Würfel [Converter](elements/converters/converter.md) erstellen, wird eine Subscription auf die Orderbuchänderung eingerichtet, damit diese anschließend mit dem Würfel [Converter](elements/converters/converter.md) weiterverarbeitet werden kann usw.:

![Designer Event model 00](../../../../images/designer_event_model_00.png)

## Empfohlene Inhalte

[First strategy](first_strategy.md)

