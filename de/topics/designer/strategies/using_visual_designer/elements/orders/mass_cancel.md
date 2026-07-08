# Massenstornierung von Orders

![Designer Mass Cancellations 00](../../../../../../images/designer_mass_cancellations_00.png)

Dieser Block wird verwendet, um alle Orders für ein Instrument zu stornieren.

### Eingehende Sockets

Eingehende Sockets

- **Trigger** - das Signal, das bestimmt, wann Orders storniert werden müssen.
- **Portfolio** - das Portfolio, für das alle Orders storniert werden sollen.
- **Security** - das Instrument, für das alle Orders storniert werden sollen.

### Ausgehende Sockets

Ausgehende Sockets

- **Result** - ein Flag, das den Erfolg des Vorgangs signalisiert.

### Parameter

Parameter

- **Direction** - die Richtung der zu stornierenden Orders (Kauf oder Verkauf); dient als Stornierungssignal für die Order.

## Siehe auch

[Orderregistrierung](register.md)

