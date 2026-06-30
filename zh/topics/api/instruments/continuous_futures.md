# 连续期货

[ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) - 连续合约（通常为期货），包含受到到期（活动期结束）影响的合约。

例如，两个 ES 指数期货 - **ESM5** 和 **ESU5**。当 **ESM5** 到期时，它会自动切换到下一个合约 - **ESU5**。

[ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) 可以像 [Security](xref:StockSharp.BusinessEntities.Security) 一样进行交易。在 **ESM5** 到期之前，算法将与该合约一起执行，到期后算法将与 **ESU5** 一起执行，依此类推。

## 创建连续安全

1. 声明将包含在 [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) 和 [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) 本身的复合工具：

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ContinuousSecurity _es;
   							
   ```
2. 要创建 [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity):

   ```cs
   _es = new ContinuousSecurity { ExchangeBoard = ExchangeBoard.Nyse, Id = "ES" };
   							
   ```
3. 要将复合工具添加到其中，请为每个添加的工具指定到期的日期和时间：

   ```cs
   _es.ExpirationJumps.Add(_esm5, new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5, new DateTime(2015, 9, 15, 18, 45, 00));
   							
   ```
