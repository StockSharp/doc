# Order Cancellation

![Designer Cancellations 00](../../../../../../images/designer_cancellations_00.png)

Dieser Block wird verwendet, um eine Order für ein Instrument zu stornieren.

### Eingehende Sockets

Eingehende Sockets

- **Trigger** - das Ereignis, das die Orderstornierung auslöst.
- **Order** - das Signal, mit dem bestimmt wird, wann eine Order storniert werden muss.

### Ausgehende Sockets

Ausgehende Sockets

- **Order** - die stornierte Order, die verwendet werden kann, um über das Element **Transactions** Transaktionen dafür abzurufen und sie mit dem Block **Chart Panel** im Chart anzuzeigen.
- **Error** - ein Fehler beim Stornieren der Order (zum Beispiel, wenn die Order bereits früher ausgeführt oder storniert wurde).

## Siehe auch

[Mass Orders Cancel](mass_cancel.md)

