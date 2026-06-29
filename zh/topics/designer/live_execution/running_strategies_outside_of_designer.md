# 在 Designer 外部运行策略

在 [Designer](../../designer.md) 中创建的策略既可以在您基于 [API](../../api.md) 开发的程序中运行，也可以在 [Runner](../../runner.md) 或 [Shell](../../shell.md) 应用程序中运行。

- [Runner](../../runner.md) 的运行速度比 [Designer](../../designer.md) 更快，占用的内存也更少，因此策略性能更好。这种方式非常适合在服务器上运行策略。详情请参阅[从 Designer 导出](../../runner/export_from_designer.md)。

- [Shell](../../shell.md) 以源代码形式提供。这种方式适合分发带有专用界面的策略，该界面可针对具体策略进行设计。详情请参阅[运行在 Designer 中创建的策略](../../shell/run_strategies_from_designer.md)。
