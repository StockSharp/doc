# 连接 Hydra 客户端

在服务器模式下，可以连接另一个作为客户端运行的 Hydra 程序，由该客户端将数据下载到本地。与[通过 FIX 协议连接](fix_fast_connectivity.md)不同，数据会以 StockSharp 格式的文件传输。因此，该数据源适合传输大量历史数据。

连接时使用专用数据源：

![hydra tasks server](../../../images/hydratasksserver_1.png)

**Settings**

![hydra tasks server](../../../images/hydratasksserver_2.png)

- **Address** - Hydra 服务器地址。
- **Login** - 登录名（服务器要求身份验证时必填）。
- **Password** - 密码（服务器要求身份验证时必填）。
- **Time Offset** - 相对于当前日期的天数偏移，用于避免下载当前交易时段尚未完成的数据。
- **Weekends** - 是否下载周末数据。

**Main**

- **Title** - 任务名称。
- **Working Hours** - 设置平台的运行时间。
- **Interval of Operation** - 运行间隔。
- **Data Directory** - 数据目录，最终生成的 [S#](../../api.md) 格式文件将保存在此处。
- **Format** - 数据格式：BIN 或 CSV。
- **Max. Errors** - 允许的最大错误数。达到该数量后，任务将停止。默认值为 0，表示忽略错误数量。
- **Dependency** - 启动当前任务前必须完成的任务。

**Logging**

- **Identifier** - 标识符。
- **Logging Level** - 日志级别。
