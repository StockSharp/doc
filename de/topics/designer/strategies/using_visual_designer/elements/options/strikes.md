# Strikes

![Designer Derivatives 00](../../../../../../images/designer_derivatives_00.png)

Der Würfel wird verwendet, um eine Liste von Optionen anhand eines angegebenen Filters zu erhalten.

### Eingehende Sockets

Eingehende Sockets

- **Instrument** - das Instrument, also der Basiswert.

### Ausgehende Sockets

Ausgehende Sockets

- **Options** - die Liste der Optionen auf den Basiswert.

### Parameter

Parameter

- **Option type** - der Optionstyp kann Call option oder Put option sein.
- **Expiry date** - das Verfallsdatum der Option.
- **Strike (less)** - die Verschiebung nach links (kleiner) vom zentralen Strike. Wenn der Preis nicht gesetzt ist, werden alle Strikes mit einem niedrigeren zentralen Wert verwendet. Die Verschiebung wird in Strike-Schritten berechnet; wenn der Strike-Schritt zum Beispiel 500 c.u. beträgt, entspricht eine Verschiebung von 3 dem Wert 1,500 c.u.
- **Strike (more)** - die Verschiebung nach rechts (größer) vom zentralen Strike. Wenn der Preis nicht gesetzt ist, werden alle Strikes mit einem höheren zentralen Wert verwendet. Die Verschiebung wird in Strike-Schritten berechnet; wenn der Strike-Schritt zum Beispiel 500 c.u. beträgt, entspricht eine Verschiebung von 3 dem Wert 1,500 c.u.

## Empfohlene Inhalte

[Crossing](../common/crossing.md)

