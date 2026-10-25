# Connecting AI agents (MCP)

**MCP** (Model Context Protocol) is an open protocol by which an AI agent (Claude Code, Claude Desktop or any other MCP client) calls the tools of an external program — an MCP server. The client starts the server itself as a child process and exchanges messages with it through the standard input and output. The server reports which tools it has, what each of them does and which parameters it accepts, and the agent decides which tool to call and receives the answer.

There are two MCP servers for StockSharp programs. Both work only on the same computer as the programs and communicate only with the programs of the same user account.

| | [DesktopDriver](mcp/desktop_driver.md) | [Studio.Mcp](mcp/studio_mcp.md) |
|---|---|---|
| What it does | reads the program interface, clicks buttons, types text, scrolls, takes snapshots | passes commands to the program: place or cancel an order, start a Hydra source, start strategy testing |
| With which programs | Terminal, Hydra, Designer and Installer (Avalonia and WPF versions), Shell and UDP Dumper — only in a build with automation enabled | the Avalonia versions of Terminal, Hydra, Designer and Shell — in the regular build |
| What the agent sees | windows, panels, tables, charts, order books, property editors, strategy schemes | only what the commands return: lists of sources, strategies and orders |
| Restriction on actions | clicks and input are accepted only by a program started with a test profile (except UDP Dumper, which always accepts them) | there is no profile restriction: the order goes wherever Terminal is connected |
| Where to get the server | the `StockSharp.DesktopDriver.Mcp` NuGet package, an extension for Claude Desktop | it is built from the source code |

## Which server to choose

- You need to check what a window shows, go through a scenario with buttons the way a person would, or get a snapshot of a panel — [DesktopDriver](mcp/desktop_driver.md).
- You need the program to perform an action on command, without clicks: place an order, start downloading with a Hydra source, start strategy testing — [Studio.Mcp](mcp/studio_mcp.md).
- The program is built the regular way, without automation — only Studio.Mcp will do, and only for the Avalonia versions of the programs.
- The WPF version of a program — only DesktopDriver, and only in a build with automation.

Both servers can be connected to the agent at the same time. For example, Studio.Mcp starts strategy testing in Designer but does not return the results — the agent can read them from the Designer panels through DesktopDriver, if Designer is built with automation.

MCP is not the same as [remote control](shell/remote_control.md) from the website and from the [Telegram bot](telegram_services/control_panel.md). Remote control works through the StockSharp account and requires neither an agent nor an MCP server.

## See also

[Interface control (DesktopDriver)](mcp/desktop_driver.md)

[Program commands (Studio.Mcp)](mcp/studio_mcp.md)

[AI-Assisted Development](api/ai_development.md)
