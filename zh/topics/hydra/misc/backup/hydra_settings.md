# Hydra 设置

下面介绍如何创建和配置备份任务。

1. 要创建任务，请单击 **Add tasks...** 按钮，在打开的窗口中选择 **Backup**，然后单击 **OK**。![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. 接下来配置任务。![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Backup**
   - **Service** — 服务地址。
   - **Address** — 区域地址，即 Bucket 设置中指定区域的地址。请参阅 [Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region)。
   - **Storage** — Bucket 名称。
   - **Login** — 登录名，即 Access Key ID。
   - **Password** — 密码，即 **Secret Access Key**。
   - **Start date** — 开始备份的日期。
   - **Time offset** — 相对于当前日期的偏移天数。

   **General**
   - **Header** — Converter。
   - **Working hours** — 配置交易板的工作时间表。![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **Interval of operation** — 任务的运行间隔。
   - **Data directory** — 用于读取待转换数据的数据目录。
   - **Format** — 转换后的数据格式：BIN\/CSV。
   - **Max. errors** — 任务停止前允许出现的最大错误数。默认值为 0，表示忽略错误数量。
   - **Dependency** — 启动当前任务前必须完成的任务。

   **Logging**
   - **Identifier** — 标识符。
   - **Logging level** — 日志记录级别。
3. 配置任务后，添加需要保存到备份存储中的交易品种，然后单击 **Start** 按钮。

## 推荐内容

[创建和配置账户](setup.md)
