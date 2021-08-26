# Commission

The [CommissionManager](../api/StockSharp.Algo.Commissions.CommissionManager.html) manager is used to calculate commissions in the trading algorithm.

The tariff plan is created by adding the relevant [CommissionRule](../api/StockSharp.Algo.Commissions.CommissionRule.html) rules, based on which commissions will be calculated further.

### The CommissionManager creating

The CommissionManager creating

1. To create the [CommissionManager](../api/StockSharp.Algo.Commissions.CommissionManager.html):

   ```cs
   private CommissionManager _commissionManager = new CommissionManager();
   						
   ```
2. Then, you must create the rule:

   ```cs
    CommissionRule commissionRule =  new CommissionPerTradeRule {  Value = new Unit(1m) };
   						
   ```
3. And add it to the [CommissionManager](../api/StockSharp.Algo.Commissions.CommissionManager.html):

   ```cs
   _commissionManager.Rules.Add(commissionRule);;
   						
   ```

The commission can be calculated both by the trades and by orders. To calculate the commission by the trade the [Process](../api/StockSharp.Algo.Commissions.CommissionManager.Process.html) method is called, in which as a parameter the [Message](../api/StockSharp.Messages.Message.html) \- a message containing information about the order or own transaction.

The total value of the commission can be found through the [Commission](../api/StockSharp.Algo.Commissions.CommissionManager.Commission.html).
