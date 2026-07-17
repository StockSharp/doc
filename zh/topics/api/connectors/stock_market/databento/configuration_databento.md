# 连接器配置: Databento

从服务提供商获取身份验证凭据，并指定连接参数。

- `Key` - 身份验证凭据。
- `Dataset` - 连接参数。 默认值: `GLBX.MDP3`.
- `LiveAddress` - 服务地址。
- `HistoricalAddress` - 服务地址。 默认值: `https://hist.databento.com/v0/timeseries.get_range`.
- `Symbology` - 连接器的运行模式或选项。 默认值: `DatabentoSymbologyTypes.RawSymbol`.
