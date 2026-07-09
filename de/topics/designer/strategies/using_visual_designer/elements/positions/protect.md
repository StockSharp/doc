# Positionsschutz

![Designer Protect positions 00](../../../../../../images/designer_protect_positions_00.png)

![Designer Protect positions 01](../../../../../../images/designer_protect_positions_01.png)

Dieser Block wird verwendet, um offene Trades automatisch mit Stop-Loss und Take-Profit zu schützen.

### Eingehende Sockets

Eingehende Sockets

- **Eigene Ausführung** - der Trade, der mit Stop-Loss und Take-Profit geschützt werden soll.
- **Preis** - der aktuelle Preis (kann aus einer Kerze, dem letzten Tick usw. entnommen werden). Er ist erforderlich, um den aktuellen Preis des Instruments zu verfolgen und Schutzorders zu aktivieren.

### Ausgehende Sockets

Ausgehende Sockets

- **Take-Profit** - eine Order zur Gewinnsicherung.
- **Stop-Loss** - eine Order zur Verlustbegrenzung.
- **Eigene Transaktion** - eine Transaktion, die durch eine der oben genannten Orders erstellt wurde.

### Parameter

Take- und Stop-Parameter

- **Wert** - der Wert des Take oder Stop.
- **Trailing** - gibt an, ob Trailing-Schutz verwendet wird.
- **Timeout** - der Timeout-Wert, nach dem der Schutz zwangsweise zum Marktpreis ausgelöst wird.
- **Market-Orders** - Market-Orders (ohne Preis) zum schnellen Schließen der Position verwenden.

![Designer Protect positions 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> Eingehende Transaktionen DÜRFEN KEINE Transaktionen der gesamten Strategie sein (der Block [Trades nach Strategie](../common/trades_by_strategy.md)), da dies zu einer fehlerhaften Berechnung der aktuellen Position führt: Die Schutztransaktionen würden ebenfalls zu Strategietransaktionen. Der Block **Positionsschutz** sollte Transaktionen aus dem Ausgabe-Socket **Transaktion** der Würfel [Orderregistrierung](../orders/register.md) und [Position ändern](modify.md) oder ähnlicher Komponenten erhalten, die die Position direkt ändern.

