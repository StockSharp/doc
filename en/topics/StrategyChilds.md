# Child strategies

With the [Strategy](../api/StockSharp.Algo.Strategies.Strategy.html), you can also create child strategies. For example, when you want to implement the trading process, which operates with standard (or previously implemented) algorithms, building them into a single chain: 

![strategychilds](~/images/strategy_childs.png)

### Prerequisites

[Creating strategies](StrategyCreate.md)

### Work with child strategies

Work with child strategies

To add a child strategy you should use the [Strategy.ChildStrategies](../api/StockSharp.Algo.Strategies.Strategy.ChildStrategies.html) property. It is possible not to set [Strategy.Connector](../api/StockSharp.Algo.Strategies.Strategy.Connector.html), [Strategy.Portfolio](../api/StockSharp.Algo.Strategies.Strategy.Portfolio.html) and [Strategy.Security](../api/StockSharp.Algo.Strategies.Strategy.Security.html) values for child strategies, and they are automatically filled when you add them to the parent strategy. 

When adding a new child strategy, it automatically receives the state from the parent. For example, if the parent strategy is in operating state ([ProcessStates.Started](../api/StockSharp.Algo.ProcessStates.Started.html)), then the child is automatically set in the operating state (vice versa, in the case of the [ProcessStates.Stopped](../api/StockSharp.Algo.ProcessStates.Stopped.html)). 

Therefore, to call the [Strategy.Start](../api/StockSharp.Algo.Strategies.Strategy.Start.html) method for the child strategy is not necessary. It will be called automatically when the parental strategy starts. Thus, the [Strategy.Start](../api/StockSharp.Algo.Strategies.Strategy.Start.html) method can be called only for the root strategies. 

The parent strategy and all its child strategies are executed in parallel. This means that, if the [iteration model](StrategyCreate.md) has been selected, the [TimeFrameStrategy.OnProcess](../api/StockSharp.Algo.Strategies.TimeFrameStrategy.OnProcess.html) methods for child and parent strategies are executed in parallel. The same situation with the [Strategy.Rules](../api/StockSharp.Algo.Strategies.Strategy.Rules.html) rules, the [event model](StrategyAction.md) has been selected. 

> [!TIP]
> The [Strategy.Stop](../api/StockSharp.Algo.Strategies.Strategy.Stop.html) method for the child strategy, unlike [Strategy.Start](../api/StockSharp.Algo.Strategies.Strategy.Start.html), can be called at any time. For example, when the algorithm needs to forcibly stop the operation of the child strategy, while the algorithm of the child strategy itself has not yet been executed to the end (for example, to interrupt [Quoting](StrategyQuoting.md)). 

By default, child strategies are not associated with each other, and are executed independently. When you need to establish a dependency between the strategies, you must use the [BasketStrategy](../api/StockSharp.Algo.Strategies.BasketStrategy.html) class. This class allows you to specify conditions for the strategies terminations depending on each other through the [BasketStrategyFinishModes](../api/StockSharp.Algo.Strategies.BasketStrategyFinishModes.html) indicators. For example, through the [First](../api/StockSharp.Algo.Strategies.BasketStrategyFinishModes.First.html) value the condition set wherein all child strategies will be stopped, when at least one of them matched. The example of the [BasketStrategy](../api/StockSharp.Algo.Strategies.BasketStrategy.html) use is shown in the [Take\-profit and stop\-loss](StrategyProtective.md). 

### Next Steps

[Quoting](StrategyQuoting.md)

[Take\-profit and stop\-loss](StrategyProtective.md)

## Recommended content
