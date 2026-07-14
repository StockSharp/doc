# 设置

[Hydra](../../hydra.md) 可以在服务器模式下运行。在此模式下，远程客户端可以连接到 [Hydra](../../hydra.md)，并获取存储中已有的数据。客户端可以连接到服务器模式下的 [Hydra](../../hydra.md)，例如使用 [Designer](../../designer.md)；具体方法请参阅[入门](../../designer/market_data_storage/getting_started.md)，该页面位于 [Designer](../../designer.md) 文档中。也可以连接到 [Hydra](../../hydra.md) 并通过 [API](../../api.md) 访问数据，详情请参阅 [FIX/FAST 连接](fix_fast_connectivity.md)。

服务器模式允许多个程序同时共用 [Hydra](../../hydra.md) 中的一个连接。用户在程序设置中指定访问密钥后，即可通过同一个账户同时使用同一个数据源。

实际上，程序通过 [Hydra](../../hydra.md) 连接数据源，例如 [Designer](../../designer.md) 和 [Terminal](../../terminal.md) 可以同时连接到 Hydra。这样无需在程序之间反复重新连接，也不必购买额外连接。同时，可以避免不同程序注册订单或执行交易时产生冲突。[Hydra](../../hydra.md) 接收请求后，会将结果返回给发出请求的程序，而不会影响其他程序的操作顺序。

要启用 [Hydra](../../hydra.md) 服务器模式，请在程序顶部菜单中选择 **服务器模式** 选项卡。

![Hydra 服务器菜单](../../../images/hydra_server_menu.png)

然后单击 **设置** 按钮，打开服务器模式设置窗口。

![Hydra 服务器](../../../images/hydra_server.png)

**Hydra 服务器**

- **FIX 服务器** \- 将 [Hydra](../../hydra.md) 切换到服务器模式，通过 FIX 协议分发实时交易数据和历史数据。

  在此部分中配置用于访问数据源的连接：
  1. **ConvertToLatin** \- 将西里尔字符转换为拉丁字符。
  2. **QuotesInterval** \- 行情更新周期。
  3. **TransactionSession** \- 交易会话设置，用于通过 [Hydra](../../hydra.md) 进行交易。

     此设置可用于配置 FIX 协议方言、发送方和接收方、数据格式及其他参数。详情请参阅 [FIXServer 属性](https://doc.stocksharp.com/html/Properties_T_StockSharp_Fix_FixServer.htm)。
  4. **MarketDataSession** \- 配置通过 [Hydra](../../hydra.md) 接收的市场数据的传输。详情请参阅 [FIXServer 属性](https://doc.stocksharp.com/html/Properties_T_StockSharp_Fix_FixServer.htm)。
  5. **KeepSubscriptionsOnDisconnect** \- 与数据源断开连接后保留订阅。
  6. **DeadSessionCleanupInterval** \- 连接断开后，经过多长时间清除相关信息。
- **身份验证** \- 访问 Hydra 服务器时使用的身份验证方式。
- **交易品种数量** \- 可从服务器请求的最大交易品种数量。
- **K线（天）** \- 可下载的K线历史数据的最大天数。
- **逐笔成交（天）** \- 可下载的逐笔成交历史数据的最大天数。
- **订单簿（天）** \- 可下载的订单簿历史数据的最大天数。
- **OL（天）** \- 可下载的订单日志数据的最大天数。
- **事务（天）** \- 可下载的事务历史数据的最大天数。
- **模拟器** \- 启用仿真模式。
- **交易品种映射** \- 启用仅传输指定交易品种的模式。

如果将 **身份验证** 设置为 **匿名** 以外的选项，**常规** 选项卡中将显示 **用户** 按钮。单击该按钮后会打开 **用户** 窗口。

![设置 截图](../../../images/hydra_users.png)

可以在窗口左侧添加新用户，并在右侧为用户设置访问权限。
