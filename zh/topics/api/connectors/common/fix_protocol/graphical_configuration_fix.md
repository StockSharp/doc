# FIX 图形化配置

对于所有 [S#](../../../../api.md) 产品，连接的图形化配置都在 [连接设置窗口](../../../graphical_user_interface/connection_settings_window.md) 中进行：

![API GUI Settings FIX](../../../../../images/api_gui_settings_fix.png)

- **Address** - 地址。
- **Dialect** - FIX 协议方言。
- **Sender** - 发送方标识符。
- **Target** - 接收方标识符。
- **Login** - 登录名。
- **Password** - 密码。
- **Portfolios** - 启动时请求所有投资组合。
- **Instruments** - 连接时请求所有金融工具。
- **Encoding** - 数据传输所用的编码。
- **Sequence reset** - 是否重置序号计数器。
- **Date format** - 日期格式。
- **Date and time format** - 日期和时间格式。
- **Time format** - 时间格式。
- **Receive timeout** - 数据接收超时。
- **Send timeout** - 数据发送超时。
- **Unknown transactions** - 处理由第三方生成的未知执行回报。
- **Protocol** - 用于建立连接的 SSL 协议。
- **Certificate** - SSL 证书。
- **Password** - SSL 证书密码。
- **Revocation check** - 证书吊销检查。
- **Check remote** - 检查远程证书。
- **Server name** - 使用 SSL 连接的服务器名称。
- **Reconnection settings** - 用于跟踪与交易系统连接状态的机制设置（[重新连接设置](../../reconnection_settings.md)）。
- **Heartbeat interval** - 向服务器通知连接仍然存活的间隔时间。默认值为 1 分钟。
- **Unified board code** - 统一金融工具的交易板代码。

## 另请参阅

[连接器](../../../connectors.md)

[图形化配置](../../graphical_configuration.md)

[创建自己的连接器](../../creating_own_connector.md)

[保存和加载设置](../../save_and_load_settings.md)
