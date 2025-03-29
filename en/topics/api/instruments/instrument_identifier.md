# Instrument identifier

The instruments in the [S\#](../../api.md) from different sources have the [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) uniform identifier. This is done in order to the code of trading algorithm does not depend on the type of connection ([OpenECry](../connectors/stock_market/openecry.md), [Rithmic](../connectors/stock_market/rithmic.md), [Interactive Brokers](../connectors/stock_market/interactive_brokers.md) etc.). For the instrument identifier the following syntax used \- **\[instrument code\]@\[board code\]**. For example, for the Apple inc shares identifier will be **AAPL@NASDAQ**. For the derivatives market instruments board will be **NYSE** (or other board name where **AAPL** futures trades). For example, for the June futures on the ES index the identifier will be **ESM5@NYSE**. 

> [!TIP]
> The [Hydra](../../hydra.md) application for market data download enumerates folders with a history based on the same mechanism. 

## Identifiers generation algorithm overriding

1. To start the instrument identifiers generation on the own algorithm, you must create the descendant of the [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) class, and override the [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)** method: 

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, ExchangeBoard board)
      {
         // will be generate in CODE--BOARD form
         return secCode + "--" + board.Code;
      }
   }
   ```

2. Then, the created generator must be passed to the connector: 

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
