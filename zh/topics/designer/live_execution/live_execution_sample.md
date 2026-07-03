# 实盘运行示例

要在 **Live** 模式下运行示例，需要完成以下准备：

1. 安装 [Interactive Brokers](../../api/connectors/stock_market/interactive_brokers.md) 的测试终端 **IB Trader Workstation (TWS) Demo**。可从厂商网站获取该终端。

2. 配置 IB TWS Demo，使其能够与 [Designer](../../designer.md) 配合使用。请参阅 [Interactive Brokers](../../api/connectors/stock_market/interactive_brokers.md) 章节中的 **IB TWS Setting demo**。

3. 在 [Designer](../../designer.md) 中配置 IB TWS Demo 连接并建立连接。

4. 下载所需交易品种的历史数据。本例使用 **AAPL@NASDAQ** 交易品种。策略将使用 5 秒周期的K线；虽然并不需要这些历史数据，但下载这些数据足以演示相应功能。

![Designer Example of Live trading 00](../../../images/designer_example_of_live_trading_00.png)

5. 配置并启动策略。

SMA 策略示例使用以下参数：

- 交易品种 **AAPL@NASDAQ**
- 标准存储 **\\Documents\\StockSharp\\Designer\\Storage**
- 存储格式 \- **CSV**
- 从存储中读取的数据类型 \- **Ticks**
- 时间周期为 5 秒的K线
- 成交量 \- 100
- 历史数据天数 \- 2

![Designer Example of Live trading 01](../../../images/designer_example_of_live_trading_01.png)

设置完所有必需参数后，单击 ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) Start 按钮启动策略。

单击 ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) Start 按钮后，图表会开始显示已下载的完整两天历史数据：

![Designer Example of Live trading 02](../../../images/designer_example_of_live_trading_02.png)

从[市场数据存储](../market_data_storage.md)下载全部历史数据，并从终端接收到匿名成交表后，策略将开始交易。

下图显示了 [Designer](../../designer.md) 中的成交：

![Designer Example of Live trading 03](../../../images/designer_example_of_live_trading_03.png)

在 [Designer](../../designer.md) 中，策略继续在 Live 模式下运行，并显示实时交易结果：

![Designer Example of Live trading 04](../../../images/designer_example_of_live_trading_04.png)

## 推荐内容

[市场数据存储](../market_data_storage.md)
