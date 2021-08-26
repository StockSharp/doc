# Стакан: алгоритмы

Стаканы в [S\#](StockSharpAbout.md) представлены типом данных [MarketDepth](../api/StockSharp.BusinessEntities.MarketDepth.html). С помощью этого типа можно совершать ряд операций над данными в стакане. Например, можно делать разреженными или, наоборот, группировать по ценовым уровням. 

### Модификации стакана

Модификации стакана

1. Создание разреженного стакана из обычного осуществляется через метод [Overload:StockSharp.Algo.TraderHelper.Sparse](../api/Overload:StockSharp.Algo.TraderHelper.Sparse.html): 

   ```cs
   MarketDepth depth = ....;
   var sparseDepth = depth.Sparse();
   ```

   Все котировки [Quote](../api/StockSharp.BusinessEntities.Quote.html) в полученном стакане будут иметь нулевой объем, и они будут созданы с шагом [Security.StepPrice](../api/StockSharp.BusinessEntities.Security.StepPrice.html). 

   Чтобы объединить разреженный стакан с первоначальным (соединить реальные котировки и разреженные), необходимо вызвать метод [TraderHelper.Join](../api/StockSharp.Algo.TraderHelper.Join.html): 

   ```cs
   var joinedDepth = sparseDepth.Join(depth);
   ```
2. Группировка стакана по ценовым уровням осуществляется через метод [TraderHelper.Group](../api/StockSharp.Algo.TraderHelper.Group.html): 

   ```cs
   MarketDepth depth = ....;
   // сгруппировать стакан по ценовому уровню в 10 пунктов
   var grouppedDepth = depth.Group(10.Points(depth.Security));
   ```

   Результатом группировки будет стакан [MarketDepth](../api/StockSharp.BusinessEntities.MarketDepth.html), состоящий из котировок типа [AggregatedQuote](../api/StockSharp.BusinessEntities.AggregatedQuote.html). Через свойство [AggregatedQuote.InnerQuotes](../api/StockSharp.BusinessEntities.AggregatedQuote.InnerQuotes.html) можно получить реальные котировки стакана, на основе которых произошла группировка по ценовому уровню. 

### Проверка данных

Проверка данных

Иногда требуется проверять данные в стакане для обнаружения в них коллизий. Например, проверка загруженных стаканов из внешних источников, или отслеживание корректности работы биржи в период аномальной работы (кризис, стоп торги). Для этого можно использовать специальный метод [MarketDepth.Verify](../api/StockSharp.BusinessEntities.MarketDepth.Verify.html), который проверяет, не перемешаны ли между собой биды и оффера. 

## См. также
