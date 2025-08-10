# Notification

![Designer Notice 00](../../../../../../images/designer_notice_00.png)

The cube sends a notification when data arrives on its input socket. The incoming value is converted to text via `ToString`. You can connect a [Variable](../common/variable.md) to send fixed text, attach trade or candle streams to see their details, or use [String format](string_format.md) and [String concat](string_concat.md) cubes to prepare a custom message.

### Incoming sockets

Incoming sockets

- **Message** - data to send. Any value is accepted and converted to a string.

### Parameters

Parameters

- **Type** - type of message (pop-up window, e-mail, sms, etc.). The types of notifications are described in the [Notification settings](../../../../../terminal/notifications.md) section.
- **Telegram** - channel used for Telegram notifications.
- **Header** - the header of the message.

## Recommended content

[String format](string_format.md)
[String concat](string_concat.md)
[Notification settings](../../../../../terminal/notifications.md)
