# Telegram 服务

为了远程管理交易机器人（例如部署在虚拟服务器上的机器人），StockSharp 实现了与常用即时通信应用 Telegram 的集成。

该集成既适用于 [Designer](designer.md) 等现成程序，也适用于使用 [API](api.md) 开发的自有软件。

目前提供两项服务：

- [通知](telegram_services/alerts.md) — 用于接收交易机器人发送的通知。
- [控制面板](telegram_services/control_panel.md) — 通过 Telegram 机器人启动或停止交易机器人。
