# Interactive Brokers 图形化配置

对于所有[S#](../../../../api.md) 产品，连接的图形化配置在[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md) 上进行：

![API GUI 设置互动经纪商](../../../../../images/api_gui_settings_interactivebrokers.png)

- **地址** - TWS 地址。
- **标识符** - 唯一ID。当多个客户端连接到同一终端或网关时使用。
- **实时** - 应该使用经纪服务器上的实时数据还是“冻结”数据。
- **日志级别** - 服务器消息的日志记录级别。
- **市场数据字段** - 市场数据字段，将随订阅的 Level1 消息一起接收。
- **协议** - 用于建立连接的 SSL 协议
- **证书** - SSL 证书。
- **密码** - SSL 证书密码。
- **检查吊销** - 检查证书吊销。
- **验证远程** - 验证远程证书。
- **主机名** - 共享 SSL 连接的服务器名称。
- **最大版本** - MaxVersion
- **心跳** - 用于跟踪连接是否存活的服务器检查间隔。默认值为1分钟。
- **重新连接设置** - 用于跟踪与交易系统设置连接的机制。([重新连接设置](../../reconnection_settings.md))

## 推荐内容

[连接器](../../../connectors.md)

[图形化配置](../../graphical_configuration.md)

[创建自己的连接器](../../creating_own_connector.md)

[保存和加载设置](../../save_and_load_settings.md)
