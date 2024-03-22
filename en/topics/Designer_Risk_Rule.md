# Risk Management

On the [Testing Properties](Designer_Properties_emulation.md) and [Live Trading Properties](Designer_Properties_Live.md) panels, you can set the risk control settings.

In the Risks window, it's necessary to select a **Risk Rule**, configure the triggering condition for the **Risk Rule**, and the action (Close positions, Stop trading, Cancel orders) that will be executed when the condition for the **Risk Rule** occurs.

It is possible to use multiple risk rules of the same type with different actions. For example, in the screenshot below, if the order volume is 20, then the actions to cancel orders and stop trading should be taken.

![Designer Risk Rule](../images/Designer_Risk_Rule.png)

### List of Risk Rules

List of Risk Rules

- **P/L** - a risk rule that monitors the size of profit/loss.
- **Position** - a risk rule that monitors the size of the position.
- **Position (Time)** - a risk rule that monitors the lifespan of a position.
- **Commission** - a risk rule that monitors the size of the commission.
- **Slippage** - a risk rule that monitors the amount of slippage.
- **Order Price** - a risk rule that monitors the price of an order.
- **Order Volume** - a risk rule that monitors the volume of an order.
- **Order (Frequency)** - a risk rule that monitors the frequency of placing orders.
- **Error in Registration/Cancellation of Order** - a risk rule that monitors the number of errors during registration/cancellation of orders.
- **Trade Price** - a risk rule that monitors the price of a trade.
- **Trade (Volume)** - a risk rule that monitors the volume of a trade.
- **Trade (Frequency)** - a risk rule that monitors the frequency of making trades.
- **Error** - a risk rule that monitors the number of any errors.