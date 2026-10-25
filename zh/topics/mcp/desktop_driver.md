# 界面控制 (DesktopDriver)

**DesktopDriver** 是一个 MCP 服务器，代理可以通过它像人一样操作正在运行的程序的窗口：查找面板和按钮，读取表格、图表、订单簿和属性编辑器，执行单击、输入文本、滚动和截图。

服务器读取的是界面本身，而不是屏幕图像。表格以其列和行作答，图表以其实际绘制的点作答，订单簿以价格档位作答。返回结果与程序的主题、字体和语言无关。截图是给人看的，而检查内容应通过读取来完成。

服务器以 NuGet 包 `StockSharp.DesktopDriver.Mcp` 的形式分发，自称 `StockSharp.DesktopDriver`；在 MCP Registry 中登记为 `io.github.StockSharp/desktop-driver`。源代码位于 [DesktopDriver.Mcp](https://github.com/StockSharp/DesktopDriver.Mcp) 仓库中。

## 工作原理

1. 驱动程序在程序编译时被包含到程序中。普通版本中没有驱动程序。
2. 包含驱动程序的程序使用 `--ui-automation` 参数启动时，会打开一个本地通道（仅当前操作系统用户可以访问的命名管道），并将其地址写入由 `--ui-automation-endpoint=<路径>` 参数指定的文件。该文件在窗口已经构建并绘制完成后才会出现。如果没有 `--ui-automation` 参数，程序不会打开任何通道。
3. MCP 服务器使用这些参数从其清单中启动程序，或者连接到由他人启动的程序。连接时，服务器会检查响应的是否为目标程序、目标副本以及兼容的协议版本，确认后才执行代理的调用。

同样的操作也可以通过命令行完成，即使用 `StockSharp.DesktopDriver.Cli` 包中的 `desktop-driver` 程序。

## 可以连接哪些版本

只能连接到启用自动化功能编译的程序，即使用 `EnableUiAutomation=true` 属性编译的程序。该属性默认关闭，普通版本中既没有驱动程序，也没有通道。普通版本使用 `--ui-automation` 启动时，会向标准错误流写入消息 `<标识符> was built without automation, so --ui-automation does nothing. Build it with EnableUiAutomation=true.`，然后照常运行。MCP 服务器会等待此类程序的通道 90 秒，之后报告程序未能启动并将其关闭。

带自动化功能的版本需要从程序源代码编译，源代码旁边必须有 DesktopDriver.Mcp 仓库。该版本输出到单独的 `uiauto\bin` 文件夹，以免与普通版本混在一起：

```sh
dotnet build Terminal/Avalonia/Terminal.Avalonia/Terminal.Avalonia.csproj -c Release -p:EnableUiAutomation=true
```

此类版本的可执行文件为 `Terminal/Avalonia/Terminal.Avalonia/uiauto/bin/Release/net10.0/StockSharp.Terminal.Avalonia.exe`。

| 程序 | Avalonia 版本标识符 | WPF 版本标识符 | 无测试配置文件时的单击和输入 |
|---|---|---|---|
| [Terminal](../terminal.md) | `stocksharp.terminal` | `stocksharp.terminal.wpf` | 拒绝 |
| [Hydra](../hydra.md) | `stocksharp.hydra` | `stocksharp.hydra.wpf` | 拒绝 |
| [Designer](../designer.md) | `stocksharp.designer` | `stocksharp.designer.wpf` | 拒绝 |
| [安装程序](../installer.md) | `stocksharp.installer` | `stocksharp.installer.wpf` | 拒绝 |
| [Shell](../shell.md) | `stocksharp.shell` | — | 拒绝 |
| [UDP Dumper](../udp_dumper.md) | `tool.udpdumper` | — | 接受 |

程序会自行报告其标识符，服务器清单中也使用相同的标识符。

在 Avalonia 版本中，驱动程序可以读取表格、带面板的工作区、图表、订单簿、属性编辑器、策略图、代码编辑器和 Markdown 视图。在 WPF 版本中，可以读取表格、停靠面板、图表、订单簿、属性编辑器、策略图、输入字段、功能区、菜单和工具栏。在 Terminal、Hydra 和 Designer 的 Avalonia 版本中，面板上的所有内容都相对于该面板寻址：`panel:<面板类型>/<标识符>`。

## 安装服务器

### .NET 工具

需要 [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)：

```sh
dotnet tool install StockSharp.DesktopDriver.Mcp --tool-path tools
```

服务器命令为 `tools/desktop-driver-mcp`。服务器将协议写入标准输出，将日志写入标准错误流。在没有客户端的情况下手动启动时，它只会等待输入。

### 从源代码编译

```sh
dotnet build DesktopDriver.Mcp/DesktopDriver.Mcp.csproj -c Release
```

### Claude Desktop 扩展

对于 Claude Desktop，服务器被打包为扩展，即 `stocksharp-desktop-driver-<版本>.mcpb` 文件，由 DesktopDriver.Mcp 仓库中的 `scripts/Pack-McpBundle.ps1` 脚本生成。扩展通过计算机上安装的 .NET 启动服务器，因此需要可通过 `PATH` 访问的 .NET 10 SDK。

1. 在 Claude Desktop 中打开 **Settings → Extensions → Advanced settings → Install Extension**，然后选择扩展文件。
2. 指定程序清单（JSON 文件，见下文）和截图文件夹。

## 程序清单

代理不能启动任意文件。服务器只启动清单中的程序，清单是由配置服务器的人编写的 JSON 文件。代理通过标识符而不是路径指定程序。

服务器只从环境变量读取设置：

| 变量 | 设置内容 | 未设置时 |
|---|---|---|
| `STOCKSHARP_UI_CATALOGUE` | 程序清单的路径 | 服务器旁边的 `applications.json` 文件 |
| `STOCKSHARP_UI_ARTIFACTS` | 截图文件夹 | 服务器旁边的 `artifacts` 文件夹 |

对于作为 .NET 工具安装的服务器，“服务器旁边”指的是工具存储文件夹，因此需要显式设置这两个变量。没有清单时，服务器只能连接到由他人启动的程序。

包含一个程序的清单示例：

```json
{
  "applications": [
    {
      "appId": "stocksharp.terminal",
      "executable": "..\\Terminal\\Avalonia\\Terminal.Avalonia\\uiauto\\bin\\Release\\net10.0\\StockSharp.Terminal.Avalonia.exe",
      "arguments": "--ui-test",
      "description": "交易终端。"
    }
  ]
}
```

- `appId` — 上表中的程序标识符。如果已启动的程序报告了不同的标识符，连接将被拒绝。
- `executable` — 带自动化功能的版本的路径。相对路径从清单文件所在位置算起。
- `arguments` — 额外的启动参数；服务器会在其后添加 `--ui-automation` 和 `--ui-automation-endpoint`。该字符串按空格拆分为参数，因此无法在此传递包含空格的路径。
- `description` — 代理将看到的描述。

清单文件中可以使用 `//` 注释。

## 测试配置文件

StockSharp 程序会连接到经纪商、交易所和数据源，因此只有在使用测试配置文件启动时，才会接受来自代理的单击和输入。没有测试配置文件时，程序会响应读取和截图请求，但会拒绝单击、文本输入、按键、滚动和拖动，并返回错误 `inputUnavailable`：`Input is delivered only to a run started on a test profile.` 例外是 UDP Dumper：它启动时不连接任何对象，因此始终允许输入。

测试配置文件通过启动参数启用；在清单中，这些参数写在 `arguments` 中。

| 参数 | 作用 |
|---|---|
| `--ui-test` | 使用单独的设置文件夹启动程序 |
| `--ui-test-dir=<路径>` | 指定该文件夹；如果没有此参数，会在可执行文件旁边新建 `ui-test\<启动标识符>` 文件夹 |
| `--ui-test-lang=<语言代码>` | 界面语言，例如 `ru` |
| `--ui-test-view=<标识符>` | 程序启动后打开的部分；标识符的含义由程序自行决定 |

使用测试配置文件时，程序会将所有内容保存在配置文件文件夹中，不读取也不更改计算机用户的设置，并从默认布局开始。Terminal、Hydra、Designer 和 Installer 还会将保存账户登录信息的 StockSharp 公共文件夹移到配置文件中，因此在测试配置文件下它们不会登录账户。在程序的 Avalonia 版本中，文件对话框从配置文件的 `Files` 子文件夹响应：将程序需要打开的文件放在那里，程序保存的文件也在那里查找。

> [!WARNING]
> 测试配置文件只是一个单独的设置文件夹，而不是沙盒。如果在配置文件中放入连接设置，或者代理自己输入了这些设置，程序就会真正建立连接，而代理的单击仍会被接受。在这样的配置文件下单击 Terminal 中的买入按钮会提交订单。

## 连接代理

Claude Code — 项目的 `.mcp.json` 文件。以下是服务器说明中针对源代码编译版本的示例：

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "DesktopDriver.Mcp/applications.json"
      }
    }
  }
}
```

对于作为 .NET 工具安装的服务器，请在 `command` 中指定 `desktop-driver-mcp` 的路径，并设置这两个变量：

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "tools/desktop-driver-mcp",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "<程序清单的路径>",
        "STOCKSHARP_UI_ARTIFACTS": "<截图文件夹>"
      }
    }
  }
}
```

Codex — `~/.codex/config.toml` 文件：

```toml
[mcp_servers.desktop-driver]
command = "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe"
env = { STOCKSHARP_UI_CATALOGUE = "DesktopDriver.Mcp/applications.json" }
```

Claude Desktop — 按上文所述安装扩展；清单和截图文件夹在扩展的设置中指定。

## 代理可以做什么

除 `ui_list_applications`、`ui_start_application` 和 `ui_attach_application` 外，所有工具都通过 `instance` 参数指定程序，即 `ui_start_application` 或 `ui_attach_application` 返回的名称。同一程序的第二个已启动副本会获得带 `#2` 后缀的名称。

界面元素通过 `区域/标识符` 形式的地址指定，例如 `panel:OrdersPanel/OrdersGrid`，即 Terminal 订单面板上的表格。地址由 `ui_windows`、`ui_find` 和 `ui_tree` 返回。将面板移到其他组、拆分到单独的窗口或重新构建时，地址都不会改变。

### 程序

| 工具 | 作用 |
|---|---|
| `ui_list_applications` | 清单中的程序：每个程序是否已编译，以及哪些副本已在运行 |
| `ui_start_application` | 从清单中启动程序，并等待其窗口绘制完成（最多 90 秒） |
| `ui_attach_application` | 连接到由他人使用 `--ui-automation` 启动的程序；需要该程序写入的地址文件 |
| `ui_release_application` | 释放程序：由本服务器启动的程序会被关闭，他人启动的程序继续运行 |
| `ui_session` | 响应的是哪个程序及其功能：版本、测试配置文件、输入方式、响应大小上限 |

### 屏幕上的内容

| 工具 | 作用 |
|---|---|
| `ui_windows` | 已打开的窗口和弹出窗口，以及其根元素的地址 |
| `ui_find` | 按名称、类型（`grid`、`chart`、`orderBook`、`propertyEditor`、`workspace`、`button`、`textBox` 等）、自动化标识符或文本查找元素 |
| `ui_tree` | 受深度和数量限制的元素树 |
| `ui_snapshot` | 单个元素的完整状态：是否存在、是否可用、是否可见、是否获得焦点，以及其类型特有的状态，例如表格的行数和排序、图表的范围、订单簿的最优价格 |

### 读取元素

| 工具 | 作用 |
|---|---|
| `ui_grid_columns`、`ui_grid_rows`、`ui_grid_groups` | 表格的列、行和分组，顺序与表格在排序、筛选和分组后显示的顺序相同 |
| `ui_chart_series`、`ui_chart_points` | 图表的序列以及图表实际绘制的点；时间为 UTC |
| `ui_order_book_levels` | 按方向和价格列出的订单簿档位 |
| `ui_property_items` | 属性编辑器显示的属性：值、屏幕上的文本、编辑器类型、错误 |
| `ui_tree_items` | 按路径列出的树元素 |
| `ui_dock_layout` | 工作区布局：面板、选项卡组、浮动窗口 |
| `ui_document_content` | 代码编辑器或 Markdown 视图中的行，从 1 开始编号 |
| `ui_diagram_nodes`、`ui_diagram_connections` | 策略图中的模块及其之间的连接，与绘制的内容一致 |

### 操作

| 工具 | 作用 |
|---|---|
| `ui_click` | 单击元素或其某个部分：列标题、单元格、面板选项卡及其关闭按钮、列表或树中的项、属性，在图表上还可以单击某个价格或某个订单；可以按住 `Shift`、`Control`、`Alt`、`Meta` |
| `ui_drag` | 在单个元素内拖动，例如将图表上的订单线拖到另一个价格 |
| `ui_type_text` | 逐字符输入文本：追加到已有文本之后（`Append`）或替换已有文本（`Replace`） |
| `ui_press_key` | 按下单个按键，例如 `Enter`、`Escape`、`Tab`、`Delete`，可以同时按住 `Shift`、`Control`、`Alt`、`Meta` |
| `ui_scroll` | 按指定格数滚动鼠标滚轮 |
| `ui_hover` | 将指针悬停在元素或其某个部分上，但不单击：会出现只有指针在上方时才显示的内容，例如行按钮、提示、图表十字光标 |
| `ui_leave` | 将指针移出元素所在的窗口：窗口中不再有任何内容以悬停状态高亮显示。在为“静止”状态的窗口截图之前需要执行此操作 |
| `ui_bring_into_view` | 将元素的某个部分显示到屏幕上，例如表格底部很远的一行，而不发送任何输入 |

### 检查结果

| 工具 | 作用 |
|---|---|
| `ui_wait` | 等待元素出现或消失，或者等待 `ui_snapshot` 响应中的某个字段取得所需的值 |
| `ui_action_status` | 根据回执中的标识符，查询已发送的输入处理得怎样 |

### 截图和诊断

| 工具 | 作用 |
|---|---|
| `ui_screenshot` | 将元素截图保存到文件；响应为文件路径和截图参数 |
| `ui_screenshot_image` | 元素截图，以图像形式返回给代理 |
| `ui_diagnostics` | 自动化模块本身的记录：拒绝、无法读取的内容以及无法送达的输入 |

> [!NOTE]
> `ui_drag`、`ui_hover`、`ui_leave`、按住按键的单击、图表上对价格、订单和面板叉号的单击，以及截图上的 `highlight` 边框，已包含在服务器源代码中，但尚未包含在已发布的 1.0.1 版本包中。

读取不会改变任何内容：服务器不会打开从未显示过的面板，不会展开属性，也不会滚动表格。如果因此无法得到结果，响应中会说明这一点。单个响应的大小有限制（默认 256 KB），因此大型表格需要使用 `limit`、`startIndex`、`columns` 和 `rowKeys` 参数分批读取。

## 限制和安全

- **仅限清单中的程序。** 代理从清单中选择程序，不能向服务器传递自己的文件路径。
- **仅限本计算机和本用户。** 通道只有当前操作系统用户的进程可以访问。连接时，服务器会检查程序、程序副本和协议版本。
- **仅在测试配置文件下接受单击。** 没有测试配置文件时，程序会响应读取和截图，但会拒绝输入（参阅“测试配置文件”）。是否允许输入由 `ui_session` 显示：`safeTestProfile` 为 `true`，并且 `inputBackend` 不为 `none`。
- **输入不会操作计算机的鼠标和键盘。** 事件进入程序内部的输入管道，因此焦点和处理程序的行为与人工单击时相同。对整个元素的普通左键单击会作为该元素的自动化操作执行。发送之前，驱动程序会检查元素是否可见、可用且未被遮挡。
- **“已送达”不等于“已完成”。** 回执 `dispatched` 只表示输入已到达程序。程序是否完成了预期的操作，需要通过 `ui_snapshot` 和 `ui_wait` 检查。
- **指针行为与人相同。** 每个指针操作都会把指针移到新的位置，指针离开和到达的元素会像收到真实鼠标事件一样得到通知：高亮的按钮会熄灭，指针离开图表时图表十字光标会消失。
- **WPF 版本** 会拒绝按住按键的单击、拖动、`ui_hover` 和 `ui_leave`，而不是用普通单击代替它们。
- **从图表下单。** 在图表上按住 `Control` 单击某个价格会提交订单。在已配置连接的测试配置文件下，这是真实订单。
- **关闭时不保存。** `ui_release_application` 会关闭由本服务器启动的程序，且不会提示保存任何内容。对于服务器只是连接上的程序，不会将其关闭。
- **截图** 保存在 `STOCKSHARP_UI_ARTIFACTS` 文件夹中，每张截图对应一个 `<截图标识符>.png` 文件。如果设置了 `maxPixelWidth` 或 `maxPixelHeight` 上限，过大的截图会被拒绝，而不是被缩小。

## 会话示例

Terminal 已使用自动化功能编译，清单与上面的示例相同。用户请求：“在 Terminal 中打开下单面板的设置，并显示其中有哪些字段”。

1. `ui_list_applications` — 清单中有 `stocksharp.terminal`，已编译且未运行。
2. `ui_start_application`，`appId` = `stocksharp.terminal` — 服务器使用测试配置文件启动 Terminal，并返回 `instance` = `stocksharp.terminal`。
3. `ui_session` — `safeTestProfile` = `true`，允许输入。
4. `ui_wait`，`node` = `panel:BuySellPanel/SettingsButton`，`timeoutSeconds` = `180` — 等待 Terminal 构建工作区：窗口会先于面板出现。
5. `ui_click`，`node` = `panel:BuySellPanel/SettingsButton` — 打开下单面板的设置。回执为 `dispatched`。
6. `ui_property_items`，`node` = `panel:BuySellPanel/SettingsPropertyGrid` — 属性及其当前值，其中包括 `Security`（交易品种）和 `Depth`。
7. 对同一 `node` 调用 `ui_screenshot` — 得到编辑器截图文件的路径，以便展示给用户。
8. `ui_release_application` — Terminal 关闭。

## 另请参阅

[连接 AI 代理 (MCP)](../mcp.md)

[程序命令 (Studio.Mcp)](studio_mcp.md)
