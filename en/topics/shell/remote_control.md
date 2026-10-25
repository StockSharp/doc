# Remote control

[Shell](../shell.md) can hand its strategies over to be managed from the website: the running program registers as an application of your account, reports the state of each strategy there and executes the commands that come from there. Remote control in [Hydra](../hydra.md) and [Designer](../designer.md) works the same way, so the list of programs and the control buttons in the [Telegram bot](../telegram_services/control_panel.md) look the same for all three.

## Enabling

The command bar of the main window has the **Remotely** switch.

![Shell main window title](../../images/shell_title_00.png)

While it is off, [Shell](../shell.md) connects nowhere and does not appear on the website. The enabled state is remembered and restored at the next start of the program.

The switch works only when the program is running under a StockSharp account and the website is available. Without signing in to the account, enabling it does nothing: the connection is not opened, and the application is not registered on the website.

## What is available remotely

The strategies of the [Emulation](user_interface/emulation.md) area — those working on history — are available remotely. The strategies of the [Real-time](user_interface/real_time.md) area are deliberately unavailable: they trade with real money, and such a strategy can be started only at the computer itself. The interprocess communication protocol used by scripts draws the same boundary.

A particular strategy appears on the website not by itself, but only if this is allowed in its settings. Open the strategy in the [Emulation](user_interface/emulation.md) area and find the **Remotely** group in its settings:

| Setting | What it sets |
|---|---|
| **Enabled** | whether the website sees this strategy and can manage it |
| **Notification** | the channel to which its messages go; if no channel is selected, the account channel |
| **Logging level** | starting from which level a message should be sent to the channel |

The channels are taken from the account, so the list appears after signing in. Setting up a channel in [Telegram](../telegram_services.md) is described in the [Authorization](../telegram_services/authorization.md) section.

## Commands

Two kinds of commands come from the website or the bot: about the program as a whole and about a particular strategy.

| Command | What it does |
|---|---|
| **Start** for a strategy | starts the run if it is stopped or has ended with an error |
| **Stop** for a strategy | stops the running run |
| **Start** / **Stop** without specifying a strategy | the same at once for all the strategies that are allowed remote control |
| **Enable** / **Disable** trading | switches the strategy to full trading mode or forbids it to trade; the current run picks this up at once |
| **Stop** the program | closes [Shell](../shell.md) |
| **Restart** the program | closes [Shell](../shell.md) and starts it again |

A command addressed to a strategy that is not allowed remote control is ignored.

## What goes to the website

- The state of the program itself: started while the window is open, and stopped when it is closed.
- The state of each allowed strategy — on every change.
- The strategy messages, starting from the selected level, to the selected channel. Identical messages are not repeated.

## See also

[Control Panel](../telegram_services/control_panel.md)

[Emulation](user_interface/emulation.md)
