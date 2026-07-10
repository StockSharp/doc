# Hydra 设置

下面介绍如何创建和配置备份任务。

1. 要创建任务，请单击 **添加任务...** 按钮，在打开的窗口中选择 **备份**，然后单击 **确定**。![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. 接下来配置任务。![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **备份**
   - **服务** — 服务地址。
   - **地址** — 区域地址，即 Bucket 设置中指定区域的地址。请参阅 [区域和端点](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region)。
   - **存储** — Bucket 名称。
   - **登录名** — 登录名，即 Access Key ID。
   - **密码** — 密码，即 **Secret Access Key**。
   - **开始日期** — 开始备份的日期。
   - **时间偏移** — 相对于当前日期的偏移天数。

   **常规**
   - **标题** — 任务标题。
   - **工作时间** — 配置交易板的工作时间表。![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **操作间隔** — 任务的运行间隔。
   - **数据目录** — 用于读取待转换数据的数据目录。
   - **格式** — 转换后的数据格式：BIN\/CSV。
   - **最大错误数** — 任务停止前允许出现的最大错误数。默认值为 0，表示忽略错误数量。
   - **依赖任务** — 启动当前任务前必须完成的任务。

   **日志**
   - **标识符** — 标识符。
   - **日志级别** — 日志记录级别。
3. 配置任务后，添加需要保存到备份存储中的交易品种，然后单击 **开始** 按钮。

## 推荐内容

[创建和配置账户](setup.md)
