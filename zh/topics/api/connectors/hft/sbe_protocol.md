# SBE 协议

对 **SBE (Simple Binary Encoding)** 的支持为低延迟市场数据和交易消息提供紧凑的二进制传输方式。

StockSharp 提供用于编码和解码记录的 [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer)、用于接受客户端连接的 [SbeServer](xref:StockSharp.Server.Sbe.SbeServer)，以及用于客户端连接的 [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter)。

该实现支持交易品种搜索、Level1、订单簿、逐笔成交、可选的原生 K线、投资组合和持仓数据以及订单操作。客户端与服务器的架构标识符和版本必须一致。

## 另请参阅

[SBE 配置](sbe_protocol/configuration_sbe.md)

[SBE 适配器初始化](sbe_protocol/adapter_initialization_sbe.md)

[FIX 协议](../common/fix_protocol.md)

[FAST 协议](../common/fast_protocol.md)
