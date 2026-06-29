# 自动导入

该任务会按照指定的文件掩码，自动从指定目录中的文件导入交易所数据。

每种所选市场数据类型的模板都在[导入](../importing.md)选项卡中配置。

![hydra tasks import](../../../images/hydra_tasks_import.png)

在面板底部，可以选择要导入数据的交易品种以及要导入的数据类型。

可以为每个交易品种指定以下数据导入属性：

![hydra tasks proper import](../../../images/hydra_tasks_proper_import.png)

**Import (auto)**

**Settings**

- **Data type** — 导入的数据类型。
- **Filename** — 文件的完整路径。
- **Data directory** — 数据目录。
- **File mask** — 扫描目录时使用的文件掩码，例如 `candles\*.csv`。
- **Subdirectories** — 是否包含子目录。
- **Column separator** — 列分隔符。制表符使用 TAB 表示。
- **Indent from the beginning** — 从文件开头跳过的行数，用于忽略包含元信息的行。
- **Time zone** — 时区。
- **Interval** — 数据更新频率。
- **Extended information** — 将导入的扩展字段保存到扩展信息存储中。
- **Duplicates** — 如果重复的交易品种已存在，是否对其进行更新。
- **Ignore without ID** — 忽略没有标识符的交易品种。

**General**

- **Header** — Converter。
- **Working hours** — 配置交易板的工作时间表。![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** — 任务的运行间隔。
- **Data directory** — 用于接收待转换数据的数据目录。
- **Format** — 转换后的数据格式：BIN\/CSV。
- **Max. errors** — 任务停止前允许出现的最大错误数。默认值为 0，表示忽略错误数量。
- **Dependency** — 启动当前任务前必须完成的任务。

**Logging**

- **Identifier** — 标识符。
- **Logging level** — 日志记录级别。
