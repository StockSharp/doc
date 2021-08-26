# Continuous futures

[ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html) \- continuous instrument (typically futures), containing the instruments affected by expiration (expiry of period of activity).

For example, two futures of the ES index \- **ESM5** and **ESU5**. When the **ESM5** expired it automatically switches to the next instrument \- **ESU5**.

[ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html) can be traded in the same way as [Security](../api/StockSharp.BusinessEntities.Security.html). Prior to the **RIM5** expiration an algo will be carried with this instrument. After the expiration an algo will be carried with **RIU5**, etc.

### ContinuousSecurity creating

ContinuousSecurity creating

1. To declare the compound instruments that will be included in the [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html) and in the [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html) itself:

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ContinuousSecurity _es;
   							
   ```
2. To create the [ContinuousSecurity](../api/StockSharp.Algo.ContinuousSecurity.html):

   ```cs
   _es = new ContinuousSecurity { ExchangeBoard = ExchangeBoard.Nyse, Id = "ES" };
   							
   ```
3. To add the compound instruments to it, specified the date and time of expiration for each added instrument:

   ```cs
   _es.ExpirationJumps.Add(_esm5, new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5, new DateTime(2015, 9, 15, 18, 45, 00));
   							
   ```
