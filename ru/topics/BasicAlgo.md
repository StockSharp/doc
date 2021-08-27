# Основные алгоритмы

Наравне с [Котирование](StrategyQuoting.md) в [S\#](StockSharpAbout.md) добавлен класс [TraderHelper](xref:StockSharp.Algo.TraderHelper), в который входят различные методы простых торговых алгоритмов:

1. Очистить стакан от собственных заявок через метод [GetFilteredQuotes](xref:StockSharp.Algo.TraderHelper.GetFilteredQuotes) (чтобы выставлять заявки относительно других участников рынка, и предотвратить борьбу своих роботов друг с другом):

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   var someOrders = new List<Order>();
   // заполняем коллекцию собственными заявками
   // вычисляем истинно-лучшую цену на покупку
   Console.WriteLine(_connector.GetMarketDepth(someSecurity).GetFilteredQuotes(Sides.Buy, someOrders, null).Max(q => q.Price));
   ```
2. Обрезать цену через метод [ShrinkPrice](xref:StockSharp.Algo.TraderHelper.ShrinkPrice), чтобы она стала кратной шагу цены, и торговая система приняла заявку:

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.ShrinkPrice(13453.65342));
   ```
3. Получить позицию по совершенным сделкам через метод [GetPosition](xref:Overload:StockSharp.Algo.TraderHelper.GetPosition):

   ```cs
   Console.WriteLine(_connector.GetPosition(Portfolio,Security, clientCode, depoName);
   ```
4. Проверить, является ли текущее время торгуемым (не закончилась ли сессия, не начался ли клиринг) через метод [IsTradeTime](xref:Overload:StockSharp.Algo.TraderHelper.IsTradeTime): 

   ```cs
   // любой инструмент
   var someSecurity = _connector.Securities.First();
   Console.WriteLine(someSecurity.Board.IsTradeTime(currentTime));
   ```
5. Остальные методы класса [TraderHelper](xref:StockSharp.Algo.TraderHelper) описываются в разделах [Снятие заявок](OrdersCancel.md) и [Замена заявок](OrdersReRegister.md). 

## См. также

[Котирование](StrategyQuoting.md)

[Снятие заявок](OrdersCancel.md)

[Замена заявок](OrdersReRegister.md)
