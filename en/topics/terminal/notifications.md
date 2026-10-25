# Notifications

[Terminal](../terminal.md) reports important events with pop-up windows in the corner of the screen: a connection is established or lost, an own trade is executed, an order could not be cancelled. A connection error is shown in a window with a warning.

Terminal can also report order and connection events in other ways. They are selected in the [Settings](settings/common_settings.md) window (**Settings → Settings**), in the **Notification** group, separately for each event:

- **Order change** — an order has changed;
- **Matching** — an order has been executed;
- **Error** — an order has not been accepted;
- **Lost** and **Restored** — the connection has been lost and restored.

For each event, you can choose how to report it:

- **Sound** - a sound signal will be played.
- **Popup window** - a small window with a message will appear in the corner of the screen.
- **Log file** - the message will be written to the log, which can be seen in the [Logs](user_interface/logs.md) panel.
- **Telegram** - the message will be sent to the Telegram channel specified in the same group of settings. The channels are taken from the StockSharp account, so this method requires signing in to the website.

If no method is selected, the event is not reported.

## See also

[General settings](settings/common_settings.md)

[Main menu](user_interface/ribbon.md)
