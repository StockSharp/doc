# Interface control (DesktopDriver)

**DesktopDriver** is an MCP server through which an agent works with the windows of a running program the same way a person works with them: it finds panels and buttons, reads tables, charts, order books and property editors, clicks, types text, scrolls and takes snapshots.

The server reads the interface itself, not a picture of the screen. A table answers with its columns and rows, a chart with the points it has drawn, an order book with its price levels. The answer does not depend on the theme, the font or the language of the program. Snapshots are for a person; the content should be checked by reading.

The server is distributed as the `StockSharp.DesktopDriver.Mcp` NuGet package and calls itself `StockSharp.DesktopDriver`; in the MCP Registry it is listed as `io.github.StockSharp/desktop-driver`. The source code is in the [DesktopDriver.Mcp](https://github.com/StockSharp/DesktopDriver.Mcp) repository.

## How it works

1. The driver is included in the program when the program is built. A regular build does not have it.
2. A program with the driver, started with the `--ui-automation` key, opens a local channel — a named pipe available only to the current OS user — and writes its address to the file set by the `--ui-automation-endpoint=<path>` key. The file appears when the window has already been built and drawn. Without the `--ui-automation` key, the program opens nothing.
3. The MCP server starts the program from its catalog with these keys or connects to a program that someone else has started. When connecting, it checks that the right program, the right copy of it and a compatible protocol version have answered, and only after that executes the agent's calls.

The same operations are available from the command line — with the `desktop-driver` program from the `StockSharp.DesktopDriver.Cli` package.

## Which builds can be connected

You can connect only to a program built with automation enabled — with the `EnableUiAutomation=true` property. By default, the property is off, and a regular build has neither the driver nor the channel. A regular build started with `--ui-automation` writes the message `<identifier> was built without automation, so --ui-automation does nothing. Build it with EnableUiAutomation=true.` to the standard error stream and works as usual. The MCP server waits 90 seconds for the channel of such a program, then reports that the program has not come up and closes it.

A build with automation is made from the source code of the program, next to which the DesktopDriver.Mcp repository must be located. It is put into a separate `uiauto\bin` folder so as not to mix with the regular one:

```sh
dotnet build Terminal/Avalonia/Terminal.Avalonia/Terminal.Avalonia.csproj -c Release -p:EnableUiAutomation=true
```

The executable file of such a build is `Terminal/Avalonia/Terminal.Avalonia/uiauto/bin/Release/net10.0/StockSharp.Terminal.Avalonia.exe`.

| Program | Identifier of the Avalonia version | Identifier of the WPF version | Clicks and input without a test profile |
|---|---|---|---|
| [Terminal](../terminal.md) | `stocksharp.terminal` | `stocksharp.terminal.wpf` | rejected |
| [Hydra](../hydra.md) | `stocksharp.hydra` | `stocksharp.hydra.wpf` | rejected |
| [Designer](../designer.md) | `stocksharp.designer` | `stocksharp.designer.wpf` | rejected |
| [Installer](../installer.md) | `stocksharp.installer` | `stocksharp.installer.wpf` | rejected |
| [Shell](../shell.md) | `stocksharp.shell` | — | rejected |
| [UDP Dumper](../udp_dumper.md) | `tool.udpdumper` | — | accepted |

A program reports its identifier itself, and the same identifier is specified in the server catalog.

In the Avalonia versions, the driver reads tables, the workspace with panels, charts, the order book, the property editor, the strategy scheme, the code editor and the Markdown viewer. In the WPF versions — tables, dock panels, charts, the order book, the property editor, the strategy scheme, input fields, the ribbon, menus and toolbars. In the Avalonia versions of Terminal, Hydra and Designer, everything that lies on a panel is addressed relative to that panel: `panel:<panel type>/<identifier>`.

## Installing the server

### .NET tool

The [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) is required:

```sh
dotnet tool install StockSharp.DesktopDriver.Mcp --tool-path tools
```

The server command is `tools/desktop-driver-mcp`. The server writes the protocol to the standard output and its log to the standard error stream. Started manually, without a client, it simply waits for input.

### From the source code

```sh
dotnet build DesktopDriver.Mcp/DesktopDriver.Mcp.csproj -c Release
```

### Claude Desktop extension

For Claude Desktop, the server is packed into an extension — the `stocksharp-desktop-driver-<version>.mcpb` file, which is built by the `scripts/Pack-McpBundle.ps1` script of the DesktopDriver.Mcp repository. The extension starts the server through the .NET installed on the computer, so the .NET 10 SDK available through `PATH` is required.

1. In Claude Desktop, open **Settings → Extensions → Advanced settings → Install Extension** and select the extension file.
2. Specify the program catalog (a JSON file, see below) and the folder for snapshots.

## Program catalog

The agent cannot start an arbitrary file. The server starts only the programs from the catalog — a JSON file written by whoever configures the server. The agent names a program by its identifier, not by a path.

The server takes its settings only from environment variables:

| Variable | What it sets | If not set |
|---|---|---|
| `STOCKSHARP_UI_CATALOGUE` | the path to the program catalog | the `applications.json` file next to the server |
| `STOCKSHARP_UI_ARTIFACTS` | the folder for snapshots | the `artifacts` folder next to the server |

For a server installed as a .NET tool, "next to the server" is the folder of the tool store, so both variables have to be set explicitly. Without a catalog, the server can only connect to programs that someone else has started.

An example of a catalog with one program:

```json
{
  "applications": [
    {
      "appId": "stocksharp.terminal",
      "executable": "..\\Terminal\\Avalonia\\Terminal.Avalonia\\uiauto\\bin\\Release\\net10.0\\StockSharp.Terminal.Avalonia.exe",
      "arguments": "--ui-test",
      "description": "The trading terminal."
    }
  ]
}
```

- `appId` — the program identifier from the table above. If the running program reports a different identifier about itself, the connection is rejected.
- `executable` — the path to the build with automation. A relative path is counted from the catalog file.
- `arguments` — additional start keys; the server adds `--ui-automation` and `--ui-automation-endpoint` to them. The string is split into keys by spaces, so a path with spaces cannot be passed here.
- `description` — the description the agent will see.

You can write `//` comments in the catalog file.

## Test profile

StockSharp programs connect to brokers, exchanges and data sources, so they accept clicks and input from the agent only when started with a test profile. Without it, a program responds to reading and snapshots, but rejects clicks, typing, keys, scrolling and dragging with the `inputUnavailable` error: `Input is delivered only to a run started on a test profile.` The exception is UDP Dumper: when started, it connects to nothing, and input to it is always allowed.

The test profile is enabled with start keys; in the catalog, they are written in `arguments`.

| Key | What it does |
|---|---|
| `--ui-test` | starts the program with a separate settings folder |
| `--ui-test-dir=<path>` | sets this folder; without the key, a new `ui-test\<run identifier>` folder is created next to the executable file |
| `--ui-test-lang=<language code>` | the interface language, for example `ru` |
| `--ui-test-view=<identifier>` | the section the program opens on; what the identifier means is decided by the program itself |

With a test profile, the program keeps everything in the profile folder, neither reads nor changes the settings of the computer user, and starts with the default layout. Terminal, Hydra, Designer and Installer also move the common StockSharp folder, where the account sign-in is stored, into the profile, so on a test profile they do not sign in to the account. In the Avalonia versions of the programs, file dialogs answer from the `Files` subfolder of the profile: put there a file the program should open, and look there for what it has saved.

> [!WARNING]
> A test profile is a separate settings folder, not a sandbox. If you put connection settings into the profile, or if the agent enters them itself, the program connects for real, and clicks from the agent are still accepted. Clicking the buy button in Terminal on such a profile places an order.

## Connecting an agent

Claude Code — the `.mcp.json` file of the project. An example from the server description, for a build from the source code:

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

For a server installed as a .NET tool, specify the path to `desktop-driver-mcp` in `command` and set both variables:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "tools/desktop-driver-mcp",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "<path to the program catalog>",
        "STOCKSHARP_UI_ARTIFACTS": "<folder for snapshots>"
      }
    }
  }
}
```

Codex — the `~/.codex/config.toml` file:

```toml
[mcp_servers.desktop-driver]
command = "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe"
env = { STOCKSHARP_UI_CATALOGUE = "DesktopDriver.Mcp/applications.json" }
```

Claude Desktop — install the extension as described above; the catalog and the folder for snapshots are set in its settings.

## What the agent can do

All tools except `ui_list_applications`, `ui_start_application` and `ui_attach_application` name the program with the `instance` parameter — the name returned by `ui_start_application` or `ui_attach_application`. A second running copy of the same program gets a name with the `#2` suffix.

An interface element is named by the address `area/identifier`, for example `panel:OrdersPanel/OrdersGrid` — the table on the orders panel of Terminal. Addresses are returned by `ui_windows`, `ui_find` and `ui_tree`. An address does not change when the panel is moved to another group, undocked into a separate window or rebuilt.

### Programs

| Tool | What it does |
|---|---|
| `ui_list_applications` | the catalog programs: whether each one is built and which of its copies are already running |
| `ui_start_application` | starts a program from the catalog and waits until its window is drawn (up to 90 seconds) |
| `ui_attach_application` | connects to a program that someone has started with `--ui-automation`; the address file it has written is required |
| `ui_release_application` | releases the program: closes one started by this server, leaves someone else's running |
| `ui_session` | which program has answered and what it can do: version, test profile, input method, response size limit |

### What is on the screen

| Tool | What it does |
|---|---|
| `ui_windows` | open windows and pop-ups with the addresses of their root elements |
| `ui_find` | searches for elements by name, kind (`grid`, `chart`, `orderBook`, `propertyEditor`, `workspace`, `button`, `textBox` and others), automation identifier or text |
| `ui_tree` | the element tree, limited by depth and count |
| `ui_snapshot` | the whole state of one element: whether it exists, is enabled, visible, focused, as well as the state of its kind — the number of rows and the sorting of a table, the ranges of a chart, the best prices of an order book |

### Reading elements

| Tool | What it does |
|---|---|
| `ui_grid_columns`, `ui_grid_rows`, `ui_grid_groups` | the columns, rows and groups of a table in the order in which the table itself shows them after sorting, filtering and grouping |
| `ui_chart_series`, `ui_chart_points` | the chart series and the points it has actually drawn; time in UTC |
| `ui_order_book_levels` | the order book levels by side and price |
| `ui_property_items` | the properties shown by the property editor: value, text on the screen, editor kind, errors |
| `ui_tree_items` | tree items by their path |
| `ui_dock_layout` | the workspace layout: panels, tab groups, floating windows |
| `ui_document_content` | the lines of the code editor or the Markdown viewer, numbered from one |
| `ui_diagram_nodes`, `ui_diagram_connections` | the cubes of the strategy scheme and the connections between them as they are drawn |

### Actions

| Tool | What it does |
|---|---|
| `ui_click` | a click on an element or its part: a column header, a cell, a panel tab and its close button, a list or tree item, a property, and on a chart — at a price or on an order; `Shift`, `Control`, `Alt`, `Meta` can be held |
| `ui_drag` | dragging within one element, for example an order line on a chart to another price |
| `ui_type_text` | typing text character by character: appending to the existing text (`Append`) or replacing it (`Replace`) |
| `ui_press_key` | pressing one key, for example `Enter`, `Escape`, `Tab`, `Delete`, optionally together with `Shift`, `Control`, `Alt`, `Meta` |
| `ui_scroll` | scrolling with the wheel by a given number of notches |
| `ui_hover` | moves the pointer over an element or its part and clicks nothing: what is visible only under the pointer appears — row buttons, a tooltip, the chart crosshair |
| `ui_leave` | moves the pointer off the window that contains the element: nothing in the window is highlighted as hovered any more. Needed before a snapshot of the window "at rest" |
| `ui_bring_into_view` | brings a part of an element into view, for example a row far down a table, without sending input |

### Checking the result

| Tool | What it does |
|---|---|
| `ui_wait` | waits until an element appears or disappears, or until a field from the `ui_snapshot` response takes the required value |
| `ui_action_status` | what has happened to the input already sent, by the identifier from the receipt |

### Snapshots and diagnostics

| Tool | What it does |
|---|---|
| `ui_screenshot` | a snapshot of an element to a file; the response is the file path and the snapshot parameters |
| `ui_screenshot_image` | a snapshot of an element that is returned to the agent as an image |
| `ui_diagnostics` | the records of the automation module itself: refusals, what could not be read, and input that could not be delivered |

> [!NOTE]
> `ui_drag`, `ui_hover`, `ui_leave`, a click with held keys, a click at a price, on an order and on the panel cross on a chart, and the `highlight` frame on snapshots are present in the source code of the server, but not in the published package of version 1.0.1.

Reading changes nothing: the server does not open a panel that has never been shown, does not expand a property and does not scroll a table. If an answer cannot be obtained because of this, the answer itself says so. The size of one response is limited (256 KB by default), so large tables are read in parts, with the `limit`, `startIndex`, `columns` and `rowKeys` parameters.

## Limitations and security

- **Only programs from the catalog.** The agent selects a program from the catalog and cannot pass its own file path to the server.
- **Only this computer and this user.** The channel is available only to the processes of the current OS user. When connecting, the server checks the program, its copy and the protocol version.
- **Clicks only on a test profile.** Without a test profile, the program responds to reading and snapshots, but rejects input (see "Test profile"). Whether input is allowed is shown by `ui_session`: `safeTestProfile` equals `true`, and `inputBackend` is not `none`.
- **Input does not touch the computer's mouse and keyboard.** The events go into the internal input pipeline of the program, so focus and handlers work the same way as when a person clicks. A regular left click on a whole element is performed as the automation action of that element. Before sending, the driver checks that the element is visible, enabled and not covered by anything.
- **"Delivered" does not mean "done".** The `dispatched` receipt means only that the input has reached the program. Whether the program has done what was expected is checked with `ui_snapshot` and `ui_wait`.
- **A pointer like a person's.** Every pointer action moves it to a new place, and the elements it has left and entered learn about it the same way as from a real mouse: a highlighted button fades, the chart crosshair disappears when the pointer leaves the chart.
- **The WPF versions** reject a click with held keys, dragging, `ui_hover` and `ui_leave`, rather than replacing them with a simple click.
- **Orders from the chart.** A click with `Control` at a price on the chart places an order. On a test profile with a configured connection, this is a real order.
- **Closing without saving.** `ui_release_application` closes a program started by this server without offering to save anything. A program the server has only connected to is not closed.
- **Snapshots** are written to the `STOCKSHARP_UI_ARTIFACTS` folder, one `<snapshot identifier>.png` file per snapshot. If the `maxPixelWidth` or `maxPixelHeight` limit is set, a snapshot that is too large is rejected rather than reduced.

## Example session

Terminal is built with automation, and the catalog is as in the example above. The user asks: "Open the settings of the order panel in Terminal and show which fields are there".

1. `ui_list_applications` — the catalog has `stocksharp.terminal`; it is built and not running.
2. `ui_start_application`, `appId` = `stocksharp.terminal` — the server starts Terminal with a test profile and returns `instance` = `stocksharp.terminal`.
3. `ui_session` — `safeTestProfile` = `true`, input is allowed.
4. `ui_wait`, `node` = `panel:BuySellPanel/SettingsButton`, `timeoutSeconds` = `180` — wait until Terminal builds the workspace: the window appears earlier than the panels.
5. `ui_click`, `node` = `panel:BuySellPanel/SettingsButton` — open the settings of the order panel. The receipt is `dispatched`.
6. `ui_property_items`, `node` = `panel:BuySellPanel/SettingsPropertyGrid` — the properties with their current values, among them `Security` (the instrument) and `Depth`.
7. `ui_screenshot` with the same `node` — the path to the file with a snapshot of the editor, to show it to the user.
8. `ui_release_application` — Terminal closes.

## See also

[Connecting AI agents (MCP)](../mcp.md)

[Program commands (Studio.Mcp)](studio_mcp.md)
