# 通知

![通知 截图](../../../../../../images/designer_notice_00.png)

当数据到达输入端口时，该模块会发送通知。传入值通过 `ToString` 转换为文本。可以连接 [变量](../data_sources/variable.md) 来发送固定文本，连接成交或K线数据流来查看其详细信息，也可以使用 [字符串格式化](string_format.md) 和 [字符串拼接](string_concat.md) 模块生成自定义消息。

### 输入端口

输入端口

- **消息** - 要发送的数据。可以接收任意值，并将其转换为字符串。

### 参数

参数

- **类型** - 消息类型（弹出窗口、电子邮件、短信等）。通知类型在[通知设置](../../../../../terminal/notifications.md)章节中说明。
- **Telegram** - 用于发送 Telegram 通知的频道。
- **标题** - 消息标题。

## 推荐内容

[字符串格式化](string_format.md)
[字符串拼接](string_concat.md)
[通知设置](../../../../../terminal/notifications.md)
