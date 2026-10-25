# Shell

**Shell** 是一个现成的交易终端图形框架，提供完全开放的 C# 源代码。它基于 [Avalonia](https://avaloniaui.net/) 构建，可在 Windows、Linux 和 macOS 上运行。

您无需再花时间创建 GUI（图形用户界面）：数据源连接，投资组合、交易品种、订单和成交表格，图表，历史测试，参数优化以及运行日志均已内置。只需编写策略本身即可。

![Shell 截图](../images/shell_title_00.png)

## 主要功能

1. 提供完整源代码，既适合定制交易机器人，也适合自己的解决方案。
2. **支持 70 多个连接器，可连接全球不同的交易所和经纪商**，详见[连接器](api/connectors.md)。
3. 四个工作区，通过侧边栏切换：[常规](shell/user_interface/common.md)、[实盘](shell/user_interface/real_time.md)、[模拟](shell/user_interface/emulation.md)和[日志](shell/user_interface/log.md)。
4. 灵活的界面：工作区内的所有面板都可以拖动、拆分、拆离到单独的窗口并重新停靠。布局会在每次启动之间保存。
5. 在历史数据上测试策略，提供统计信息、成交图表和收益曲线。
6. [优化](shell/user_interface/optimization.md) — 在同一历史数据上遍历策略参数。
7. 保存和恢复策略设置，包括风险规则。
8. 同时运行多个策略：每个策略独立运行，拥有自己的订单、成交和统计信息。
9. 无需编写代码即可运行在 [Designer](designer.md) 中构建的策略。
10. [远程控制](shell/remote_control.md) — 通过网站或 Telegram 机器人启动和停止策略。

## 内置策略

Shell 附带四个策略，启动后即可直接运行：

| 策略 | 作用 |
|---|---|
| **SMA 策略** | 短期和长期移动平均线交叉 |
| **SMA Index 策略** | 同上，但针对由表达式定义的合成交易品种 |
| **StairsTrend 策略** | 连续数根K线朝同一方向运行后顺势入场 |
| **StairsCountertrend 策略** | 在相同情况下逆势入场 |

## 推荐内容

[安装 Shell](shell/installing_shell.md)

[快速入门](shell/quick_start.md)
