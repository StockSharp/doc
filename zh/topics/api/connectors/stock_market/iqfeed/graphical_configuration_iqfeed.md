# IQFeed 图形化配置

对于所有[S#](../../../../api.md) 产品，连接的图形化配置在[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md) 上进行：

![API GUI 设置 IQFeed](../../../../../images/api_gui_settings_iqfeed.png)

- **Level1 服务器** - 用于获取 Level1 数据的地址。
- **Level2 服务器** - 获取 Level2 数据的地址。
- **查找服务器** - 获取历史数据的地址。
- **管理服务器** - 用于获取服务数据的地址。
- **衍生品** - 获取衍生品数据的地址。
- **Level1 的数据** - Level1 的所有类型的数据，需要翻译。
- **数据类型** - 必须接收其数据的交易品种类型。
- **加载交易品种** - 是否应从 IQFeed 网站存档中加载整套交易品种。
- **交易品种文件** - 指向包含IQFeed交易品种列表的文件路径，从网站下载。如果指定了路径，则不会从网站进行二次下载，只解析本地副本。
- **版本** \- 版本。
- **心跳** - 用于跟踪连接是否存活的服务器检查间隔。默认值为1分钟。
- **重新连接设置** - 用于跟踪与交易系统设置连接的机制。([重新连接设置](../../reconnection_settings.md))

## 推荐内容

[连接器](../../../connectors.md)

[图形化配置](../../graphical_configuration.md)

[创建自己的连接器](../../creating_own_connector.md)

[保存和加载设置](../../save_and_load_settings.md)
