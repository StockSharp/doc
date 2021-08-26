# Take\-profit and stop\-loss

The [S\#](StockSharpAbout.md) has a mechanism of automatic position protection through [TakeProfitStrategy](../api/StockSharp.Algo.Strategies.Protective.TakeProfitStrategy.html) and [StopLossStrategy](../api/StockSharp.Algo.Strategies.Protective.StopLossStrategy.html) strategies using the [child strategies](StrategyChilds.md) approach and the [BasketStrategy](../api/StockSharp.Algo.Strategies.BasketStrategy.html). These strategies have a number of advantages over usual stop order: 

1. Protective strategies do not register orders as long as the condition occurs. The orders deposit size is not blocked by the broker.
2. Strategies are platform\-independent, and hence will work through any broker regardless of its technology. And stop orders, which conditions set via [Order.Condition](../api/StockSharp.BusinessEntities.Order.Condition.html), emulated by the [Rithmic](Rithmic.md), [OpenECry](OEC.md) etc. platforms by itself, and their logic is locked to the broker.
3. Protective strategies can work with direct connection to exchanges.
4. Automatic tracking of partial or complete closing of the protected position (with the following stops removal). And also the position reverting. For example, when the position was long, and then it was turned into a short. In this case stops should be also "reverted".

### Prerequisites

[Child strategies](StrategyChilds.md)

[Event model](StrategyAction.md)

### Take\-profit and stop\-loss

Take\-profit and stop\-loss

1. As an example, the order registration for the "at market" purchase and the following protection of the long position considered. To do this, the rule responsive to the order’s trades occurring (for details see the [Event model](StrategyAction.md)) created: 

   ```cs
   public class MyStrategy : Strategy
   {
   	public void OpenPosition()
   	{
   		var longPos \= this.BuyAtMarket();
   		
   		\/\/ applying rules to track the order's trades
   		longPos
   			.WhenNewTrade()
   			.Do(OnNewOrderTrade)
   			.Apply(this);
   		
   		RegisterOrder(longPos);
   	}
   }
   					
   ```
2. In order to protect the position you should use [TakeProfitStrategy](../api/StockSharp.Algo.Strategies.Protective.TakeProfitStrategy.html) or [StopLossStrategy](../api/StockSharp.Algo.Strategies.Protective.StopLossStrategy.html) strategies. If you need simultaneous protection from both sides, it is recommended to use the [TakeProfitStopLossStrategy](../api/StockSharp.Algo.Strategies.Protective.TakeProfitStopLossStrategy.html). This strategy automatically changes the one of strategies volume with partial activation (for example, at the touch of a stop\-loss level only part of the position has been closed, and then the market came back to break\-even zone): 

   ```cs
   private void OnNewOrderTrade(MyTrade trade)
   {
       \/\/ take is 40 points
       var takeProfit \= new TakeProfitStrategy(trade, 40);
       \/\/ stop is 20 points
       var stopLoss \= new StopLossStrategy(trade, 20);
       var protectiveStrategies \= new TakeProfitStopLossStrategy(takeProfit, stopLoss);
       ChildStrategies.AddRange(protectiveStrategies);
   }
   ```

### Automatic closing and position reverting

Automatic closing and position reverting

[TakeProfitStrategy](../api/StockSharp.Algo.Strategies.Protective.TakeProfitStrategy.html) and [StopLossStrategy](../api/StockSharp.Algo.Strategies.Protective.StopLossStrategy.html) strategies do not track partial position closing or its reverting (for example, the position was closed by hands at the terminal and was opened in the opposite direction). To automatically track such situations in the algorithm, you must use [AutoProtectiveStrategy](../api/StockSharp.Algo.Strategies.Protective.AutoProtectiveStrategy.html). This strategy by trades incoming into it ([AutoProtectiveStrategy.ProcessNewMyTrade](../api/StockSharp.Algo.Strategies.Protective.AutoProtectiveStrategy.ProcessNewMyTrade.html)) decides what to do: to protect them (if there is a position opening or its increase) or to stop the protective strategies (if there is a position closing or its decrease). The strategy also automatically reverts protective strategies in case of position reverting (from long to short or from short to long). 

### Next Steps

[Reports](StrategyReports.md)
