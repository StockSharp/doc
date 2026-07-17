# 图形化配置: 交易技术

在所有 StockSharp 产品中，均通过[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)配置连接。

- `SdkPath` - 本地文件或目录的路径。
- `AppSecretKey` - 身份验证凭据。
- `Environment` - 连接器的运行模式或选项。 默认值: `TradingTechnologiesEnvironments.ProdSim`.
- `InitializationTimeout` - 时间间隔。 默认值: `5000`.
- `MarketDepth` - 连接器的数值参数。 默认值: `20`.
- `IsBinaryProtocol` - 控制连接器行为的开关。 默认值: `true`.
- `IsOptionsEnabled` - 控制连接器行为的开关。 默认值: `true`.

## 另请参阅

[连接器](../../../connectors.md)

[图形化配置](../../graphical_configuration.md)

[保存和加载设置](../../save_and_load_settings.md)

[创建自定义连接器](../../creating_own_connector.md)
