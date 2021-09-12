# Commission

The [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager) manager is used to calculate commissions in the trading algorithm.

The tariff plan is created by adding the relevant [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) rules, based on which commissions will be calculated further.

## The CommissionManager creating

1. To create the [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager):

   ```cs
   private CommissionManager _commissionManager = new CommissionManager();
   						
   ```
2. Then, you must create the rule:

   ```cs
    CommissionRule commissionRule =  new CommissionPerTradeRule {  Value = new Unit(1m) };
   						
   ```
3. And add it to the [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager):

   ```cs
   _commissionManager.Rules.Add(commissionRule);;
   						
   ```

The commission can be calculated both by the trades and by orders. To calculate the commission by the trade the [CommissionManager.Process](xref:StockSharp.Algo.Commissions.CommissionManager.Process(StockSharp.Messages.Message))**(**[StockSharp.Messages.Message](xref:StockSharp.Messages.Message) message**)** method is called, in which as a parameter the [Message](xref:StockSharp.Messages.Message) \- a message containing information about the order or own transaction.

The total value of the commission can be found through the [CommissionManager.Commission](xref:StockSharp.Algo.Commissions.CommissionManager.Commission).
