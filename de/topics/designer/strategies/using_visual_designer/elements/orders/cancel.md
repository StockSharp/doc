# Orderstornierung

![Orderstornierung Bildschirmfoto](../../../../../../images/designer_cancellations_00.png)

Dieser Block wird verwendet, um eine Order für ein Instrument zu stornieren.

### Eingehende Sockets

Eingehende Sockets

- **Auslöser** - das Ereignis, das die Orderstornierung auslöst.
- **Auftrag** - das Signal, mit dem bestimmt wird, wann eine Order storniert werden muss.

### Ausgehende Sockets

Ausgehende Sockets

- **Auftrag** - die stornierte Order, die verwendet werden kann, um über das Element **Transaktionen** Transaktionen dafür abzurufen und sie mit dem Block **Diagrammbereich** im Chart anzuzeigen.
- **Fehler** - ein Fehler beim Stornieren der Order (zum Beispiel, wenn die Order bereits früher ausgeführt oder storniert wurde).

## Siehe auch

[Massenstornierung von Orders](mass_cancel.md)

