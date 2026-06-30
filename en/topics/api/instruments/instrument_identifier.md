# Instrument identifier

In [S#](../../api.md), instruments from different sources use a unified [Security.Id](xref:StockSharp.BusinessEntities.Security.Id). This keeps trading algorithm code independent of the connection type, such as [OpenECry](../connectors/stock_market/openecry.md), [Rithmic](../connectors/stock_market/rithmic.md), or [Interactive Brokers](../connectors/stock_market/interactive_brokers.md).

Instrument identifiers use the following syntax: **\[instrument code\]@\[board code\]**. For Apple Inc. shares, the identifier is **AAPL@NASDAQ**. For derivatives, the board code is the board on which the contract is traded. For example, the June futures contract on the ES index can be identified as **ESM5@NYSE**.

> [!TIP]
> [Hydra](../../hydra.md) uses the same mechanism to name folders with historical market data.

## Overriding the identifier generation algorithm

1. To generate instrument identifiers with your own algorithm, create a descendant of the [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) class and override the [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)** method:

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, string boardCode)
      {
         // generate identifiers in CODE--BOARD format
         return secCode + "--" + boardCode;
      }
   }
   ```

2. Pass the created generator to the connector:

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
