# Hydra

![hydra main](../images/hydra_main.png)

**Hydra** 用于从不同数据源自动下载市场数据（包括交易品种、蜡烛图、逐笔成交和订单簿）并将其保存在本地。数据可以使用两种格式存储：一种是 **Hydra** 专用的二进制格式（BIN），可获得最高压缩率；另一种是纯文本 CSV 格式，便于在其他程序中分析数据。保存后的数据可供交易策略使用（有关策略测试的详细信息，请参阅[回测](api/testing.md)）。可以直接通过 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) 访问数据（详见[市场数据存储](api/market_data_storage.md)），也可以将数据导出为 [Excel](https://en.wikipedia.org/wiki/Excel)、XML、TXT 等常用格式（详见[安装与使用](hydra/installing_hydra.md)）。

同时，**Hydra** 既可以使用历史数据源，也可以使用实时数据源。例如，它可以连接 [OpenECry](api/connectors/stock_market/openecry.md) 或 [Rithmic](api/connectors/stock_market/rithmic.md) 来获取订单簿。这得益于其可扩展的插件式数据源模型。

借助插件模型，您还可以开发自己的数据源。有关如何为 **Hydra** 创建和安装自定义数据源，请参阅[创建数据源](hydra/create_new_source.md)。
