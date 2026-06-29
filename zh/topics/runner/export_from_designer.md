# 从 Designer 导出

**Runner** 可以运行在 [Designer](../designer.md) 中创建的策略。这是配置 **Runner** 最方便的方式，因为所有设置都通过图形界面完成。

要从 [Designer](../designer.md) 导出策略：

- 在树形列表中选择所需策略，单击鼠标右键，然后选择 **Runner** 菜单项：

  ![Designer_Runner_1](../../images/designer_runner_1.png)

- 在打开的窗口中，选择需要导出到 **Runner** 的连接类型，并设置通过 [Telegram](../telegram_services.md) 管理策略所需的参数：

  ![Designer_Runner_1](../../images/designer_runner_2.png)

以下文件将复制到选定的导出目录：

- connector.json — 包含连接设置的文件。
- params.json — 包含策略参数的文件。
- start.bat — 已写入命令行的 bat 文件，用于快速启动 **Runner**。
- strategy.json — 包含策略的文件。
- connector.json — 包含连接设置的文件。
- telegram.json — 包含 [Telegram](../telegram_services.md) 集成设置的文件。
