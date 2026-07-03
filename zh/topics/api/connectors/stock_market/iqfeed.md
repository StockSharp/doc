# IQFeed

**DTN IQFeed** - 提供股票报价、外汇、新闻、期货合约等的实时市场数据。

在开始为当前交易平台编写交易机器人之前，建议阅读[连接器](../../connectors.md)中的链接。

## 配置 IQFeed

交互机制如图所示：

![IQFeed](../../../../images/iqfeed.jpg)

要使用 **IQFeed** 连接器，需要在计算机上安装 **IQ Feed Client** 路由器，它可以安装在本地计算机或远程计算机上。客户端应用程序与 **IQ Feed Client** 之间，以及 **IQ Feed Client** 与服务器之间的数据交换通过 TCP/IP 协议进行。

要从 [IQFeed](https://www.iqfeed.net/stocksharp/) 网站下载 **IQ Feed Client**，必须先使用从 **iQFeed** 获取的用户名和密码进行授权。

安装 **IQ Feed Client** 后，建议重启计算机。

安装完成后，必须启动 **IQ Feed Client, IQLink Launcher**。

![iQFeedIQLinkLauncher](../../../../images/iqfeediqlinklauncher.png)

在打开的 **IQLink Launcher** 窗口中，点击 **启动 IQLink**。

![iQFeedIQConnectLogin](../../../../images/iqfeediqconnectlogin.png)

在打开的 **IQ Connect 登录** 窗口中，输入从 **iQFeed** 服务获取的 **登录名** 和 **密码**（或 PIN）。这些凭据不同于 **iQFeed** 网站上的登录名和密码。输入凭据后，点击 **连接**。

为了接收数据，客户端应用程序通过不同端口使用四个连接：

1. Level1（端口5009）用于获取交易品种的实时数据（行情、开盘价和收盘价、波动性等）以及新闻。
2. Level2（端口 9200）用于获取交易品种的扩展报价；对于每个 ECN，都可以获取最佳报价对。
3. 查找（端口 9100）用于搜索交易品种、检索历史数据、获取新闻的高级信息。
4. 管理员（端口 9300）用于获取连接的一般信息和更改设置。

默认用于连接 **IQ Feed Client** 的端口号在括号中给出。对于客户端连接，可以在注册表中更改端口号，例如 Level1 的端口号可在以下路径中更改：\[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]。连接 IQ 服务器的端口号不可更改。

> [!CAUTION]
> 连接器仅支持市场数据馈送，不支持交易。

## 推荐内容

[连接器](../../connectors.md)

[图形化配置](../graphical_configuration.md)

[保存和加载设置](../save_and_load_settings.md)

[创建自己的连接器](../creating_own_connector.md)

[订单管理](../../orders_management.md)

[创建新订单](../../orders_management/create_new_order.md)

[创建新止损订单](../../orders_management/create_new_stop_order.md)

[IQFeed 连接](iqfeed/connection_iqfeed.md)

[IQFeed 适配器初始化](iqfeed/adapter_initialization_iqfeed.md)
