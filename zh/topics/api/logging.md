# 记录

对于用 [S#](../api.md) 编写的交易算法的监控，你可以使用专用的 [LogManager](xref:Ecng.Logging.LogManager) 类。该类通过 [ILogSource.Log](xref:Ecng.Logging.ILogSource.Log) 事件从 [LogManager.Sources](xref:Ecng.Logging.LogManager.Sources) 接收 [LogMessage](xref:Ecng.Logging.LogMessage) 消息，并将它们传递给 [LogManager.Listeners](xref:Ecng.Logging.LogManager.Listeners) 监听器。因此，算法代码将能够传递调试信息（例如，操作过程中发生的错误，或关于数学计算的额外信息），而 [LogManager](xref:Ecng.Logging.LogManager) 将决定如何向操作者显示这些信息。

通常，[S#](../api.md) 包含以下 [ILogListener](xref:Ecng.Logging.ILogListener) 的实现，其选择会影响从策略接收到的消息将被传递到的位置：

1. [FileLogListener](xref:Ecng.Logging.FileLogListener) - 将消息写入文本文件。建议用于已创建的算法，并在不可抗力情况下使用日志。
2. [ConsoleLogListener](xref:Ecng.Logging.ConsoleLogListener) - 将信息输出到控制台窗口（如果算法没有窗口，将自动创建）。建议用于调试和测试算法
3. [DebugLogListener](xref:Ecng.Logging.DebugLogListener) - 将消息输出到调试窗口。可以通过像 [DebugView](https://technet.microsoft.com/en-us/sysinternals/bb896647.aspx) 这样的特殊程序查看该窗口。建议用于调试和测试算法。
4. [EmailLogListener](xref:Ecng.Logging.EmailLogListener) - 向指定的电子邮件地址发送消息。如果算法位于不受控制的计算机（在主机服务器上），建议使用它。
5. [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) - 通过特殊的 [LogControl](xref:StockSharp.Xaml.LogControl) 窗口显示消息。能够以两种模式工作：所有消息在单一窗口输出，或者为每个 [ILogSource](xref:Ecng.Logging.ILogSource) 创建单独的窗口。如果算法具有图形界面，建议使用此功能。

[LogListener](xref:Ecng.Logging.LogListener) 可以通过 [LogListener.Filters](xref:Ecng.Logging.LogListener.Filters) 属性配置以过滤消息。例如，通过这些过滤器，你可以指定应处理的消息类型。当使用 [EmailLogListener](xref:Ecng.Logging.EmailLogListener) 时，这尤其有用，例如，仅在紧急情况下（交易算法错误）发送电子邮件，而不是在每条调试消息时都发送。

## 下一步

[策略日志记录](logging/strategy_logging.md)

[IConnector 日志记录](logging/iconnector_logging.md)

[其他日志来源](logging/other_logs_sources.md)

[可视化监控](logging/visual_monitoring.md)

[ILogListener 创建](logging/custom_iloglistener.md)
