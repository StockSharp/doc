# Program commands (Studio.Mcp)

**Studio.Mcp** is an MCP server through which an agent gives commands to running StockSharp programs: it places and cancels orders in Terminal, manages Hydra data sources, and starts strategy testing in Designer and Shell. The server does not click buttons and does not read windows — the command is executed by the program itself.

The server starts nothing and stores nothing. A command reaches a program only when the program is open; if the program is not running, the server replies so.

## Which programs respond

| Program | Which commands it accepts |
|---|---|
| [Terminal](../terminal.md) | the list of orders, placing and cancelling an order |
| [Hydra](../hydra.md) | source types, the list of sources, adding, enabling, starting, stopping and removing a source |
| [Designer](../designer.md) | the list of strategies, starting and stopping backtesting |
| [Shell](../shell.md) | the list of strategies of the [Emulation](../shell/user_interface/emulation.md) area, starting and stopping their runs |

Only the Avalonia versions of these programs accept the commands, and in the regular build: no special build is needed. A program opens the command channel together with its main window and closes it when the window closes. The WPF versions of Terminal, Hydra and Designer do not accept these commands.

The channel is a local named pipe available only to the processes of the current OS user. Commands are not transmitted over the network.

## Installation

The server is built from the StockSharp source code:

```sh
dotnet build Studio/Studio.Mcp/Studio.Mcp.csproj -c Release
```

The resulting file is `Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe`. The server has no settings of its own: it needs neither arguments nor environment variables. It writes the protocol to the standard output and its log to the standard error stream; started manually, without a client, it simply waits for input.

## Connecting an agent

Claude Code — the `.mcp.json` file of the project:

```json
{
  "mcpServers": {
    "stocksharp": {
      "command": "Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe"
    }
  }
}
```

In other MCP clients, including Claude Desktop, the server is connected the same way: the command is the path to `StockSharp.Studio.Mcp.exe`, without arguments and environment variables.

## What the agent can do

The identifiers of sources, strategies and orders are taken from the responses of the list tools: first the list, then the action.

### Common

| Tool | What it does |
|---|---|
| `list_products` | which programs are running and which commands each of them accepts |
| `describe_product` | the commands of one program; `product` — `Terminal`, `Hydra`, `Designer` or `Shell` |
| `close_product` | asks the program to close |

### Hydra

| Tool | What it does |
|---|---|
| `hydra_list_source_types` | the source types Hydra can create, with their identifiers |
| `hydra_list_sources` | the Hydra sources: whether each one is enabled and whether it is downloading now |
| `hydra_add_source` (`typeId`) | adds a source of the selected type and returns it; the new source is disabled |
| `hydra_enable_source` (`sourceId`, `isEnabled`) | includes the source in downloading or excludes it |
| `hydra_start_source` (`sourceId`) | starts downloading by the source; a disabled source does not start |
| `hydra_stop_source` (`sourceId`) | stops downloading |
| `hydra_remove_source` (`sourceId`) | removes the source together with its settings; a running source has to be stopped first |

To download data with a new source, three calls are needed: `hydra_add_source`, `hydra_enable_source`, `hydra_start_source`. These commands do not change the source settings — instruments, data types, connection parameters: they are set in the Hydra window (see [Data sources](../hydra/data_sources.md)).

### Designer

| Tool | What it does |
|---|---|
| `designer_list_strategies` | the Designer strategies: the kind (`Backtest`, `Optimization`, `Live`, `Remote`, `Other`) and the current state |
| `designer_start_backtest` (`strategyId`, `startTime`, `stopTime`) | starts a strategy of the `Backtest` or `Optimization` kind on history; the period is set in UTC in the ISO 8601 format and replaces the period in the strategy settings; without it, the period from the settings is used |
| `designer_stop_backtest` (`strategyId`) | stops a running strategy |

### Terminal

| Tool | What it does |
|---|---|
| `terminal_list_orders` | the Terminal orders with the transaction number of each |
| `terminal_register_order` (`securityId`, `portfolioName`, `side`, `volume`, `price`, `orderType`) | places an order and returns its transaction number |
| `terminal_cancel_order` (`orderTransactionId`) | cancels an order by its transaction number |

The instrument is specified the way Terminal knows it, for example `SBER@TQBR`; the portfolio — by the name known to Terminal; `side` — `Buy` or `Sell`. A limit order needs a price; for orders of other types, the passed price is not used. An order is identified by the transaction number returned by `terminal_register_order` or `terminal_list_orders`, not by the number the exchange assigns later.

### Shell

The server builds the Shell tools from the command description that the program itself sends. Therefore they exist only if Shell was running when the server started or at the last call of `list_products` or `describe_product`.

| Tool | What it does |
|---|---|
| `shell_list_strategies` | the strategies of the Emulation area and their state |
| `shell_start_backtest` (`StrategyId`, `StartTime`, `StopTime`) | starts a run of the strategy on history |
| `shell_stop_backtest` (`StrategyId`) | stops the run |

Shell always tests a strategy on the period from its settings: if `StartTime` or `StopTime` is passed, the command is rejected, so pass `null` in them.

## Limitations and security

> [!CAUTION]
> `terminal_register_order` places a real order wherever Terminal is connected. The server asks nothing: confirmation is up to the MCP client, in which a person sees the call before it is executed.

- **Strategies that trade for real are not started.** Designer starts only strategies of the `Backtest` and `Optimization` kinds; for the others, the command is rejected. Shell opens only the strategies of the Emulation area for commands.
- **Removal is irreversible.** `hydra_remove_source` removes the source together with its settings.
- **Closing.** `close_product` closes the program.
- **The program must be running.** Otherwise the command returns the `<program> is not running.` error. Waiting for a response from a program that is not running takes about 5 seconds, so `list_products` does not respond at once when the programs are closed. A refusal of a running program reaches the agent as text: `<program> refused the command: <reason>`.
- **No password.** The channel is open to any program started under the same user account: there is no separate check of who sent the command.
- **Copies of programs are not distinguished.** If several copies of Terminal or Designer are running, or a Hydra on a test profile works next to the regular Hydra, the command goes to one of them, and which one is not known in advance.
- **No testing results.** The server returns only the strategy state, but neither statistics nor trades. [DesktopDriver](desktop_driver.md) can read them from the program panels.

## Example session

The user asks: "Start testing the strategy for January in Designer and check that it is running".

1. `list_products` — Designer is running; for the other programs — `… is not running.`
2. `designer_list_strategies` — the strategies include the required one, of the `Backtest` kind, in the `Stopped` state.
3. `designer_start_backtest`, `strategyId` from step 2, `startTime` = `2026-01-01T00:00:00Z`, `stopTime` = `2026-02-01T00:00:00Z` — the response is `Strategy <identifier> was started.`
4. `designer_list_strategies` — the strategy state has changed.
5. `designer_stop_backtest` with the same `strategyId`, if the testing needs to be interrupted.

## See also

[Connecting AI agents (MCP)](../mcp.md)

[Interface control (DesktopDriver)](desktop_driver.md)

[Remote control](../shell/remote_control.md)
