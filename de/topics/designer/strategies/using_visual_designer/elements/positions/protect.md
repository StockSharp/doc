# Position Protection

![Designer Protect positions 00](../../../../../../images/designer_protect_positions_00.png)

![Designer Protect positions 01](../../../../../../images/designer_protect_positions_01.png)

Dieser Block wird verwendet, um offene Trades automatisch mit Stop-Loss und Take-Profit zu schützen.

### Eingehende Sockets

Eingehende Sockets

- **Own trade** - der Trade, der mit Stop-Loss und Take-Profit geschützt werden soll.
- **Price** - der aktuelle Preis (kann aus einer Kerze, dem letzten Tick usw. entnommen werden). Er ist erforderlich, um den aktuellen Preis des Instruments zu verfolgen und Schutzorders zu aktivieren.

### Ausgehende Sockets

Ausgehende Sockets

- **Take-profit** - eine Order zur Gewinnsicherung.
- **Stop-loss** - eine Order zur Verlustbegrenzung.
- **Own transaction** - eine Transaktion, die durch eine der oben genannten Orders erstellt wurde.

### Parameter

Take- und Stop-Parameter

- **Value** - der Wert des Take oder Stop.
- **Trailing** - gibt an, ob Trailing-Schutz verwendet wird.
- **Timeout** - der Timeout-Wert, nach dem der Schutz zwangsweise zum Marktpreis ausgelöst wird.
- **Market orders** - Market-Orders (ohne Preis) zum schnellen Schließen der Position verwenden.

![Designer Protect positions 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> Eingehende Transaktionen DÜRFEN KEINE Transaktionen der gesamten Strategie sein (der Block [Strategy Trades](../common/trades_by_strategy.md)), da dies zu einer fehlerhaften Berechnung der aktuellen Position führt: Die Schutztransaktionen würden ebenfalls zu Strategietransaktionen. Der Block **Position Protection** sollte Transaktionen aus dem Ausgabe-Socket **Transaction** der Würfel [Order Registration](../orders/register.md) und [Modify Position](modify.md) oder ähnlicher Komponenten erhalten, die die Position direkt ändern.

