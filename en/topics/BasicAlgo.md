# Base algorithms

Along with [Quoting](StrategyQuoting.md), the [S\#](StockSharpAbout.md) contains the [TraderHelper](xref:StockSharp.Algo.TraderHelper) class, which includes a variety of simple trading algorithms methods:

1. To adjust the price through the [TraderHelper.ShrinkPrice](xref:StockSharp.Algo.TraderHelper.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal,StockSharp.Messages.ShrinkRules))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Decimal](xref:System.Decimal) price, [StockSharp.Messages.ShrinkRules](xref:StockSharp.Messages.ShrinkRules) rule**)** method, so it become a multiple of price increment and trading system accepts the order:

   ```cs
   // the sample security
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.ShrinkPrice(13453.65342));
   ```
2. To check whether the current time traded (is session closed? is clearing started?) through the [TraderHelper.IsTradeTime](xref:StockSharp.Algo.TraderHelper.IsTradeTime(StockSharp.Messages.WorkingTime,System.DateTime,System.Nullable{System.Boolean}@,StockSharp.Messages.WorkingTimePeriod@))**(**[StockSharp.Messages.WorkingTime](xref:StockSharp.Messages.WorkingTime) workingTime, [System.DateTime](xref:System.DateTime) time, [System.Nullable\<System.Boolean\>@](xref:System.Nullable`1) isWorkingDay, **out** [StockSharp.Messages.WorkingTimePeriod](xref:StockSharp.Messages.WorkingTimePeriod) period**)** method: 

   ```cs
   // the sample security
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.Board.IsTradeTime(currentTime));
   ```
3. The rest of the [TraderHelper](xref:StockSharp.Algo.TraderHelper) class methods are described in the [Order cancel](OrdersCancel.md) and [Order replace](OrdersReRegister.md) sections. 

## Recommended content

[Quoting](StrategyQuoting.md)

[Order cancel](OrdersCancel.md)

[Order replace](OrdersReRegister.md)
