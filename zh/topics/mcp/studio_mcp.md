# 程序命令 (Studio.Mcp)

**Studio.Mcp** 是一个 MCP 服务器，代理可以通过它向正在运行的 StockSharp 程序发出命令：在 Terminal 中提交和撤销订单，管理 Hydra 的数据源，在 Designer 和 Shell 中启动策略测试。服务器不会单击按钮，也不会读取窗口，命令由程序本身执行。

服务器不启动任何程序，也不保存任何内容。只有在程序已打开时，命令才会送达程序；如果程序未运行，服务器会如实报告。

## 哪些程序会响应

| 程序 | 接受哪些命令 |
|---|---|
| [Terminal](../terminal.md) | 订单列表、提交和撤销订单 |
| [Hydra](../hydra.md) | 数据源类型、数据源列表，以及添加、启用、启动、停止和删除数据源 |
| [Designer](../designer.md) | 策略列表、启动和停止历史测试 |
| [Shell](../shell.md) | [模拟](../shell/user_interface/emulation.md)区域的策略列表，以及启动和停止这些策略的运行 |

只有这些程序的 Avalonia 版本接受命令，而且使用普通版本即可，不需要单独编译。程序在打开主窗口时打开命令通道，并在窗口关闭时将其关闭。Terminal、Hydra 和 Designer 的 WPF 版本不接受这些命令。

通道是本地命名管道，只有当前操作系统用户的进程可以访问。命令不会通过网络传输。

## 安装

服务器从 StockSharp 源代码编译：

```sh
dotnet build Studio/Studio.Mcp/Studio.Mcp.csproj -c Release
```

编译结果为 `Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe`。服务器没有自己的设置：不需要任何参数或环境变量。服务器将协议写入标准输出，将日志写入标准错误流；在没有客户端的情况下手动启动时，它只会等待输入。

## 连接代理

Claude Code — 项目的 `.mcp.json` 文件：

```json
{
  "mcpServers": {
    "stocksharp": {
      "command": "Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe"
    }
  }
}
```

在其他 MCP 客户端中（包括 Claude Desktop），连接服务器的方式相同：命令为 `StockSharp.Studio.Mcp.exe` 的路径，不需要参数和环境变量。

## 代理可以做什么

数据源、策略和订单的标识符取自列表类工具的响应：先获取列表，再执行操作。

### 通用

| 工具 | 作用 |
|---|---|
| `list_products` | 哪些程序正在运行，以及每个程序接受哪些命令 |
| `describe_product` | 单个程序的命令；`product` 为 `Terminal`、`Hydra`、`Designer` 或 `Shell` |
| `close_product` | 请求程序关闭 |

### Hydra

| 工具 | 作用 |
|---|---|
| `hydra_list_source_types` | Hydra 可以创建的数据源类型及其标识符 |
| `hydra_list_sources` | Hydra 的数据源：每个数据源是否已启用，以及当前是否正在下载 |
| `hydra_add_source` (`typeId`) | 添加所选类型的数据源并将其返回；新数据源处于禁用状态 |
| `hydra_enable_source` (`sourceId`, `isEnabled`) | 将数据源加入下载或从下载中排除 |
| `hydra_start_source` (`sourceId`) | 启动数据源的下载；已禁用的数据源不会启动 |
| `hydra_stop_source` (`sourceId`) | 停止下载 |
| `hydra_remove_source` (`sourceId`) | 删除数据源及其设置；正在运行的数据源需要先停止 |

要使用新数据源下载数据，需要三次调用：`hydra_add_source`、`hydra_enable_source`、`hydra_start_source`。这些命令不会更改数据源的设置（交易品种、数据类型、连接参数），这些设置需要在 Hydra 窗口中指定（参阅[数据源](../hydra/data_sources.md)）。

### Designer

| 工具 | 作用 |
|---|---|
| `designer_list_strategies` | Designer 的策略：类型（`Backtest`、`Optimization`、`Live`、`Remote`、`Other`）和当前状态 |
| `designer_start_backtest` (`strategyId`, `startTime`, `stopTime`) | 在历史数据上启动 `Backtest` 或 `Optimization` 类型的策略；时间段以 ISO 8601 格式的 UTC 时间指定，并替换策略设置中的时间段；未指定时使用设置中的时间段 |
| `designer_stop_backtest` (`strategyId`) | 停止正在运行的策略 |

### Terminal

| 工具 | 作用 |
|---|---|
| `terminal_list_orders` | Terminal 中的订单及其事务编号 |
| `terminal_register_order` (`securityId`, `portfolioName`, `side`, `volume`, `price`, `orderType`) | 提交订单并返回其事务编号 |
| `terminal_cancel_order` (`orderTransactionId`) | 按事务编号撤销订单 |

交易品种按 Terminal 已知的形式指定，例如 `SBER@TQBR`；投资组合按 Terminal 已知的名称指定；`side` 为 `Buy` 或 `Sell`。限价订单需要价格；其他类型的订单不使用传入的价格。订单由 `terminal_register_order` 或 `terminal_list_orders` 返回的事务编号标识，而不是由交易所稍后分配的编号标识。

### Shell

Shell 的工具由服务器根据程序本身发送的命令描述生成。因此，只有在服务器启动时或最近一次调用 `list_products` 或 `describe_product` 时 Shell 正在运行，这些工具才会存在。

| 工具 | 作用 |
|---|---|
| `shell_list_strategies` | “模拟”区域的策略及其状态 |
| `shell_start_backtest` (`StrategyId`, `StartTime`, `StopTime`) | 在历史数据上启动策略运行 |
| `shell_stop_backtest` (`StrategyId`) | 停止运行 |

Shell 始终使用策略设置中的时间段测试策略：如果传入 `StartTime` 或 `StopTime`，命令会被拒绝，因此请为它们传入 `null`。

## 限制和安全

> [!CAUTION]
> `terminal_register_order` 会向 Terminal 所连接的地方提交真实订单。服务器不会再次确认：确认由 MCP 客户端负责，用户可以在调用执行之前在客户端中看到该调用。

- **不会启动进行真实交易的策略。** Designer 只启动 `Backtest` 和 `Optimization` 类型的策略；对于其他类型，命令会被拒绝。Shell 只对命令开放“模拟”区域的策略。
- **删除不可撤销。** `hydra_remove_source` 会删除数据源及其设置。
- **关闭。** `close_product` 会关闭程序。
- **程序必须正在运行。** 否则命令会返回错误 `<程序> is not running.`。等待未运行程序的响应大约需要 5 秒，因此在程序关闭时，`list_products` 不会立即响应。正在运行的程序拒绝命令时，代理会收到文本：`<程序> refused the command: <原因>`。
- **没有密码。** 同一用户账户下运行的任何程序都可以使用该通道：不会单独检查命令的发送者。
- **不区分程序副本。** 如果运行了多个 Terminal 或 Designer 副本，或者在普通 Hydra 旁边运行着使用测试配置文件的 Hydra，命令会发送到其中一个副本，而且事先无法知道是哪一个。
- **不返回测试结果。** 服务器只返回策略的状态，不返回统计信息和成交。可以使用 [DesktopDriver](desktop_driver.md) 从程序面板中读取这些内容。

## 会话示例

用户请求：“在 Designer 中启动策略一月份的测试，并检查测试是否正在进行”。

1. `list_products` — Designer 正在运行；其他程序返回 `… is not running.`
2. `designer_list_strategies` — 策略中有所需的策略，类型为 `Backtest`，状态为 `Stopped`。
3. `designer_start_backtest`，使用第 2 步中的 `strategyId`，`startTime` = `2026-01-01T00:00:00Z`，`stopTime` = `2026-02-01T00:00:00Z` — 响应为 `Strategy <标识符> was started.`
4. `designer_list_strategies` — 策略状态已改变。
5. 如果需要中断测试，使用相同的 `strategyId` 调用 `designer_stop_backtest`。

## 另请参阅

[连接 AI 代理 (MCP)](../mcp.md)

[界面控制 (DesktopDriver)](desktop_driver.md)

[远程控制](../shell/remote_control.md)
