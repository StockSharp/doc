# 连接 AI 代理 (MCP)

**MCP**（Model Context Protocol）是一种开放协议，AI 代理（Claude Code、Claude Desktop 或任何其他 MCP 客户端）通过它调用外部程序（即 MCP 服务器）的工具。客户端自行将服务器作为子进程启动，并通过标准输入和输出与其交换消息。服务器会说明它提供哪些工具、每个工具的作用以及接受哪些参数，代理则决定调用哪个工具并接收响应。

StockSharp 程序有两个 MCP 服务器。两者都只在程序所在的计算机上运行，并且只与同一用户账户下的程序通信。

| | [DesktopDriver](mcp/desktop_driver.md) | [Studio.Mcp](mcp/studio_mcp.md) |
|---|---|---|
| 作用 | 读取程序界面、单击按钮、输入文本、滚动、截图 | 向程序发出命令：提交或撤销订单、启动 Hydra 数据源、启动策略测试 |
| 适用的程序 | Terminal、Hydra、Designer 和 Installer（Avalonia 和 WPF 版本）、Shell 和 UDP Dumper，仅限启用自动化功能编译的版本 | Terminal、Hydra、Designer 和 Shell 的 Avalonia 版本，普通版本即可 |
| 代理能看到的内容 | 窗口、面板、表格、图表、订单簿、属性编辑器、策略图 | 只有命令返回的内容：数据源、策略和订单的列表 |
| 操作限制 | 只有使用测试配置文件启动的程序才接受单击和输入（UDP Dumper 除外，它始终接受） | 没有配置文件限制：订单会发送到 Terminal 所连接的地方 |
| 获取服务器的途径 | NuGet 包 `StockSharp.DesktopDriver.Mcp`、Claude Desktop 扩展 | 从源代码编译 |

## 选择哪个服务器

- 需要检查窗口显示的内容、像人一样通过按钮完成某个场景，或者获取面板截图 — [DesktopDriver](mcp/desktop_driver.md)。
- 需要程序按命令执行操作而无需单击：提交订单、启动 Hydra 数据源下载、启动策略测试 — [Studio.Mcp](mcp/studio_mcp.md)。
- 程序以普通方式编译，未包含自动化功能 — 只能使用 Studio.Mcp，且仅限程序的 Avalonia 版本。
- 程序的 WPF 版本 — 只能使用 DesktopDriver，且仅限带自动化功能的版本。

两个服务器可以同时连接到代理。例如，Studio.Mcp 可以在 Designer 中启动策略测试，但不返回结果；如果 Designer 是带自动化功能编译的，代理可以通过 DesktopDriver 从 Designer 的面板中读取结果。

MCP 不同于通过网站和 [Telegram 机器人](telegram_services/control_panel.md)进行的[远程控制](shell/remote_control.md)。远程控制通过 StockSharp 账户工作，既不需要代理，也不需要 MCP 服务器。

## 另请参阅

[界面控制 (DesktopDriver)](mcp/desktop_driver.md)

[程序命令 (Studio.Mcp)](mcp/studio_mcp.md)

[AI 辅助开发](api/ai_development.md)
