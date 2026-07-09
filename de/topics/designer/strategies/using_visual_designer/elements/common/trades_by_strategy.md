# Trades nach Strategie

![Designer The transaction strategy 00](../../../../../../images/designer_trades_strategy_00.png)

Der Würfel wird verwendet, um alle Trades der Strategie zu erhalten.

## Eingehende Sockets

- **Handelsinstrument** - das Instrument, für das Trades abgerufen werden sollen. Wenn kein Instrument übergeben wird, werden Trades für alle Instrumente der Strategie an den Ausgang übertragen.

## Ausgehende Sockets

- **Ausführungen** - Trades, die aus dem übergebenen Instrument entstehen. Sie können sowohl zur Anzeige auf dem Chart über das Element **Diagrammbereich** als auch für den Positionsschutz über das Element **Positionsschutz** verwendet werden.

