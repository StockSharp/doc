# Position Protection

![Designer Protect positions 00](../../../../../../images/designer_protect_positions_00.png)

![Designer Protect positions 01](../../../../../../images/designer_protect_positions_01.png)

This block is used for automatically protecting open trades with stop-loss and take-profit.

### Incoming Sockets

Incoming Sockets

- **Own trade** – the trade that needs to be protected with a stop-loss and take-profit.
- **Price** – the current price (can be taken from a candle, the last tick, etc.). Required for tracking the current price of the instrument and activating protective orders.

### Outgoing Sockets

Outgoing Sockets

- **Take-profit** – an order to lock in profits.
- **Stop-loss** – an order to limit losses.
- **Own transaction** – a transaction created by one of the above orders.

### Parameters

Take and Stop Parameters

- **Value** - the value of the take or stop.
- **Trailing** – is trailing protection used.
- **Timeout** - the timeout value after which protection is forcibly triggered at the market price.
- **Market orders** – use market orders (without price) for quick position closing.

![Designer Protect positions 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> Incoming transactions CANNOT be transactions of the entire strategy (the [Strategy Trades](../common/trades_by_strategy.md) block) as this will lead to incorrect calculation of the current position: the protection transactions will also become strategy transactions. The **Position Protection** block should receive transactions from the **Transaction** output socket of the [Order Registration](../orders/register.md) and [Modify Position](modify.md) cubes, or similar components that directly change the position.