# Order book: algorithms

Order books in the [S\#](StockSharpAbout.md) are presented by the [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) data type. A number of operations on the data in the order book can be performed with this type. For example, you can sparse or, vice versa, groupe the data by price levels. 

## Order book modifications

1. The creating of sparse order book from the normal one is carried out through the [TraderHelper.Sparse](xref:StockSharp.Algo.TraderHelper.Sparse(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) depth, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) priceStep**)** method: 

   ```cs
   MarketDepth depth = ....;
   var sparseDepth = depth.Sparse();
   ```

   All the [Quote](xref:StockSharp.BusinessEntities.Quote) quotations in the resulting order book will have zero volume, and they will created with the [Security.StepPrice](xref:StockSharp.BusinessEntities.Security.StepPrice) step. 

   To combine the sparse order book with the original one (to connect the real and sparse quotations), you must call the [TraderHelper.Join](xref:StockSharp.Algo.TraderHelper.Join(StockSharp.BusinessEntities.MarketDepth,StockSharp.BusinessEntities.MarketDepth))**(**[StockSharp.BusinessEntities.MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) original, [StockSharp.BusinessEntities.MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) rare**)** method: 

   ```cs
   var joinedDepth = sparseDepth.Join(depth);
   ```
2. The grouping of the order book by price levels is carried out through the [TraderHelper.Group](xref:StockSharp.Algo.TraderHelper.Group(StockSharp.BusinessEntities.MarketDepth,StockSharp.Messages.Unit))**(**[StockSharp.BusinessEntities.MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) depth, [StockSharp.Messages.Unit](xref:StockSharp.Messages.Unit) priceRange**)** method: 

   ```cs
   MarketDepth depth = ....;
   // grouping of the order book by 10 points price levels
   var grouppedDepth = depth.Group(10.Points(depth.Security));
   ```

   The result of the grouping will be the [MarketDepth](xref:StockSharp.BusinessEntities.MarketDepth) order book, consisting of quotations of the [AggregatedQuote](xref:StockSharp.BusinessEntities.AggregatedQuote) type. Through the [AggregatedQuote.InnerQuotes](xref:StockSharp.BusinessEntities.AggregatedQuote.InnerQuotes) property the real order book quotations can get, on which the grouping by price level was done. 
