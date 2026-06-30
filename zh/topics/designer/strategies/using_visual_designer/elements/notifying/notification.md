# 通知

![Designer Notice 00](../../../../../../images/designer_notice_00.png)

当数据到达输入端口时，该模块会发送通知。传入值通过 `ToString` 转换为文本。可以连接 [Variable](../data_sources/variable.md) 来发送固定文本，连接成交或蜡烛数据流来查看其详细信息，也可以使用 [String format](string_format.md) 和 [String concat](string_concat.md) 模块生成自定义消息。

### 输入端口

输入端口

- **Message** - 要发送的数据。可以接收任意值，并将其转换为字符串。

### 参数

参数

- **Type** - 消息类型（弹出窗口、电子邮件、短信等）。通知类型在 [Notification settings](../../../../../terminal/notifications.md) 章节中说明。
- **Telegram** - 用于发送 Telegram 通知的频道。
- **Header** - 消息标题。

## 推荐内容

[String format](string_format.md)
[String concat](string_concat.md)
[Notification settings](../../../../../terminal/notifications.md)
