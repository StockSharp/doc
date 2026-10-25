# Debugging

Beim Strategietesting muss häufig geprüft werden, welche Daten am Eingang eines bestimmten Würfels ankommen oder an seinem Ausgang weitergegeben werden. Dafür bietet der [Designer](../../designer.md) den **Debugger**.

![Designer Fehlersuche 00](../../../images/designer_debug_00.png)

Die Schaltflächen des Debuggers befinden sich auf der Leiste des Schemas selbst, über der Zeichenfläche. Die meisten von ihnen haben Tasten: **F9** — Haltepunkt hinzufügen, **F10** — zum nächsten Element, **F11** — in das zusammengesetzte Element, **Shift+F11** — aus dem zusammengesetzten Element. Ein Rechtsklick auf einen Anschluss setzt und entfernt einen Haltepunkt an diesem Anschluss, und die Schaltfläche **Start** (**F5**) auf der Registerkarte **Backtest** setzt den Test bis zum nächsten Haltepunkt fort.

Die Schaltflächen des Debuggers:

- ![Designer Fehlersuche 01](../../../images/designer_debug_01.png)**Haltepunkt hinzufügen** - fügt dem ausgewählten Element einen Haltepunkt hinzu. Elemente, für die ein Haltepunkt hinzugefügt wurde, werden mit einem roten Rahmen markiert.
- ![Designer Fehlersuche 02](../../../images/designer_debug_02.png)**Haltepunkt löschen** - löscht einen Haltepunkt.
- ![Designer Fehlersuche 03](../../../images/designer_debug_03.png)**Nächstes Element** - wechselt beim Auslösen des Haltepunkts zum nächsten Element des Schemas.
- **Schritt zum Ausgang** - wechselt beim Auslösen des Haltepunkts zum Ausgang des aktuellen Elements und wird verwendet, um die am Elementausgang übergebenen Werte zu prüfen.
- ![Designer Fehlersuche 04](../../../images/designer_debug_04.png)**Schritt hinein** - wechselt beim Auslösen des Haltepunkts in das zusammengesetzte Element. Öffnet automatisch das Schema des zusammengesetzten Elements und hält an dem Element an, an das die Daten zuerst übertragen werden.
- ![Designer Fehlersuche 05](../../../images/designer_debug_05.png)**Schritt heraus** - verlässt beim Auslösen des Haltepunkts, wenn sich die Ausführung innerhalb eines zusammengesetzten Elements befindet, eine Ebene nach oben zu der Stelle, an der das geöffnete zusammengesetzte Element verwendet wird.
- ![Designer Fehlersuche 06](../../../images/designer_debug_06.png)**Fortsetzen** - setzt die Ausführung bis zum nächsten ausgelösten Haltepunkt fort. Das ist die Schaltfläche **Start** der Registerkarte **Backtest**: Solange der Test läuft, heißt sie **Fortsetzen**.

## Empfohlene Inhalte

[Haltepunkte](debugging/break_points.md)
