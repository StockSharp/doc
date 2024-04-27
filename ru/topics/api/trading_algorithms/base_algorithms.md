# Основные алгоритмы

Наравне с [Котирование](../strategies/quoting.md) в [S\#](../../api.md) добавлен класс [TraderHelper](xref:StockSharp.Algo.TraderHelper), в который входят различные методы простых торговых алгоритмов:

1. Обрезать цену через метод [TraderHelper.ShrinkPrice](xref:StockSharp.Algo.TraderHelper.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal,StockSharp.Messages.ShrinkRules))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [System.Decimal](xref:System.Decimal) price, [StockSharp.Messages.ShrinkRules](xref:StockSharp.Messages.ShrinkRules) rule **)**, чтобы она стала кратной шагу цены, и торговая система приняла заявку:

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.ShrinkPrice(13453.65342));
   ```
2. Проверить, является ли текущее время торгуемым (не закончилась ли сессия, не начался ли клиринг) через метод [TraderHelper.IsTradeTime](xref:StockSharp.Algo.TraderHelper.IsTradeTime(StockSharp.Messages.WorkingTime,System.DateTimeOffset,System.Nullable{System.Boolean}@,StockSharp.Messages.WorkingTimePeriod@))**(**[StockSharp.Messages.WorkingTime](xref:StockSharp.Messages.WorkingTime) workingTime, [System.DateTimeOffset](xref:System.DateTimeOffset) time, [System.Nullable\<System.Boolean\>@](xref:System.Nullable`1) isWorkingDay, **out** [StockSharp.Messages.WorkingTimePeriod](xref:StockSharp.Messages.WorkingTimePeriod) period **)**: 

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.Board.IsTradeTime(currentTime));
   ```
3. Остальные методы класса [TraderHelper](xref:StockSharp.Algo.TraderHelper) описываются в разделах [Снятие заявок](../orders_management/order_cancel.md) и [Замена заявок](../orders_management/orders_replacement.md). 

## См. также

[Котирование](../strategies/quoting.md)

[Снятие заявок](../orders_management/order_cancel.md)

[Замена заявок](../orders_management/orders_replacement.md)
