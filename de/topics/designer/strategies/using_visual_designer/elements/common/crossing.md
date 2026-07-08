# Crossing

![Designer Crossing 00](../../../../../../images/designer_crossing_00.png)

Dieses Element wird verwendet, um die Position zweier Werte relativ zueinander zu verfolgen, zum Beispiel um den Zeitpunkt der Kreuzung zweier Linien zu bestimmen.

Der Vergleich erfolgt anhand der Werte an den beiden Sockets **Up** und **Down**.

## Eingehende Sockets

- **Up** – Werte, die einen Vergleich erlauben (zum Beispiel ein numerischer Wert, ein Indikatorwert usw.).
- **Down** – Werte, die einen Vergleich erlauben (zum Beispiel ein numerischer Wert, ein Indikatorwert usw.).

## Ausgehende Sockets

- **Flag** – true, wenn **Up** größer als **Down** ist, andernfalls false.

![Designer Crossing 01](../../../../../../images/designer_crossing_01.png)

Ein Beispiel für die Verwendung des Blocks Crossing zur Verfolgung der Kreuzungen zweier [SMA-Indikatoren](../../../../../api/indicators/list_of_indicators/sma.md). Es werden zwei Crossing-Blöcke verwendet, und jeder gibt separat true aus, je nachdem, ob der länge SMA größer als der kurze ist oder kleiner.

## Siehe auch

[Value Delay](delay_value.md)

