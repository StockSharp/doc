# Основные алгоритмы

Наравне с [Котирование](StrategyQuoting.md) в [S\#](StockSharpAbout.md) добавлен класс [TraderHelper](xref:StockSharp.Algo.TraderHelper), в который входят различные методы простых торговых алгоритмов:

1. Обрезать цену через метод [ShrinkPrice](xref:StockSharp.Algo.TraderHelper.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal,StockSharp.Messages.ShrinkRules)), чтобы она стала кратной шагу цены, и торговая система приняла заявку:

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.ShrinkPrice(13453.65342));
   ```
2. Проверить, является ли текущее время торгуемым (не закончилась ли сессия, не начался ли клиринг) через метод [IsTradeTime](xref:StockSharp.Algo.TraderHelper.IsTradeTime(StockSharp.Messages.WorkingTime,System.DateTime,System.Nullable{System.Boolean}@,StockSharp.Messages.WorkingTimePeriod@)): 

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.Board.IsTradeTime(currentTime));
   ```
3. Остальные методы класса [TraderHelper](xref:StockSharp.Algo.TraderHelper) описываются в разделах [Снятие заявок](OrdersCancel.md) и [Замена заявок](OrdersReRegister.md). 

## См. также

[Котирование](StrategyQuoting.md)

[Снятие заявок](OrdersCancel.md)

[Замена заявок](OrdersReRegister.md)
