# Position

![Designer Position 00](../../../../../../images/designer_position_00.png)

Das Element wird verwendet, um Informationen über Positionsänderungen für das angegebene Instrument und Portfolio zu erhalten.

### Eingehende Sockets

Eingehende Sockets

- **Handelsinstrument** - das Instrument, für das Sie eine Position erhalten möchten.
- **Portfolio** - das Portfolio, für das Sie eine Position erhalten möchten.

### Ausgehende Sockets

Ausgehende Sockets

- **Position** - der numerische Wert der Position im Instrument oder der aktuelle Betrag verfügbarer Mittel auf dem Konto. Dieser Wert wird erzeugt, wenn sich entweder die Position oder die Mittel ändern, sowie nach dem Start der Strategie.

### Parameter

Parameter

- **Geld** - wenn das Flag am Eingang gesetzt ist, akzeptiert das Element nur das Portfolio; die Geldposition für das ausgewählte Portfolio wird an den Ausgang des Elements übergeben.

## Empfohlene Inhalte

[Positionsschutz](protect.md)
