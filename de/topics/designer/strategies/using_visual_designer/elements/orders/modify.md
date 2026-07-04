# Order Movement

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

Dieser Block wird verwendet, um eine Order für ein Instrument zu ändern.

### Eingehende Sockets

Eingehende Sockets

- **Trigger** - das Signal, das bestimmt, wann eine Order verschoben wird.
- **Order** - die Order, die geändert wird.
- **Price** - numerischer Wert des neuen Preises.
- **Volume** - numerischer Wert des neuen Volumens.

### Ausgehende Sockets

Ausgehende Sockets

- **Order** - die geänderte Order, die verwendet werden kann, um über das Element **Transactions by Order** Transaktionen dafür zu erhalten und sie mit dem Block **Chart Panel** im Chart anzuzeigen.
- **Error** - ein Fehler beim Verschieben der Order.
- **Trade** - der Trade für die platzierte Order.

Parameter

- **Zero Price** - ein Preis von null registriert eine Market-Order.

## Siehe auch

[Cancel Order](cancel.md)

