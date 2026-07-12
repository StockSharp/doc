# 交易品种标识符

[S#](../../api.md) 中来自不同来源的交易品种具有 [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) 统一标识符。这样做的目的是使交易算法代码不依赖于连接类型（[OpenECry](../connectors/stock_market/openecry.md)、[Rithmic](../connectors/stock_market/rithmic.md)、[Interactive Brokers](../connectors/stock_market/interactive_brokers.md) 等）。对于交易品种标识符，使用以下语法 - **[交易品种代码]@[交易板代码]**。例如，苹果公司股票的标识符将是 **AAPL@NASDAQ**。对于衍生品市场交易品种，交易板为 **NYSE**（或 **AAPL** 期货交易的其他板名称）。例如，对于 ES 指数六月份期货，其标识符将是 **ESM5@NYSE**。

> [!TIP]
> [Hydra](../../hydra.md) 市场数据下载应用程序使用相同机制枚举具有历史记录的文件夹。

## 标识符生成算法重写

1. 要在自己的算法中生成交易品种标识符，您必须创建 [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) 类的子类，并重写 [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)** 方法：

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, ExchangeBoard board)
      {
         // 将以 CODE--BOARD 形式生成
         return secCode + "--" + board.Code;
      }
   }
   ```

2. 然后，创建的生成器必须传递给连接器：

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
