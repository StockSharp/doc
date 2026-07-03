# Continuous Futures

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) 是一种连续交易品种，通常是期货合约，内部包含会到期的交易品种。

例如，考虑两个 E-mini S&P 500 期货合约：**ESM5** 和 **ESU5**。当 **ESM5** 到期时，连续交易品种会自动切换到下一个合约 **ESU5**。

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) 可以像 [Security](xref:StockSharp.BusinessEntities.Security) 一样交易。在 **ESM5** 到期前，交易通过该合约执行；到期后，交易通过 **ESU5** 执行，依此类推。

## 创建 ExpirationContinuousSecurity

1. 声明将包含在 [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) 中的组成交易品种，并声明 [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) 本身：

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ExpirationContinuousSecurity _es;
   							
   ```
2. 创建 [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity)：

   ```cs
   _es = new ExpirationContinuousSecurity { Board = ExchangeBoard.Cme, Id = "ES" };
   							
   ```
3. 添加组成交易品种，并为每个交易品种指定到期日期和时间：

   ```cs
   _es.ExpirationJumps.Add(_esm5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

除 [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) 外，StockSharp 还提供 [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity)。这种连续交易品种根据交易量而不是到期日切换合约。当新合约的交易量超过当前合约的交易量时，会切换到下一个合约。
