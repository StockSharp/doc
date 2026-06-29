# 盈透证券

**盈透证券（Interactive Brokers）** - 一个用于交易包括股票、期权、期货、EFP、期货期权、外汇、债券和基金在内的金融资产的交易平台。

在开始为当前交易平台编写交易机器人之前，建议先阅读 [连接器](../../connectors.md) 中的链接。

## 配置互动经纪商

要正确使用 `Interactive Brokers`，你应当预先设置 [TWS](https://interactivebrokers.com/en/index.php?f=1537) 交易终端。

### 交易工作站终端配置

1. 您必须允许来自其他程序的连接（例如 [S\#](../../../api.md) 上的交易算法）。为此，请打开设置菜单 "文件 -> 全局配置..."。在新窗口中选择 "配置 -> API -> 设置"：

   ![ib 设置](../../../../images/ib_settings.png)
2. 打开“启用 ActiveX 和 Socket 客户端”模式。
3. 还建议添加将运行算法的计算机的地址（本地地址-127.0.0.1）。这样可以消除每次启动算法时确认终端连接权限的需要。

## 推荐内容

[连接器](../../connectors.md)

[图形配置](../graphical_configuration.md)

[保存和加载设置](../save_and_load_settings.md)

[创建自定义连接器](../creating_own_connector.md)

[订单管理](../../orders_management.md)

[创建新订单](../../orders_management/create_new_order.md)

[创建新止损订单](../../orders_management/create_new_stop_order.md)

[适配器初始化 Interactive Brokers](interactive_brokers/adapter_initialization_interactive_brokers.md)
