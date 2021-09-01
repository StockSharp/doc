# Instrument identifier

The instruments in the [S\#](StockSharpAbout.md) from different sources have the [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) uniform identifier. This is done in order to the code of trading algorithm does not depend on the type of connection ([OpenECry](OEC.md), [Rithmic](Rithmic.md), [Interactive Brokers](IB.md) etc.). For the instrument identifier the following syntax used \- **\[instrument code\]@\[board code\]**. For example, for the Apple inc shares identifier will be **AAPL@NASDAQ**. For the derivatives market instruments board will be **NYSE** (or other board name where **AAPL** futures trades). For example, for the June futures on the ES index the identifier will be **ESM5@NYSE**. 

> [!TIP]
> The [S\#.Data](Hydra.md) application for market data download enumerates folders with a history based on the same mechanism. 

## Identifiers generation algorithm overriding

1. To start the instrument identifiers generation on the own algorithm, you must create the descendant of the [SecurityIdGenerator](xref:StockSharp.Algo.SecurityIdGenerator) class, and override the [SecurityIdGenerator.GenerateId](xref:StockSharp.Algo.SecurityIdGenerator.GenerateId(System.String,StockSharp.BusinessEntities.ExchangeBoard)) method: 

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
   var _connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
