# 仪器标识符

[S\#](../../api.md) 中来自不同来源的工具具有 [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) 统一标识符。这样做的目的是使交易算法代码不依赖于连接类型（[OpenECry](../connectors/stock_market/openecry.md)、[Rithmic](../connectors/stock_market/rithmic.md)、[Interactive Brokers](../connectors/stock_market/interactive_brokers.md) 等）。对于工具标识符，使用以下语法 - **[instrument code]@[board code]**。例如，苹果公司股票的标识符将是 **AAPL@NASDAQ**。对于衍生品市场工具，交易板为 **NYSE**（或 **AAPL** 期货交易的其他板名称）。例如，对于 ES 指数六月份期货，其标识符将是 **ESM5@NYSE**。

> [!TIP]
> [Hydra](../../hydra.md) 市场数据下载应用程序使用相同机制枚举具有历史记录的文件夹。

## 标识符生成算法重写

1. 要在自己的算法上启动工具标识符生成，您必须创建 [SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) 类的子类，并重写 [SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String)**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)** 方法：

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

2. 然后，创建的生成器必须传递给连接器：

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
