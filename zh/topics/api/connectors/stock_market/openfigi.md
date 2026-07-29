# OpenFIGI

**OpenFIGI** 将 StockSharp 连接到用于全球金融品种标识符搜索的 OpenFIGI API v3。

## 主要功能

- 已根据源代码核实的功能：按交易所、MIC、货币、市场板块和交易品种类型搜索及筛选品种。该适配器返回品种元数据，不提供行情或交易操作。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

可用于将外部标识符映射到 FIGI 记录，并构建规范化的品种目录。

结果覆盖、请求限制以及匿名或密钥访问方式由 OpenFIGI 决定。

## 另请参阅

[连接器配置](openfigi/configuration_openfigi.md)

[图形化配置](openfigi/graphical_configuration_openfigi.md)

[适配器初始化](openfigi/adapter_initialization_openfigi.md)
