# Take\-profit and stop\-loss

The [S\#](StockSharpAbout.md) has a mechanism of automatic position protection through [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) and [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy) strategies using the [child strategies](StrategyChilds.md) approach and the [BasketStrategy](xref:StockSharp.Algo.Strategies.BasketStrategy). These strategies have a number of advantages over usual stop order: 

1. Protective strategies do not register orders as long as the condition occurs. The orders deposit size is not blocked by the broker.
2. Strategies are platform\-independent, and hence will work through any broker regardless of its technology. And stop orders, which conditions set via [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition), emulated by the [Rithmic](Rithmic.md), [OpenECry](OEC.md) etc. platforms by itself, and their logic is locked to the broker.
3. Protective strategies can work with direct connection to exchanges.
4. Automatic tracking of partial or complete closing of the protected position (with the following stops removal). And also the position reverting. For example, when the position was long, and then it was turned into a short. In this case stops should be also "reverted".

## Prerequisites

[Child strategies](StrategyChilds.md)

[Event model](StrategyAction.md)

## Take\-profit and stop\-loss

1. As an example, the order registration for the "at market" purchase and the following protection of the long position considered. To do this, the rule responsive to the order’s trades occurring (for details see the [Event model](StrategyAction.md)) created: 

   ```cs
   public class MyStrategy : Strategy
   {
   	public void OpenPosition()
   	{
   		var longPos = this.BuyAtMarket();
   		
   		// applying rules to track the order's trades
   		longPos
   			.WhenNewTrade()
   			.Do(OnNewOrderTrade)
   			.Apply(this);
   		
   		RegisterOrder(longPos);
   	}
   }
   					
   ```
2. In order to protect the position you should use [TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) or [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy) strategies. If you need simultaneous protection from both sides, it is recommended to use the [TakeProfitStopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStopLossStrategy). This strategy automatically changes the one of strategies volume with partial activation (for example, at the touch of a stop\-loss level only part of the position has been closed, and then the market came back to break\-even zone): 

   ```cs
   private void OnNewOrderTrade(MyTrade trade)
   {
       // take is 40 points
       var takeProfit = new TakeProfitStrategy(trade, 40);
       // stop is 20 points
       var stopLoss = new StopLossStrategy(trade, 20);
       var protectiveStrategies = new TakeProfitStopLossStrategy(takeProfit, stopLoss);
       ChildStrategies.AddRange(protectiveStrategies);
   }
   ```

## Automatic closing and position reverting

[TakeProfitStrategy](xref:StockSharp.Algo.Strategies.Protective.TakeProfitStrategy) and [StopLossStrategy](xref:StockSharp.Algo.Strategies.Protective.StopLossStrategy) strategies do not track partial position closing or its reverting (for example, the position was closed by hands at the terminal and was opened in the opposite direction). To automatically track such situations in the algorithm, you must use [AutoProtectiveStrategy](xref:StockSharp.Algo.Strategies.Protective.AutoProtectiveStrategy). This strategy by trades incoming into it ([AutoProtectiveStrategy.ProcessNewMyTrade](xref:StockSharp.Algo.Strategies.Protective.AutoProtectiveStrategy.ProcessNewMyTrade(StockSharp.BusinessEntities.MyTrade))**(**[StockSharp.BusinessEntities.MyTrade](xref:StockSharp.BusinessEntities.MyTrade) trade **)**) decides what to do: to protect them (if there is a position opening or its increase) or to stop the protective strategies (if there is a position closing or its decrease). The strategy also automatically reverts protective strategies in case of position reverting (from long to short or from short to long). 

## Next Steps

[Reports](StrategyReports.md)
