# Continuous Futures

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) is a continuous instrument, usually a futures contract, that contains instruments subject to expiration.

For example, consider two E-mini S&P 500 futures: **ESM5** and **ESU5**. When **ESM5** expires, the continuous instrument automatically switches to the next contract, **ESU5**.

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) can be traded in the same way as [Security](xref:StockSharp.BusinessEntities.Security). Before **ESM5** expires, trading is performed through that instrument. After expiration, trading is performed through **ESU5**, and so on.

## Creating ExpirationContinuousSecurity

1. Declare the component instruments that will be included in [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity), and declare the [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) itself:

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ExpirationContinuousSecurity _es;
   							
   ```
2. Create the [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity):

   ```cs
   _es = new ExpirationContinuousSecurity { Board = ExchangeBoard.Cme, Id = "ES" };
   							
   ```
3. Add component instruments and specify the expiration date and time for each one:

   ```cs
   _es.ExpirationJumps.Add(_esm5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

In addition to [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity), StockSharp also provides [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity). This type of continuous instrument switches between contracts based on trading volume instead of expiration date. The transition to the next contract happens when the new contract's trading volume exceeds the current contract's volume.
