# Стакан: алгоритмы

Стаканы в [S\#](StockSharpAbout.md) представлены типом данных [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth). С помощью этого типа можно совершать ряд операций над данными в стакане. Например, можно делать разреженными или, наоборот, группировать по ценовым уровням. 

## Модификации стакана

1. Создание разреженного стакана из обычного осуществляется через метод [Overload:StockSharp.Algo.TraderHelper.Sparse](xref:Overload:StockSharp.Algo.TraderHelper.Sparse): 

   ```cs
   MarketDepth depth = ....;
   var sparseDepth = depth.Sparse();
   ```

   Все котировки [Quote](xref:StockSharp.BusinessEntities.Quote) в полученном стакане будут иметь нулевой объем, и они будут созданы с шагом [Security.StepPrice](xref:StockSharp.BusinessEntities.Security.StepPrice). 

   Чтобы объединить разреженный стакан с первоначальным (соединить реальные котировки и разреженные), необходимо вызвать метод [TraderHelper.Join](xref:StockSharp.Algo.TraderHelper.Join): 

   ```cs
   var joinedDepth = sparseDepth.Join(depth);
   ```
2. Группировка стакана по ценовым уровням осуществляется через метод [TraderHelper.Group](xref:StockSharp.Algo.TraderHelper.Group): 

   ```cs
   MarketDepth depth = ....;
   // сгруппировать стакан по ценовому уровню в 10 пунктов
   var grouppedDepth = depth.Group(10.Points(depth.Security));
   ```

   Результатом группировки будет стакан [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth), состоящий из котировок типа [AggregatedQuote](xref:StockSharp.BusinessEntities.AggregatedQuote). Через свойство [AggregatedQuote.InnerQuotes](xref:StockSharp.BusinessEntities.AggregatedQuote.InnerQuotes) можно получить реальные котировки стакана, на основе которых произошла группировка по ценовому уровню. 

## Проверка данных

Иногда требуется проверять данные в стакане для обнаружения в них коллизий. Например, проверка загруженных стаканов из внешних источников, или отслеживание корректности работы биржи в период аномальной работы (кризис, стоп торги). Для этого можно использовать специальный метод [MarketDepth.Verify](xref:StockSharp.BusinessEntities.MarketDepth.Verify), который проверяет, не перемешаны ли между собой биды и оффера. 
