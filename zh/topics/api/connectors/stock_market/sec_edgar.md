# SEC EDGAR

**SEC EDGAR** 将 StockSharp 连接到 SEC EDGAR 官方数据和申报服务。

## 主要功能

- 已根据源代码核实的功能：上市公司搜索、申报新闻、历史申报文件，以及通过 `SecEdgarDataTypes.CompanyFacts` 提供的结构化公司事实。该适配器不支持交易操作。
- 服务商特有的传输、会话和数据格式均封装在标准 StockSharp API 之后。

## 适用场景

可用于查找 SEC 注册主体，并收集申报文件、相关新闻和 XBRL 公司事实用于研究。

覆盖范围和可用性遵循 SEC EDGAR 数据、公平访问规则及已配置的请求限制。

## 另请参阅

[连接器配置](sec_edgar/configuration_sec_edgar.md)

[图形化配置](sec_edgar/graphical_configuration_sec_edgar.md)

[适配器初始化](sec_edgar/adapter_initialization_sec_edgar.md)
